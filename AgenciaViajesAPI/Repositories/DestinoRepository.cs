using AgenciaViajesAPI.Data;
using AgenciaViajesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajesAPI.Repositories
{
    public class DestinoRepository
    {
        private readonly AgenciaDbContext _context;

        public DestinoRepository(AgenciaDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los destinos de la base de datos
        public async Task<List<Destino>> ObtenerTodos()
        {
            return await _context.Destinos.ToListAsync();
        }

        // Busca un destino por ID
        public async Task<Destino?> ObtenerPorId(int id)
        {
            return await _context.Destinos.FirstOrDefaultAsync(d => d.Id == id);
        }

        // Agrega un nuevo destino y guarda los cambios
        public async Task<Destino> Agregar(Destino destino)
        {
            _context.Destinos.Add(destino);
            await _context.SaveChangesAsync(); // Esto ejecuta el INSERT 
            return destino;
        }
    }
}