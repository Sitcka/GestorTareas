using System;
using System.Collections.Generic;
using System.Text;

namespace Gestor_Tareas
{
    public abstract class Tarea
    {
        //CAMPOS PRIVADOS
        private EstadoTarea _estado;
        private string? _motivoCancelacion;

        //PUBLICO
        public Guid Id { get; }
        public string Titulo { get; }
        public string Descripcion { get; }
        public DateTime FechaCreacion { get; }
        public DateTime FechaLimite { get; }
        public PrioridadTarea Prioridad { get; }

        //SOLO LECTURA
        public EstadoTarea Estado => _estado;

        //CONSTRUCTOR CON VALIDACIONES

        //Hacer FETCH antes que PULL
        public Tarea(
            string titulo,
            DateTime fechaLimite,
            PrioridadTarea prioridad, //Enum
            string ? descripcion = null
            )
        {
            //Tiutlo vacio
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El titulo es obligatorio "
                    , nameof(titulo));
            //Fecha limite
            if (fechaLimite.Date < DateTime.Today)
                throw new ArgumentException("La fecha limite no puede ser anterior a hoy",
                    nameof(fechaLimite));

            Id = Guid.NewGuid();
            Titulo = titulo.Trim();
            Descripcion = descripcion?.Trim() ?? string.Empty;
            FechaCreacion = DateTime.Now;
            FechaLimite = fechaLimite.Date;
            Prioridad = prioridad;
            _estado = EstadoTarea.Pendiente;

        }

        //METODOS
        public bool Iniciar()
        {

            if (_estado != EstadoTarea.Pendiente)
                return false;
            _estado = EstadoTarea.EnProgreso;
            return true;
        } 
        public bool Completar()
        {
            if (_estado == EstadoTarea.Completada || _estado == EstadoTarea.Cancelada)
                return false;
            _estado = EstadoTarea.Completada;
            return true;
        }
        public bool Cancelar(string motivo)
        {
            if (_estado == EstadoTarea.Cancelada)
                return false;
            _estado = EstadoTarea.Cancelada;
            _motivoCancelacion = motivo ?? "Sin especificar";
            return true;
        }

        public bool EstaVencida() =>
            _estado != EstadoTarea.Completada &&
            _estado != EstadoTarea.Cancelada &&
            DateTime.Today > FechaLimite;

        public int DiasRestantes => (FechaLimite - DateTime.Today).Days;

        //Metodo abstracto 
        public abstract string ObtenerResumen();

        //ToString sobreescrito
        public override string ToString() => ObtenerResumen();

    }
}
