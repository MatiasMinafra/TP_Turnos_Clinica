using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConexionesBD
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector => lector;

        public AccesoDatos()
        {

            conexion = new SqlConnection(
                @"server=.\SQLEXPRESS; database=ClinicaTurnosDB; integrated security=true"
            );
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;

            
            comando.Parameters.Clear();
        }

        public void setearProcedimiento(string nombreSP)
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = nombreSP;

            comando.Parameters.Clear();
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public object ejecutarScalar()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                return comando.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void cerrarConexion()
        {
            if (lector != null && !lector.IsClosed)
                lector.Close();

            if (conexion.State == ConnectionState.Open)
                conexion.Close();
        }

    }
}
