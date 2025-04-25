using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Tipos_Placa
    {
        [Key]
        public int ID_Tip_Placa { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }

        [Required]
        public int Registro { get; set; }
    }
}
