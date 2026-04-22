using System;
using System.Collections.Generic;
using System.Text;

namespace Gestor_Tareas
{
    public class TareaRecurrente : Tarea
    {
        public int IntervaloDias { get; }
        public DateTime? ProximaOcurrencia { get; private set; }
        public TareaRecurrente(
            string titulo,
            DateTime fechaLimite,
            PrioridadTarea prioridad,
            int intervaloDias,
            string? descripcion = null)
            :base(titulo, fechaLimite, prioridad, descripcion)
        {
            if (intervaloDias < 1)
                throw new ArgumentException("El intervalo debe de ser mayor a 0 dias.",
                    nameof(intervaloDias));
            IntervaloDias = intervaloDias;
            ProximaOcurrencia = fechaLimite.AddDays(intervaloDias);
        }

        //Metodo para generar la iguiente ocurrencia
        public TareaRecurrente GenerarSiguiente()
        {
            if (Estado != EstadoTarea.Completada)
                throw new InvalidOperationException("Solo se puede generar la siguiente ocurrencia de una tarea completada");
            return new TareaRecurrente(
                Titulo,
                ProximaOcurrencia ?? FechaLimite.AddDays(IntervaloDias),
                Prioridad,
                IntervaloDias,
                Descripcion);
        }

        //Metodo heredado de la clase abstracta
        public override string ObtenerResumen()
        {
            return $"[Recurrente cada {IntervaloDias} dias] {Titulo} | {Estado}| Vence: " +
                $"{FechaLimite:dd/MM/yy} | Proxima: {ProximaOcurrencia?.ToString("dd/MM/yy") ??
                "N/A"}";
        }
    }
}
