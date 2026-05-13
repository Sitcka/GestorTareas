using System.ComponentModel.DataAnnotations;
using Domain.Enums;
namespace Application.DTOs;

public class CrearTareaSimpleDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [MaxLength(150, ErrorMessage = "El titulo no puede superar los 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage = "La fecha limite para la tarea es obligatoria.")]
    public DateTime FechaLimite { get; set; }
    [Required(ErrorMessage = "La prioridad para la tarea es obligatoria.")]
    public PrioridadTarea Prioridad { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "UsuarioId debe ser mayor que 0.")]
    public int UsuarioId { get; set; }
    [MaxLength(300, ErrorMessage = "La descripcion no puede superar los 300 caracteres.")]
    public string? Descripcion { get; set; }
}
