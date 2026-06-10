using AgenciaViajesAPI.Data;
using AgenciaViajesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajesAPI.Repositories
{
    public class PaqueteRepository
    {
        private readonly AgenciaDbContext _context;

        public PaqueteRepository(AgenciaDbContext context)
        {
            _context = context;
        }

        // Recibimos parámetros que pueden ser nulos (opcionales)
        public async Task<List<PaqueteTuristico>> ObtenerTodos(string? busqueda, decimal? precioMaximo)
        {
            // 1. Iniciamos la consulta base (El equivalente a un SELECT con su JOIN), pero NO la ejecutamos todavía
            IQueryable<PaqueteTuristico> query = _context.Paquetes.Include(p => p.Destino);

            // 2. Si el usuario envió un texto, agregamos el WHERE dinámico
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                // Buscamos si el texto coincide con el nombre del paquete o con el país del destino
                query = query.Where(p =>
                    p.Nombre.Contains(busqueda) ||
                    (p.Destino != null && p.Destino.Pais.Contains(busqueda))
                );
            }

            // 3. Si el usuario especificó un tope de precio, agregamos otro WHERE
            if (precioMaximo.HasValue)
            {
                query = query.Where(p => p.Precio <= precioMaximo.Value);
            }

            // 4. Recién acá, al llamar a ToListAsync(), se compila el SQL y se ejecuta en la base de datos
            return await query.ToListAsync();
        }

        // Busca un paquete específico por su ID (incluyendo los datos del destino)
        public async Task<PaqueteTuristico?> ObtenerPorId(int id)
        {
            return await _context.Paquetes
                .Include(p => p.Destino)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Actualiza un paquete existente
        public async Task Actualizar(PaqueteTuristico paquete)
        {
            _context.Paquetes.Update(paquete);
            await _context.SaveChangesAsync();
        }

        // Elimina un paquete
        public async Task Eliminar(PaqueteTuristico paquete)
        {
            _context.Paquetes.Remove(paquete);
            await _context.SaveChangesAsync();
        }

        public async Task<PaqueteTuristico> Agregar(PaqueteTuristico paquete)
        {
            _context.Paquetes.Add(paquete);
            await _context.SaveChangesAsync();
            return paquete;
        }
    }
}
