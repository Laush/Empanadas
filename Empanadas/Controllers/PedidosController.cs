﻿using Empanadas.Models;
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
        public ActionResult Iniciar(int? id)
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            if (usuarioLogueado != null)
            {
                DateTime fecha = DateTime.Now;
                Pedido pedido = new Pedido();
                pedido.IdUsuarioResponsable = usuarioLogueado.IdUsuario;
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

            /* //esta version anda e inicia bien los pedidos--pero OJO hay k cambiar la vista Iniciar los viewBag
                        var pedido = p;
                        pedido.IdEstadoPedido = 1;
                        pedido.FechaCreacion = DateTime.Now;
                        //gustos
                        List<GustoEmpanada> gustosSeleccionados = new List<GustoEmpanada>();
                        foreach (int gId in p.IdGustosSeleccionados)
                        {
                            gustosSeleccionados.Add(MiBD.GustoEmpanada.FirstOrDefault(ge => ge.IdGustoEmpanada == gId));

                        }
                        pedido.GustoEmpanada = gustosSeleccionados;
                        MiBD.Pedido.Add(pedido);
                        //usuarios invitados
                        if (p.IdUsuariosInvitados != null)
                        {
                            foreach (var id in pedido.IdUsuariosInvitados)
                            {
                                InvitacionPedido invitacion = new InvitacionPedido();
                                invitacion.IdPedido = pedido.IdPedido;
                                invitacion.Completado = true;
                                invitacion.Token = Guid.NewGuid();
                                invitacion.IdUsuario = id;
                                MiBD.InvitacionPedido.Add(invitacion);
                                //EnviarCorreo(invitacion);
                            }*/
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            if (ModelState.IsValid)
            {
                var usuarioLogueado = Session["Usuario"] as Usuario;
                servicioPedido.Agregar(p, usuarioLogueado);
                return RedirectToAction("Iniciado", new { id = p.IdPedido });
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
            // Pedido p = servicioPedido.ObtenerPorId(id);
            //ViewBag.ListaUsuario = servicioUsuario.ObtenerUsuariosPorPedido(p.IdPedido);
            //ViewBag.ListaDeUsuarios = servicioUsuario.ObtenerTodosLosUsuarios();
            //List<GustoEmpanada> InitGustos = servicioPedido.ObtenerGustosPorPedido(id);
            //ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            //ViewBag.ListaDeGustos = servicioPedido.ObtenerGustosDeEmpanada();
            //return View(MiBD.Pedido.Find(id));
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
            //esto es a futuro para mostrar el si o no de lo sk ya confirmaron
            //  List<Usuario> gustosElegidos = servicioInvitacionPedido.ObtenerGustosConfirmados(id);
            //   ViewBag.GustosElegidos = gustosElegidos;
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");
            ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

            return View(pedido);

        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido, string btnConfirmar, string btnCancelar)
        {
            if (ModelState.IsValid)
            {

                if (btnCancelar == "Cancelar")
                {
                    return RedirectToAction("Listar");
                }

                if (btnConfirmar == "Confirmar")
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

            var usuarioLogueado = Session["Usuario"] as Usuario;
            Pedido p = servicioPedido.ObtenerPorId(id);
            InvitacionPedido token = servicioInvitacion.GetInvitacionPedidoPorPedido(id, usuarioLogueado.IdUsuario);
            ViewBag.Token = token.Token;
            List<GustoEmpanada> InitGustos = servicioPedido.ObtenerGustosPorPedido(id);
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            return View(p);

            /* Pedido p = servicioPedido.ObtenerPorId(id);
             ViewBag.ListaGustos = servicioGustos.ListarGustos(id);
             InvitacionPedidoGustoEmpanadaUsuario i = servicioGustos.ObtenerInvitacionPedidoUsuarioGustoPorIdPedido(id);
             i.Pedido.IdPedido = p.IdPedido;
             return View(i);*/
        }


        [HttpPost]
        public ActionResult Elegir(InvitacionPedidoGustoEmpanadaUsuario i)
        {
            List<InvitacionPedidoGustoEmpanadaUsuario> lista = servicioGustos.ListarGustos(i.IdPedido);
            return View("ListaFiltrada", lista);
        }


    }
}