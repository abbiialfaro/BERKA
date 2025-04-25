using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Cita
    {
        [Key]
        public int ID_Cita { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; }

        // Llaves foráneas
        public int ID_Cliente { get; set; }
        public int ID_Vehiculo { get; set; }
        public int ID_Inspector { get; set; }

        // Relaciones
        [ForeignKey("ID_Cliente")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("ID_Vehiculo")]
        public Vehiculo? Vehiculo { get; set; }

        [ForeignKey("ID_Inspector")]
        public Inspector? Inspector { get; set; }
    }
}