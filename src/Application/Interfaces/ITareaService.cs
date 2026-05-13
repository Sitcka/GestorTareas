using Application.DTOs;

namespace Application.Interfaces;

public interface ITareaService
{
    TareaSimpleResponseDto CrearTareaSimple(CrearTareaSimpleDto dto);

    TareaPrioritariaResponseDto CrearTareaPrioritaria(CrearTareaPrioritariaDto dto);

    TareaRecurrenteResponseDto CrearTareaRecurrente(CrearTareaRecurrenteDto dto);

    List<TareaResponseDto> ObtenerTodas();

    TareaResponseDto? ObtenerPorId(int id);

    List<TareaResponseDto> ObtenerPendientes();

    List<TareaResponseDto> ObtenerPorUsuarioId(int usuarioId);

    bool MarcarComoCompletada(int id);

    bool Eliminar(int id);
}