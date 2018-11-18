using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Empanadas.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Error al procesar la solicitud";
            return View();
        }

        public ActionResult NotFound404()
        {
            ViewBag.Title = "Error 404 - File not Found";
            return View("Index");
        }
    }
}