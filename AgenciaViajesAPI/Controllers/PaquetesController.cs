using AgenciaViajesAPI.Models;
using AgenciaViajesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaquetesController : ControllerBase
    {
        private readonly PaqueteService _service;

        public PaquetesController(PaqueteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaqueteTuristico>>> GetAll()
        {
            var paquetes = await _service.ObtenerTodos();
            return Ok(paquetes);
        }

        [HttpPost]
        public async Task<ActionResult<PaqueteTuristico>> Create([FromBody] PaqueteTuristico paquete)
        {
            try
            {
                var nuevoPaquete = await _service.AgregarPaquete(paquete);
                return CreatedAtAction(nameof(GetAll), new { id = nuevoPaquete.Id }, nuevoPaquete);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }
    }
}