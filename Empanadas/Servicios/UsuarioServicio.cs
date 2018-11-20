using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class UsuarioServicio
    {

        private Entities MiBD = new Entities();

          public int devolverIdUsuario { get; set; }

        public Usuario VerificarExistenciaUsuario(Usuario u)
        {
            var user = MiBD.Usuario.Where(us => us.Email.Equals(u.Email) && us.Password.Equals(u.Password)).FirstOrDefault();

            return user;
        }

        public Usuario BuscarUsuarioPorMail(string mail)
        {
            var usuarioBuscado = MiBD.Usuario.Where(us => us.Email.Equals(mail)).FirstOrDefault();
            return usuarioBuscado;
        }
        
        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            return MiBD.Usuario.ToList();
        }

        public Usuario GetById(int id)
        {
            return MiBD.Usuario.FirstOrDefault(x => x.IdUsuario == id);
        }

        public List<Usuario> ObtenerUsuariosPorPedido(int idPedido)
        {
            var list = (from u in MiBD.Usuario
                        join i in MiBD.InvitacionPedido on u.IdUsuario equals i.IdUsuario
                        join p in MiBD.Pedido on i.IdPedido equals p.IdPedido
                        where p.IdPedido == idPedido
                        select u).ToList();
            return list;
        }

        public Usuario ObtenerPorId(int id)
        {
            return MiBD.Usuario.FirstOrDefault(u => u.IdUsuario == id);
        }
    }
}