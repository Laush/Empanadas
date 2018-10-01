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

        public void Agregar(Pedido p)
        {

            Lista.Add(p);


        }

        public List<Pedido> Listar()
        {
            return Lista;

        }
//futuro CRUD
/*
        public void Eliminar(int id)
        {
            Lista.RemoveAll(p => p.idPedido == id);
        }


        public Pedido ObtenerPorId(int id)
        {
            Pedido pedidoEncontrado = null;

            foreach (Pedido p in Lista)
            {
                if (p.idPedido.Equals(id))
                {
                    pedidoEncontrado = p;
                }
            }

            return pedidoEncontrado;
        }

        public void Modificar(Pedido p)
        {

            Lista.RemoveAll(p1 => p1.idPedido == p.idPedido);
            Lista.Add(p);




        }*/
    }
}