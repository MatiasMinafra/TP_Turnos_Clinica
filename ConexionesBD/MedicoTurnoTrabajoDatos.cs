using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace ConexionesBD
{
    public class MedicoTurnoTrabajoDatos
    {
        public List<Dto_MedicosTurnosTrabajo> ListarPorMedico(int medicoId, bool soloActivos = true)
        {
            List<Dto_MedicosTurnosTrabajo> lista = new List<Dto_MedicosTurnosTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT 
    mtt.MedicoTurnoID,
    mtt.MedicoID,
    mtt.TurnoTrabajoID,
    mtt.DiaSemana,
    mtt.Activo,
    tt.Nombre AS TurnoNombre,
    tt.HoraInicio,
    tt.HoraFin
FROM dbo.MedicosTurnosTrabajo mtt
INNER JOIN dbo.TurnosTrabajo tt ON tt.TurnoTrabajoID = mtt.TurnoTrabajoID
WHERE mtt.MedicoID = @med
  AND (@soloActivos = 0 OR mtt.Activo = 1)
ORDER BY mtt.DiaSemana, tt.HoraInicio;
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@soloActivos", soloActivos ? 1 : 0);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    byte dia = (byte)datos.Lector["DiaSemana"];

                    lista.Add(new Dto_MedicosTurnosTrabajo
                    {
                        MedicoTurnoID = (int)datos.Lector["MedicoTurnoID"],
                        MedicoID = (int)datos.Lector["MedicoID"],
                        TurnoTrabajoID = (int)datos.Lector["TurnoTrabajoID"],
                        DiaSemana = dia,
                        DiaNombre = DiaNombre(dia),
                        Activo = (bool)datos.Lector["Activo"],
                        TurnoNombre = datos.Lector["TurnoNombre"].ToString(),
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"]
                    });
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public MedicosTurnosTrabajo ObtenerPorId(int medicoTurnoId)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT MedicoTurnoID, MedicoID, TurnoTrabajoID, DiaSemana, Activo
FROM dbo.MedicosTurnosTrabajo
WHERE MedicoTurnoID = @id;
");
            datos.setearParametro("@id", medicoTurnoId);

            try
            {
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return new MedicosTurnosTrabajo
                    {
                        MedicoTurnoID = (int)datos.Lector["MedicoTurnoID"],
                        MedicoID = (int)datos.Lector["MedicoID"],
                        TurnoTrabajoID = (int)datos.Lector["TurnoTrabajoID"],
                        DiaSemana = (byte)datos.Lector["DiaSemana"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                }
                return null;
            }
            finally { datos.cerrarConexion(); }
        }

        public bool Existe(int medicoId, int turnoTrabajoId, byte diaSemana)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.MedicosTurnosTrabajo
WHERE MedicoID = @med
  AND TurnoTrabajoID = @tt
  AND DiaSemana = @dia;
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@tt", turnoTrabajoId);
            datos.setearParametro("@dia", diaSemana);

            try
            {
                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }

        public void Agregar(int medicoId, int turnoTrabajoId, byte diaSemana)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
INSERT INTO dbo.MedicosTurnosTrabajo (MedicoID, TurnoTrabajoID, DiaSemana, Activo)
VALUES (@med, @tt, @dia, 1);
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@tt", turnoTrabajoId);
            datos.setearParametro("@dia", diaSemana);

            try
            {
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Activar(int medicoTurnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("UPDATE dbo.MedicosTurnosTrabajo SET Activo = 1 WHERE MedicoTurnoID = @id");
            datos.setearParametro("@id", medicoTurnoId);

            try { datos.ejecutarAccion(); }
            finally { datos.cerrarConexion(); }
        }

        public void Desactivar(int medicoTurnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("UPDATE dbo.MedicosTurnosTrabajo SET Activo = 0 WHERE MedicoTurnoID = @id");
            datos.setearParametro("@id", medicoTurnoId);

            try { datos.ejecutarAccion(); }
            finally { datos.cerrarConexion(); }
        }

        public TurnoTrabajo ObtenerTurnoTrabajo(int turnoTrabajoId)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT TurnoTrabajoID, Nombre, HoraInicio, HoraFin, Activo
FROM dbo.TurnosTrabajo
WHERE TurnoTrabajoID = @id;
");
            datos.setearParametro("@id", turnoTrabajoId);

            try
            {
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

        public bool ExisteSolapado(int medicoId, byte diaSemana, TimeSpan inicio, TimeSpan fin)
        {
            AccesoDatos datos = new AccesoDatos();

           
            datos.setearConsulta(@"
SELECT COUNT(1)
FROM dbo.MedicosTurnosTrabajo mtt
INNER JOIN dbo.TurnosTrabajo tt ON tt.TurnoTrabajoID = mtt.TurnoTrabajoID
WHERE mtt.MedicoID = @med
  AND mtt.DiaSemana = @dia
  AND mtt.Activo = 1
  AND tt.Activo = 1
  AND (@inicio < tt.HoraFin AND @fin > tt.HoraInicio);
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@dia", diaSemana);
            datos.setearParametro("@inicio", inicio);
            datos.setearParametro("@fin", fin);

            try
            {
                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }


        private string DiaNombre(byte dia)
        {
            switch (dia)
            {
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miércoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sábado";
                case 7: return "Domingo";
                default: return "-";
            }
        }
    }
}
