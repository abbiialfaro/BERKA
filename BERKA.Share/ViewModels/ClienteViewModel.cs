// Esta clase representa los campos del formulario para agregar un nuevo cliente, con validaciones

using System.ComponentModel.DataAnnotations;

namespace BERKA.Share.ViewModels
{
    public class ClienteViewModel
    {
        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, Phone]
        public string Telefono { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Categoria { get; set; }
    }

}
