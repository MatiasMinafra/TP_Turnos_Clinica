using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MedicoTurnoTrabajoNegocio
    {
       
            private readonly MedicoTurnoTrabajoDatos datos = new MedicoTurnoTrabajoDatos();

            public List<Dto_MedicosTurnosTrabajo> ListarPorMedico(int medicoId, bool soloActivos = true)
            {
                if (medicoId <= 0) return new List<Dto_MedicosTurnosTrabajo>();
                return datos.ListarPorMedico(medicoId, soloActivos);
            }

            public MedicosTurnosTrabajo ObtenerPorId(int medicoTurnoId)
            {
                if (medicoTurnoId <= 0) return null;
                return datos.ObtenerPorId(medicoTurnoId);
            }



        public void Asignar(int medicoId, int turnoTrabajoId, int especialidadId, byte diaSemana)
        {
            if (medicoId <= 0)
                throw new Exception("Médico inválido.");

            if (turnoTrabajoId <= 0)
                throw new Exception("Turno de trabajo inválido.");

            if (especialidadId <= 0)
                throw new Exception("Especialidad inválida.");

            if (diaSemana < 1 || diaSemana > 7)
                throw new Exception("Día inválido. (1=Lun ... 7=Dom)");

            if (!datos.MedicoTieneEspecialidad(medicoId, especialidadId))
            {
                datos.AsegurarEspecialidad(medicoId, especialidadId);
            }

            TurnoTrabajo turno = datos.ObtenerTurnoTrabajo(turnoTrabajoId);
            if (turno == null)
                throw new Exception("No existe el turno de trabajo seleccionado.");

            if (!turno.Activo)
                throw new Exception("El turno de trabajo está inactivo.");

            ValidarHorasTurno(turno.HoraInicio, turno.HoraFin);

           
            if (datos.ExisteBloque(medicoId, diaSemana, turnoTrabajoId))
            {
                datos.ActualizarBloque(medicoId, turnoTrabajoId, especialidadId, diaSemana);
                return; 
            }

            
            if (datos.Existe(medicoId, turnoTrabajoId, especialidadId, diaSemana))
                throw new Exception("Ese horario ya está asignado para ese médico, especialidad y día.");

            if (datos.ExisteSolapado(medicoId, diaSemana, turno.HoraInicio, turno.HoraFin))
                throw new Exception("Ese turno se superpone con otro horario ya asignado a ese médico en ese día.");

            datos.Agregar(medicoId, turnoTrabajoId, especialidadId, diaSemana);
        }

        private void ValidarHorasTurno(TimeSpan inicio, TimeSpan fin)
        {
            if (inicio == TimeSpan.Zero)
                throw new Exception("Hora inicio inválida (no puede ser 00:00).");

            if (fin == TimeSpan.Zero)
                throw new Exception("Hora fin inválida (no puede ser 00:00).");

            if (fin <= inicio)
                throw new Exception("La hora fin debe ser mayor a la hora inicio.");

            
            TimeSpan min = new TimeSpan(6, 0, 0);   
            TimeSpan max = new TimeSpan(23, 59, 0); 

            if (inicio < min || inicio > max)
                throw new Exception("La hora de inicio debe estar entre 06:00 y 23:59.");

            if (fin < min || fin > max)
                throw new Exception("La hora de fin debe estar entre 06:00 y 23:59.");

            
            if (inicio.Minutes != 0 || fin.Minutes != 0)
                throw new Exception("Las horas deben ser en punto (ej: 08:00, 12:00).");

       
            TimeSpan duracion = fin - inicio;
            if (duracion < TimeSpan.FromHours(1))
                throw new Exception("El turno de trabajo debe durar al menos 1 hora.");
        }

        public void Activar(int medicoTurnoId) => datos.Activar(medicoTurnoId);


        

        public void Desactivar(int medicoTurnoId) => datos.Desactivar(medicoTurnoId);
        }
    }

