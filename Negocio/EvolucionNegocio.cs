using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionesBD;
using Dominio;

namespace Negocio
{
    public class EvolucionNegocio
    {
        private readonly EvolucionDatos datos = new EvolucionDatos();

        
        private bool Eq(string a, string b)
            => string.Equals((a ?? "").Trim(), (b ?? "").Trim(), StringComparison.OrdinalIgnoreCase);

   
        public List<TurnoHistorialPaciente> HistorialPaciente(int pacienteId)
        {
            if (pacienteId <= 0)
                throw new Exception("Paciente inválido.");

            return datos.ListarHistorialPaciente(pacienteId);
        }

      
        public bool PuedeEvolucionar(string estadoTurno, string estadoPago)
        {
           
            if (Eq(estadoTurno, "Cancelado")) return false;
            if (Eq(estadoTurno, "No Asistio")) return false;
            if (Eq(estadoTurno, "No Asistió")) return false;
            if (Eq(estadoTurno, "Cerrado")) return false;
            if (Eq(estadoTurno, "Nuevo")) return false;

            
            bool pagoOk = Eq(estadoPago, "Confirmado") || Eq(estadoPago, "Aprobado");
            return pagoOk;
        }

   
        public void RegistrarEvolucion(Usuario usuarioLogueado, int turnoId, string descripcion)
        {
            if (usuarioLogueado == null)
                throw new Exception("Debe iniciar sesión.");

           
            if (usuarioLogueado.RolID != RolesIds.MEDICO)
                throw new Exception("Solo un médico puede registrar evoluciones.");

           
            if (!usuarioLogueado.MedicoID.HasValue || usuarioLogueado.MedicoID.Value <= 0)
                throw new Exception("Este usuario no tiene médico asociado.");

           
            if (turnoId <= 0)
                throw new Exception("Turno inválido.");

            
            descripcion = (descripcion ?? "").Trim();
            if (descripcion.Length < 10)
                throw new Exception("La evolución es muy corta. Detalle un poco más la atención ");

           
            var info = datos.ObtenerEstadosTurnoPago(turnoId);

            if (info.MedicoID <= 0)
                throw new Exception("El turno no existe o está inactivo.");

           
            if (info.MedicoID != usuarioLogueado.MedicoID.Value)
                throw new Exception("No puede evolucionar un turno que no pertenece a su agenda.");

           
            if (Eq(info.EstadoTurno, "Cancelado") ||
                Eq(info.EstadoTurno, "Nuevo") ||
                Eq(info.EstadoTurno, "Cerrado") ||
                Eq(info.EstadoTurno, "No Asistio") ||
                Eq(info.EstadoTurno, "No Asistió"))
            {
                throw new Exception("No se puede registrar evolución para este estado de turno.");
            }

           
            bool pagoOk = Eq(info.EstadoPago, "Confirmado") || Eq(info.EstadoPago, "Aprobado");
            if (!pagoOk)
                throw new Exception("No se puede evolucionar: el pago no está confirmado/aprobado.");

           
            if (datos.ExistePorTurno(turnoId))
                throw new Exception("Este turno ya tiene una evolución registrada.");

           
            var evo = new Evolucion
            {
                TurnoID = turnoId,
                PacienteID = info.PacienteID,
                MedicoID = usuarioLogueado.MedicoID.Value,
                Descripcion = descripcion
            };

            
            datos.RegistrarEvolucionYMarcarEstado(evo, "Atendido");
        }

        
        public Evolucion ObtenerDetalleTurno(int turnoId)
        {
            if (turnoId <= 0) return null;
            return datos.ObtenerDetalleTurno(turnoId);
        }
    }
}