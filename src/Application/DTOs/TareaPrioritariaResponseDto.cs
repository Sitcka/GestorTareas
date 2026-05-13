using Domain.Enums;
namespace Application.DTOs;

public class TareaPrioritariaResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public int NivelUrgencia { get; set; }
    public EstadoTarea Estado { get; set; }
    public int UsuarioId { get; set; }
}