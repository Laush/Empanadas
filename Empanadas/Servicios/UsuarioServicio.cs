using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Empanadas.Servicios
{
    public class UsuarioServicio
    {

        private Entities MiBD = new Entities();

        public List<Usuario> ListarUsuarios()
        {
            return MiBD.Usuario.ToList();
        }

        public Usuario ObtenerUsuario(int id)
        {
            return MiBD.Usuario.FirstOrDefault(u => u.IdUsuario == id);
        }

        //verifico que el usuario esté registrado en el sistema
        public bool VerificarUsuarioRegistrado(Usuario usuario)
        {
            var UsuarioRegistrado = MiBD.Usuario.Where(u => u.Email == usuario.Email).FirstOrDefault();
            if (UsuarioRegistrado != null)
            {
                return true;
            }
            return false;
        }

        //verifico que la contraseña ingresada sea correcta
        public bool VerificarContraseniaLogin(Usuario usuario)
        {
            Usuario usuarioContraseniasCoincidentes = MiBD.Usuario.Where(u => u.Password == usuario.Password && u.Email == usuario.Email).FirstOrDefault();
            if (usuarioContraseniasCoincidentes != null)
            {
                // Session["usuarioLogueado"] = usuarioContraseniasCoincidentes; //guardo la variable de sesión del usuario logueado
                //  Session["idUsuario"] = usuarioContraseniasCoincidentes.IdUsuario;
                return true;
            }
            return false;
        }

    }
}