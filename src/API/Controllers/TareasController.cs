using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _servicio;

    public TareasController(ITareaService servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var tareas = await _servicio.ObtenerTodasAsync();
        return Ok(tareas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var tarea = await _servicio.ObtenerPorIdAsync(id);
        if (tarea is null) return NotFound();
        return Ok(tarea);
    }

    [HttpGet("paginadas")]
    public async Task<IActionResult> ObtenerPaginadas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var resultado = await _servicio.ObtenerPaginadasAsync(pageNumber, pageSize);
        return Ok(resultado);
    }

    [HttpPost("simple")]
    public async Task<IActionResult> CrearSimple([FromBody] CrearTareaSimpleDto dto)
    {
        var creada = await _servicio.CrearSimple(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPost("prioritaria")]
    public async Task<IActionResult> CrearPrioritaria([FromBody] CrearTareaPrioritariaDto dto)
    {
        var creada = await _servicio.CrearPrioritaria(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPost("recurrente")]
    public async Task<IActionResult> CrearRecurrente([FromBody] CrearTareaRecurrenteDto dto)
    {
        var creada = await _servicio.CrearRecurrente(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}/completar")]
    public async Task<IActionResult> MarcarComoCompletada(int id)
    {
        var ok = await _servicio.MarcarComoCompletada(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpPut("{id:int}/simple")]
    public async Task<IActionResult> ActualizarSimple(int id, [FromBody] CrearTareaSimpleDto dto)
    {
        var actualizada = await _servicio.ActualizarSimple(id, dto);
        if (actualizada is null) return NotFound();
        return Ok(actualizada);
    }

    [HttpPut("{id:int}/prioritaria")]
    public async Task<IActionResult> ActualizarPrioritaria(int id, [FromBody] CrearTareaPrioritariaDto dto)
    {
        try
        {
            var actualizada = await _servicio.ActualizarPrioritaria(id, dto);
            if (actualizada is null) return NotFound();
            return Ok(actualizada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/recurrente")]
    public async Task<IActionResult> ActualizarRecurrente(int id, [FromBody] CrearTareaRecurrenteDto dto)
    {
        try
        {
            var actualizada = await _servicio.ActualizarRecurrente(id, dto);
            if (actualizada is null) return NotFound();
            return Ok(actualizada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await _servicio.Eliminar(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}