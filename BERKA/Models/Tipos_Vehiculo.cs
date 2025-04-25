using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Tipos_Vehiculo
    {
        [Key]
        public int ID_Tipo_Vehiculo { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }
    }
}
