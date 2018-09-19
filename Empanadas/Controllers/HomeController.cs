using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()//logueado, va a index con Usuariolayout 
        {
            //lista de pedidos

            return View();
        }

        public ActionResult IndexLogout() //sin loguear, va a index logout con baselayout
        {

            return View();
        }
    }
}