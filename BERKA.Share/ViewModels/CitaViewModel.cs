// Esta clase representa los campos del formulario para agregar una nueva cita, con validaciones

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BERKA.Share.ViewModels
{
    public class CitaViewModel
    {
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Categoria { get; set; }

        public List<string> Categorias { get; set; } = new();
    }
}