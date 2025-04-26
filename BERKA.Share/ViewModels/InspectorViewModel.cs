using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BERKA.Share.ViewModels
{
    public class InspectorViewModel
    {
        public int ID_Inspector { get; set; }
        [Required] public string Nombre { get; set; }
        [Required] public string Apellido { get; set; }
        [Required] public string Correo { get; set; }
        [Required] public string Telefono { get; set; }
        [Required] public int Estacion { get; set; }
        [Required] public string Contraseña { get; set; }
        [Required] public string Estado { get; set; }
    }

}
