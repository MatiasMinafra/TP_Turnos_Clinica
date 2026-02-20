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


        public List<RangoHorario> RangosLaborales(int medicoId, byte diaSemana, int especialidadId)
        {
            var lista = new List<RangoHorario>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT tt.HoraInicio, tt.HoraFin
FROM dbo.MedicosTurnosTrabajo mtt
INNER JOIN dbo.TurnosTrabajo tt ON tt.TurnoTrabajoID = mtt.TurnoTrabajoID
WHERE mtt.MedicoID = @med
  AND mtt.DiaSemana = @dia
  AND mtt.EspecialidadID = @esp
  AND mtt.Activo = 1
  AND tt.Activo = 1;
");
            datos.setearParametro("@med", medicoId);
            datos.setearParametro("@dia", diaSemana);
            datos.setearParametro("@esp", especialidadId);

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
SELECT t.HoraInicio
FROM dbo.Turnos t
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
WHERE t.MedicoID = @med
  AND t.Fecha = @fec
  AND t.Activo = 1
  AND et.Nombre <> 'Cancelado';
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
    t.PacienteID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    t.MotivoConsulta,
    t.Diagnostico,
    p.Apellido + ' ' + p.Nombre AS Paciente,
    e.Nombre AS Especialidad,
    et.Nombre AS EstadoTurno,

    -- ✅ PAGO
    ISNULL(ep.Nombre, 'Pendiente') AS EstadoPago,

    t.Activo
FROM Turnos t
INNER JOIN Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN Especialidades e ON e.EspecialidadID = t.EspecialidadID
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
LEFT JOIN Pagos pa ON pa.TurnoID = t.TurnoID
LEFT JOIN EstadosPago ep ON ep.EstadoPagoID = pa.EstadoPagoID
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
                        PacienteID = (int)datos.Lector["PacienteID"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        HoraFin = (TimeSpan)datos.Lector["HoraFin"],
                        MotivoConsulta = datos.Lector["MotivoConsulta"].ToString(),
                        Diagnostico = datos.Lector["Diagnostico"] == DBNull.Value ? null : datos.Lector["Diagnostico"].ToString(),
                        PacienteNombre = datos.Lector["Paciente"].ToString(),
                        EspecialidadNombre = datos.Lector["Especialidad"].ToString(),
                        EstadoTurno = datos.Lector["EstadoTurno"].ToString(),


                        EstadoPago = datos.Lector["EstadoPago"].ToString(),

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


        public List<DtoTurnoDia> ListarDelDia(DateTime fecha, string dniPaciente = "", bool incluirCancelados = false)
        {
            List<DtoTurnoDia> lista = new List<DtoTurnoDia>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                dniPaciente = (dniPaciente ?? "").Trim().Replace(".", "").Replace(" ", "");


                string sqlConDni = @"
SELECT TOP 1
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    p.DNI,
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
WHERE t.Activo = 1
  AND REPLACE(REPLACE(p.DNI, '.', ''), ' ', '') = @dni
  AND (@incluirCancelados = 1 OR et.Nombre <> 'Cancelado')
ORDER BY t.Fecha DESC, t.HoraInicio DESC;";

                string sqlSinDni = @"
SELECT 
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    p.DNI,
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
WHERE (t.Activo = 1 OR @incluirCancelados = 1)
  AND t.Fecha = @fecha
  AND (@incluirCancelados = 1 OR et.Nombre <> 'Cancelado')
ORDER BY t.HoraInicio;";

                if (!string.IsNullOrEmpty(dniPaciente))
                {
                    datos.setearConsulta(sqlConDni);
                    datos.setearParametro("@dni", dniPaciente);
                }
                else
                {
                    datos.setearConsulta(sqlSinDni);
                    datos.setearParametro("@fecha", fecha.Date);
                }

                datos.setearParametro("@incluirCancelados", incluirCancelados ? 1 : 0);

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

                    dto.Dni = datos.Lector["DNI"].ToString();
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
DECLARE @EstadoCancelado INT =
(
    SELECT EstadoTurnoID
    FROM EstadosTurno
    WHERE Nombre = 'Cancelado'
);

IF (@EstadoCancelado IS NULL)
    THROW 50021, 'Falta el estado Cancelado.', 1;

-- Validar que exista el turno y esté activo
IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @turnoId AND Activo = 1)
    THROW 50024, 'El turno no existe o ya está inactivo.', 1;

-- Si ya está cancelado, avisar
IF EXISTS (
    SELECT 1
    FROM Turnos
    WHERE TurnoID = @turnoId AND EstadoTurnoID = @EstadoCancelado
)
    THROW 50022, 'El turno ya está cancelado.', 1;

-- No permitir cancelar si el pago está confirmado (si existe pago)
IF EXISTS (
    SELECT 1
    FROM Pagos pa
    INNER JOIN EstadosPago ep ON ep.EstadoPagoID = pa.EstadoPagoID
    WHERE pa.TurnoID = @turnoId
      AND ep.Nombre = 'Confirmado'
)
    THROW 50023, 'No se puede cancelar un turno con pago confirmado.', 1;

-- Cancelar y liberar el horario (clave: Activo = 0)
UPDATE Turnos
SET EstadoTurnoID = @EstadoCancelado,
    Activo = 0
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


        public void ReprogramarTurno(int turnoId, DateTime fecha, TimeSpan horaInicio, int medicoId, string motivo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta(@"
DECLARE @Cancelado INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'Cancelado');
DECLARE @Atendido  INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'Atendido');

IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @TurnoID)
    THROW 50040, 'No existe el turno.', 1;

IF EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @TurnoID AND EstadoTurnoID = @Cancelado)
    THROW 50041, 'No se puede reprogramar un turno Cancelado.', 1;

