using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface ITareaService
    {
        Task<IEnumerable<TareaResponseDto>> ObtenerTodasAsync();
        Task<IEnumerable<TareaResponseDto>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<TareaResponseDto?> ObtenerPorIdAsync(int id);

        Task<PaginadoResponseDto<TareaResponseDto>> ObtenerPaginadasAsync(int numeroPagina, int tamanoPagina);

        Task<TareaResponseDto> CrearSimple(CrearTareaSimpleDto dto);
        Task<TareaResponseDto> CrearPrioritaria(CrearTareaPrioritariaDto dto);
        Task<TareaResponseDto> CrearRecurrente(CrearTareaRecurrenteDto dto);

        Task<TareaResponseDto?> ActualizarSimple(int id, CrearTareaSimpleDto dto);
        Task<TareaResponseDto?> ActualizarPrioritaria(int id, CrearTareaPrioritariaDto dto);
        Task<TareaResponseDto?> ActualizarRecurrente(int id, CrearTareaRecurrenteDto dto);

        Task<bool> MarcarComoCompletada(int id);
        Task<bool> Eliminar(int id);
    }
}