using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
	private readonly IUsuarioService _servicio;

	public UsuariosController(IUsuarioService servicio)
	{
		_servicio = servicio;
	}

	[HttpGet]
	public async Task<IActionResult> ObtenerTodos()
	{
		var usuarios = await _servicio.GetAllAsync();
		return Ok(usuarios);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> ObtenerPorId(int id)
	{
		var usuario = await _servicio.GetByIdAsync(id);
		if (usuario is null) return NotFound();
		return Ok(usuario);
	}

	[HttpPost]
	public async Task<IActionResult> Crear([FromBody] CrearUsuarioDto crearUsuarioDto)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var usuarioCreado = await _servicio.CreateAsync(crearUsuarioDto);
		return CreatedAtAction(nameof(ObtenerPorId), new { id = usuarioCreado.Id }, usuarioCreado);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Actualizar(int id, [FromBody] CrearUsuarioDto actualizarUsuarioDto)
	{
		if (!ModelState.IsValid) return BadRequest(ModelState);

		var usuarioActualizado = await _servicio.UpdateAsync(id, actualizarUsuarioDto);
		if (usuarioActualizado is null) return NotFound();
		return Ok(usuarioActualizado);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Eliminar(int id)
	{
		var eliminado = await _servicio.DeleteAsync(id);
		if (!eliminado) return NotFound();
		return NoContent();
	}
}
