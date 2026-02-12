using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DtoTurnoMail
    {
        public int TurnoID { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public string PacienteNombre { get; set; }
        public string PacienteEmail { get; set; }

        public string MedicoNombre { get; set; }
        public string Especialidad { get; set; }

        public string MedioPago { get; set; }
        public decimal Importe { get; set; }

        public string MotivoConsulta { get; set; }
    }
}
