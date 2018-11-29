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
using Empanadas.Servicios;

namespace Empanadas.Controllers
{
    public class PedidoApiController : ApiController
    {
        private Entities db = new Entities();

        GustoEmpanadaServicio srvGustos = new GustoEmpanadaServicio();

        [HttpPost]
        public void ConfirmarGustos([FromBody] InvitacionPedidoGustoEmpanadaUsuario i)
        {
            srvGustos.ObtenerPorId(i.IdGustoEmpanada);
        }

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
    }
    public class GustoConfirmado
  
    {
        public int IdGustoEmpanada { get; set; }
        public int Cantidad { get; set; }
    }
    public class ConfirmarGustosRespuesta
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
    }
}