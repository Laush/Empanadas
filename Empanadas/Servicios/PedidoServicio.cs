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
            // tenesmo que tener un invitacion pedido con los mails seleccionados 

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
        //no elimina =(
        public void Eliminar(int id)
        {
            Pedido pedEl = MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id);
            MiBD.Pedido.Remove(pedEl);
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

    }
}