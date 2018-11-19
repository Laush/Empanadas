using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoMetaData
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^([^<>]){1,50}$", ErrorMessage = "¡{0} No Válido! Tiene un máximo 50 caracteres.")]
        [Display(Name = "NombreNegocio")]
        public string NombreNegocio { get; set; }

    }
}