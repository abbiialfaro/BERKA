namespace BERKA.Web.Models
{
    public class RevisionViewModel
    {
        public int ID_Cita { get; set; }

        public int ID_Vehiculo { get; set; }
        public string Placa { get; set; }
        public string Luces { get; set; }
        public string Suspension { get; set; }
        public string Frenos { get; set; }
        public string Neumaticos { get; set; }
        public string Carroceria { get; set; }
        public string Gases { get; set; }
        public string Observaciones { get; set; }
    }
}
