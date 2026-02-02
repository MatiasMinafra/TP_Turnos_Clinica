using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesBD
{
    public class TurnoTrabajoDatos
    {
        public List<TurnoTrabajo> Listar(string filtro = "", bool soloActivos = true)
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            filtro = (filtro ?? "").Trim();

            datos.setearConsulta(@"
SELECT TurnoTrabajoID, Nombre, HoraInicio, HoraFin, Activo
FROM dbo.TurnosTrabajo
WHERE
    (@filtro = '' OR Nombre LIKE @like)
    AND (@soloActivos = 0 OR Activo = 1)
ORDER BY Nombre;
");
            datos.setearParametro("@filtro", filtro);
            datos.setearParametro("@like", "%" + filtro + "%");
            datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    TurnoTrabajo t = new TurnoTrabajo();
                    t.TurnoTrabajoID = (int)datos.Lector["TurnoTrabajoID"];
                    t.Nombre = datos.Lector["Nombre"].ToString();
                    t.HoraInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    t.HoraFin = (TimeSpan)datos.Lector["HoraFin"];
                    t.Activo = (bool)datos.Lector["Activo"];
                    lista.Add(t);
                }
                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public TurnoTrabajo ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT TurnoTrabajoID, Nombre, HoraInicio, HoraFin, Activo
FROM dbo.TurnosTrabajo
WHERE TurnoTrabajoID = @id;
");
                datos.setearParametro("@id", id);

                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return new TurnoTrabajo
                    {
                        TurnoTrabajoID = (int)datos.Lector["TurnoTrabajoID"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                }
                return null;
            }
            finally { datos.cerrarConexion(); }
        }

        public int Agregar(TurnoTrabajo t)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
INSERT INTO dbo.TurnosTrabajo (Nombre, HoraInicio, HoraFin, Activo)
VALUES (@nombre, @inicio, @fin, 1);
SELECT SCOPE_IDENTITY();
");
                datos.setearParametro("@nombre", t.Nombre);
                datos.setearParametro("@inicio", t.HoraInicio);
                datos.setearParametro("@fin", t.HoraFin);

                object r = datos.ejecutarScalar();
                return Convert.ToInt32(r);
            }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(TurnoTrabajo t)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
UPDATE dbo.TurnosTrabajo
SET Nombre = @nombre,
    HoraInicio = @inicio,
    HoraFin = @fin
WHERE TurnoTrabajoID = @id;
");
                datos.setearParametro("@id", t.TurnoTrabajoID);
                datos.setearParametro("@nombre", t.Nombre);
                datos.setearParametro("@inicio", t.HoraInicio);
                datos.setearParametro("@fin", t.HoraFin);

                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Activar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE dbo.TurnosTrabajo SET Activo = 1 WHERE TurnoTrabajoID = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Desactivar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE dbo.TurnosTrabajo SET Activo = 0 WHERE TurnoTrabajoID = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public bool ExisteNombre(string nombre, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.TurnosTrabajo
WHERE Nombre = @nombre
  AND (@idExcluido IS NULL OR TurnoTrabajoID <> @idExcluido);
");
                datos.setearParametro("@nombre", nombre);
                datos.setearParametro("@idExcluido", idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }
    }
}

