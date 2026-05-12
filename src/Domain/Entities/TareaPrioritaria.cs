using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class TareaPrioritaria : Tarea
{
    public int NivelUrgencia { get; private set; }
    public TareaPrioritaria(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        int nivelUrgencia,
        int usuarioId,
        string? descripcion = null)
        : base(titulo, fechaLimite, prioridad, usuarioId, descripcion)
    {
        if (nivelUrgencia < 1 || nivelUrgencia > 10)
            throw new ArgumentException("El nivel de urgencia debe " +
                "estar entre 1 y 10", nameof(nivelUrgencia));
        NivelUrgencia = nivelUrgencia;
    }

}
