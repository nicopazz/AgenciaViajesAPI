using AgenciaViajesAPI.Models; 
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajesAPI.Data
{
    
    public class AgenciaDbContext : DbContext
    {
        
        public AgenciaDbContext(DbContextOptions<AgenciaDbContext> options) : base(options)
        {
        }

        
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<PaqueteTuristico> Paquetes { get; set; }
    }
}