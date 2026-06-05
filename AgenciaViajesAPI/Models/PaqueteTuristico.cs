namespace AgenciaViajesAPI.Models
{
    public class PaqueteTuristico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int DestinoId { get; set; } 
        public Destino? Destino { get; set; } 
        public decimal Precio { get; set; }
        public int CuposDisponibles { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRegreso { get; set; }
    }
}
