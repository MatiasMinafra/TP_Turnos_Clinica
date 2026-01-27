using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesBD
{
    public class EspecialidadDatos
    {
           public List<Especialidad> Listar(string filtro = "", bool soloActivos = true)
            {
                List<Especialidad> lista = new List<Especialidad>();
                AccesoDatos datos = new AccesoDatos();

                filtro = (filtro ?? "").Trim();

                datos.setearConsulta(@"
SELECT EspecialidadID, Nombre, Activo
FROM dbo.Especialidades
WHERE
    (@filtro = '' OR Nombre LIKE @like)
    AND (@soloActivos = 0 OR Activo = 1)
ORDER BY Nombre;");

                datos.setearParametro("@filtro", filtro);
                datos.setearParametro("@like", "%" + filtro + "%");
                datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

                try
                {
                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Especialidad e = new Especialidad();
                        e.EspecialidadID = (int)datos.Lector["EspecialidadID"];
                        e.Nombre = datos.Lector["Nombre"].ToString();
                        e.Activo = (bool)datos.Lector["Activo"];
                        lista.Add(e);
                    }

                    return lista;
                }
                finally { datos.cerrarConexion(); }
            }

            public Especialidad ObtenerPorId(int id)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta(@"
SELECT EspecialidadID, Nombre, Activo
FROM dbo.Especialidades
WHERE EspecialidadID = @id;");

                    datos.setearParametro("@id", id);
                    datos.ejecutarLectura();

                    if (datos.Lector.Read())
                    {
                        return new Especialidad
                        {
                            EspecialidadID = (int)datos.Lector["EspecialidadID"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Activo = (bool)datos.Lector["Activo"]
                        };
                    }

                    return null;
                }
                finally { datos.cerrarConexion(); }
            }

            public int Agregar(Especialidad e)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta(@"
INSERT INTO dbo.Especialidades (Nombre, Activo)
VALUES (@nombre, 1);
SELECT SCOPE_IDENTITY();");

                    datos.setearParametro("@nombre", e.Nombre);

                    object r = datos.ejecutarScalar();
                    return Convert.ToInt32(r);
                }
                finally { datos.cerrarConexion(); }
            }

            public void Modificar(Especialidad e)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta(@"
UPDATE dbo.Especialidades
SET Nombre = @nombre
WHERE EspecialidadID = @id;");

                    datos.setearParametro("@id", e.EspecialidadID);
                    datos.setearParametro("@nombre", e.Nombre);

                    datos.ejecutarAccion();
                }
                finally { datos.cerrarConexion(); }
            }

            public void Desactivar(int id)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta("UPDATE dbo.Especialidades SET Activo = 0 WHERE EspecialidadID = @id");
                    datos.setearParametro("@id", id);
                    datos.ejecutarAccion();
                }
                finally { datos.cerrarConexion(); }
            }

            public void Activar(int id)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta("UPDATE dbo.Especialidades SET Activo = 1 WHERE EspecialidadID = @id");
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
FROM dbo.Especialidades
WHERE Nombre = @nombre
  AND (@idExcluido IS NULL OR EspecialidadID <> @idExcluido);");

                    datos.setearParametro("@nombre", nombre);
                    datos.setearParametro("@idExcluido", idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                    return Convert.ToInt32(datos.ejecutarScalar()) > 0;
                }
                finally { datos.cerrarConexion(); }
            }

        public List<Especialidad> ListarActivas()
        {
            // Reutiliza tu Listar()
            return Listar("", true);
        }

        public List<Especialidad> ListarPorMedico(int medicoId)
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT e.EspecialidadID, e.Nombre, e.Activo
FROM dbo.MedicosEspecialidades me
INNER JOIN dbo.Especialidades e ON e.EspecialidadID = me.EspecialidadID
WHERE me.MedicoID = @medicoId
ORDER BY e.Nombre;");

                datos.setearParametro("@medicoId", medicoId);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad e = new Especialidad();
                    e.EspecialidadID = (int)datos.Lector["EspecialidadID"];
                    e.Nombre = datos.Lector["Nombre"].ToString();
                    e.Activo = (bool)datos.Lector["Activo"];
                    lista.Add(e);
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public bool ExisteRelacion(int medicoId, int especialidadId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.MedicosEspecialidades
WHERE MedicoID = @medicoId AND EspecialidadID = @espId;");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@espId", especialidadId);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }

        public void AsignarAMedico(int medicoId, int especialidadId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
INSERT INTO dbo.MedicosEspecialidades (MedicoID, EspecialidadID)
VALUES (@medicoId, @espId);");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@espId", especialidadId);

                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void QuitarDeMedico(int medicoId, int especialidadId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
DELETE FROM dbo.MedicosEspecialidades
WHERE MedicoID = @medicoId AND EspecialidadID = @espId;");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@espId", especialidadId);

                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }



    }
}

