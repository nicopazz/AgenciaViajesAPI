namespace AgenciaViajesAPI.DTOs
{
    
    public class PaqueteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CuposDisponibles { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRegreso { get; set; }

        // Datos aplanados del JOIN:
        public string DestinoNombre { get; set; } = string.Empty;
        public string DestinoPais { get; set; } = string.Empty;
    }
}