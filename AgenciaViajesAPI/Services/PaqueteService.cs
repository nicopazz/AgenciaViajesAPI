using AgenciaViajesAPI.Models;
using AgenciaViajesAPI.Repositories;

namespace AgenciaViajesAPI.Services
{
    public class PaqueteService
    {
        private readonly PaqueteRepository _repository;
        private readonly DestinoRepository _destinoRepository; // Inyectamos este para validar que el destino exista

        public PaqueteService(PaqueteRepository repository, DestinoRepository destinoRepository)
        {
            _repository = repository;
            _destinoRepository = destinoRepository;
        }

        public async Task<List<PaqueteTuristico>> ObtenerTodos()
        {
            return await _repository.ObtenerTodos();
        }

        public async Task<PaqueteTuristico> AgregarPaquete(PaqueteTuristico paquete)
        {
            // Regla 1: Validar fechas lógicas
            if (paquete.FechaRegreso <= paquete.FechaSalida)
            {
                throw new ArgumentException("La fecha de regreso debe ser posterior a la fecha de salida.");
            }

            // Regla 2: Validar precios y cupos
            if (paquete.Precio <= 0)
            {
                throw new ArgumentException("El precio del paquete debe ser mayor a 0.");
            }
            if (paquete.CuposDisponibles < 0)
            {
                throw new ArgumentException("Los cupos no pueden ser negativos.");
            }

            // Regla 3: Validar Integridad Referencial (El destino DEBE existir)
            var destinoExiste = await _destinoRepository.ObtenerPorId(paquete.DestinoId);
            if (destinoExiste == null)
            {
                throw new ArgumentException($"No se puede crear el paquete. El destino con ID {paquete.DestinoId} no existe.");
            }

            return await _repository.Agregar(paquete);
        }
    }
}