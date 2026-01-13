using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
namespace ConexionesBD
{
    public class UsuarioDatos
    {
        public Usuario Login(string usuario, string password)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string query = @"
                    SELECT  u.UsuarioID, u.Usuario, u.Nombre, u.Apellido, u.Email,
                            u.RolID, u.MedicoID, u.Activo, u.FechaAlta,
                            r.Nombre as RolNombre
                    FROM Usuarios u
                    INNER JOIN Roles r ON r.RolID = u.RolID
                    WHERE u.Usuario = @usuario
                      AND u.Password = @password
                      AND u.Activo = 1";

                datos.setearConsulta(query);
                datos.setearParametro("@usuario", usuario);
                datos.setearParametro("@password", password);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Usuario u = new Usuario();
                    u.UsuarioID = (int)datos.Lector["UsuarioID"];
                    u.UsuarioNombre = (string)datos.Lector["Usuario"];
                    u.Nombre = (string)datos.Lector["Nombre"];
                    u.Apellido = (string)datos.Lector["Apellido"];
                    u.Email = datos.Lector["Email"] != DBNull.Value ? (string)datos.Lector["Email"] : null;
                    u.RolID = (int)datos.Lector["RolID"];
                    u.MedicoID = datos.Lector["MedicoID"] != DBNull.Value ? (int?)datos.Lector["MedicoID"] : null;
                    u.Activo = (bool)datos.Lector["Activo"];
                    u.FechaAlta = (DateTime)datos.Lector["FechaAlta"];
                    u.rol = new Rol
                    {
                        RolID = u.RolID,
                        Nombre = (string)datos.Lector["RolNombre"]
                    };

                    return u;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
