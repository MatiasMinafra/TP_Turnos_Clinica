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

            return datos.AltaConSP(
                pacienteId,
                especialidadId,
                medicoId,
                fecha,
                horaInicio,
                motivo,
                importe,
                medioPago);
        }


        public List<OpcionTurno> SugerirTurnos(int especialidadId, DateTime fecha, string franja)
        {
            List<OpcionTurno> lista = new List<OpcionTurno>();

            var medicos = datos.MedicosPorEspecialidad(especialidadId);

            
            TimeSpan desde = new TimeSpan(8, 0, 0);
            TimeSpan hasta = new TimeSpan(12, 0, 0);

            if (franja == "TARDE")
            {
                desde = new TimeSpan(14, 0, 0);
                hasta = new TimeSpan(18, 0, 0);
            }
            else if (franja == "NOCHE")
            {
                desde = new TimeSpan(19, 0, 0);
                hasta = new TimeSpan(22, 0, 0);
            }

            byte diaSemana = (byte)fecha.DayOfWeek;

            foreach (var m in medicos)
            {
                var rangos = datos.RangosLaborales(m.MedicoID, diaSemana);
                if (rangos == null || rangos.Count == 0)
                    continue;

                var ocupadas = datos.HorasOcupadas(m.MedicoID, fecha);

                foreach (var r in rangos)
                {
                    TimeSpan inicio = r.Inicio > desde ? r.Inicio : desde;
                    TimeSpan fin = r.Fin < hasta ? r.Fin : hasta;

                    if (fin <= inicio) continue;

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

        public List<DtoTurnoDia> ListarDelDia(DateTime fecha)
        {
            return datos.ListarDelDia(fecha);
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

        public void ReprogramarTurno(int turnoId, DateTime nuevaFecha, TimeSpan nuevaHoraInicio, int nuevoMedicoId)
        {
            TurnosDatos datos = new TurnosDatos();
            datos.ReprogramarTurno(turnoId, nuevaFecha, nuevaHoraInicio, nuevoMedicoId);
        }

        public int ObtenerEspecialidadDelTurno(int turnoId)
        {
            return datos.ObtenerEspecialidadDelTurno(turnoId);
        }

        
        public void MarcarNoAsistio(int turnoId)
        {
            datos.MarcarNoAsistio(turnoId);
        }

    }
}