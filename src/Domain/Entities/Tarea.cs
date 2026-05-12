using Domain.Enums;
namespace Domain.Entities;

public abstract class Tarea
{
    private EstadoTarea _estado;
    private string? _motivoCancelacion;

    //Constructor protegido
    protected Tarea() { }

    protected Tarea(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        int usuarioId,
        string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El título es obligatorio.", nameof(titulo));
        }

        if (fechaLimite.Date < DateTime.Today)
        {
            throw new ArgumentException("La fecha límite no puede ser anterior a hoy.", nameof(fechaLimite));
        }

        if(usuarioId <= 0)
        {
            throw new ArgumentException("El usuario es obligatorio.", nameof(usuarioId));
        }

        Titulo = titulo.Trim();
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        FechaCreacion = DateTime.UtcNow;
        FechaLimite = fechaLimite.Date;
        Prioridad = prioridad;
        UsuarioId = usuarioId;
        _estado = EstadoTarea.Pendiente;
    }

    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaLimite { get; private set; }
    public PrioridadTarea Prioridad { get; private set; }
    public EstadoTarea Estado => _estado;
    public string? MotivoCancelacion => _motivoCancelacion;

    //Relacion con Usuario
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public bool Iniciar()
    {
        if (_estado != EstadoTarea.Pendiente)
        {
            return false;
        }

        _estado = EstadoTarea.EnProgreso;
        return true;
    }

    public bool Completar()
    {
        if (_estado == EstadoTarea.Completada || _estado == EstadoTarea.Cancelada)
        {
            return false;
        }

        _estado = EstadoTarea.Completada;
        return true;
    }

    public bool Cancelar(string motivo)
    {
        if (_estado == EstadoTarea.Cancelada)
        {
            return false;
        }

        _estado = EstadoTarea.Cancelada;
        _motivoCancelacion = string.IsNullOrWhiteSpace(motivo) ? "Sin especificar" : motivo.Trim();
        return true;
    }

    public bool EstaVencida() =>
        _estado != EstadoTarea.Completada &&
        _estado != EstadoTarea.Cancelada &&
        DateTime.Today > FechaLimite;

    public void ActualizarDescripcion(string? descripcion)
    {
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
    }

    public void ActualizarFechaLimite(DateTime fechaLimite)
    {
        if(fechaLimite.Date < DateTime.Today)
        {
            throw new ArgumentException("La fecha no puede ser anterior a la de hoy", 
                nameof(fechaLimite));
        }
        FechaLimite = fechaLimite.Date;
    }

    public int ObtenerDiasRestantes => (FechaLimite - DateTime.Today).Days;

}