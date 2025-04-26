using System;
using System.ComponentModel.DataAnnotations;

namespace BERKA.Share.ViewModels
{
    public class CitaViewModel
    {
        public int ID_Cita { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required, StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public int ID_Cliente { get; set; }

        [Required]
        public int ID_Vehiculo { get; set; }

        [Required]
        public int ID_Inspector { get; set; }
    }
}
