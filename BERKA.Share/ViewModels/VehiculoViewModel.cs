using System.ComponentModel.DataAnnotations;

namespace BERKA.Share.ViewModels
{
    public class VehiculoViewModel
    {
        [Required]
        public string Marca { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int Año { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public int Tip_Combustible { get; set; }

        [Required]
        public int Kilometraje { get; set; }

        [Required]
        public int ID_Cliente { get; set; }
    }
}
