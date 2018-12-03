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
        private InvitacionPedidoServicio srvInvitacion = new InvitacionPedidoServicio();

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
            }

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




        //Envio de mail cuando se inicia el pedido
        public void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
            Usuario usuario = MiBD.Usuario.Find(invitacion.IdUsuario);
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(usuario.Email));
            msg.From = new MailAddress("empanadas.ya18@gmail.com");
            msg.Subject = "Bienvenido a Empanadas YA!";
            //msg.Body = "Recibiste una invitacion de" + invitacion.Usuario.Email + "Invitacion: " + HttpContext.Current.Request.Url.Authority + " /Pedidos/Elegir/" + invitacion.Token;
            //msg.Body = "Recibiste una invitacion para elegir gustos..... Ingresa aqui--> " + HttpContext.Current.Request.Url.Authority + " /Pedidos/Elegir/" + invitacion.IdPedido;
            msg.Body = "Recibiste una invitacion para elegir gustos del pedido: " + invitacion.IdPedido + " Ingresa aqui--> " + HttpContext.Current.Request.Url.Authority + "/Pedidos/Elegir/" + invitacion.IdPedido;
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


        public void CerrarPedido(Pedido pedido)
        {
            Pedido p = MiBD.Pedido.Find(pedido.IdPedido);
            p.FechaModificacion = DateTime.Now;
            p.IdEstadoPedido = 2;
            MiBD.SaveChanges();

            foreach (var idUsuario in pedido.IdUsuariosInvitados)
            {
                var invitacion = MiBD.InvitacionPedido.Where(m => m.IdPedido == p.IdPedido)
                                                    .Where(m => m.IdUsuario == idUsuario)
                                                    .First();
                EnviarMailCerrado(invitacion, pedido);
            }

        }

        public void Modificar(Pedido j)
        {
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
            // MiBD.Pedido.FirstOrDefault(p => p.IdPedido == id).GustoEmpanada.ToList();
            /*
            InvitacionPedido nuevaInvitacionPedido = MiBD.InvitacionPedido.FirstOrDefault(x => x.IdPedido == j.IdPedido);
            foreach ( var invitadoNuevo in j.IdUsuariosInvitados)
            {
                if (nuevaInvitacionPedido.IdUsuario != invitadoNuevo)
                {
                    InvitacionPedido newInvitado = new InvitacionPedido();
                    newInvitado.IdUsuario = invitadoNuevo;
                    newInvitado.Token = Guid.NewGuid();
                    newInvitado.IdPedido = j.IdPedido;
                    newInvitado.Completado = false;
                    p.InvitacionPedido.Add(newInvitado);
                    MiBD.SaveChanges();
                }

              
            } */
            //p.InvitacionPedido.Clear();
            
            
         
                MiBD.SaveChanges();
        }


        public void EnviarInvitaciones(Pedido pedido, string ReEnviarInvitacion)
        {
            switch (ReEnviarInvitacion)
            {
                case "1":
                    foreach (var item in MiBD.InvitacionPedido.Where(m => m.IdPedido == pedido.IdPedido))
                    {
                        EnviarEmailInvitados(item);
                    }
                    break;
                case "2":
                    List<int> lista = new List<int>();
                    foreach (var item in pedido.IdUsuariosInvitados)
                    {
                        if (!MiBD.InvitacionPedido.Where(x => x.IdPedido == pedido.IdPedido)
                                                     .Select(x => x.IdUsuario).Contains(item))
                        {
                            lista.Add(item);
                        }
                    }
                    if (lista.Count() > 0)
                    {
                        foreach (var item in lista)
                        {
                            EnviarEmailInvitados(srvInvitacion.Crear(pedido, item));
                        }
                    }
                    break;
                case "3":
                    foreach (var item in MiBD.InvitacionPedido.Where(m => m.IdPedido == pedido.IdPedido).Where(m => m.Completado == false))
                    {
                        EnviarEmailInvitados(item);
                    }
                    break;
            }
        }

        public void EnviarMailCerrado(InvitacionPedido invitacionPedido, Pedido pedido)
        {
            Usuario usuario = MiBD.Usuario.Find(invitacionPedido.IdUsuario);

            int cantidadTotal = invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.Sum(m => m.Cantidad);
            int docenasTotales = cantidadTotal / 12;
            int resto = cantidadTotal - (docenasTotales * 12);
            int TotalPorDocenas = docenasTotales * invitacionPedido.Pedido.PrecioDocena;
            int precioResto = resto * invitacionPedido.Pedido.PrecioUnidad;
            int Total = TotalPorDocenas + precioResto;

            List<String> usuarioPrecioPorAbonar = new List<String>();

            foreach (var item in pedido.IdUsuariosInvitados)
            {
                Usuario user = MiBD.Usuario.Find(item);
                int cantidadTotalesPorUsuario = user.InvitacionPedidoGustoEmpanadaUsuario
                                                            .Where(m => m.IdUsuario == item)
                                                            .Where(m => m.IdPedido == invitacionPedido.IdPedido)
                                                            .Sum(m => m.Cantidad);
                int docenasTotalesPorUsuario = cantidadTotalesPorUsuario / 12;
                int restoPorUsuario = cantidadTotalesPorUsuario - (docenasTotalesPorUsuario * 12);
                int RestoPorUsuario = restoPorUsuario * invitacionPedido.Pedido.PrecioUnidad;
                int TotalPorDocenasDeUsuario = docenasTotalesPorUsuario * invitacionPedido.Pedido.PrecioDocena;
                int TotalPorUsuario = TotalPorDocenasDeUsuario + RestoPorUsuario;
                usuarioPrecioPorAbonar.Add("Invitado: " + user.Email + " Precio a abonar: $" + Convert.ToString(RestoPorUsuario));
            }

            List<string> detalle = new List<string>();

            var newlist = invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.GroupBy(d => d.IdGustoEmpanada)
            .Select(g => new {  Key = g.Key,
                                Value = g.Sum(s => s.Cantidad),
                                Category = g.First().GustoEmpanada,
                                Name = g.First().GustoEmpanada.Nombre});

            foreach (var item in newlist.ToList())
            {
                detalle.Add(item.Name + ": " + item.Value);
            }

            var fromAddress = new MailAddress("empanadas.ya18@gmail.com", "From Name");
            var toAddress = new MailAddress("empanadas.ya18@gmail.com", "To Name");
            string fromPassword = "empanadas2018";
            string subject = "Subject";
            string body = "";
            //mail para el responsable
            if (invitacionPedido.IdUsuario == invitacionPedido.Pedido.IdUsuarioResponsable)
            {
                body = "<h1>Empanadas Ya</h1>Precio Total:</b> $" + Total + "<br><b>Invitados:</b><br> " +
                    String.Join(",<br>", usuarioPrecioPorAbonar.ToArray()) + "<br><b>Detalle:</b><br>" + String.Join(",<br>", detalle.ToArray()) +
                    "<br><b>Total de empanadas: </b>" + cantidadTotal;
            }
            else
            {//mail para el resto
                List<string> datosInvitados = new List<string>();
                foreach (var item in invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.Where(m => m.IdUsuario == invitacionPedido.IdUsuario))
                {
                    GustoEmpanada empanadas = MiBD.GustoEmpanada.Find(item.IdGustoEmpanada);
                    datosInvitados.Add("Gusto: " + empanadas.Nombre + ", Cantidad: " + item.Cantidad);
                }
                body = "<h1>Empanadas Ya</h1>Total de empanadas del pedido: " + cantidadTotal + "<br>" +
                    String.Join(",<br>", datosInvitados.ToArray()) + "<br>Precio Total: $" + Total + "</b>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}