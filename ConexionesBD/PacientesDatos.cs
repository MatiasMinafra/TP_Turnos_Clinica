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
    public class PacientesDatos
    {
      
            public List<Pacientes> Listar(string filtro = "")
            {
                List<Pacientes> lista = new List<Pacientes>();
                AccesoDatos datos = new AccesoDatos();

                filtro = (filtro ?? "").Trim();

                datos.setearConsulta(@"
SELECT PacienteID, DNI, Nombre, Apellido, Email, Telefono, Activo
FROM dbo.Pacientes
WHERE (@filtro = '' OR DNI LIKE @like OR Nombre LIKE @like OR Apellido LIKE @like)
ORDER BY Apellido, Nombre;");

                datos.setearParametro("@filtro", filtro);
                datos.setearParametro("@like", "%" + filtro + "%");

                try
                {
                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Pacientes p = new Pacientes();
                        p.PacienteID = (int)datos.Lector["PacienteID"];
                        p.DNI = datos.Lector["DNI"].ToString();
                        p.Nombre = datos.Lector["Nombre"].ToString();
                        p.Apellido = datos.Lector["Apellido"].ToString();
                        p.Email = datos.Lector["Email"].ToString();
                        p.Telefono = datos.Lector["Telefono"] == DBNull.Value
                            ? null
                            : datos.Lector["Telefono"].ToString();
                        p.Activo = (bool)datos.Lector["Activo"];

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
    }
}
