using AgenciaViajesAPI.DTOs; 
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
        
        public async Task<ActionResult<List<PaqueteDTO>>> GetAll(
            [FromQuery] string? busqueda,
            [FromQuery] decimal? precioMaximo)
        {
           
            var paquetes = await _service.ObtenerTodos(busqueda, precioMaximo);
            return Ok(paquetes);
        }

        [HttpPost]
        public async Task<ActionResult<PaqueteDTO>> Create([FromBody] PaqueteCreateDTO paqueteDto)
        {
            try
            {
                var nuevoPaquete = await _service.AgregarPaquete(paqueteDto);
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

        
        [HttpPut("{id}")]
        public async Task<ActionResult<PaqueteDTO>> Update(int id, [FromBody] PaqueteCreateDTO paqueteDto)
        {
            try
            {
                var paqueteActualizado = await _service.ActualizarPaquete(id, paqueteDto);

                if (paqueteActualizado == null)
                {
                    return NotFound(new { mensaje = $"No se encontró el paquete con ID {id}" });
                }

                return Ok(paqueteActualizado);
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

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _service.EliminarPaquete(id);

                if (!eliminado)
                {
                    return NotFound(new { mensaje = $"No se encontró el paquete con ID {id}" });
                }

                return NoContent(); 
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

    }
}