IF (@Atendido IS NOT NULL)
BEGIN
    IF EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @TurnoID AND EstadoTurnoID = @Atendido)
        THROW 50042, 'No se puede reprogramar un turno Atendido/Cerrado.', 1;
END
");
                datos.setearParametro("@TurnoID", turnoId);
                datos.ejecutarLectura();

                datos.cerrarConexion();


                datos = new AccesoDatos();
                datos.setearProcedimiento("dbo.SP_ReprogramarTurno");
                datos.setearParametro("@TurnoID", turnoId);
                datos.setearParametro("@NuevaFecha", fecha.Date);
                datos.setearParametro("@NuevaHoraInicio", horaInicio);
                datos.setearParametro("@NuevoMedicoID", medicoId);
                datos.setearParametro("@Motivo", motivo);

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
DECLARE @NoAsistio INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'No Asistió');
DECLARE @Cancelado INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'Cancelado');
DECLARE @Atendido  INT = (SELECT EstadoTurnoID FROM EstadosTurno WHERE Nombre = 'Atendido');

IF (@NoAsistio IS NULL) THROW 50030, 'Falta estado No Asistió.', 1;

-- Validar que exista el turno
IF NOT EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @turnoId)
    THROW 50032, 'No existe el turno.', 1;

-- Bloqueo por estados finales
IF EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @turnoId AND EstadoTurnoID = @Cancelado)
    THROW 50031, 'No se puede marcar No Asistió si está Cancelado.', 1;

IF (@Atendido IS NOT NULL) 
BEGIN
    IF EXISTS (SELECT 1 FROM Turnos WHERE TurnoID = @turnoId AND EstadoTurnoID = @Atendido)
        THROW 50033, 'No se puede marcar No Asistió si el turno ya está Atendido/Cerrado.', 1;
END

-- Bloqueo por fecha/hora: solo si ya pasó
DECLARE @Fecha DATE, @Hora TIME(7);
SELECT @Fecha = Fecha, @Hora = HoraInicio
FROM Turnos
WHERE TurnoID = @turnoId;

IF (CAST(@Fecha AS datetime) + CAST(@Hora AS datetime)) > GETDATE()
    THROW 50034, 'No se puede marcar No Asistió antes de la fecha/hora del turno.', 1;

