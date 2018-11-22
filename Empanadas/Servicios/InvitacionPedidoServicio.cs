using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class InvitacionPedidoServicio
    {
        private Entities MiBD = new Entities();


        public InvitacionPedido ObtenerPorId(int id)
        {
            return MiBD.InvitacionPedido.FirstOrDefault(i => i.IdInvitacionPedido == id);
        }

        

    }
}