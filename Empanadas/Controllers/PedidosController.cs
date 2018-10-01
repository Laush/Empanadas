using Empanadas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class PedidosController : Controller
    {

        PedidoServicio sp = new PedidoServicio();
        // GET: Pedidos

        public ActionResult Listar()
        {
            List<Pedido> pedidosList = sp.Listar();
            return View(pedidosList);
        }

        // metodo que agrega pedido desde cero
        public ActionResult Iniciar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            sp.Agregar(p);

            return RedirectToAction("Listar", "Pedidos");

        }
    }
}