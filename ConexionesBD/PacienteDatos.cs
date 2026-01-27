using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConexionesBD
{
    public class PacienteDatos
    {

        public List<Paciente> Listar(string filtro = "", bool soloActivos = true)
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            filtro = (filtro ?? "").Trim();

            datos.setearConsulta(@"
SELECT PacienteID, DNI, Nombre, Apellido, Email, Telefono, Activo
FROM dbo.Pacientes
WHERE
    (@filtro = '' OR DNI LIKE @like OR Nombre LIKE @like OR Apellido LIKE @like OR Email LIKE @like)
    AND (@soloActivos = 0 OR Activo = 1)
ORDER BY Apellido, Nombre;");

            datos.setearParametro("@filtro", filtro);
            datos.setearParametro("@like", "%" + filtro + "%");
            datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

            try
            {
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente p = new Paciente
                    {
                        PacienteID = (int)datos.Lector["PacienteID"],
                        DNI = datos.Lector["DNI"].ToString(),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"] == DBNull.Value ? null : datos.Lector["Telefono"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(p);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Desactivar(int pacienteId)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Pacientes SET Activo = 0 WHERE PacienteID = @id");
                datos.setearParametro("@id", pacienteId);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public Paciente ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT PacienteID, DNI, Nombre, Apellido, Sexo, Nacionalidad, FechaNacimiento,
       Telefono, Email, Ciudad, Direccion, Activo, FechaAlta
FROM Pacientes
WHERE PacienteID = @id;");

                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Paciente p = new Paciente();
                    p.PacienteID = (int)datos.Lector["PacienteID"];
                    p.DNI = datos.Lector["DNI"].ToString();
                    p.Nombre = datos.Lector["Nombre"].ToString();
                    p.Apellido = datos.Lector["Apellido"].ToString();

                    
                    p.Sexo = datos.Lector["Sexo"] == DBNull.Value ? (char?)null : Convert.ToChar(datos.Lector["Sexo"]);
                    p.Nacionalidad = datos.Lector["Nacionalidad"] == DBNull.Value ? null : datos.Lector["Nacionalidad"].ToString();

                   
                    p.FechaNacimiento = datos.Lector["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(datos.Lector["FechaNacimiento"]);

                    p.Telefono = datos.Lector["Telefono"] == DBNull.Value ? null : datos.Lector["Telefono"].ToString();
                    p.Email = datos.Lector["Email"].ToString();
                    p.Ciudad = datos.Lector["Ciudad"] == DBNull.Value ? null : datos.Lector["Ciudad"].ToString();
                    p.Direccion = datos.Lector["Direccion"] == DBNull.Value ? null : datos.Lector["Direccion"].ToString();

                    p.Activo = (bool)datos.Lector["Activo"];
                    p.FechaAlta = Convert.ToDateTime(datos.Lector["FechaAlta"]);

                    return p;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Agregar(Paciente p)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
INSERT INTO Pacientes
(DNI, Nombre, Apellido, Sexo, Nacionalidad, FechaNacimiento, Telefono, Email, Ciudad, Direccion, Activo)
VALUES
(@dni, @nombre, @apellido, @sexo, @nacionalidad, @fechaNac, @telefono, @email, @ciudad, @direccion, 1);");

                datos.setearParametro("@dni", p.DNI);
                datos.setearParametro("@nombre", p.Nombre);
                datos.setearParametro("@apellido", p.Apellido);

               
                datos.setearParametro("@sexo", p.Sexo.HasValue ? (object)p.Sexo.Value.ToString() : DBNull.Value);

                datos.setearParametro("@nacionalidad", p.Nacionalidad);

                
                datos.setearParametro("@fechaNac", p.FechaNacimiento.HasValue ? (object)p.FechaNacimiento.Value.Date : DBNull.Value);

                datos.setearParametro("@telefono", p.Telefono);
                datos.setearParametro("@email", p.Email);
                datos.setearParametro("@ciudad", p.Ciudad);
                datos.setearParametro("@direccion", p.Direccion);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Modificar(Paciente p)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
UPDATE Pacientes
SET DNI = @dni,
    Nombre = @nombre,
    Apellido = @apellido,
    Sexo = @sexo,
    Nacionalidad = @nacionalidad,
    FechaNacimiento = @fechaNac,
    Telefono = @telefono,
    Email = @email,
    Ciudad = @ciudad,
    Direccion = @direccion
WHERE PacienteID = @id;");

                datos.setearParametro("@id", p.PacienteID);
                datos.setearParametro("@dni", p.DNI);
                datos.setearParametro("@nombre", p.Nombre);
                datos.setearParametro("@apellido", p.Apellido);
                datos.setearParametro("@sexo", p.Sexo.HasValue ? (object)p.Sexo.Value.ToString() : DBNull.Value);
                datos.setearParametro("@nacionalidad", p.Nacionalidad);
                datos.setearParametro("@fechaNac", p.FechaNacimiento.HasValue ? (object)p.FechaNacimiento.Value.Date : DBNull.Value);
                datos.setearParametro("@telefono", p.Telefono);
                datos.setearParametro("@email", p.Email);
                datos.setearParametro("@ciudad", p.Ciudad);
                datos.setearParametro("@direccion", p.Direccion);

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
FROM Pacientes
WHERE DNI = @dni
  AND (@idExcluido IS NULL OR PacienteID <> @idExcluido);");

                datos.setearParametro("@dni", dni);
                datos.setearParametro("@idExcluido",
                    idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool ExisteEmail(string email, int? idExcluido = null)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM Pacientes
WHERE Email = @email
  AND (@idExcluido IS NULL OR PacienteID <> @idExcluido);");

                datos.setearParametro("@email", email);
                datos.setearParametro("@idExcluido",
                    idExcluido.HasValue ? (object)idExcluido.Value : DBNull.Value);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
