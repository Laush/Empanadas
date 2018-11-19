using Empanadas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class GustoEmpanadaServicio
    {
        private Entities MiBD = new Entities();
    //  private PedidoServicio srvPedido = new PedidoServicio();
        
        public GustoEmpanada ObtenerPorId(int id)
        {
            return MiBD.GustoEmpanada.FirstOrDefault(g => g.IdGustoEmpanada == id);
        }
        /*
         * 
         * // ESTO NO ANDA Y NO SE USA, LO DEJO POR SI SIRVE LA SINTAXIS 
        public GustoEmpanada ObtenerPorIdPedido(int idPedido)
        {
            Pedido p = srvPedido.ObtenerPorId(idPedido);
          
            --foreach (int idGustos in p.IdGustosSeleccionados)
            {
                var gusto = (from g in MiBD.GustoEmpanada
                             join pp in MiBD.Pedido on g.IdGustoEmpanada equals idGustos
                             where pp.IdPedido == idPedido
                             select g).SingleOrDefault();
                p.GustoEmpanada.Add(gusto);
            }--
       
            var pp = MiBD.Pedido.Where(o => o.IdPedido == p.IdPedido);

            var gg = MiBD.GustoEmpanada.Where(o => o.IdGustoEmpanada == pp.FirstOrDefault(x => x.IdGustosSeleccionados == p.IdGustosSeleccionados)
           
            return MiBD.Pedido.Where(pp => pp.IdPedido == p.IdPedido);
        }*/


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