using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Paciente
    {
        public int PacienteID { get; set; }
        public string DNI { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public char? Sexo { get; set; }              
        public string Nacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; } 

        public string Telefono { get; set; }
        public string Email { get; set; }            
        public string Ciudad { get; set; }
        public string Direccion { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
