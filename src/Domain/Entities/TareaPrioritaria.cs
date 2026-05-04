using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class TareaPrioritaria : Tarea
{
    public int NivelUrgencia { get; }
    public TareaPrioritaria(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        int nivelUrgencia,
        string? descripcion = null)
        : base(titulo, fechaLimite, prioridad, descripcion)
    {
        if (nivelUrgencia < 1 || nivelUrgencia > 10)
            throw new ArgumentException("El nivel de urgencia debe " +
                "estar entre 1 y 10", nameof(nivelUrgencia));
        NivelUrgencia = nivelUrgencia;
    }

    public override string ObtenerResumen()
    {
        string indicador = NivelUrgencia switch
        {
            >= 8 => "🔴🔴🔴",
            >= 5 => "🟡🟡",
            _ => "🟢"
        };
        return $"{indicador} [Urgente {NivelUrgencia}/10]" +
            $" {Titulo} | {Estado} | Vence: {FechaLimite:dd//MM/yy}";
    }
}
