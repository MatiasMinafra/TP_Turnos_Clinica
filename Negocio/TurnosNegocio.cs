using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio
{
  
    public class TurnosNegocio
    {
        private TurnosDatos datos = new TurnosDatos();
        private readonly EmailServicio emailServicio = new EmailServicio();

        public int AltaTurno(
     int pacienteId,
     int especialidadId,
     int medicoId,
     DateTime fecha,
     TimeSpan horaInicio,
     string motivo,
     decimal importe,
     string medioPago = "MERCADOPAGO")
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("Debe indicar el motivo de la consulta.");

            if (importe <= 0)
                throw new Exception("El importe debe ser mayor a 0.");

            
            if (datos.PacienteTieneTurnoEseDia(pacienteId, fecha))
                throw new Exception("El paciente ya tiene un turno asignado para esa fecha (máximo 1 por día).");

            int turnoId = datos.AltaConSP(
                pacienteId,
                especialidadId,
                medicoId,
                fecha,
                horaInicio,
                motivo,
                importe,
                medioPago);

            try
            {
                var dto = datos.ObtenerDatosMailTurno(turnoId);

                if (string.IsNullOrWhiteSpace(dto.PacienteEmail))
                    throw new Exception("El paciente NO tiene email cargado.");

                emailServicio.EnviarConfirmacionTurno(dto);
            }
            catch (Exception ex)
            {
                throw new Exception("El turno se creó, pero el envío de mail falló: " + ex.Message);
            }

            return turnoId;
        }


        public List<OpcionTurno> SugerirTurnos(int especialidadId, DateTime fecha, string franja)
        {
            var lista = new List<OpcionTurno>();

            
            int diaSemanaInt = ((int)fecha.DayOfWeek + 6) % 7 + 1; 
            byte diaSemana = (byte)diaSemanaInt;

            if (diaSemana == 7) 
                return lista;


            franja = (franja ?? "").Trim().ToUpperInvariant().Replace("Ñ", "N");

            TimeSpan desde;
            TimeSpan hasta;

            switch (franja)
            {
                case "MAÑANA":
                case "MANANA":
                    desde = new TimeSpan(8, 0, 0);
                    hasta = new TimeSpan(12, 0, 0);
                    break;

                case "TARDE":
                    desde = new TimeSpan(14, 0, 0);
                    hasta = new TimeSpan(18, 0, 0);
                    break;

                case "NOCHE":
                    desde = new TimeSpan(19, 0, 0);
                    hasta = new TimeSpan(22, 0, 0);
                    break;

                default:
                    throw new Exception("No existe franja horaria. (MAÑANA/TARDE/NOCHE)");
            }

            var medicos = datos.MedicosPorEspecialidad(especialidadId);

            foreach (var m in medicos)
            {
                
                var rangos = datos.RangosLaborales(m.MedicoID, diaSemana, especialidadId);
                if (rangos == null || rangos.Count == 0)
                    continue;

                
                var ocupadas = datos.HorasOcupadas(m.MedicoID, fecha.Date);

                foreach (var r in rangos)
                {
                   
                    TimeSpan inicio = r.Inicio > desde ? r.Inicio : desde;
                    TimeSpan fin = r.Fin < hasta ? r.Fin : hasta;

                    if (fin <= inicio)
                        continue;

                    for (TimeSpan h = inicio; h + TimeSpan.FromHours(1) <= fin; h = h.Add(TimeSpan.FromHours(1)))
                    {
                        DateTime fechaHora = fecha.Date + h;
                        if (fechaHora < DateTime.Now) continue;

                        bool ocupado = ocupadas != null && ocupadas.Contains(h);

                        lista.Add(new OpcionTurno
                        {
                            MedicoID = m.MedicoID,
                            Medico = m.Medico,
                            Matricula = m.Matricula,
                            Fecha = fecha.Date,
                            HoraInicio = h,
                            HoraFin = h.Add(TimeSpan.FromHours(1)),
                            Ocupado = ocupado
                        });
                    }
                }
            }

            return lista
                .OrderBy(x => x.Medico)
                .ThenBy(x => x.HoraInicio)
                .ToList();
        }
        public List<Turno> ListarMisTurnos(int medicoId, DateTime desde, DateTime hasta)
        {
            if (medicoId <= 0)
                throw new Exception("Médico inválido.");

            return datos.ListarPorMedico(medicoId, desde, hasta);
        }

        public List<DtoTurnoDia> ListarDelDia(DateTime fecha, string dniPaciente = "", bool incluirCancelados = false)
        {
            if (fecha == DateTime.MinValue)
                throw new Exception("Fecha inválida.");

            return datos.ListarDelDia(fecha, dniPaciente, incluirCancelados);
        }

        public void ConfirmarPago(int turnoId, string comprobante)
        {
            if (!string.IsNullOrWhiteSpace(comprobante) && comprobante.Length > 200)
                throw new Exception("El comprobante no puede superar los 200 caracteres.");

            datos.ConfirmarPago(turnoId, comprobante);
        }

       
        public void ConfirmarPago(int turnoId)
        {
            ConfirmarPago(turnoId, null);
        }

        public void CerrarTurno(int turnoId, string diagnostico)
        {
           
            if (!string.IsNullOrWhiteSpace(diagnostico) && diagnostico.Length > 800)
                throw new Exception("El diagnóstico no puede superar 800 caracteres.");

            datos.CerrarTurno(turnoId, diagnostico);
        }

        public void CancelarTurno(int turnoId)
        {
            datos.CancelarTurno(turnoId);
        }

        public void ReprogramarTurno(int turnoId, DateTime nuevaFecha, TimeSpan nuevaHoraInicio, int nuevoMedicoId, string motivo)
        {
            if (turnoId <= 0) throw new Exception("Turno inválido.");
            if (nuevoMedicoId <= 0) throw new Exception("Médico inválido.");
            if (string.IsNullOrWhiteSpace(motivo)) throw new Exception("El motivo es obligatorio.");

           
            var antes = datos.ObtenerFechaHoraTurno(turnoId);

           
            datos.ReprogramarTurno(turnoId, nuevaFecha, nuevaHoraInicio, nuevoMedicoId, motivo.Trim());

         
            try
            {
                var dto = datos.ObtenerDatosMailTurno(turnoId);

                if (string.IsNullOrWhiteSpace(dto.PacienteEmail))
                    throw new Exception("El paciente NO tiene email cargado.");

                emailServicio.EnviarReprogramacionTurno(dto, antes.Fecha, antes.HoraInicio);
            }
            catch (Exception ex)
            {
               
                throw new Exception("Se reprogramó, pero el envío de mail falló: " + ex.Message);
            }
        }

        public int ObtenerEspecialidadDelTurno(int turnoId)
        {
            return datos.ObtenerEspecialidadDelTurno(turnoId);
        }

        public (DateTime Fecha, TimeSpan HoraInicio) ObtenerFechaHoraTurno(int turnoId)
        {
            return datos.ObtenerFechaHoraTurno(turnoId);
        }
        public void MarcarNoAsistio(int turnoId)
        {
            datos.MarcarNoAsistio(turnoId);
        }

        public List<Turno> ListarPorMedico(int medicoId, DateTime desde, DateTime hasta)
        {
            return datos.ListarPorMedico(medicoId, desde, hasta);
        }

        public (int Atendidos, int NoAsistio, int Reprogramados) StatsMedicoMes(int medicoId, int anio, int mes)
        {
            if (medicoId <= 0) throw new Exception("Médico inválido.");
            return datos.StatsMedicoMes(medicoId, anio, mes);
        }

        public EstadisticasMedicoMes ObtenerEstadisticasMes(int medicoId, int anio, int mes)
        {
            if (medicoId <= 0)
                throw new Exception("Médico inválido.");

            return datos.ObtenerEstadisticasMes(medicoId, anio, mes);
        }

    }
}