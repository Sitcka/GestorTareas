using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _service;

    public TareasController(ITareaService service)
    {
        _service = service;
    }

    [HttpPost("simple")]
    public IActionResult CrearSimple([FromBody] CrearTareaSimpleDto dto)
    {
        var res = _service.CrearTareaSimple(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = res.Id }, res);
    }

    [HttpPost("prioritaria")]
    public IActionResult CrearPrioritaria([FromBody] CrearTareaPrioritariaDto dto)
    {
        var res = _service.CrearTareaPrioritaria(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = res.Id }, res);
    }

    [HttpPost("recurrente")]
    public IActionResult CrearRecurrente([FromBody] CrearTareaRecurrenteDto dto)
    {
        var res = _service.CrearTareaRecurrente(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = res.Id }, res);
    }

    [HttpGet]
    public IActionResult ObtenerTodas()
    {
        var items = _service.ObtenerTodas();
        return Ok(items);
    }

    [HttpGet("paged")]
    public IActionResult ObtenerPaginadas([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        // como la interfaz actual no soporta paginado con DTOs, usamos ObtenerTodas y paginamos en memoria
        var items = _service.ObtenerTodas();
        var total = items.Count;
        var pageItems = items.Skip((page - 1) * size).Take(size).ToList();
        return Ok(new { Items = pageItems, Total = total });
    }

    [HttpGet("{id:int}")]
    public IActionResult ObtenerPorId(int id)
    {
        var item = _service.ObtenerPorId(id);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpGet("usuario/{usuarioId:int}")]
    public IActionResult ObtenerPorUsuario(int usuarioId)
    {
        var items = _service.ObtenerPorUsuarioId(usuarioId);
        return Ok(items);
    }

    [HttpPut("{id:int}/completar")]
    public IActionResult Completar(int id)
    {
        var ok = _service.MarcarComoCompletada(id);
        if (!ok) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        var ok = _service.Eliminar(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
