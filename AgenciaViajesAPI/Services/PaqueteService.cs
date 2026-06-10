using AgenciaViajesAPI.DTOs; 
using AgenciaViajesAPI.Models;
using AgenciaViajesAPI.Repositories;

namespace AgenciaViajesAPI.Services
{
    public class PaqueteService
    {
        private readonly PaqueteRepository _repository;
        private readonly DestinoRepository _destinoRepository;

        public PaqueteService(PaqueteRepository repository, DestinoRepository destinoRepository)
        {
            _repository = repository;
            _destinoRepository = destinoRepository;
        }


        
        public async Task<List<PaqueteDTO>> ObtenerTodos(string? busqueda = null, decimal? precioMaximo = null)
        {
           
            var paquetesBaseDatos = await _repository.ObtenerTodos(busqueda, precioMaximo);

            
            var paquetesDTO = paquetesBaseDatos.Select(p => new PaqueteDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                CuposDisponibles = p.CuposDisponibles,
                FechaSalida = p.FechaSalida,
                FechaRegreso = p.FechaRegreso,
                DestinoNombre = p.Destino != null ? p.Destino.Nombre : "Sin asignar",
                DestinoPais = p.Destino != null ? p.Destino.Pais : "Sin asignar"
            }).ToList();

            return paquetesDTO;
        }


        public async Task<PaqueteDTO> AgregarPaquete(PaqueteCreateDTO dto)
        {
            if (dto.FechaRegreso <= dto.FechaSalida)
                throw new ArgumentException("La fecha de regreso debe ser posterior a la de salida.");
            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
            if (dto.CuposDisponibles < 0)
                throw new ArgumentException("Los cupos no pueden ser negativos.");

            var destinoExiste = await _destinoRepository.ObtenerPorId(dto.DestinoId);
            if (destinoExiste == null)
                throw new ArgumentException($"El destino con ID {dto.DestinoId} no existe.");

            // Convertimos el DTO a Modelo para que Entity Framework lo entienda
            var nuevoPaquete = new PaqueteTuristico
            {
                Nombre = dto.Nombre,
                DestinoId = dto.DestinoId,
                Precio = dto.Precio,
                CuposDisponibles = dto.CuposDisponibles,
                FechaSalida = dto.FechaSalida,
                FechaRegreso = dto.FechaRegreso
            };

            var paqueteGuardado = await _repository.Agregar(nuevoPaquete);

            // Devolvemos el DTO final
            return new PaqueteDTO
            {
                Id = paqueteGuardado.Id,
                Nombre = paqueteGuardado.Nombre,
                Precio = paqueteGuardado.Precio,
                CuposDisponibles = paqueteGuardado.CuposDisponibles,
                FechaSalida = paqueteGuardado.FechaSalida,
                FechaRegreso = paqueteGuardado.FechaRegreso,
                DestinoNombre = destinoExiste.Nombre,
                DestinoPais = destinoExiste.Pais
            };

        }

        
        public async Task<PaqueteDTO?> ActualizarPaquete(int id, PaqueteCreateDTO dto)
        {
            // 1. Buscamos si el paquete existe
            var paqueteExistente = await _repository.ObtenerPorId(id);
            if (paqueteExistente == null) return null;

            // 2. Validamos las reglas (igual que en el POST)
            if (dto.FechaRegreso <= dto.FechaSalida)
                throw new ArgumentException("La fecha de regreso debe ser posterior a la de salida.");
            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
            if (dto.CuposDisponibles < 0)
                throw new ArgumentException("Los cupos no pueden ser negativos.");

            var destinoExiste = await _destinoRepository.ObtenerPorId(dto.DestinoId);
            if (destinoExiste == null)
                throw new ArgumentException($"El destino con ID {dto.DestinoId} no existe.");

            // 3. Modificamos los valores del objeto existente
            paqueteExistente.Nombre = dto.Nombre;
            paqueteExistente.DestinoId = dto.DestinoId;
            paqueteExistente.Precio = dto.Precio;
            paqueteExistente.CuposDisponibles = dto.CuposDisponibles;
            paqueteExistente.FechaSalida = dto.FechaSalida;
            paqueteExistente.FechaRegreso = dto.FechaRegreso;

            // 4. Guardamos los cambios
            await _repository.Actualizar(paqueteExistente);

            // 5. Devolvemos el DTO actualizado
            return new PaqueteDTO
            {
                Id = paqueteExistente.Id,
                Nombre = paqueteExistente.Nombre,
                Precio = paqueteExistente.Precio,
                CuposDisponibles = paqueteExistente.CuposDisponibles,
                FechaSalida = paqueteExistente.FechaSalida,
                FechaRegreso = paqueteExistente.FechaRegreso,
                DestinoNombre = destinoExiste.Nombre,
                DestinoPais = destinoExiste.Pais
            };
        }

        
        public async Task<bool> EliminarPaquete(int id)
        {
            var paqueteExistente = await _repository.ObtenerPorId(id);
            if (paqueteExistente == null) return false;

            await _repository.Eliminar(paqueteExistente);
            return true;
        }
    }
}