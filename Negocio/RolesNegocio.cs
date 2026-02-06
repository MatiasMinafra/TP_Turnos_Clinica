using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionesBD;
using Dominio;


namespace Negocio
{
    

    public class RolesNegocio
    {
        private RolesDatos datos = new RolesDatos();

        public List<Rol> Listar()
        {
            return datos.Listar();
        }
    }
}
