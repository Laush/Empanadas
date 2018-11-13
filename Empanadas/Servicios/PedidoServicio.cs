using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoServicio
    {
        private Entities MiBD = new Entities();

        public void Agregar(Pedido p)
        {
            p.FechaCreacion = DateTime.Now;
            foreach (int gId in p.IdGustosSeleccionados)
            {
                GustoEmpanada gEmpanadaDisponible = MiBD.GustoEmpanada.FirstOrDefault(o => o.IdGustoEmpanada == gId);
                p.GustoEmpanada.Add(gEmpanadaDisponible);
            }
            // tenemos que tener un invitacion pedido con los mails seleccionados
            // InvitacionPedido invPedido = MiBD.InvitacionPedido.FirstOrDefault(o => o.IdInvitacionPedido == p.InvitacionPedido);

            MiBD.Pedido.Add(p);
            MiBD.SaveChanges();
        }

        public List<Pedido> Listar()
        {
            return MiBD.Pedido.ToList();
        }

        public List<Pedido> GetPedidosByUsuario(int idUsuario)
        {
            return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable == idUsuario).OrderByDescending(x => x.FechaCreacion).ToList();
        }

        public void Eliminar(int id)
        {
            // var invitaciones = Context.InvitacionPedido.Where(i => i.IdPedido == id).ToList();
            // Context.InvitacionPedido.RemoveRange(invitaciones);
            // Context.SaveChanges();

            // var gustosPedido = Context.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.IdPedido == id).ToList();
            // Context.InvitacionPedidoGustoEmpanadaUsuario.RemoveRange(gustosPedido);
            // Context.SaveChanges();

            Pedido ped = MiBD.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
            ped.GustoEmpanada.Clear();
            MiBD.Pedido.Remove(ped);
            MiBD.SaveChanges();
        }

        public Pedido ObtenerPorId(int id)
        {
            return MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id);
        }

        public List<GustoEmpanada> ObtenerGustosDeEmpanada()
        {
            return MiBD.GustoEmpanada.ToList();
        }

        public List<Usuario> ObtenerUsuarios(Usuario u)
        {
            return MiBD.Usuario.Where(m => m.IdUsuario != u.IdUsuario).ToList();
        }

        public int ObtenerInvitacionesConfirmadas(int id)
        {
            return MiBD.InvitacionPedido.Where(c => c.IdPedido == id)
                .Where(c => c.Completado == true).Count();
        }
    }
}