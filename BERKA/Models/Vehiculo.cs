using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BERKA.Models
{
    public class Vehiculo
    {
        [Key]
        public int ID_Vehiculo { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Marca { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Modelo { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string Categoria { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string Color { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Año { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Placa { get; set; }

        [Required]
        public int tip_Combustible { get; set; }

        [Required]
        public int Kilometraje { get; set; }

        [ForeignKey("Cliente")]
        [Column("ID_Cliente")]    // mapea la llave foránea
        public int ID_Cliente { get; set; }

        public Cliente Cliente { get; set; }
    }
}
