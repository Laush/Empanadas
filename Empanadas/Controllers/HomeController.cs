using Empanadas.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class HomeController : Controller
    {
        UsuarioServicio servicioUsuario = new UsuarioServicio();

        // GET: Home
        public ActionResult Index()//logueado, va a index con Usuariolayout, texto de presentacion
        {
            //lista de pedidos

            return View();
        }

        //formulario de login
        public ActionResult Login() //sin loguear, va a index logout con baselayout
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario usu)
        {
            if (servicioUsuario.VerificarUsuarioRegistrado(usu))
            {
                if (servicioUsuario.VerificarContraseniaLogin(usu))
                {
                    
                    // int id = usu.IdUsuario;
                    return RedirectToAction("Listar", "Pedidos");
                }
            }
            ViewBag.ErrorLogin = "Usuario y/o Contrasenia invalidos";
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}