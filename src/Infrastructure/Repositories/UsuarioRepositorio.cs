using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly GestorTareasDbContext _contexto;

    public UsuarioRepositorio(GestorTareasDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        var emailNormalizado = email.Trim().ToLower();

        return await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Email.ToLower() == emailNormalizado);
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id);
        if (usuario is null) return false;

        _contexto.Usuarios.Remove(usuario);
        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsEmailAsync(string email)
    {
        var emailExistente = await GetByEmailAsync(email);
        return emailExistente is not null;
    }
}