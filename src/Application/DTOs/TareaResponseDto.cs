namespace Application.DTOs
{
    public class TareaResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaLimite { get; set; }
        public bool EstaCompletada { get; set; }
        public string TipoTarea { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
    }
}