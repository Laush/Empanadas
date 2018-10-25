using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Models
{
    public class PedidoServicio
    {

        // la inicializo y no necesito un constructor, es privada para que no la modifique nadie mas 
        private static List<Pedido> Lista = new List<Pedido>();

        private Entities MiBD = new Entities();

        public void Agregar(Pedido p)
        {
            MiBD.Pedido.Add(p);
            MiBD.SaveChanges();
        }

        public List<Pedido> Listar()
        {
            return MiBD.Pedido.ToList();

        }

        //listar pedido segun usuario ue ingreso
        /* public List<Pedido> ListarPedidiosPorUsuario(int id)
         {
             return MiBD.Pedido.Where(t => t.IdUsuario == id).OrderBy(t => t.Nombre).ToList();  //muestro pedidos por orden ascendente
         }
         */

    }
}