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
           //si se desea abtraer el servicio se pasa a aclases repository para que no dependan las consultas de la base
           //  private readonly PedidoRepository  pedRepo = new PedidoRepository();

            public void Agregar(Pedido p)
            {
                MiBD.Pedido.Add(p);
                MiBD.SaveChanges();
            }

            public List<Pedido> Listar()
            {
                return MiBD.Pedido.ToList();

            }
            //listar pedido segun usuario que ingreso
            public List<Pedido> GetPedidosByUsuario(int idUsuario)
            {
                //return pedRepo.GetPedidossByUsuario(idUsuario);
                return MiBD.Pedido.Where(x => x.IdUsuarioResponsable == idUsuario).OrderBy(x => x.FechaCreacion).ToList();
            }



        }
    }