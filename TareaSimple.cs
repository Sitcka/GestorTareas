using System;
using System.Collections.Generic;
using System.Text;

namespace Gestor_Tareas
{
    public class TareaSimple : Tarea
    {
        public TareaSimple(string titulo,
            DateTime fechaLimite,
            PrioridadTarea prioridad,
            string? descripcion = null)
            :base(titulo, fechaLimite, prioridad, descripcion)
        {
            
        }

        public override string ObtenerResumen()
        {
            return $"[SIMPLE] {Titulo} | {Estado} | Vence: {FechaLimite:dd/MM/yy}";
        }
    }
}
