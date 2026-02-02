using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConexionesBD.TurnosDatos;


namespace Negocio
{
    public class TurnosNegocio
    {
        private TurnosDatos datos = new TurnosDatos();

        public List<Turno> ListarTurnosDelDia(DateTime fecha)
        {
            return datos.ListarDelDia(fecha);
        }

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

        public List<OpcionTurno> SugerirTurnos(int especialidadId, DateTime desdeFecha, int cantidad = 3)
        {
            if (especialidadId <= 0)
                throw new Exception("Especialidad inválida.");

            if (cantidad <= 0) cantidad = 3;

            List<OpcionTurno> sugerencias = new List<OpcionTurno>();

            
            var medicos = datos.MedicosPorEspecialidad(especialidadId);

           
            for (int d = 0; d < 14 && sugerencias.Count < cantidad; d++)
            {
                DateTime fecha = desdeFecha.Date.AddDays(d);

               
                byte diaSemana = (byte)(((int)fecha.DayOfWeek + 6) % 7 + 1);

                foreach (var m in medicos)
                {
                    if (sugerencias.Count >= cantidad) break;

                    
                    var rangos = datos.RangosLaborales(m.MedicoID, diaSemana);
                    if (rangos == null || rangos.Count == 0) continue;

                    
                    var ocupadas = datos.HorasOcupadas(m.MedicoID, fecha);

                    foreach (var r in rangos)
                    {
                      
                        for (TimeSpan h = r.Inicio; h + TimeSpan.FromHours(1) <= r.Fin; h = h.Add(TimeSpan.FromHours(1)))
                        {
                            if (sugerencias.Count >= cantidad) break;

                          
                            if (ocupadas != null && ocupadas.Contains(h)) continue;

                            
                            DateTime fechaHora = fecha.Date + h;
                            if (fechaHora < DateTime.Now) continue;

                            sugerencias.Add(new OpcionTurno
                            {
                                MedicoID = m.MedicoID,
                                Medico = m.Medico,
                                Matricula = m.Matricula,
                                Fecha = fecha.Date,
                                HoraInicio = h,
                                HoraFin = h.Add(TimeSpan.FromHours(1))
                            });

                            
                            break;
                        }

                        if (sugerencias.Count >= cantidad) break;
                    }
                }
            }

            return sugerencias;
        }
    }
}
