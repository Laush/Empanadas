using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Empanadas;
using Empanadas.Models;
using Empanadas.Servicios;

namespace Empanadas.Controllers
{
    public class PedidoApiController : ApiController
    {
        private Entities db = new Entities();

        GustoEmpanadaServicio srvGustos = new GustoEmpanadaServicio();
        InvitacionPedidoServicio srvInvitacion = new InvitacionPedidoServicio();


        [HttpPost]
        public IHttpActionResult ConfirmarGustos([FromBody]ConfirmarGustosModel datos)
        {
            try
            {
                bool estado = srvInvitacion.ValidarGustos(datos);
                if (estado)
                {
                    srvInvitacion.ConfirmarGustos(datos);
                    return Json(new { Resultado = "OK", Mensaje = "Gustos elegidos satisfactoriamente" });
                }
                else
                {
                    return Json(new { success = false, Resultado = "ERROR", Mensaje = "Error al confirmar los gustos" });
                }
            }
            catch (Exception err)
            {
                return Json(new { success = false, Resultado = "ERROR", Mensaje = "No se pudo efectuar la operación porque " + err.Message });
            }

        }



        /*
        

        // GET: api/PedidoApi
        public dynamic GetPedido()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Pedido.Select(o => new { o.IdPedido, o.IdUsuarioResponsable }).ToList();
        }

        [HttpGet]
        public List<GustoEmpanada> ObtenerTodos([FromBody] GustoEmpanada i)
        {
            return srvGustos.ObtenerTodos();
        }

        // POST: api/PedidoApi
        public ConfirmarGustosRespuesta ConfirmarGustos(int IdUsuario, string Token, List<GustoConfirmado> GustosEmpanadasCantidad)
        {
            var respuesta = new ConfirmarGustosRespuesta();return respuesta;
        }

     */
    }

    /* Explicacion Pablo
    public class GustoConfirmado
  
    {
        public int IdGustoEmpanada { get; set; }
        public int Cantidad { get; set; }
    }
    public class ConfirmarGustosRespuesta
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
    }*/
}