using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public interface ITareaRepositorio
{
	Task<Tarea?> GetByIdAsync(int id);
	Task<IEnumerable<Tarea>> GetAllAsync();
	Task<IEnumerable<Tarea>> GetByUsuarioAsync(int usuarioId);
	Task<(IEnumerable<Tarea> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
	Task<(IEnumerable<Tarea> Items, int TotalCount)> GetPagedByUsuarioAsync(int usuarioId, int pageNumber, int pageSize);
	Task AddAsync(Tarea tarea);
	Task UpdateAsync(Tarea tarea);
	Task<bool> DeleteAsync(int id);
}
