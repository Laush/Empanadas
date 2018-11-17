using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class GustoEmpanadaServicio
    {
        private Entities MiBD = new Entities();
        /*
        var query =
               (from p in MiBD.Pedido.DefaultIfEmpty()
                join ep in MiBD.EstadoPedido on p.IdEstadoPedido equals ep.IdEstadoPedido
                join ip in MiBD.InvitacionPedido.DefaultIfEmpty() on p.IdPedido equals ip.IdPedido
                where ip.IdUsuario == usu.IdUsuario
                orderby p.FechaCreacion descending
                select
                    p).ToList(); */

        public InvitacionPedidoGustoEmpanadaUsuario ObtenerInvitacionPedidoUsuarioGustoPorIdPedido(int idPedido)
        {
            return MiBD.InvitacionPedidoGustoEmpanadaUsuario.FirstOrDefault(i => i.IdPedido == idPedido);
        }


            public List<InvitacionPedidoGustoEmpanadaUsuario> listarGustosConCantidad(int idPedido)
        {
            var listGustos = (from InvitacionPedidoGustoEmpanadaUsuario iv in MiBD.InvitacionPedidoGustoEmpanadaUsuario
                              join p in MiBD.Pedido on iv.IdPedido equals p.IdPedido
                              join g in MiBD.GustoEmpanada on iv.IdGustoEmpanada equals g.IdGustoEmpanada
                              where p.IdPedido == idPedido
                              select iv
                              ).ToList();
            return listGustos;
        }








    }
}