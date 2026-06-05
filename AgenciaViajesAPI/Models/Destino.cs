namespace AgenciaViajesAPI.Models
{
    public class Destino
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty; 
    }
}
