using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TareaRepositorio : ITareaRepositorio
{
    private readonly GestorTareasDbContext _contexto;

    public TareaRepositorio(GestorTareasDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Tarea>> GetAllAsync()
    {
        return await _contexto.Tareas
            .Include(tarea => tarea.Usuario)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Tarea?> GetByIdAsync(int id)
    {
        return await _contexto.Tareas
            .Include(tarea => tarea.Usuario)
            .FirstOrDefaultAsync(tarea => tarea.Id == id);
    }

    public async Task AddAsync(Tarea tarea)
    {
        await _contexto.Tareas.AddAsync(tarea);
        await _contexto.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tarea tarea)
    {
        _contexto.Tareas.Update(tarea);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tarea = await _contexto.Tareas.FirstOrDefaultAsync(tarea => tarea.Id == id);
        if (tarea is null) return false;

        _contexto.Tareas.Remove(tarea);
        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<(List<Tarea> Items, int Total)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _contexto.Tareas
            .Include(tarea => tarea.Usuario)
            .AsNoTracking();

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(tarea => tarea.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<List<Tarea>> GetByUsuarioAsync(int usuarioId)
    {
        return await _contexto.Tareas
            .Include(tarea => tarea.Usuario)
            .AsNoTracking()
            .Where(tarea => tarea.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<(List<Tarea> Items, int Total)> GetPagedByUsuarioAsync(int usuarioId, int pageNumber, int pageSize)
    {
        var query = _contexto.Tareas
            .Include(tarea => tarea.Usuario)
            .AsNoTracking()
            .Where(tarea => tarea.UsuarioId == usuarioId);

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(tarea => tarea.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}