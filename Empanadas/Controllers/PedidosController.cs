﻿using Empanadas.Models;
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
        
            var usuarioLogueado = Session["Usuario"] as Usuario;
            DateTime fecha = DateTime.Now;
            Pedido pedido = new Pedido();

            pedido.IdUsuarioResponsable = usuarioLogueado.IdUsuario;
            pedido.IdEstadoPedido = 1; 
            pedido.FechaCreacion = fecha;

            int idUsuarioReponsable = Convert.ToInt32(Session["usuarioID"]);
            ViewBag.ListaGusto = servicioPedido.ObtenerGustosDeEmpanada();
            ViewBag.ListaUsuario = servicioUsuario.ObtenerTodosLosUsuarios();
            ViewBag.listadoDeUsuarios = new MultiSelectList(MiBD.Usuario.Where(m => m.IdUsuario != idUsuarioReponsable).ToList(), "IdUsuario", "Email");

            return View(pedido);
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