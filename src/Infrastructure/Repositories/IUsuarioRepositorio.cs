using Domain.Entities;

namespace Application.Interfaces;

public interface IUsuarioRepositorio
{
    Task<List<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsEmailAsync(string email);
}