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
        private Entities MiBD = new Entities();



        public ActionResult Listar()
        {
            //para listar segun quien se logueo
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                ViewBag.ListaPedidos = servicioPedido.ObtenerPedidosByUsuario(usuarioLogueado);
                return View(usuarioLogueado);
            }

            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");
        }

   //*****// falta que agregue invitados por token
        [HttpGet]
        public ActionResult Iniciar()
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
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
            Session["RedireccionLogin"] = "Pedidos/Iniciar";
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            servicioPedido.Agregar(p);// tal vez a futuro haya que mandare por parametro el usuariologueado para k no lo incluya en la lista de invitados
            return RedirectToAction("Iniciado", new { id = p.IdPedido });
        }


     //*****//no muestra el nombre del negocio
        public ActionResult Iniciado(Pedido p)
        {
            return View(p);
        }

        public ActionResult Detalle(int id)
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
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
        //*****// no me modifica los gustos  
        // GET: Editar
        public ActionResult Editar(int id)
        {

            if (MiBD.Pedido.Find(id).IdEstadoPedido == 1)
            {
                ViewBag.ListaUsuario = servicioUsuario.ObtenerTodosLosUsuarios();
                ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
                return View(MiBD.Pedido.Find(id));
            }
            else
            { //si el estado es cerrado no deja editar y va a detalle
                return RedirectToAction("Detalle", "Pedidos");
            }
        }
        //*** no edita los gustos**//
        [HttpPost]
        public ActionResult Editar(Pedido pedido, string btnConfirmar)
        {
            if (ModelState.IsValid)
            {
                if (btnConfirmar == "confirmar")
                {
                    servicioPedido.cerrarPedido(pedido);
                }
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
            Pedido p = servicioPedido.ObtenerPorId(id);
            ViewBag.ListaGustos = servicioGustos.listarGustosConCantidad(id);
            InvitacionPedidoGustoEmpanadaUsuario i = servicioGustos.ObtenerInvitacionPedidoUsuarioGustoPorIdPedido(id);
            i.Pedido.IdPedido = p.IdPedido;
            return View(i);
        }

        [HttpPost]
        public ActionResult Elegir(InvitacionPedidoGustoEmpanadaUsuario i)
        {
            List<InvitacionPedidoGustoEmpanadaUsuario> lista = servicioGustos.listarGustosConCantidad(i.IdPedido);
            return View("ListaFiltrada", lista);
        }


    }
}