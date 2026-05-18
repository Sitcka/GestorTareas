using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services;

public class UsuarioService : IUsuarioService
{
	private readonly IUsuarioRepositorio _repositorio;

	public UsuarioService(IUsuarioRepositorio repositorio)
	{
		_repositorio = repositorio;
	}

	public UsuarioResponseDto CrearUsuario(CrearUsuarioDto dto)
	{
		var usuario = new Usuario(dto.Nombre, dto.Email, dto.EsAdmin);
		_repositorio.AddAsync(usuario).GetAwaiter().GetResult();

		return new UsuarioResponseDto
		{
			Id = usuario.Id,
			Nombre = usuario.Nombre,
			Email = usuario.Email,
			EsAdmin = usuario.EsAdmin
		};
	}

	public List<UsuarioResponseDto> ObtenerTodos()
	{
		var usuarios = _repositorio.GetAllAsync().GetAwaiter().GetResult();
		return usuarios.Select(u => new UsuarioResponseDto
		{
			Id = u.Id,
			Nombre = u.Nombre,
			Email = u.Email,
			EsAdmin = u.EsAdmin
		}).ToList();
	}

	public UsuarioResponseDto? ObtenerPorId(int id)
	{
		var u = _repositorio.GetByIdAsync(id).GetAwaiter().GetResult();
		if (u is null) return null;
		return new UsuarioResponseDto
		{
			Id = u.Id,
			Nombre = u.Nombre,
			Email = u.Email,
			EsAdmin = u.EsAdmin
		};
	}

	public bool Eliminar(int id)
	{
		return _repositorio.DeleteAsync(id).GetAwaiter().GetResult();
	}
}
