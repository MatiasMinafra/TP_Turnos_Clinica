using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioNegocio
    {
        private UsuarioDatos datos = new UsuarioDatos();

        public Usuario Login(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                return null;

            return datos.Login(usuario.Trim(), password.Trim());
        }
    }
}
