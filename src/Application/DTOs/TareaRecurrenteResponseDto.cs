using Domain.Enums;

namespace Application.DTOs;

public class TareaRecurrenteResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public int IntervaloDias { get; set; }
    public DateTime ProximaOcurrencia { get; set; }
    public EstadoTarea Estado { get; set; }
    public int UsuarioId { get; set; }
}