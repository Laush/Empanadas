using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoMetaData
    {
        [Required(ErrorMessage = "El Nombre  es obligatorio")]
        [RegularExpression(@"^([^<>]){1,50}$", ErrorMessage = "¡{0} No Válido! Tiene un máximo 50 caracteres.")]
        [Display(Name = "NombreNegocio")]
        public string NombreNegocio { get; set; }

        [Required(ErrorMessage = "La Descripcion es obligatoria")]
        [RegularExpression(@"^([^<>]){1,50}$", ErrorMessage = "¡{0} No Válido! Tiene un máximo 50 caracteres.")]
        [Display(Name = "NombreNegocio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Precio por Unidad es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo valores numericos")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "El Precio por Docena es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo valores numericos")]
        public int PrecioDocena { get; set; }




    }
}



  

 