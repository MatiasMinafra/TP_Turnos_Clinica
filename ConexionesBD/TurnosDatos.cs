using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesBD
{
    public class TurnosDatos
    {
        public List<Turno> ListarDelDia(DateTime fecha)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT 
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    t.MotivoConsulta,
    t.Activo,
    p.Nombre + ' ' + p.Apellido AS Paciente,
    m.Nombre + ' ' + m.Apellido AS Medico,
    e.Nombre AS Especialidad,
    et.Nombre AS Estado
FROM Turnos t
INNER JOIN Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN Medicos m ON m.MedicoID = t.MedicoID
INNER JOIN Especialidades e ON e.EspecialidadID = t.EspecialidadID
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
WHERE t.Fecha = @fecha
ORDER BY t.HoraInicio;
");

            datos.setearParametro("@fecha", fecha.Date);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Turno t = new Turno
                    {
                        TurnoID = (int)datos.Lector["TurnoID"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                        MotivoConsulta = datos.Lector["MotivoConsulta"].ToString(),
                        PacienteNombre = datos.Lector["Paciente"].ToString(),
                        MedicoNombre = datos.Lector["Medico"].ToString(),
                        EspecialidadNombre = datos.Lector["Especialidad"].ToString(),
                        EstadoTurno = datos.Lector["Estado"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(t);
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public int AltaConSP(
            int pacienteId,
            int especialidadId,
            int medicoId,
            DateTime fecha,
            TimeSpan horaInicio,
            string motivo,
            decimal importe,
            string medioPago)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_AltaTurno");
                datos.setearParametro("@PacienteID", pacienteId);
                datos.setearParametro("@EspecialidadID", especialidadId);
                datos.setearParametro("@MedicoID", medicoId);
                datos.setearParametro("@Fecha", fecha.Date);
                datos.setearParametro("@HoraInicio", horaInicio);
                datos.setearParametro("@MotivoConsulta", motivo);
                datos.setearParametro("@Importe", importe);
                datos.setearParametro("@MedioPago", medioPago);

                return Convert.ToInt32(datos.ejecutarScalar());
            }
            finally { datos.cerrarConexion(); }
        }

        public List<MedicoBasico> MedicosPorEspecialidad(int especialidadId)
        {
            var lista = new List<MedicoBasico>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT MedicoID, Medico, Matricula
FROM (
    SELECT DISTINCT
        m.MedicoID,
        (m.Apellido + ', ' + m.Nombre) AS Medico,
        m.Matricula,
        m.Apellido,
        m.Nombre
    FROM dbo.Medicos m
    INNER JOIN dbo.MedicosEspecialidades me ON me.MedicoID = m.MedicoID
    WHERE m.Activo = 1
      AND me.EspecialidadID = @esp
) x
ORDER BY x.Apellido, x.Nombre;
");

            datos.setearParametro("@esp", especialidadId);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new MedicoBasico
                    {
                        MedicoID = (int)datos.Lector["MedicoID"],
                        Medico = datos.Lector["Medico"].ToString(),
                        Matricula = datos.Lector["Matricula"].ToString()
                    });
                }
                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public List<RangoHorario> RangosLaborales(int medicoId, byte diaSemana)
        {
            var lista = new List<RangoHorario>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT tt.HoraInicio, tt.HoraFin
FROM dbo.MedicosTurnosTrabajo mtt
INNER JOIN dbo.TurnosTrabajo tt ON tt.TurnoTrabajoID = mtt.TurnoTrabajoID
WHERE mtt.MedicoID = @med
  AND mtt.DiaSemana = @dia
  AND mtt.Activo = 1
  AND tt.Activo = 1;
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@dia", diaSemana);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new RangoHorario
                    {
                        Inicio = (TimeSpan)datos.Lector["HoraInicio"],
                        Fin = (TimeSpan)datos.Lector["HoraFin"]
                    });
                }
                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public HashSet<TimeSpan> HorasOcupadas(int medicoId, DateTime fecha)
        {
            var set = new HashSet<TimeSpan>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT HoraInicio
FROM dbo.Turnos
WHERE MedicoID = @med
  AND Fecha = @fec
  AND Activo = 1;
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@fec", fecha.Date);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                    set.Add((TimeSpan)datos.Lector["HoraInicio"]);

                return set;
            }
            finally { datos.cerrarConexion(); }
        }

       
        public List<Turno> ListarPorMedico(int medicoId, DateTime desde, DateTime hasta)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT 
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    t.MotivoConsulta,
    t.Diagnostico,
    t.Activo,
    p.Nombre + ' ' + p.Apellido AS Paciente,
    e.Nombre AS Especialidad,
    et.Nombre AS Estado
FROM Turnos t
INNER JOIN Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN Especialidades e ON e.EspecialidadID = t.EspecialidadID
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
WHERE t.MedicoID = @medicoId
  AND t.Fecha BETWEEN @desde AND @hasta
ORDER BY t.Fecha, t.HoraInicio;
");

            datos.setearParametro("@medicoId", medicoId);
            datos.setearParametro("@desde", desde.Date);
            datos.setearParametro("@hasta", hasta.Date);

            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Turno t = new Turno
                    {
                        TurnoID = (int)datos.Lector["TurnoID"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                        MotivoConsulta = datos.Lector["MotivoConsulta"].ToString(),
                        Diagnostico = datos.Lector["Diagnostico"] == DBNull.Value ? null : datos.Lector["Diagnostico"].ToString(),
                        PacienteNombre = datos.Lector["Paciente"].ToString(),
                        EspecialidadNombre = datos.Lector["Especialidad"].ToString(),
                        EstadoTurno = datos.Lector["Estado"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(t);
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }
    }

   
    public class MedicoBasico
    {
        public int MedicoID { get; set; }
        public string Medico { get; set; }
        public string Matricula { get; set; }
    }

    public class RangoHorario
    {
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fin { get; set; }
    }
}
