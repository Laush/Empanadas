using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class Pedido
    {
        public int idPedido { get; set; }
        public int idUsuarioResponsable { get; set; }
        public String nombreNegocio { get; set; }
        public String descripcion { get; set; }
        public int idEstadoPedido { get; set; }
        public int precioUnidad { get; set; }
        public int precioDocena { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }


    }
}