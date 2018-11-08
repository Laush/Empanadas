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
        private Entities MiBD = new Entities();

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

        // falta que agregue invitados por token
        [HttpGet]
        public ActionResult Iniciar()
        {
            /*  int IdUsuarioLogin = servicioUsuario.devolverIdUsuario;
              Pedido pedido = new Pedido();
              pedido.IdUsuarioResponsable = IdUsuarioLogin;
              ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
              return View(pedido);*/

            int idUsuarioReponsable = Convert.ToInt32(Session["usuarioID"]);
            ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
            ViewBag.listadoDeUsuarios = new MultiSelectList(MiBD.Usuario.Where(m => m.IdUsuario != idUsuarioReponsable).ToList(), "IdUsuario", "Email");

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
            return RedirectToAction("Listar", "Pedidos");
        }
    }
}