using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TurnoHistorialPaciente
    {
        public int TurnoID { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public string Especialidad { get; set; }
        public string Medico { get; set; }

        public string EstadoTurno { get; set; }
        public string EstadoPago { get; set; }

        public bool TieneEvolucion { get; set; }
        public DateTime? FechaEvolucion { get; set; }
        public string DescripcionEvolucion { get; set; }
    }
}
