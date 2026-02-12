using Dominio;
using System;
using System.Collections.Generic;

namespace ConexionesBD
{
    public class TurnosDatos
    {
    
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

            try
            {
                datos.setearConsulta(@"
SELECT 
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    t.MotivoConsulta,
    t.Diagnostico,
    p.Apellido + ' ' + p.Nombre AS Paciente,
    e.Nombre AS Especialidad,
    et.Nombre AS EstadoTurno,
    t.Activo
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
                        EstadoTurno = datos.Lector["EstadoTurno"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(t);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

 
        public List<DtoTurnoDia> ListarDelDia(DateTime fecha)
        {
            List<DtoTurnoDia> lista = new List<DtoTurnoDia>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT 
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    (p.Apellido + ' ' + p.Nombre) as Paciente,
    (m.Apellido + ' ' + m.Nombre) as Medico,
    e.Nombre as Especialidad,
    et.Nombre as EstadoTurno,
    ep.Nombre as EstadoPago,
    pa.Importe,
    pa.MedioPago
FROM Turnos t
INNER JOIN Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN Medicos m ON m.MedicoID = t.MedicoID
INNER JOIN Especialidades e ON e.EspecialidadID = t.EspecialidadID
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
LEFT JOIN Pagos pa ON pa.TurnoID = t.TurnoID
LEFT JOIN EstadosPago ep ON ep.EstadoPagoID = pa.EstadoPagoID
WHERE t.Activo = 1 AND t.Fecha = @fecha
ORDER BY t.HoraInicio;
");
                datos.setearParametro("@fecha", fecha.Date);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var dto = new DtoTurnoDia();
                    dto.TurnoID = (int)datos.Lector["TurnoID"];

                    DateTime f = (DateTime)datos.Lector["Fecha"];
                    dto.Fecha = f.ToString("dd/MM/yyyy");

                    TimeSpan hi = (TimeSpan)datos.Lector["HoraInicio"];
                    TimeSpan hf = (TimeSpan)datos.Lector["HoraFin"];
                    dto.Hora = $"{hi:hh\\:mm} - {hf:hh\\:mm}";

                    dto.Paciente = (string)datos.Lector["Paciente"];
                    dto.Medico = (string)datos.Lector["Medico"];
                    dto.Especialidad = (string)datos.Lector["Especialidad"];
                    dto.EstadoTurno = (string)datos.Lector["EstadoTurno"];

                    dto.EstadoPago = datos.Lector["EstadoPago"] != DBNull.Value ? (string)datos.Lector["EstadoPago"] : "Pendiente";
                    dto.Importe = datos.Lector["Importe"] != DBNull.Value ? (decimal)datos.Lector["Importe"] : 0;
                    dto.MedioPago = datos.Lector["MedioPago"] != DBNull.Value ? (string)datos.Lector["MedioPago"] : "-";

                    lista.Add(dto);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }




        public void ConfirmarPago(int turnoId, string comprobante)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("dbo.SP_ConfirmarPago");
                datos.setearParametro("@TurnoID", turnoId);
                datos.setearParametro("@Comprobante", (object)(comprobante ?? ""));
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void CancelarTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
DECLARE @EstadoCancelado INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'Cancelado');
DECLARE @EstadoPagoConfirmado INT = (SELECT EstadoPagoID FROM EstadosPago WHERE Nombre = 'Confirmado');

IF (@EstadoCancelado IS NULL)
    THROW 50021, 'Falta el estado Cancelado.', 1;


IF EXISTS (
    SELECT 1 FROM Turnos
    WHERE TurnoID = @turnoId AND EstadoTurnoID = @EstadoCancelado
)
    THROW 50022, 'El turno ya está cancelado.', 1;


IF EXISTS (
    SELECT 1 FROM Pagos
    WHERE TurnoID = @turnoId AND EstadoPagoID = @EstadoPagoConfirmado
)
    THROW 50023, 'No se puede cancelar un turno con pago confirmado.', 1;


UPDATE Turnos
SET EstadoTurnoID = @EstadoCancelado
WHERE TurnoID = @turnoId;
");

                datos.setearParametro("@turnoId", turnoId);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void CerrarTurno(int turnoId, string diagnostico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.SP_CerrarTurno");
                datos.setearParametro("@TurnoID", turnoId);
                datos.setearParametro("@Diagnostico", (object)(diagnostico ?? ""));
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void ReprogramarTurno(int turnoId, DateTime nuevaFecha, TimeSpan nuevaHoraInicio, int nuevoMedicoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.SP_ReprogramarTurno");
                datos.setearParametro("@TurnoID", turnoId);
                datos.setearParametro("@NuevaFecha", nuevaFecha.Date);
                datos.setearParametro("@NuevaHoraInicio", nuevaHoraInicio);
                datos.setearParametro("@NuevoMedicoID", nuevoMedicoId);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int ObtenerEspecialidadDelTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT EspecialidadID
FROM Turnos
WHERE TurnoID = @turnoId
");
                datos.setearParametro("@turnoId", turnoId);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return (int)datos.Lector["EspecialidadID"];

                throw new Exception("No se pudo obtener la especialidad del turno.");
            }
            finally
            {
                datos.cerrarConexion();
            }



        }

        public void MarcarNoAsistio(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
DECLARE @NoAsistio INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre='No asistió');
DECLARE @Cancelado INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre='Cancelado');

IF (@NoAsistio IS NULL) THROW 50030, 'Falta estado No asistió.', 1;

IF EXISTS (SELECT 1 FROM Turnos WHERE TurnoID=@turnoId AND EstadoTurnoID=@Cancelado)
    THROW 50031, 'No se puede marcar No asistió si está cancelado.', 1;

UPDATE Turnos
SET EstadoTurnoID=@NoAsistio
WHERE TurnoID=@turnoId;
");
                datos.setearParametro("@turnoId", turnoId);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }


