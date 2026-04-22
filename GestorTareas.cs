using System;
using System.Collections.Generic;
using System.Text;
using System.IO;          
using System.Linq;        
using System.Text.Json;    


namespace Gestor_Tareas
{
    public class GestorTareas
    {

        //Mantiene el orden de insercion
        private readonly List<Tarea> _tareas;

        //Busqueda de 0(1) por ID
        /**
         * El Dictionary permite una busqueda mas eficiente siendo sus claves unicas
         */
        private readonly Dictionary<Guid, Tarea> _indicePorId;

        //Constante que servira para el archivo de persistencia
        private const string ArchivoTareas = "tareas.json";

        //Constructor con capacidad inicial
        public GestorTareas(int capacidadInicial = 100)
        {
            _tareas = new List<Tarea>(capacidadInicial);
            _indicePorId = new Dictionary<Guid, Tarea>(capacidadInicial);
        }

        //Agregar
        public void AgregarTarea(Tarea tarea)
        {
            ArgumentNullException.ThrowIfNull(tarea);

            if (_indicePorId.ContainsKey(tarea.Id))
                throw new InvalidOperationException($"Ya existe una tarea con ID {tarea.Id}");
            
            //0(1)
            _indicePorId.Add(tarea.Id, tarea);
            //0(1) al final
            _tareas.Add(tarea);

        }


        //Buscar por ID
        public Tarea? BuscarPorId(Guid id)
        {
            return _indicePorId.GetValueOrDefault(id);
        }

        //Filtrar con predicado con uso de LINQ y Delegado FUNC
        public IEnumerable<Tarea>Filtrar(Func<Tarea, bool> criterio)
        {
            return _tareas.Where(criterio);
        }

        //Obtiene todas las tareas (una interfaz, no la clase)
        public IReadOnlyList<Tarea> ObtenerTodas()
        {
            return _tareas.AsReadOnly();
        }

        //Obtener por estado
        public IEnumerable<Tarea>ObtenerPorEstado(EstadoTarea estado)
        {
            return _tareas.Where(tarea => tarea.Estado == estado);
        }

        //Obtener vencidas
        public IEnumerable<Tarea> ObtenerVencidas()
        {
            return _tareas.Where(tarea => tarea.EstaVencida());
        }

        
        //Obtener por prioritarias
        public IEnumerable<Tarea>ObtenerPorPrioritarias(int n)
        {
            return _tareas
                .OrderByDescending(tarea => tarea.Prioridad)
                .ThenByDescending(
                tarea => tarea is TareaPrioritaria tareaPrioritaria ? tareaPrioritaria.NivelUrgencia
                : 0)
                .Take(n);
        }

        //Eliminar por ID
        public bool EliminarPorId(Guid id)
        {
            if (!_indicePorId.Remove(id, out var tarea))
                return false;
            return true;
        }

        //Obtener el total de las tareas
        public int Total => _tareas.Count;

        //Contar tareas
        public Dictionary<EstadoTarea, int> ObtenerEstadisticas()
        {
            return _tareas
                .GroupBy(tarea => tarea.Estado)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());
        }
        //Metodos con persistencia JSON
        public void Guardar()
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(_tareas, opciones);
            File.WriteAllText(ArchivoTareas, json);
            Console.WriteLine($"{_tareas.Count} tareas guardadas en {ArchivoTareas}");
        }

        public void Cargar()
        {
            if (!File.Exists(ArchivoTareas))

                Console.WriteLine(
                    "No se encontro archivo de tareas previo. Iniciando vacio"
                    );
            return;

            try
            {
                string json = File.ReadAllText(ArchivoTareas);
                var tareasCargadas = JsonSerializer.Deserialize<List<Tarea>>(json);

                if (tareasCargadas != null)
                {
                    foreach (var tarea in tareasCargadas)
                    {
                        _indicePorId.TryAdd(tarea.Id, tarea);
                        _tareas.Add(tarea);
                    }
                    Console.WriteLine($"Cargadas {tareasCargadas.Count} tareas desdde" +
                        $" {ArchivoTareas}");

                }

            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
            }
            catch(JsonException ex)
            {
                Console.WriteLine($"Error al deserializar JSON: {ex.Message}");
            }

        }

        public void MostrarTodas()
        {
            if(_tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas registradas.");
                return;
            }

            Console.WriteLine($"\n Lista de tareas ({_tareas.Count} total):");
            Console.WriteLine(new string ('-', 60));

            foreach (var tarea in _tareas)
            {
                Console.WriteLine(tarea.ObtenerResumen());
            }

            Console.WriteLine(new string('-', 60));

            var stats = ObtenerEstadisticas();
            Console.WriteLine($"Estadisticas: Pendientes:" +
                $" {stats.GetValueOrDefault(EstadoTarea.Pendiente)} |" + $"En progreso: " +
                $"{stats.GetValueOrDefault(EstadoTarea.EnProgreso)} | " + 
                $"Completadas: {stats.GetValueOrDefault(EstadoTarea.Completada)} | " +
                $"Canceladas: {stats.GetValueOrDefault(EstadoTarea.Cancelada)}");
        }

    }
}
