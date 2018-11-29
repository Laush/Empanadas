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

        public InvitacionPedido GetInvitacionPedidoPorPedido(int id, int idUsuario)
        {
            return MiBD.InvitacionPedido.FirstOrDefault(ip => ip.IdPedido == id && ip.IdUsuario == idUsuario);
        }

        public InvitacionPedidoGustoEmpanadaUsuario ElegirGustos()
        {
            return null;
        }

        public List<Usuario> ObtenerGustosConfirmados(int idPedido)
        {
            List<InvitacionPedido> invitacionesDelUsuario = MiBD.InvitacionPedido.Include("Pedido")
              .Where(o => o.IdPedido == idPedido).ToList();
            List<Usuario> usuarios = MiBD.Usuario.ToList();
            int i = 0;
            foreach (InvitacionPedido item in invitacionesDelUsuario)
            {
                if (usuarios[i].IdUsuario != item.IdUsuario)
                {
                    usuarios.RemoveAt(i);
                    i++;
                }
            }
            return usuarios;
        }

        /* public bool ValidarGustos(ConfirmarGusto datos)
         {
             try
             {
                 var estadoPedido = MiBD.InvitacionPedido.Where(i => i.Token == datos.Token).FirstOrDefault();

                 if (estadoPedido.Pedido.IdEstadoPedido == 2)
                 {
                     return false;
                 }
                 return true;
             }
             catch
             {
                 return false;
             }
         }*/

    }
}