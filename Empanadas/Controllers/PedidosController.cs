using Empanadas.Models;
using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class PedidosController : Controller
    {

        PedidoServicio servicioPedido = new PedidoServicio();
        UsuarioServicio servicioUsuario = new UsuarioServicio();
        GustoEmpanadaServicio servicioGustos = new GustoEmpanadaServicio();
        InvitacionPedidoServicio servicioInvitacion = new InvitacionPedidoServicio();
        InvitacionPedidoGustoEmpanadaUsuarioServicio servicioInvPedGusUsu = new InvitacionPedidoGustoEmpanadaUsuarioServicio();

        private Entities MiBD = new Entities();



        public ActionResult Listar()
        {
            //para listar segun quien se logueo
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                ViewBag.ListaPedidos = servicioPedido.ObtenerPedidosByUsuario(usuarioLogueado);
                //               ViewBag.SaberQuienEligioGusto = servicioInvPedGusUsu.saberSiSeEligioGusto();
                return View(usuarioLogueado);
            }

            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult Iniciar(int? id)//para copiar pedidos
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                DateTime fecha = DateTime.Now;
                Pedido pedido = new Pedido();
                pedido.IdUsuarioResponsable = usuarioLogueado.IdUsuario;
                pedido.PrecioUnidad = 30;
                pedido.PrecioDocena = 280;
                pedido.IdEstadoPedido = 1;
                pedido.FechaCreacion = fecha;
                pedido.FechaModificacion = fecha;

                ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
                ViewBag.ListaUsuario = servicioUsuario.ObtenerTodosLosUsuarios();
                //   ViewBag.listadoDeUsuarios = new MultiSelectList(MiBD.Usuario.Where(m => m.IdUsuario != usuarioLogueado.IdUsuario).ToList(), "IdUsuario", "Email");

                //  InvitacionPedido invitacionPedidoDelUsuarioResponsable = new InvitacionPedido();
                //  invitacionPedidoDelUsuarioResponsable.IdUsuario = usuarioLogueado.IdUsuario;

                if (id != null)
                {
                    return View(MiBD.Pedido.Find(id));
                }

                return View(pedido);
            }
            Session["RedireccionLogin"] = "Pedidos/Iniciar";
            return RedirectToAction("Login", "Home");

        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            if (ModelState.IsValid)
            {
                var usuarioLogueado = Session["Usuario"] as Usuario;
                servicioPedido.Agregar(p, usuarioLogueado);
                return RedirectToAction("Iniciado",p);
            }
            else
            {
                return View(p);
            }
        }



        public ActionResult Iniciado(Pedido p)
        {

            return View(p);
        }

        public ActionResult Detalle(int id)
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                List<GustoEmpanada> InitGustos = servicioPedido.ObtenerGustosPorPedido(id);
                ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");

                List<Usuario> InitInvitados = servicioUsuario.ObtenerUsuariosPorPedido(id);
                ViewBag.ListaInvitados = new MultiSelectList(InitInvitados, "IdUsuario", "Email");

                return View(servicioPedido.ObtenerPorId(id));
            }
            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");

        }


        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                ViewBag.Cantidad = servicioPedido.ObtenerInvitacionesConfirmadas(id);
                return View(servicioPedido.ObtenerPorId(id));
            }
            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Eliminar(Pedido p)
        {
            TempData["mensaje"] = "Pedido " + servicioPedido.ObtenerPorId(p.IdPedido).NombreNegocio + " ha sido eliminado exitosamente";
            servicioPedido.Eliminar(p.IdPedido);
            return RedirectToAction("Listar", "Pedidos");

        }

        // GET: Editar
        public ActionResult Editar(int id)
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            Pedido pedido = servicioPedido.ObtenerPorId(id);

            List<GustoEmpanada> InitGustos = servicioPedido.ObtenerGustosDeEmpanada();

            foreach (GustoEmpanada item in pedido.GustoEmpanada)
            {
                InitGustos.Remove(item);
            }

            List<Usuario> mails = servicioUsuario.ObtenerMailsUsuarios();
            List<Usuario> mailsNuevos = new List<Usuario>();

            for (int i = 0; i < mails.Count; i++)
            {
                foreach (InvitacionPedido item in pedido.InvitacionPedido)
                {
                    if (mails[i].IdUsuario == item.IdUsuario && item.IdUsuario != usuarioLogueado.IdUsuario)
                    {
                        mailsNuevos.Add(mails[i]);
                        mails.Remove(mails[i]);
                        break;
                    }
                }
            }
            ViewBag.Confirmados = new MultiSelectList(servicioUsuario.ObtenerUsuariosPorPedidoQueConfirmaron(id), "IdUsuario", "Email");
            ViewBag.usuariosCompletaronPedido = new MultiSelectList(servicioUsuario.UsuariosCompletaronPedido(id), "IdUsuario", "Email");
            ViewBag.usuariosQueNoCompletaronPedido = new MultiSelectList(servicioUsuario.UsuariosQueNoCompletaronPedido(id), "IdUsuario", "Email");
            ViewBag.usuariosQueNoTienenInvitacion = new MultiSelectList(servicioUsuario.usuariosQueNoTienenInvitacion(id), "IdUsuario", "Email");

            ViewBag.invitados= new MultiSelectList(servicioUsuario.ObtenerUsuariosPorPedido(id), "IdUsuario", "Email"); 
            ViewBag.ListaGustos = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");
            ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

            return View(pedido);

        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido, string btnConfirmar, string btnCancelar, string EnviarInvitaciones)
        {
            if (ModelState.IsValid)
            {

                if (btnCancelar == "Cancelar")
                {
                    return RedirectToAction("Listar");
                }

                if (btnConfirmar == "Confirmar")
                {
                    servicioPedido.CerrarPedido(pedido);
                }
                servicioPedido.EnviarInvitaciones(pedido, EnviarInvitaciones);
                servicioPedido.Modificar(pedido);
                return RedirectToAction("Listar");
            }
            else
            {
                return View(pedido);
            }
        }


        [HttpGet]
        public ActionResult Elegir(int id)
        {

            var usuarioLogueado = Session["Usuario"] as Usuario;
            ViewBag.UsuarioLog = usuarioLogueado;
            Pedido p = servicioPedido.ObtenerPorId(id);
            InvitacionPedido token = servicioInvitacion.GetInvitacionPedidoPorPedido(id, usuarioLogueado.IdUsuario);
            ViewBag.Token = token.Token;
            List<GustoEmpanada> InitGustos = servicioPedido.ObtenerGustosPorPedido(id);
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            return View(p);
        }




    }
}