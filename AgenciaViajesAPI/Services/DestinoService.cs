using AgenciaViajesAPI.Models;
using AgenciaViajesAPI.Repositories;

namespace AgenciaViajesAPI.Services
{
    public class DestinoService
    {
        private readonly DestinoRepository _repository;

        public DestinoService(DestinoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Destino>> ObtenerTodos()
        {
            return await _repository.ObtenerTodos();
        }

        public async Task<Destino> AgregarDestino(Destino destino)
        {
            // Validaciones de negocio: No permitimos guardar destinos sin nombre o sin país
            if (string.IsNullOrWhiteSpace(destino.Nombre) || string.IsNullOrWhiteSpace(destino.Pais))
            {
                // Lanzamos una excepción que luego capturaremos en el controlador
                throw new ArgumentException("El nombre y el país del destino son obligatorios.");
            }

            // Si pasa las reglas, le decimos al repositorio que lo guarde
            return await _repository.Agregar(destino);
        }
    }           
}   
