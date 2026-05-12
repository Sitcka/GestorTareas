using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;


namespace Domain.Entities;

public class TareaSimple : Tarea
{
    public TareaSimple(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        int usuarioId,
        string? descripcion = null)
        : base(titulo, fechaLimite, prioridad, usuarioId, descripcion)
    {

    }

}
