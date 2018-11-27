using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class ConfirmarGustosModel
    {
        public InvitacionPedidoGustoEmpanadaUsuario[] GustosEmpanadasCantidad { get; set; }
        public Guid Token { get; set; }
        public int IdUsuario { get; set; }
    }
}