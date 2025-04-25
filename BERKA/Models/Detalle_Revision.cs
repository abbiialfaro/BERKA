using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Detalle_Revision
    {
        [Key]
        public int ID_Detalle { get; set; }

        [Required]
        public int ID_Rev { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Resultado { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string Observacion { get; set; }
    }
}
