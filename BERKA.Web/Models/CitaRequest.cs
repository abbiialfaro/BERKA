 namespace BERKA.Web.Models
{
    public class CitaRequest
    {
        public int ID_Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string Placa { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}
