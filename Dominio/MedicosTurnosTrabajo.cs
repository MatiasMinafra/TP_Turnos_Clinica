using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class MedicosTurnosTrabajo
    {
        public int MedicoTurnoID { get; set; }
        public int MedicoID { get; set; }
        public int TurnoTrabajoID { get; set; }
        public byte DiaSemana { get; set; }
        public bool Activo { get; set; }

    }
}
