using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class TareaRepositorio : ITareaRepositorio
{
	private readonly GestorTareasDbContext _context;

	public TareaRepositorio(GestorTareasDbContext context)
	{
		_context = context;
	}

	public async Task AddAsync(Tarea tarea)
	{
		await _context.Tareas.AddAsync(tarea);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var entidad = await _context.Tareas.FindAsync(id);
		if (entidad is null) return false;
		_context.Tareas.Remove(entidad);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Tarea>> GetAllAsync()
	{
		return await _context.Tareas
			.Include(t => t.Usuario)
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<(IEnumerable<Tarea> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
	{
		if (pageNumber < 1) pageNumber = 1;
		if (pageSize < 1) pageSize = 10;

		var query = _context.Tareas
			.Include(t => t.Usuario)
			.AsNoTracking();
		var total = await query.CountAsync();
		var items = await query
			.OrderBy(t => t.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return (items, total);
	}

	public async Task<Tarea?> GetByIdAsync(int id)
	{
		return await _context.Tareas
			.Include(t => t.Usuario)
			.AsNoTracking()
			.FirstOrDefaultAsync(t => t.Id == id);
	}

	public async Task<IEnumerable<Tarea>> GetByUsuarioAsync(int usuarioId)
	{
		return await _context.Tareas
			.Include(t => t.Usuario)
			.AsNoTracking()
			.Where(t => t.UsuarioId == usuarioId)
			.ToListAsync();
	}

	public async Task<(IEnumerable<Tarea> Items, int TotalCount)> GetPagedByUsuarioAsync(int usuarioId, int pageNumber, int pageSize)
	{
		if (pageNumber < 1) pageNumber = 1;
		if (pageSize < 1) pageSize = 10;

		var query = _context.Tareas
			.Include(t => t.Usuario)
			.AsNoTracking()
			.Where(t => t.UsuarioId == usuarioId);
		var total = await query.CountAsync();
		var items = await query
			.OrderBy(t => t.Id)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return (items, total);
	}

	public async Task UpdateAsync(Tarea tarea)
	{
		_context.Tareas.Update(tarea);
		await _context.SaveChangesAsync();
	}
}
