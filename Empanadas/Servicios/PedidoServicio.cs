using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoServicio
    {
        private Entities MiBD = new Entities();
        private GustoEmpanadaServicio srvGustoEmpanda = new GustoEmpanadaServicio();

        public void Agregar(Pedido p)
        {

            MiBD.Pedido.Add(p);
            MiBD.SaveChanges();

            foreach (int gId in p.IdGustosSeleccionados)
            {
                GustoEmpanada gEmpanadaDisponible = MiBD.GustoEmpanada.FirstOrDefault(o => o.IdGustoEmpanada == gId);
                p.GustoEmpanada.Add(gEmpanadaDisponible);
                MiBD.SaveChanges();
                
                // agregamos el pedido en InvitacionPedidoGustoEmpanadaUsuario
                InvitacionPedidoGustoEmpanadaUsuario InvitacionCompleta = new InvitacionPedidoGustoEmpanadaUsuario();
                InvitacionCompleta.IdPedido = p.IdPedido;
                InvitacionCompleta.IdGustoEmpanada = srvGustoEmpanda.ObtenerPorId(gId).IdGustoEmpanada;
                InvitacionCompleta.IdUsuario = p.IdUsuarioResponsable;
                InvitacionCompleta.Cantidad = 0;

                MiBD.InvitacionPedidoGustoEmpanadaUsuario.Add(InvitacionCompleta);
                MiBD.SaveChanges();
            }
            
            foreach (int invitadoId in p.IdUsuariosInvitados)
            {
                InvitacionPedido InPedido = new InvitacionPedido();
                InPedido.IdPedido = p.IdPedido;
                InPedido.IdUsuario = invitadoId;
                InPedido.Token = Guid.NewGuid();
                InPedido.Completado = false;
                MiBD.InvitacionPedido.Add(InPedido);
                MiBD.SaveChanges();

            }

            
            

            MiBD.SaveChanges();
        }

        public List<Pedido> Listar()
        {
            return MiBD.Pedido.ToList();
        }

        public List<InvitacionPedido> ObtenerPedidosByUsuario(Usuario usu)
        {
            // return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable == idUsuario).OrderByDescending(x => x.FechaCreacion).ToList();

            // return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable.Equals(usu.IdUsuario)).OrderByDescending(p => p.FechaCreacion).ToList();
            var query =
               (from p in MiBD.Pedido
                    //  join ep in MiBD.EstadoPedido on p.IdEstadoPedido equals ep.IdEstadoPedido
                join ip in MiBD.InvitacionPedido on p.IdPedido equals ip.IdPedido
                where p.IdUsuarioResponsable == usu.IdUsuario
                || ip.IdUsuario == usu.IdUsuario
                orderby p.FechaCreacion descending
                select
                    ip).ToList();
            return query;


        }

        /*   public Boolean PedidoUsuarioResponsableIsTrue(int idPedido, Usuario usuario)
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
           }*/

        public void Eliminar(int id)
        {
            var invitaciones = MiBD.InvitacionPedido.Where(i => i.IdPedido == id).ToList();
            MiBD.InvitacionPedido.RemoveRange(invitaciones);
            MiBD.SaveChanges();

            var gustosPedido = MiBD.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.IdPedido == id).ToList();
            MiBD.InvitacionPedidoGustoEmpanadaUsuario.RemoveRange(gustosPedido);
            MiBD.SaveChanges();

            Pedido pedidoEliminar = MiBD.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);

            pedidoEliminar.GustoEmpanada.Clear();

            MiBD.Pedido.Remove(pedidoEliminar);
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