UPDATE Turnos
SET EstadoTurnoID = @NoAsistio
WHERE TurnoID = @turnoId;
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
    CAST(t.Fecha AS date) AS Fecha,
    CAST(t.HoraInicio AS time) AS HoraInicio,
    CAST(t.HoraFin AS time) AS HoraFin,

    (p.Apellido + ' ' + p.Nombre) AS PacienteNombre,
    LTRIM(RTRIM(ISNULL(p.Email,''))) AS PacienteEmail,

    (m.Apellido + ' ' + m.Nombre) AS MedicoNombre,
    e.Nombre AS Especialidad,

    ISNULL(pa.MedioPago, '-') AS MedioPago,
    CAST(ISNULL(pa.Importe, 0) AS decimal(18,2)) AS Importe,

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


                TimeSpan horaIni = datos.Lector["HoraInicio"] is TimeSpan ts1
                    ? ts1
                    : TimeSpan.Parse(datos.Lector["HoraInicio"].ToString());

                TimeSpan horaFin = datos.Lector["HoraFin"] is TimeSpan ts2
                    ? ts2
                    : TimeSpan.Parse(datos.Lector["HoraFin"].ToString());


                decimal importe = datos.Lector["Importe"] is decimal d
                    ? d
                    : Convert.ToDecimal(datos.Lector["Importe"]);

                return new DtoTurnoMail
                {
                    TurnoID = Convert.ToInt32(datos.Lector["TurnoID"]),
                    Fecha = Convert.ToDateTime(datos.Lector["Fecha"]),
                    HoraInicio = horaIni,
                    HoraFin = horaFin,
                    PacienteNombre = datos.Lector["PacienteNombre"].ToString(),
                    PacienteEmail = datos.Lector["PacienteEmail"].ToString(),
                    MedicoNombre = datos.Lector["MedicoNombre"].ToString(),
                    Especialidad = datos.Lector["Especialidad"].ToString(),
                    MedioPago = datos.Lector["MedioPago"].ToString(),
                    Importe = importe,
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

        public (int Atendidos, int NoAsistio, int Reprogramados) StatsMedicoMes(int medicoId, int anio, int mes)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT
    SUM(CASE WHEN et.Nombre = 'Atendido' THEN 1 ELSE 0 END) AS Atendidos,
    SUM(CASE WHEN et.Nombre IN ('No Asistio','No Asistió') THEN 1 ELSE 0 END) AS NoAsistio,
    SUM(CASE WHEN et.Nombre = 'Reprogramado' THEN 1 ELSE 0 END) AS Reprogramados
FROM Turnos t
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
WHERE t.Activo = 1
  AND t.MedicoID = @medicoId
  AND YEAR(t.Fecha) = @anio
  AND MONTH(t.Fecha) = @mes;
");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@anio", anio);
                datos.setearParametro("@mes", mes);

                datos.ejecutarLectura();

                int a = 0, n = 0, r = 0;

                if (datos.Lector.Read())
                {
                    a = datos.Lector["Atendidos"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["Atendidos"]);
                    n = datos.Lector["NoAsistio"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["NoAsistio"]);
                    r = datos.Lector["Reprogramados"] == DBNull.Value ? 0 : Convert.ToInt32(datos.Lector["Reprogramados"]);
                }

                return (a, n, r);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool PacienteTieneTurnoEseDia(int pacienteId, DateTime fecha)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
SELECT COUNT(1)
FROM Turnos
WHERE PacienteID = @pacienteId
  AND Fecha = @fecha
  AND Activo = 1
");
                datos.setearParametro("@pacienteId", pacienteId);
                datos.setearParametro("@fecha", fecha.Date);

                return Convert.ToInt32(datos.ejecutarScalar()) > 0;
            }
            finally { datos.cerrarConexion(); }
        }
        public EstadisticasMedicoMes ObtenerEstadisticasMes(int medicoId, int anio, int mes)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
SELECT 
    SUM(CASE WHEN et.Nombre = 'Atendido' THEN 1 ELSE 0 END) AS Atendidos,
    SUM(CASE WHEN et.Nombre IN ('No Asistió','No Asistio') THEN 1 ELSE 0 END) AS NoAsistio,
    SUM(CASE WHEN et.Nombre = 'Reprogramado' THEN 1 ELSE 0 END) AS Reprogramados
FROM Turnos t
INNER JOIN EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
WHERE t.MedicoID = @medicoId
  AND YEAR(t.Fecha) = @anio
  AND MONTH(t.Fecha) = @mes
  AND t.Activo = 1;
");

                datos.setearParametro("@medicoId", medicoId);
                datos.setearParametro("@anio", anio);
                datos.setearParametro("@mes", mes);

                datos.ejecutarLectura();

                EstadisticasMedicoMes est = new EstadisticasMedicoMes();

                if (datos.Lector.Read())
                {
                    est.Atendidos = datos.Lector["Atendidos"] != DBNull.Value ? (int)datos.Lector["Atendidos"] : 0;
                    est.NoAsistio = datos.Lector["NoAsistio"] != DBNull.Value ? (int)datos.Lector["NoAsistio"] : 0;
                    est.Reprogramados = datos.Lector["Reprogramados"] != DBNull.Value ? (int)datos.Lector["Reprogramados"] : 0;
                }

                return est;
            }
            finally
            {
                datos.cerrarConexion();
            }
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