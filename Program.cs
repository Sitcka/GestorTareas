using System;
namespace Gestor_Tareas
{
    public class Program
    {
        static GestorTareas gestor = new GestorTareas();

        //Metodo para mostrar menu
        static void MostrarMenu()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────┐");
            Console.WriteLine("│                 MENÚ                     │");
            Console.WriteLine("├─────────────────────────────────────────┤");
            Console.WriteLine("│ 1. Agregar Tarea Simple                  │");
            Console.WriteLine("│ 2. Agregar Tarea Prioritaria             │");
            Console.WriteLine("│ 3. Agregar Tarea Recurrente              │");
            Console.WriteLine("│ 4. Listar todas las tareas               │");
            Console.WriteLine("│ 5. Buscar tarea por ID                   │");
            Console.WriteLine("│ 6. Filtrar por estado                    │");
            Console.WriteLine("│ 7. Mostrar tareas vencidas               │");
            Console.WriteLine("│ 8. Guardar (forzar)                      │");
            Console.WriteLine("│ 0. Salir                                 │");
            Console.WriteLine("└─────────────────────────────────────────┘");
            Console.Write("Seleccione una opción: ");
        }

        //Metodo para crear TareaSimple
        public static TareaSimple ? CrearTareaSimple()
        {
            Console.Write("-- Nueva Tarea Simple --");

            Console.Write("Titulo: ");
            string titulo  = Console.ReadLine();

            Console.Write("Descripcion: ");
            string descripcion = Console.ReadLine();

            Console.Write("Fecha limite: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaLimite))
            {
                Console.WriteLine("Fecha invalida");
                return null;
            }

            Console.WriteLine("Prioridad: ");
            if (!Enum.TryParse<PrioridadTarea>(Console.ReadLine(), out PrioridadTarea prioridad))
            {
                Console.WriteLine("Prioridad invalida");
                return null;
            }

            var tarea = new TareaSimple(titulo, fechaLimite, prioridad, descripcion);
            Console.WriteLine($"Tarea agregada con ID: {tarea.Id}");
            Console.WriteLine($" {tarea.ObtenerResumen()}");
            return tarea;
        }

        //Metodo para crear tarea prioritaria
        public static TareaPrioritaria? CrearTareaPrioritaria()
        {
            Console.WriteLine("--- NUEVA TAREA PRIORITARIA ---");

            Console.Write("Titulo: ");
            string titulo = Console.ReadLine();

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine();

            Console.Write("Fecha limite (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaLimite))
            {
                Console.WriteLine(" Fecha invalida");
                return null;
            }

            Console.WriteLine("Prioridad (0=Baja, 1=Media, 2=Alta): ");
            if (!Enum.TryParse<PrioridadTarea>(Console.ReadLine(), out PrioridadTarea prioridad))
            {
                Console.WriteLine("❌ Prioridad inválida");
                return null;
            }

            Console.Write("Nivel de urgencia (1-10): ");
            if (!int.TryParse(Console.ReadLine(), out int nivelUrgencia) || nivelUrgencia < 1 || nivelUrgencia > 10)
            {
                Console.WriteLine("❌ Nivel de urgencia debe ser entre 1 y 10");
                return null;
            }

            var tarea = new TareaPrioritaria(titulo, fechaLimite, prioridad, nivelUrgencia, descripcion);
            Console.WriteLine($"✅ Tarea agregada con ID: {tarea.Id}");
            Console.WriteLine($"   {tarea.ObtenerResumen()}");

            return tarea;
        }

        //Metodo para crear tarea recurrente
        public static TareaRecurrente? CrearTareaRecurrente()
        {
            Console.WriteLine("--- NUEVA TAREA RECURRENTE ---");

            Console.Write("Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Descripción (opcional): ");
            string descripcion = Console.ReadLine();

            Console.Write("Fecha límite (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaLimite))
            {
                Console.WriteLine("❌ Fecha inválida");
                return null;
            }

            Console.WriteLine("Prioridad (0=Baja, 1=Media, 2=Alta): ");
            if (!Enum.TryParse<PrioridadTarea>(Console.ReadLine(), out PrioridadTarea prioridad))
            {
                Console.WriteLine("❌ Prioridad inválida");
                return null;
            }

            Console.Write("Intervalo en días (ej: 7 para semanal): ");
            if (!int.TryParse(Console.ReadLine(), out int intervaloDias) || intervaloDias < 1)
            {
                Console.WriteLine("❌ Intervalo debe ser mayor a 0");
                return null;
            }

            var tarea = new TareaRecurrente(titulo, fechaLimite, prioridad, intervaloDias, descripcion);
            Console.WriteLine($"✅ Tarea agregada con ID: {tarea.Id}");
            Console.WriteLine($"   {tarea.ObtenerResumen()}");

            return tarea;
        }



        static void Main()
        {
            Console.WriteLine("--GESTOR DE TAREAS--\n");

            //Cargar tareas guardadas
            gestor.Cargar();

            bool ejecutando = true;

            while (ejecutando)
            {
                MostrarMenu();

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                   
                }
            }
        }
    }
}
