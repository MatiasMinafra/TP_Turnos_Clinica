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

            public void Asignar(int medicoId, int turnoTrabajoId, byte diaSemana)
            {
                if (medicoId <= 0) throw new Exception("Médico inválido.");
                if (turnoTrabajoId <= 0) throw new Exception("Turno de trabajo inválido.");
                if (diaSemana < 1 || diaSemana > 7) throw new Exception("Día inválido.");

                if (datos.Existe(medicoId, turnoTrabajoId, diaSemana))
                    throw new Exception("Ese horario ya está asignado para ese médico y día.");

                datos.Agregar(medicoId, turnoTrabajoId, diaSemana);
            }

            public void Activar(int medicoTurnoId) => datos.Activar(medicoTurnoId);

            public void Desactivar(int medicoTurnoId) => datos.Desactivar(medicoTurnoId);
        }
    }

