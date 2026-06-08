using AgenciaViajesAPI.Models;
using AgenciaViajesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Esto mapeará la URL a: api/destinos
    public class DestinosController : ControllerBase
    {
        private readonly DestinoService _service;

        // Inyectamos nuestro servicio de forma automática gracias a .NET
        public DestinosController(DestinoService service)
        {
            _service = service;
        }

        // GET: api/destinos
        [HttpGet]
        public async Task<ActionResult<List<Destino>>> GetAll()
        {
            var destinos = await _service.ObtenerTodos();
            return Ok(destinos); // Devuelve un estado HTTP 200 OK con la lista en JSON
        }

        // POST: api/destinos
        [HttpPost]
        public async Task<ActionResult<Destino>> Create([FromBody] Destino destino)
        {
            try
            {
                var nuevoDestino = await _service.AgregarDestino(destino);

                // Devuelve un estado 201 Created e indica dónde encontrar el nuevo recurso
                return CreatedAtAction(nameof(GetAll), new { id = nuevoDestino.Id }, nuevoDestino);
            }
            catch (ArgumentException ex)
            {
                // Si salta nuestra validación del Service (falta nombre o país), devolvemos 400 Bad Request
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                // Captura cualquier otro error inesperado y protege la información del servidor
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }
    }
}