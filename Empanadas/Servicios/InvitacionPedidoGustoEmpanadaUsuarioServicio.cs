using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class InvitacionPedidoGustoEmpanadaUsuarioServicio
    {

        private Entities MiBD = new Entities();

        /*
        public String saberSiSeEligioGusto(List<Pedido> p)
        {

            String resultado = "";
            
            foreach ( Pedido pedido in p)
            {
                var invitacion = (from pp in MiBD.Pedido
                                  join ip in MiBD.InvitacionPedidoGustoEmpanadaUsuario
                                  on pp.IdPedido equals ip.IdPedido
                                  where ip.IdPedido == pedido.IdPedido
                              select ip).ToList();

                foreach (InvitacionPedidoGustoEmpanadaUsuario i in invitacion)
                {
                    if (i.Cantidad != 0)
                    {
                        resultado = "SI";
                    }
                    else
                    {
                        resultado = "NO";
                    }

                }
            }
            
            return resultado;
        }
        */


    }
}