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

        public Usuario ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT  u.UsuarioID, u.Usuario, u.Password, u.Nombre, u.Apellido, u.Email,
        u.RolID, u.MedicoID, u.Activo, u.FechaAlta,
        r.Nombre as RolNombre
FROM Usuarios u
INNER JOIN Roles r ON r.RolID = u.RolID
WHERE u.UsuarioID = @id");
                datos.setearParametro("@id", id);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Usuario u = new Usuario();
                    u.UsuarioID = (int)datos.Lector["UsuarioID"];
                    u.UsuarioNombre = (string)datos.Lector["Usuario"];
                    u.Password = (string)datos.Lector["Password"];
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

        public List<Usuario> Listar(string filtro = "", bool soloActivos = true)
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            filtro = (filtro ?? "").Trim();

            try
            {
                datos.setearConsulta(@"
SELECT  u.UsuarioID, u.Usuario, u.Nombre, u.Apellido, u.Email,
        u.RolID, u.MedicoID, u.Activo, u.FechaAlta,
        r.Nombre as RolNombre
FROM Usuarios u
INNER JOIN Roles r ON r.RolID = u.RolID
WHERE
    (@filtro = '' OR u.Usuario LIKE @like OR u.Nombre LIKE @like OR u.Apellido LIKE @like)
    AND (@soloActivos = 0 OR u.Activo = 1)
ORDER BY u.Usuario;");

                datos.setearParametro("@filtro", filtro);
                datos.setearParametro("@like", "%" + filtro + "%");
                datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
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

                    lista.Add(u);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Agregar(Usuario u)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
INSERT INTO Usuarios (Usuario, Password, Nombre, Apellido, Email, RolID, MedicoID, Activo)
VALUES (@Usuario, @Password, @Nombre, @Apellido, @Email, @RolID, @MedicoID, @Activo);");

                datos.setearParametro("@Usuario", u.UsuarioNombre);
                datos.setearParametro("@Password", u.Password);
                datos.setearParametro("@Nombre", u.Nombre);
                datos.setearParametro("@Apellido", u.Apellido);
                datos.setearParametro("@Email", (object)u.Email ?? DBNull.Value);
                datos.setearParametro("@RolID", u.RolID);
                datos.setearParametro("@MedicoID", (object)u.MedicoID ?? DBNull.Value);
                datos.setearParametro("@Activo", u.Activo);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Modificar(Usuario u)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
               
                bool actualizarPassword = !string.IsNullOrWhiteSpace(u.Password);

                datos.setearConsulta(actualizarPassword ? @"
UPDATE Usuarios
SET Usuario=@Usuario, Password=@Password, Nombre=@Nombre, Apellido=@Apellido, Email=@Email,
    RolID=@RolID, MedicoID=@MedicoID, Activo=@Activo
WHERE UsuarioID=@UsuarioID;"
                : @"
UPDATE Usuarios
SET Usuario=@Usuario, Nombre=@Nombre, Apellido=@Apellido, Email=@Email,
    RolID=@RolID, MedicoID=@MedicoID, Activo=@Activo
WHERE UsuarioID=@UsuarioID;");

                datos.setearParametro("@UsuarioID", u.UsuarioID);
                datos.setearParametro("@Usuario", u.UsuarioNombre);
                if (actualizarPassword) datos.setearParametro("@Password", u.Password);
                datos.setearParametro("@Nombre", u.Nombre);
                datos.setearParametro("@Apellido", u.Apellido);
                datos.setearParametro("@Email", (object)u.Email ?? DBNull.Value);
                datos.setearParametro("@RolID", u.RolID);
                datos.setearParametro("@MedicoID", (object)u.MedicoID ?? DBNull.Value);
                datos.setearParametro("@Activo", u.Activo);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void BajaLogica(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Usuarios SET Activo = 0 WHERE UsuarioID = @id;");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

     
        public bool ExisteUsuario(string usuario, int? excluirUsuarioId = null)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT 1
FROM Usuarios
WHERE UPPER(Usuario) = UPPER(@usuario)
  AND (@excluir IS NULL OR UsuarioID <> @excluir);");

                datos.setearParametro("@usuario", usuario?.Trim());
                datos.setearParametro("@excluir", (object)excluirUsuarioId ?? DBNull.Value);

                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool ExisteUsuarioMedicoActivo(int medicoId, int? excluirUsuarioId = null)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT 1
FROM Usuarios
WHERE MedicoID = @medicoId
  AND Activo = 1
  AND (@excluir IS NULL OR UsuarioID <> @excluir);");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@excluir", (object)excluirUsuarioId ?? DBNull.Value);

                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
