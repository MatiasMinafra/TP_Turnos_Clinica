using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string UsuarioNombre { get; set; } 
        public string Password { get; set; } 
        public string Nombre { get; set; } 
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int RolID { get; set; }
        public int? MedicoID { get; set; }
        public bool Activo { get; set; }
        public System.DateTime FechaAlta { get; set; }
        
        public Rol rol { get; set; }
    }
}
