using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace ConexionesBD
{
    public class EvolucionDatos
    {
        public List<TurnoHistorialPaciente> ListarHistorialPaciente(int pacienteId)
        {
            var lista = new List<TurnoHistorialPaciente>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
SELECT
    t.TurnoID,
    t.Fecha,
    t.HoraInicio,
    t.HoraFin,
    (m.Apellido + ', ' + m.Nombre) AS Medico,
    e2.Nombre AS Especialidad,
    et.Nombre AS EstadoTurno,
    ep.Nombre AS EstadoPago,

    CASE WHEN ev.EvolucionID IS NULL THEN 0 ELSE 1 END AS TieneEvolucion,
    ev.Fecha AS FechaEvolucion,
    ev.Descripcion AS DescripcionEvolucion
FROM dbo.Turnos t
INNER JOIN dbo.Medicos m ON m.MedicoID = t.MedicoID
INNER JOIN dbo.Especialidades e2 ON e2.EspecialidadID = t.EspecialidadID
INNER JOIN dbo.EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
INNER JOIN dbo.Pagos p ON p.TurnoID = t.TurnoID
INNER JOIN dbo.EstadosPago ep ON ep.EstadoPagoID = p.EstadoPagoID
LEFT JOIN dbo.Evoluciones ev ON ev.TurnoID = t.TurnoID
WHERE t.PacienteID = @pacienteId
ORDER BY t.Fecha DESC, t.HoraInicio DESC;
");
            datos.setearParametro("@pacienteId", pacienteId);

            datos.ejecutarLectura();
            while (datos.Lector.Read())
            {
                lista.Add(new TurnoHistorialPaciente
                {
                    TurnoID = Convert.ToInt32(datos.Lector["TurnoID"]),
                    Fecha = Convert.ToDateTime(datos.Lector["Fecha"]),
                    HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                    HoraFin = (TimeSpan)datos.Lector["HoraFin"],

                    Medico = datos.Lector["Medico"].ToString(),
                    Especialidad = datos.Lector["Especialidad"].ToString(),
                    EstadoTurno = datos.Lector["EstadoTurno"].ToString(),
                    EstadoPago = datos.Lector["EstadoPago"].ToString(),

                    TieneEvolucion = Convert.ToInt32(datos.Lector["TieneEvolucion"]) == 1,
                    FechaEvolucion = datos.Lector["FechaEvolucion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(datos.Lector["FechaEvolucion"]),
                    DescripcionEvolucion = datos.Lector["DescripcionEvolucion"] == DBNull.Value ? "" : datos.Lector["DescripcionEvolucion"].ToString()
                });
            }

            return lista;
        }

        public bool ExistePorTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("SELECT COUNT(1) FROM dbo.Evoluciones WHERE TurnoID = @turnoId;");
            datos.setearParametro("@turnoId", turnoId);
            datos.ejecutarLectura();
            if (datos.Lector.Read())
                return Convert.ToInt32(datos.Lector[0]) > 0;
            return false;
        }

        public (int MedicoID, int PacienteID, string EstadoTurno, string EstadoPago) ObtenerEstadosTurnoPago(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta(@"
SELECT
    t.MedicoID,
    t.PacienteID,
    et.Nombre AS EstadoTurno,
    ep.Nombre AS EstadoPago
FROM dbo.Turnos t
INNER JOIN dbo.EstadosTurno et ON et.EstadoTurnoID = t.EstadoTurnoID
INNER JOIN dbo.Pagos pa ON pa.TurnoID = t.TurnoID
INNER JOIN dbo.EstadosPago ep ON ep.EstadoPagoID = pa.EstadoPagoID
WHERE t.TurnoID = @turnoId AND t.Activo = 1;
");
            datos.setearParametro("@turnoId", turnoId);
            datos.ejecutarLectura();

            if (!datos.Lector.Read())
                return (0, 0, null, null);

            return (
                Convert.ToInt32(datos.Lector["MedicoID"]),
                Convert.ToInt32(datos.Lector["PacienteID"]),
                datos.Lector["EstadoTurno"].ToString(),
                datos.Lector["EstadoPago"].ToString()
            );
        }

        public Evolucion ObtenerDetalleTurno(int turnoId)
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta(@"
SELECT
    t.TurnoID, t.PacienteID, t.MedicoID,
    t.Fecha AS TurnoFecha, t.HoraInicio, t.HoraFin,
    (p.Apellido + ', ' + p.Nombre) AS PacienteNombre,
    (m.Apellido + ', ' + m.Nombre) AS MedicoNombre
FROM dbo.Turnos t
INNER JOIN dbo.Pacientes p ON p.PacienteID = t.PacienteID
INNER JOIN dbo.Medicos m ON m.MedicoID = t.MedicoID
WHERE t.TurnoID = @turnoId AND t.Activo = 1;
");
            datos.setearParametro("@turnoId", turnoId);
            datos.ejecutarLectura();

            if (!datos.Lector.Read())
                return null;

            return new Evolucion
            {
                TurnoID = Convert.ToInt32(datos.Lector["TurnoID"]),
                PacienteID = Convert.ToInt32(datos.Lector["PacienteID"]),
                MedicoID = Convert.ToInt32(datos.Lector["MedicoID"]),
                TurnoFecha = Convert.ToDateTime(datos.Lector["TurnoFecha"]),
                TurnoHoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                TurnoHoraFin = (TimeSpan)datos.Lector["HoraFin"],
                PacienteNombre = datos.Lector["PacienteNombre"].ToString(),
                MedicoNombre = datos.Lector["MedicoNombre"].ToString()
            };
        }

        public void RegistrarEvolucionYMarcarEstado(Evolucion e, string nuevoEstadoTurno)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
DECLARE @EstadoTurnoID INT = (SELECT TOP 1 EstadoTurnoID FROM dbo.EstadosTurno WHERE Nombre = @estadoNombre);
IF @EstadoTurnoID IS NULL
    THROW 50030, 'No existe el estado indicado en EstadosTurno.', 1;

BEGIN TRY
    BEGIN TRAN;

    INSERT INTO dbo.Evoluciones (PacienteID, MedicoID, TurnoID, Descripcion)
    VALUES (@pacienteId, @medicoId, @turnoId, @desc);

    UPDATE dbo.Turnos
    SET EstadoTurnoID = @EstadoTurnoID
    WHERE TurnoID = @turnoId;

    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    THROW;
END CATCH
");
            datos.setearParametro("@estadoNombre", nuevoEstadoTurno);
            datos.setearParametro("@pacienteId", e.PacienteID);
            datos.setearParametro("@medicoId", e.MedicoID);
            datos.setearParametro("@turnoId", e.TurnoID);
            datos.setearParametro("@desc", e.Descripcion);

            datos.ejecutarAccion();
        }
    }
}
