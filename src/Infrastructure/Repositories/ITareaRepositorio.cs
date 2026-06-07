using Domain.Entities;

namespace Application.Interfaces;

public interface ITareaRepositorio
{
    Task<List<Tarea>> GetAllAsync();
    Task<Tarea?> GetByIdAsync(int id);
    Task AddAsync(Tarea tarea);
    Task UpdateAsync(Tarea tarea);
    Task<bool> DeleteAsync(int id);
    Task<(List<Tarea> Items, int Total)> GetPagedAsync(int pageNumber, int pageSize);
    Task<List<Tarea>> GetByUsuarioAsync(int usuarioId);
    Task<(List<Tarea> Items, int Total)> GetPagedByUsuarioAsync(int usuarioId, int pageNumber, int pageSize);
}