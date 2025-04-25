using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Revision
    {
        [Key]
        public int ID_Rev { get; set; }

        [Required]
        public int ID_Cita { get; set; }

        [ForeignKey("ID_Cita")]
        public Cita Cita { get; set; }
        public int ID_Vehiculo { get; set; }

        [Required]
        public string Resultado { get; set; }

        public string Observaciones { get; set; }

        public DateTime FechaRevision { get; set; }
    }

}
