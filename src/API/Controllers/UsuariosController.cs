using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Crear([FromBody] CrearUsuarioDto dto)
    {
        var result = _service.CrearUsuario(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Id }, result);
    }

    [HttpGet]
    public IActionResult ObtenerTodos()
    {
        var items = _service.ObtenerTodos();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        var item = _service.ObtenerPorId(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        var ok = _service.Eliminar(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
