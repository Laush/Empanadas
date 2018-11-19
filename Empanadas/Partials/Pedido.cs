using Empanadas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Empanadas
{
    [MetadataType(typeof(PedidoMetaData))]
    public partial class Pedido
    {

        public int[] IdGustosSeleccionados { get; set; }


        public int[] IdUsuariosInvitados { get; set; }



    }
}