using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
        Task<UsuarioResponseDto?> GetByIdAsync(int id);
        Task<UsuarioResponseDto> CreateAsync(CrearUsuarioDto dto);
        Task<UsuarioResponseDto?> UpdateAsync(int id, CrearUsuarioDto dto);
        Task<bool> DeleteAsync(int id);

        // Para poder reutilizarlo donde haga falta
        Task<bool> ExistsEmailAsync(string email);
    }
}