        public DtoTurnoMail ObtenerDatosMailTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    (p.Apellido + ' ' + p.Nombre) AS PacienteNombre,
    p.Email AS PacienteEmail,
    (m.Apellido + ' ' + m.Nombre) AS MedicoNombre,
    e.Nombre AS Especialidad,
    ISNULL(pa.MedioPago, '-') AS MedioPago,
    ISNULL(pa.Importe, 0) AS Importe,
    ISNULL(t.MotivoConsulta, '') AS MotivoConsulta
FROM Turnos t
INNER JOIN Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN Medicos m ON m.MedicoID = t.MedicoID
INNER JOIN Especialidades e ON e.EspecialidadID = t.EspecialidadID
LEFT JOIN Pagos pa ON pa.TurnoID = t.TurnoID
WHERE t.TurnoID = @turnoId;
");

                datos.setearParametro("@turnoId", turnoId);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                    throw new Exception("No se encontró el turno para enviar mail.");

                return new DtoTurnoMail
                {
                    TurnoID = (int)datos.Lector["TurnoID"],
                    Fecha = (DateTime)datos.Lector["Fecha"],
                    HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                    HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                    PacienteNombre = datos.Lector["PacienteNombre"].ToString(),
                    PacienteEmail = datos.Lector["PacienteEmail"] == DBNull.Value ? "" : datos.Lector["PacienteEmail"].ToString(),
                    MedicoNombre = datos.Lector["MedicoNombre"].ToString(),
                    Especialidad = datos.Lector["Especialidad"].ToString(),
                    MedioPago = datos.Lector["MedioPago"].ToString(),
                    Importe = (decimal)datos.Lector["Importe"],
                    MotivoConsulta = datos.Lector["MotivoConsulta"].ToString()
                };
            }
            finally { datos.cerrarConexion(); }
        }

        public (DateTime Fecha, TimeSpan HoraInicio) ObtenerFechaHoraTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"SELECT Fecha, HoraInicio FROM Turnos WHERE TurnoID = @id;");
                datos.setearParametro("@id", turnoId);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                    throw new Exception("No se encontró el turno.");

                return ((DateTime)datos.Lector["Fecha"], (TimeSpan)datos.Lector["HoraInicio"]);
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