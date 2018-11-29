using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoServicio
    {
        private Entities MiBD = new Entities();
        private GustoEmpanadaServicio srvGustoEmpanda = new GustoEmpanadaServicio();
        private UsuarioServicio srvUsuario = new UsuarioServicio();

        public void Agregar(Pedido p, Usuario idUsuarioReponsable)
        {
            Pedido nuevoPedido = p;
            nuevoPedido.IdUsuarioResponsable = idUsuarioReponsable.IdUsuario;
            nuevoPedido.IdEstadoPedido = 1;//al crearlos estado uno
            nuevoPedido.FechaCreacion = DateTime.Now;
            MiBD.Pedido.Add(nuevoPedido);
            MiBD.SaveChanges();

            //ADD GustoEmpanada AL PEDIDO (GustoEmpanadaDisponiblePedido)
            foreach (var idGusto in nuevoPedido.IdGustosSeleccionados)
            {
                GustoEmpanada gustoEmpanadaDisponible = MiBD.GustoEmpanada.Find(idGusto);
                nuevoPedido.GustoEmpanada.Add(gustoEmpanadaDisponible);
                MiBD.SaveChanges();
            }
            //verifico q seleccione usuarios
            if (p.IdUsuariosInvitados != null)
            {
                foreach (var idUsuario in nuevoPedido.IdUsuariosInvitados)
                {
                    InvitacionPedido nuevaInvitacionPedido = new InvitacionPedido();
                    nuevaInvitacionPedido.IdPedido = nuevoPedido.IdPedido;
                    nuevaInvitacionPedido.Completado = false;
                    nuevaInvitacionPedido.Token = Guid.NewGuid();
                    nuevaInvitacionPedido.IdUsuario = idUsuario;
                    MiBD.InvitacionPedido.Add(nuevaInvitacionPedido);
                    MiBD.SaveChanges();
                    EnviarEmailInvitados(nuevaInvitacionPedido);
                }
            }//end if

            //AGREGO AL USUARIO RESPONSABLE EN LA INVITACION PEDIDO
            InvitacionPedido invitacionPedidoDelUsuarioResponsable = new InvitacionPedido();
            invitacionPedidoDelUsuarioResponsable.IdPedido = nuevoPedido.IdPedido;
            invitacionPedidoDelUsuarioResponsable.Completado = false;
            invitacionPedidoDelUsuarioResponsable.Token = Guid.NewGuid();
            invitacionPedidoDelUsuarioResponsable.IdUsuario = idUsuarioReponsable.IdUsuario;
            MiBD.InvitacionPedido.Add(invitacionPedidoDelUsuarioResponsable);
            MiBD.SaveChanges();
            EnviarEmailInvitados(invitacionPedidoDelUsuarioResponsable);
        }

        public void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
            Usuario usuario = MiBD.Usuario.Find(invitacion.IdUsuario);
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(usuario.Email));
            msg.From = new MailAddress("empanadas.ya18@gmail.com");
            msg.Subject = "Bienvenido a Empanadas YA!";
            //   msg.Body = "Recibiste una invitacion de" + invitacion.Usuario.Email + "Invitacion: " + HttpContext.Current.Request.Url.Authority + " /Pedidos/Elegir/" + invitacion.Token;
            msg.Body = "Recibiste una invitacion para elegir gustos..... Ingresa aqui--> " + HttpContext.Current.Request.Url.Authority + " /Pedidos/Elegir/" + invitacion.IdPedido;
            SmtpClient clienteSmtp = new SmtpClient();
            clienteSmtp.Host = "smtp.gmail.com";
            clienteSmtp.Port = 587;
            clienteSmtp.UseDefaultCredentials = false;
            clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            clienteSmtp.Credentials = new System.Net.NetworkCredential("empanadas.ya18@gmail.com", "empanadas2018");
            clienteSmtp.EnableSsl = true;
            clienteSmtp.Send(msg);

        }

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

        //obtener pedidos
        public Pedido ObtenerPorId(int id)
        {
            return MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id);
        }
        //obtener usuarios
        public List<Pedido> ObtenerPedidosByUsuario(Usuario usu)
        {
            // return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable == idUsuario).OrderByDescending(x => x.FechaCreacion).ToList();
            // return MiBD.Pedido.Include("GustoEmpanada").Where(x => x.IdUsuarioResponsable.Equals(usu.IdUsuario)).OrderByDescending(p => p.FechaCreacion).ToList();
            /* var query =
                (from p in MiBD.Pedido
                 join ip in MiBD.InvitacionPedido on p.IdPedido equals ip.IdPedido
                 where p.IdUsuarioResponsable == usu.IdUsuario || ip.IdUsuario == usu.IdUsuario
                 orderby p.FechaCreacion descending
                 select p).Distinct().ToList();
             return query;*/

            //otra version

            List<Pedido> pedidosResultado = new List<Pedido>();
            List<InvitacionPedido> imvitacionesDelUsuario = MiBD.InvitacionPedido.Include("Pedido")
                          .Where(o => o.IdUsuario.Equals(usu.IdUsuario)).Distinct().ToList();
            foreach (var inv in imvitacionesDelUsuario)
            {
                pedidosResultado.Add(inv.Pedido);
            }
            return pedidosResultado.OrderByDescending(p => p.FechaCreacion).ToList();

        }


        public Usuario ObtenerUsuarioPorId(int id)
        {
            return MiBD.Usuario.FirstOrDefault(p => p.IdUsuario == id);
        }

        public List<Usuario> ObtenerUsuarios(Usuario u)
        {
            return MiBD.Usuario.Where(m => m.IdUsuario != u.IdUsuario).ToList();
        }

        //obtener de gustos
        public List<GustoEmpanada> ObtenerGustosDeEmpanada()
        {
            return MiBD.GustoEmpanada.ToList();
        }

        public List<GustoEmpanada> ObtenerGustosPorPedido(int id)
        {
            return MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id).GustoEmpanada.ToList();
        }
        //obtener de invitaciones
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

        // consultar como modificar pedidos ahora que no guardo en invitacionPedidoGustoEmpa
        public void Modificar(Pedido j)
        {
            //Pedido p = MiBD.Pedido.Include("InvitacionPedido").First(pedido => pedido.IdPedido == j.IdPedido);
            Pedido p = MiBD.Pedido.Find(j.IdPedido);
            p.NombreNegocio = j.NombreNegocio;
            p.Descripcion = j.Descripcion;
            p.PrecioUnidad = j.PrecioUnidad;
            p.PrecioDocena = j.PrecioDocena;
            p.FechaModificacion = DateTime.Now;
            foreach (var idGusto in j.IdGustosSeleccionados)
            {
                GustoEmpanada gustoEmpanadaDisponible = MiBD.GustoEmpanada.Find(idGusto);
                p.GustoEmpanada.Add(gustoEmpanadaDisponible);
                MiBD.SaveChanges();
            }


            foreach (var idIn in j.IdUsuariosInvitados)
            {

                Usuario u = srvUsuario.ObtenerPorId(idIn);
                InvitacionPedido i = srvGustoEmpanda.ObtenerInvitacionPorPedido(j.IdPedido);
                p.InvitacionPedido.Add(i);
            }

            MiBD.SaveChanges();
        }

    }
}