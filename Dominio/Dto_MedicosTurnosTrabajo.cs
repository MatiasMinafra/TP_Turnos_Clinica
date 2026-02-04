using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Dto_MedicosTurnosTrabajo
    {
        public int MedicoTurnoID { get; set; }
        public int MedicoID { get; set; }
        public int TurnoTrabajoID { get; set; }

        public byte DiaSemana { get; set; }
        public string DiaNombre { get; set; }

        public bool Activo { get; set; }

        public string TurnoNombre { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
