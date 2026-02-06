using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace ConexionesBD
{
    public class RolesDatos
    {
        public List<Rol> Listar()
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT RolID, Nombre
FROM Roles
ORDER BY RolID;");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Rol
                    {
                        RolID = (int)datos.Lector["RolID"],
                        Nombre = (string)datos.Lector["Nombre"]
                    });
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
