using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Evolucion
    {
        public int EvolucionID { get; set; }
        public int PacienteID { get; set; }
        public int MedicoID { get; set; }
        public int TurnoID { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

       
        public string PacienteNombre { get; set; }
        public string MedicoNombre { get; set; }
        public DateTime TurnoFecha { get; set; }
        public TimeSpan TurnoHoraInicio { get; set; }
        public TimeSpan TurnoHoraFin { get; set; }
    }
}
