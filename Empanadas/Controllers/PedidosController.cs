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
            List<Pedido> pedidosList = servicioPedido.Listar();
            return View(pedidosList);
        }

        // metodo que agrega pedido desde cero
        public ActionResult Iniciar()
        {
            int IdUsuarioLogin = servicioUsuario.devolverIdUsuario;
            Pedido pedido = new Pedido();
            pedido.IdUsuarioResponsable = IdUsuarioLogin;

            return View(pedido);
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            servicioPedido.Agregar(p);

            return RedirectToAction("Listar", "Pedidos");

        }
    }
}