using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;


namespace Domain.Entities;

public class TareaRecurrente : Tarea
{
    public int IntervaloDias { get; }
    public DateTime ProximaOcurrencia { get; private set; }
    public TareaRecurrente(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        int usuarioId,
        int intervaloDias,
        string? descripcion = null)
        : base(titulo, fechaLimite, prioridad, usuarioId, descripcion)
    {
        if (intervaloDias < 1)
        {
            throw new ArgumentException("El intervalo debe de ser mayor a 0 dias.",
                nameof(intervaloDias));
        }
        IntervaloDias = intervaloDias;
        ProximaOcurrencia = fechaLimite.AddDays(intervaloDias);
    }

    //Metodo para generar una tarea recurrente
    public TareaRecurrente GenerarSiguiente()
    {
        if (Estado != EstadoTarea.Completada)
        {
            throw new InvalidOperationException("Solo se puede generar la siguiente ocurrencia de una tarea completada");
        }
        return new TareaRecurrente(
            Titulo,
            ProximaOcurrencia,
            Prioridad,
            UsuarioId,
            IntervaloDias,
            Descripcion);
    }

}
