using Empanadas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class InvitacionPedidoGustoEmpanadaUsuarioServicio
    {

        private Entities MiBD = new Entities();

        public bool Evaluar(ConfirmarGustosModel model)
        {
            var invitacion = MiBD.InvitacionPedido.Where(m => m.Token == model.Token).First();
            //PREGUNTO POR EL ESTADO DEL PEDIDO
            if (invitacion.Pedido.IdEstadoPedido == 2)
            {
                return false;
            }
            else
            {
                //PREGUNTO POR LOS GUSTOS SELECCIONADOS ESTAN EN EL PEDIDO
                foreach (var item in model.GustosEmpanadasCantidad)
                {
                    if (!invitacion.Pedido.GustoEmpanada.Select(x => x.IdGustoEmpanada).Contains(item.IdGustoEmpanada))
                    {
                        return false;
                    }
                }
                //GUARDAR aca o el controlador
                return true;
            }
        }



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