using Empanadas.Models;
using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class HomeController : Controller
    {
        UsuarioServicio servicioUsuario = new UsuarioServicio();
        PedidoServicio servicioPedido = new PedidoServicio();

        // GET: Home
        public ActionResult Index()
        {
            var usuarioLogueado = Session["Usuario"] as Usuario;
            // Usuario usuarioLogueado = new Usuario();
            usuarioLogueado = servicioPedido.ObtenerUsuarioPorId(Convert.ToInt32(Session["Usuario"]));
            if (usuarioLogueado != null)
            {
                var model = this.servicioPedido.ObtenerPedidosByUsuario(usuarioLogueado);
                ViewBag.PedidosUsuario = servicioPedido.ObtenerPedidosByUsuario(usuarioLogueado);

                return View(model);
            }
            return RedirectToAction("Login");
        }


        //formulario de login
        public ActionResult Login()
        {
            if (Request.Cookies.AllKeys.Contains("usuarioSesion") && Request.Cookies["usuarioSesion"].Values.Count > 0)
            {
                var cookie = Request.Cookies["usuarioSesion"].Value;
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie))
                {
                    byte[] decryted = Convert.FromBase64String(string.IsNullOrWhiteSpace(cookie) ? string.Empty : cookie);
                    var result = Int32.Parse(System.Text.Encoding.Unicode.GetString(decryted));

                    var usuario = servicioUsuario.GetById(result);
                    if (usuario != null)
                    {
                        Session["Usuario"] = usuario;
                        return RedirectToAction("Listar", "Pedidos");
                    }
                    else
                    {
                        return View();
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            // bool recuerdame = Request.Form["Recordame"] == "on";
            var user = servicioUsuario.VerificarExistenciaUsuario(u);
            if (user != null)
            {
                Session["Usuario"] = user;
                // para un futuro recordar al usuario al loguearse
                /*
                   string result = string.Empty;
                   Usuario usuarioCookie = servicioUsuario.BuscarUsuarioPorMail(u.Email);

                  if (recuerdame)
                   {
                       byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Convert.ToString(usuarioCookie.IdUsuario));
                       result = Convert.ToBase64String(encryted);
                       Response.Cookies["usuarioSesion"].Value = result;
                   }
                   */
                if (Session["RedireccionLogin"] != null)
                {
                    String accionSesion = (String)Session["RedireccionLogin"];
                    String pattern = "/";
                    String[] accion = Regex.Split(accionSesion, pattern);
                    Session.Remove("RedireccionLogin");
                    return RedirectToAction(accion[1], accion[0]);
                }
                return RedirectToAction("Listar", "Pedidos");

            }
            else
            {
                ViewBag.ErrorLogin = " Usuario y/o Contraseña Invalidos";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}