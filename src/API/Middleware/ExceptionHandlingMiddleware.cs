using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _siguiente;

    public ExceptionHandlingMiddleware(RequestDelegate siguiente)
    {
        _siguiente = siguiente;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (ArgumentException ex)
        {
            await WriteProblemDetails(
                contexto,
                HttpStatusCode.BadRequest,
                "Datos incorrectos",
                ex.Message
            );
        }
        catch (KeyNotFoundException ex)
        {
            await WriteProblemDetails(
                contexto,
                HttpStatusCode.NotFound,
                "Recurso no encontrado",
                ex.Message
            );
        }
        catch (DbUpdateException)
        {
            await WriteProblemDetails(
                contexto,
                HttpStatusCode.BadRequest,
                "Error de base de datos",
                "Error al guardar en la base de datos."
            );
        }
        catch (Exception)
        {
            await WriteProblemDetails(
                contexto,
                HttpStatusCode.InternalServerError,
                "Error interno del servidor",
                "Ha ocurrido un error inesperado."
            );
        }
    }

    private static async Task WriteProblemDetails(
        HttpContext contexto,
        HttpStatusCode statusCode,
        string title,
        string detail)
    {
        contexto.Response.StatusCode = (int)statusCode;
        contexto.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = contexto.Request.Path
        };

        var json = JsonSerializer.Serialize(problem);
        await contexto.Response.WriteAsync(json);
    }
}