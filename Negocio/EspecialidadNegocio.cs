using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EspecialidadNegocio
    {
        
            private readonly EspecialidadDatos datos = new EspecialidadDatos();

            public List<Especialidad> Listar(string filtro = "", bool soloActivos = true)
            {
                return datos.Listar(filtro, soloActivos);
            }

            public Especialidad ObtenerPorId(int id)
            {
                if (id <= 0) throw new Exception("Id inválido.");
                return datos.ObtenerPorId(id);
            }

            public int Agregar(Especialidad e)
            {
                Validar(e);
                if (datos.ExisteNombre(e.Nombre))
                    throw new Exception("Ya existe una especialidad con ese nombre.");
                return datos.Agregar(e);
            }

            public void Modificar(Especialidad e)
            {
                if (e.EspecialidadID <= 0) throw new Exception("Id inválido.");
                Validar(e);
                if (datos.ExisteNombre(e.Nombre, e.EspecialidadID))
                    throw new Exception("Ya existe una especialidad con ese nombre.");
                datos.Modificar(e);
            }

            public void Desactivar(int id)
            {
                if (id <= 0) throw new Exception("Id inválido.");
                datos.Desactivar(id);
            }

            public void Activar(int id)
            {
                if (id <= 0) throw new Exception("Id inválido.");
                datos.Activar(id);
            }

            private void Validar(Especialidad e)
            {
                if (e == null) throw new Exception("Especialidad inválida.");
                if (string.IsNullOrWhiteSpace(e.Nombre))
                    throw new Exception("El nombre de la especialidad es obligatorio.");
                e.Nombre = e.Nombre.Trim();
            }

        public List<Especialidad> ListarActivas()
        {
            return datos.ListarActivas();
        }

        public List<Especialidad> ListarPorMedico(int medicoId)
        {
            if (medicoId <= 0) throw new Exception("Médico inválido.");
            return datos.ListarPorMedico(medicoId);
        }

        public void AsignarAMedico(int medicoId, int especialidadId)
        {
            if (medicoId <= 0) throw new Exception("Médico inválido.");
            if (especialidadId <= 0) throw new Exception("Especialidad inválida.");

            if (datos.ExisteRelacion(medicoId, especialidadId))
                throw new Exception("Esa especialidad ya está asignada al médico.");

            datos.AsignarAMedico(medicoId, especialidadId);
        }

        public void QuitarDeMedico(int medicoId, int especialidadId)
        {
            if (medicoId <= 0) throw new Exception("Médico inválido.");
            if (especialidadId <= 0) throw new Exception("Especialidad inválida.");
            datos.QuitarDeMedico(medicoId, especialidadId);
        }


    }

  }
