using APItodo.Configs;
using MongoDB.Driver;

namespace APItodo.Services
{
    public class TareaServicio
    {
        private IMongoCollection<Tarea>? _coleccion;
        private readonly List<Tarea> _tareasEnMemoria = new();
        private readonly object _lock = new();
        private int _ultimoId = 1;
        private readonly bool _mongoDisponible;

        public TareaServicio(IConfiguration configuracion)
        {
            var configuraciones = configuracion.GetSection("MongoDbSettings").Get<MongoDbSettings>();

            if (configuraciones is not null &&
                !string.IsNullOrWhiteSpace(configuraciones.ConeccionString) &&
                !string.IsNullOrWhiteSpace(configuraciones.DataBaseName))
            {
                try
                {
                    var cliente = new MongoClient(configuraciones.ConeccionString);
                    var db = cliente.GetDatabase(configuraciones.DataBaseName);
                    _coleccion = db.GetCollection<Tarea>("tareas");
                    _mongoDisponible = true;
                    Console.WriteLine("[MongoDB] Conexion exitosa");
                }
                catch (Exception ex)
                {
                    _mongoDisponible = false;
                    _coleccion = null;
                    Console.WriteLine($"[MongoDB] Error de conexion: {ex.Message}");
                    Console.WriteLine("[MongoDB] Usando almacenamiento en memoria");
                }
            }
            else
            {
                Console.WriteLine("[MongoDB] Configuracion no encontrada, usando memoria");
            }
        }

        public async Task<Tarea> CrearAsync(Tarea tarea)
        {
            if (_mongoDisponible && _coleccion is not null)
            {
                try
                {
                    tarea.Id = 0;
                    var opciones = new InsertOneOptions { BypassDocumentValidation = false };
                    await _coleccion.InsertOneAsync(tarea, opciones);
                    Console.WriteLine($"[MongoDB] Tarea '{tarea.Titulo}' guardada con Id={tarea.Id}");
                    return tarea;
                }
                catch (MongoDB.Driver.MongoWriteException ex) when (ex.WriteError.Category == MongoDB.Driver.ServerErrorCategory.DuplicateKey)
                {
                    Console.WriteLine($"[MongoDB] Duplicado detectado, reasignando nuevo Id...");
                    var maxId = await _coleccion.Find(t => true).Sort(Builders<Tarea>.Sort.Descending(t => t.Id)).Limit(1).Project(t => t.Id).FirstOrDefaultAsync();
                    tarea.Id = maxId + 1;
                    await _coleccion.InsertOneAsync(tarea);
                    Console.WriteLine($"[MongoDB] Tarea '{tarea.Titulo}' guardada con Id={tarea.Id}");
                    return tarea;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MongoDB] Error al insertar: {ex.Message}");
                }
            }

            lock (_lock)
            {
                tarea.Id = _ultimoId++;
                _tareasEnMemoria.Add(tarea);
                Console.WriteLine($"[Memoria] Tarea '{tarea.Titulo}' guardada con Id={tarea.Id}");
                return tarea;
            }
        }

        public async Task<Tarea?> ObtenerAsync(int id)
        {
            if (_mongoDisponible && _coleccion is not null)
            {
                try
                {
                    return await _coleccion.Find(t => t.Id == id).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MongoDB] Error al obtener: {ex.Message}");
                }
            }

            lock (_lock)
            {
                return _tareasEnMemoria.FirstOrDefault(t => t.Id == id);
            }
        }

        public async Task<List<Tarea>> ObtenerTodosAsync()
        {
            if (_mongoDisponible && _coleccion is not null)
            {
                try
                {
                    return await _coleccion.Find(t => true).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MongoDB] Error al listar: {ex.Message}");
                }
            }

            lock (_lock)
            {
                return _tareasEnMemoria.ToList();
            }
        }

        public async Task<Tarea?> ActualizarAsync(int id, Tarea tarea)
        {
            if (_mongoDisponible && _coleccion is not null)
            {
                try
                {
                    await _coleccion.ReplaceOneAsync(t => t.Id == id, tarea);
                    return await _coleccion.Find(t => t.Id == id).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MongoDB] Error al actualizar: {ex.Message}");
                }
            }

            lock (_lock)
            {
                var index = _tareasEnMemoria.FindIndex(t => t.Id == id);
                if (index < 0) return null;

                tarea.Id = id;
                _tareasEnMemoria[index] = tarea;
                return tarea;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            if (_mongoDisponible && _coleccion is not null)
            {
                try
                {
                    var resultado = await _coleccion.DeleteOneAsync(t => t.Id == id);
                    return resultado.IsAcknowledged && resultado.DeletedCount > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MongoDB] Error al eliminar: {ex.Message}");
                }
            }

            lock (_lock)
            {
                var index = _tareasEnMemoria.FindIndex(t => t.Id == id);
                if (index < 0) return false;

                _tareasEnMemoria.RemoveAt(index);
                return true;
            }
        }
    }
}
