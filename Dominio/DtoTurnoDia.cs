using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DtoTurnoDia
    {
        public int TurnoID { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }

        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }

        public string EstadoTurno { get; set; }
        public string EstadoPago { get; set; }

        public decimal Importe { get; set; }
        public string MedioPago { get; set; }

        public string Dni { get; set; }
    }
}
