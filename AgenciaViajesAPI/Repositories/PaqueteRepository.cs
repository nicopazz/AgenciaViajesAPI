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

        public async Task<List<PaqueteTuristico>> ObtenerTodos()
        {
            
            return await _context.Paquetes
                .Include(p => p.Destino)
                .ToListAsync();
        }

        public async Task<PaqueteTuristico> Agregar(PaqueteTuristico paquete)
        {
            _context.Paquetes.Add(paquete);
            await _context.SaveChangesAsync();
            return paquete;
        }
    }
}
