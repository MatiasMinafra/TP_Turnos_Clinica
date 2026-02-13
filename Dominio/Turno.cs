using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Turno
    {
        public int TurnoID { get; set; }

        public int PacienteID { get; set; }
        public string PacienteNombre { get; set; }

        public int MedicoID { get; set; }
        public string MedicoNombre { get; set; }

        public int EspecialidadID { get; set; }
        public string EspecialidadNombre { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public int EstadoTurnoID { get; set; }
        public string EstadoTurno { get; set; }

        public string MotivoConsulta { get; set; }
        public string Diagnostico { get; set; }

        public bool Activo { get; set; }

        public string EstadoPago { get; set; }
    }
}
