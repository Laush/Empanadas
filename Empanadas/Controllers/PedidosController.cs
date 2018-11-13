using Empanadas.Models;
using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class PedidosController : Controller
    {
        public const int ESTADO_ABIERTO = 1;

        PedidoServicio servicioPedido = new PedidoServicio();
        UsuarioServicio servicioUsuario = new UsuarioServicio();
        private Entities MiBD = new Entities();


        public ActionResult Listar()
        {
            //para listar segun quien se logueo
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                return View(servicioPedido.GetPedidosByUsuario(usuarioLogueado.IdUsuario));
            }

            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");
        }

        //*****// falta que agregue invitados por token
        [HttpGet]
        public ActionResult Iniciar()
        {
            //int idUsuarioReponsable = Convert.ToInt32(Session["IdUsuario"] as Usuario);
            var usuarioLogueado = Session["Usuario"] as Usuario;
            DateTime fecha = DateTime.Now;
            Pedido pedido = new Pedido();

            pedido.IdUsuarioResponsable = usuarioLogueado.IdUsuario;
            pedido.IdEstadoPedido = 1;
            pedido.PrecioUnidad = 15;
            pedido.PrecioDocena = 200;
            pedido.FechaCreacion = fecha;
            pedido.FechaModificacion = fecha;

            ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
            ViewBag.ListaUsuario = servicioUsuario.ObtenerTodosLosUsuarios();
            ViewBag.listadoDeUsuarios = new MultiSelectList(MiBD.Usuario.Where(m => m.IdUsuario != usuarioLogueado.IdUsuario).ToList(), "IdUsuario", "Email");

            return View(pedido);
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            servicioPedido.Agregar(p);
            return RedirectToAction("Listar", "Pedidos");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            ViewBag.Cantidad = servicioPedido.ObtenerInvitacionesConfirmadas(id);
            return View(servicioPedido.ObtenerPorId(id));
        }

        [HttpPost]
        public ActionResult Eliminar(Pedido p)
        {
            TempData["mensaje"] = "Pedido " + servicioPedido.ObtenerPorId(p.IdPedido).NombreNegocio + " ha sido eliminado exitosamente";
            servicioPedido.Eliminar(p.IdPedido);
            return RedirectToAction("Listar", "Pedidos");
        }
    }
}