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

            foreach (int invitadoId in p.IdUsuariosInvitados)
            {
                InvitacionPedido iDisponibe = MiBD.InvitacionPedido.FirstOrDefault(x => x.IdUsuario == invitadoId);
                p.InvitacionPedido.Add(iDisponibe);
            }
            MiBD.Pedido.Add(p);
            MiBD.SaveChanges();
        }

        public List<Pedido> Listar()
        {
            return MiBD.Pedido.ToList();
        }

        public List<Pedido> ObtenerPedidosByUsuario(Usuario usu)
        {
            //Asi estaba antes// return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable == idUsuario).OrderByDescending(x => x.FechaCreacion).ToList();

            //con esta consulta me muestra todos(los k no tienen mail tmb)
            //pero no marca bien el rol de invitado 
            // return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable.Equals(usu.IdUsuario)).OrderByDescending(p => p.FechaCreacion).ToList();

            ///con esta consulta me muestra solo los que tienen invitados(mails cargados)
            ////ATENCION: si no selecciono mi propio mail no aparece en mi lista
            var query =
               (from p in MiBD.Pedido
                join ep in MiBD.EstadoPedido on p.IdEstadoPedido equals ep.IdEstadoPedido
                join ip in MiBD.InvitacionPedido on p.IdPedido equals ip.IdPedido
                 where ip.IdUsuario == usu.IdUsuario 
                orderby p.FechaCreacion descending
                select
                    p).ToList();
             return query;
        }

        public Boolean PedidoUsuarioResponsableIsTrue(int idPedido, Usuario usuario)
        {
            var query = (from p in MiBD.Pedido
                         where p.IdUsuarioResponsable == usuario.IdUsuario &&
                                p.IdPedido == idPedido
                         select p).ToList();

            if (query.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void Eliminar(int id)
        {
           //falta eliminar las invitaciones
            Pedido ped = MiBD.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
            ped.GustoEmpanada.Clear();
            MiBD.Pedido.Remove(ped);
            MiBD.SaveChanges();
        }

        public Pedido ObtenerPorId(int id)
        {
            return MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id);
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            return MiBD.Usuario.FirstOrDefault(p => p.IdUsuario == id);
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

        //CAMBIO DE ESTADO DE UN PEDIDO
        public void cerrarPedido(Pedido pedido)
        {
            Pedido p = MiBD.Pedido.Find(pedido.IdPedido);
            p.FechaModificacion = DateTime.Now;
            p.IdEstadoPedido = 2;
            MiBD.SaveChanges();

        }

        public void Modificar(Pedido j)
        {
            Pedido p = MiBD.Pedido.Find(j.IdPedido);
            p.NombreNegocio = j.NombreNegocio;
            p.Descripcion = j.Descripcion;
            p.PrecioUnidad = j.PrecioUnidad;
            p.PrecioDocena = j.PrecioDocena;
            p.FechaModificacion = DateTime.Now;
            foreach (var idGusto in j.GustoEmpanada)
            {
                GustoEmpanada gustoEmpanadaDisponible = MiBD.GustoEmpanada.Find(idGusto);
                p.GustoEmpanada.Add(gustoEmpanadaDisponible);
            }
            MiBD.SaveChanges();
        }

    }
}