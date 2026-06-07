// Application/DTOs/TareaResponseDto.cs
namespace Application.DTOs
{
    public class TareaResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public bool EstaVencida { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        public string TipoTarea { get; set; } = "Simple";
        public int? NivelUrgencia { get; set; }
        public int? IntervaloDias { get; set; }
        public DateTime? ProximaOcurrencia { get; set; }
    }
}