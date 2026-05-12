using Domain.Enums;

namespace Application.DTOs;

public class CrearTareaSimpleDto
{
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public int UsuarioId { get; set; }
    public string? Descripcion { get; set; }
}
