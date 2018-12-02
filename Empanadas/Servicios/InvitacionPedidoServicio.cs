using Empanadas.Models;
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

        public bool ValidarGustos(ConfirmarGustosModel datos)
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
        }

        public void ConfirmarGustos(ConfirmarGustosModel datos)
        {
            int idpedido = MiBD.InvitacionPedido
                            .Where(p => p.Token == datos.Token)
                            .Select(p => p.IdPedido).First();
            foreach (InvitacionPedidoGustoEmpanadaUsuario item in datos.GustosEmpanadasCantidad)
            {
                item.IdPedido = idpedido;
                item.IdUsuario = datos.IdUsuario;
                MiBD.InvitacionPedidoGustoEmpanadaUsuario.Add(item);
                MiBD.SaveChanges();

                //InvitacionPedido pedCompletado = MiBD.InvitacionPedido.FirstOrDefault(i => i.IdPedido == item.IdPedido );
                //InvitacionPedido pedCompleto = MiBD.InvitacionPedido.Where(i => i.IdPedido == item.IdPedido && i.IdUsuario == item.IdUsuario).FirstOrDefault();
                InvitacionPedido pedCompleto = MiBD.InvitacionPedido.FirstOrDefault(y => y.Token == datos.Token);
                pedCompleto.Completado = true;
                MiBD.SaveChanges();
            }

        }

        public List<Usuario> ObtenerGustosConfirmados(int idPedido)
        {
            //List<InvitacionPedido> invitacionesDelUsuario = Contexto.InvitacionPedido.Include("Pedido")
            //  .Where(o => o.IdPedido == idPedido).ToList();
            List<Usuario> usuarios = MiBD.Usuario.ToList();
            //int i = 0;
            //foreach (InvitacionPedido item in invitacionesDelUsuario)
            //{
            //    if (usuarios[i].IdUsuario != item.IdUsuario)
            //    {
            //        usuarios.RemoveAt(i);
            //        i++;
            //    }
            //}
            return usuarios;
        }

        public InvitacionPedido Crear(Pedido pedido, int idUsuario)
        {
            InvitacionPedido nuevaInvitacion = new InvitacionPedido();
            Pedido p = MiBD.Pedido.Find(pedido.IdPedido);
            nuevaInvitacion.IdPedido = p.IdPedido;
            nuevaInvitacion.IdUsuario = idUsuario;
            nuevaInvitacion.Completado = false;
            nuevaInvitacion.Token = Guid.NewGuid();
            MiBD.InvitacionPedido.Add(nuevaInvitacion);
            MiBD.SaveChanges();
            return nuevaInvitacion;
        }
    }

}
