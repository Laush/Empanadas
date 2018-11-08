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
        PedidoServicio servicioPedido = new PedidoServicio();
        UsuarioServicio servicioUsuario = new UsuarioServicio();

        public ActionResult Listar()
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                return View(servicioPedido.GetPedidosByUsuario(usuarioLogueado.IdUsuario));
            }
            Session["RedireccionLogin"] = "Pedidos/Listar";
            return RedirectToAction("Login", "Home");

            //asi estaba antes..traia toda a lista
            //  List<Pedido> pedidosList = servicioPedido.Listar();
            //  return View(pedidosList);
        }

        // metodo que agrega pedido desde cero// falta continuarlo...
        [HttpGet]
        public ActionResult Iniciar()
        {
            /*  int IdUsuarioLogin = servicioUsuario.devolverIdUsuario;
              Pedido pedido = new Pedido();
              pedido.IdUsuarioResponsable = IdUsuarioLogin;

              ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
              return View(pedido);*/
            ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
            return View();
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            servicioPedido.Agregar(p);

            return RedirectToAction("Listar", "Pedidos");

        }


        public ActionResult Eliminar(int id)
        {
            var sp = new PedidoServicio();
            sp.Eliminar(id);

          //  servicioPedido.Eliminar(p.IdPedido);
            return RedirectToAction("Listar", "Pedidos");
        }
    }
}