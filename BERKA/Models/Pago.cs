using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Pago
    {
        [Key]
        public int ID_Pago { get; set; }

        [Required]
        public int ID_Cita { get; set; }

        [Required]
        public int ID_Rev { get; set; }

        [Required]
        public int ID_Vehiculo { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Metodo_Pago { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Estado { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }
    }
}
