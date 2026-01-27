using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesBD
{
    public class MedicoDatos
    {

        public List<Medico> Listar(string filtro = "", bool soloActivos = true)
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            filtro = (filtro ?? "").Trim();

            datos.setearConsulta(@"
SELECT 
    m.MedicoID, m.DNI, m.Matricula, m.Nombre, m.Apellido, m.Email, m.Telefono, m.Activo,
    (SELECT COUNT(*) 
     FROM dbo.MedicosEspecialidades me
     WHERE me.MedicoID = m.MedicoID) AS CantidadEspecialidades
FROM dbo.Medicos m
WHERE
    (@filtro = '' OR m.DNI LIKE @like OR m.Matricula LIKE @like OR m.Nombre LIKE @like OR m.Apellido LIKE @like)
    AND (@soloActivos = 0 OR m.Activo = 1)
ORDER BY m.Apellido, m.Nombre;
");

            datos.setearParametro("@filtro", filtro);
            datos.setearParametro("@like", "%" + filtro + "%");
            datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

            try
            {
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico m = new Medico();
                    m.MedicoID = (int)datos.Lector["MedicoID"];
                    m.DNI = datos.Lector["DNI"].ToString();
                    m.Matricula = datos.Lector["Matricula"].ToString();
                    m.Nombre = datos.Lector["Nombre"].ToString();
                    m.Apellido = datos.Lector["Apellido"].ToString();
                    m.Email = datos.Lector["Email"].ToString();
                    m.Telefono = datos.Lector["Telefono"] == DBNull.Value ? null : datos.Lector["Telefono"].ToString();
                    m.Activo = (bool)datos.Lector["Activo"];
                    m.CantidadEspecialidades = (int)datos.Lector["CantidadEspecialidades"];

                    lista.Add(m);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Medico ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT *
FROM Medicos
WHERE MedicoID = @id;");

                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Medico
                    {
                        MedicoID = (int)datos.Lector["MedicoID"],
                        DNI = datos.Lector["DNI"].ToString(),
                        Matricula = datos.Lector["Matricula"].ToString(),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Sexo = datos.Lector["Sexo"] == DBNull.Value ? (char?)null : Convert.ToChar(datos.Lector["Sexo"]),
                        Nacionalidad = datos.Lector["Nacionalidad"] == DBNull.Value ? null : datos.Lector["Nacionalidad"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"] == DBNull.Value ? null : datos.Lector["Telefono"].ToString(),
                        Ciudad = datos.Lector["Ciudad"] == DBNull.Value ? null : datos.Lector["Ciudad"].ToString(),
                        Direccion = datos.Lector["Direccion"] == DBNull.Value ? null : datos.Lector["Direccion"].ToString(),
                        Activo = (bool)datos.Lector["Activo"],
                        FechaAlta = Convert.ToDateTime(datos.Lector["FechaAlta"])
                    };
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int Agregar(Medico m)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
INSERT INTO Medicos
(DNI, Matricula, Nombre, Apellido, Sexo, Nacionalidad, Email, Telefono, Ciudad, Direccion, Activo, FechaAlta)
VALUES
(@dni, @matricula, @nombre, @apellido, @sexo, @nacionalidad, @email, @telefono, @ciudad, @direccion, 1, GETDATE());

SELECT SCOPE_IDENTITY();
");

                datos.setearParametro("@dni", m.DNI);
                datos.setearParametro("@matricula", m.Matricula);
                datos.setearParametro("@nombre", m.Nombre);
                datos.setearParametro("@apellido", m.Apellido);
                datos.setearParametro("@sexo", m.Sexo.HasValue ? (object)m.Sexo.Value.ToString() : DBNull.Value);
                datos.setearParametro("@nacionalidad", m.Nacionalidad);
                datos.setearParametro("@email", m.Email);
                datos.setearParametro("@telefono", m.Telefono);
                datos.setearParametro("@ciudad", m.Ciudad);
                datos.setearParametro("@direccion", m.Direccion);

                object result = datos.ejecutarScalar();
                return Convert.ToInt32(result);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Medico m)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta(@"
UPDATE Medicos
SET DNI = @dni,
    Matricula = @matricula,
    Nombre = @nombre,
    Apellido = @apellido,
    Sexo = @sexo,
    Nacionalidad = @nacionalidad,
    Email = @email,
    Telefono = @telefono,
    Ciudad = @ciudad,
    Direccion = @direccion
WHERE MedicoID = @id;");

                    datos.setearParametro("@id", m.MedicoID);
                    datos.setearParametro("@dni", m.DNI);
                    datos.setearParametro("@matricula", m.Matricula);
                    datos.setearParametro("@nombre", m.Nombre);
                    datos.setearParametro("@apellido", m.Apellido);
                    datos.setearParametro("@sexo", m.Sexo.HasValue ? (object)m.Sexo.Value.ToString() : DBNull.Value);
                    datos.setearParametro("@nacionalidad", m.Nacionalidad);
                    datos.setearParametro("@email", m.Email);
                    datos.setearParametro("@telefono", m.Telefono);
                    datos.setearParametro("@ciudad", m.Ciudad);
                    datos.setearParametro("@direccion", m.Direccion);

                    datos.ejecutarAccion();
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

        public void Activar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE dbo.Medicos SET Activo = 1 WHERE MedicoID = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Desactivar(int id)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta("UPDATE Medicos SET Activo = 0 WHERE MedicoID = @id");
                    datos.setearParametro("@id", id);
                    datos.ejecutarAccion();
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

        public bool ExisteDni(string dni, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.Medicos
WHERE DNI = @dni
  AND (@idExcluido IS NULL OR MedicoID <> @idExcluido);");

                datos.setearParametro("@dni", dni);
                datos.setearParametro("@idExcluido", idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }


        public bool ExisteMatricula(string matricula, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.Medicos
WHERE Matricula = @matricula
  AND (@idExcluido IS NULL OR MedicoID <> @idExcluido);");

                datos.setearParametro("@matricula", matricula);
                datos.setearParametro("@idExcluido", idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }

        public bool ExisteEmail(string email, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.Medicos
WHERE Email = @email
  AND (@idExcluido IS NULL OR MedicoID <> @idExcluido);");

                datos.setearParametro("@email", email);
                datos.setearParametro("@idExcluido", idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }

    }
}

