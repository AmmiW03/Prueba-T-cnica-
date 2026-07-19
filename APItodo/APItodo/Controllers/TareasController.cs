using APItodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace APItodo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareaServicio _tareaServicio;

        public TareasController(TareaServicio tareaServicio)
        {
            _tareaServicio = tareaServicio;
        }

        [HttpPost]
        [ActionName("Crear")]
        public async Task<ActionResult<Tarea>> Crear([FromBody] Tarea tarea)
        {
            var resultado = await _tareaServicio.CrearAsync(tarea);
            return CreatedAtAction(nameof(Obtener), new { id = resultado.Id }, resultado);
        }

        [HttpGet("{id}")]
        [ActionName("Obtener")]
        public async Task<ActionResult<Tarea>> Obtener(int id)
        {
            var tarea = await _tareaServicio.ObtenerAsync(id);
            return tarea is null ? NotFound() : Ok(tarea);
        }

        [HttpGet]
        [ActionName("ObtenerTodos")]
        public async Task<ActionResult<List<Tarea>>> ObtenerTodos()
        {
            var tareas = await _tareaServicio.ObtenerTodosAsync();
            return Ok(tareas);
        }

        [HttpPut("{id}")]
        [ActionName("Actualizar")]
        public async Task<ActionResult<Tarea>> Actualizar(int id, [FromBody] Tarea tarea)
        {
            tarea.Id = id;
            var resultado = await _tareaServicio.ActualizarAsync(id, tarea);
            return resultado is null ? NotFound() : Ok(resultado);
        }

        [HttpDelete("{id}")]
        [ActionName("Eliminar")]
        public async Task<ActionResult<bool>> Eliminar(int id)
        {
            var eliminado = await _tareaServicio.EliminarAsync(id);
            return eliminado ? Ok(true) : NotFound(false);
        }
    }
}
