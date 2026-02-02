using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TurnoTrabajoNegocio
    {
        private readonly TurnoTrabajoDatos datos = new TurnoTrabajoDatos();

        public System.Collections.Generic.List<TurnoTrabajo> Listar(string filtro = "", bool soloActivos = true)
           => datos.Listar(filtro, soloActivos);

        public TurnoTrabajo ObtenerPorId(int id)
        {
            if (id <= 0) throw new Exception("Id inválido.");
            return datos.ObtenerPorId(id);
        }

        public int Agregar(TurnoTrabajo t)
        {
            Validar(t);
            if (datos.ExisteNombre(t.Nombre))
                throw new Exception("Ya existe un turno de trabajo con ese nombre.");
            return datos.Agregar(t);
        }

        public void Modificar(TurnoTrabajo t)
        {
            if (t.TurnoTrabajoID <= 0) throw new Exception("Id inválido.");
            Validar(t);
            if (datos.ExisteNombre(t.Nombre, t.TurnoTrabajoID))
                throw new Exception("Ya existe un turno de trabajo con ese nombre.");
            datos.Modificar(t);
        }

        public void Activar(int id)
        {
            if (id <= 0) throw new Exception("Id inválido.");
            datos.Activar(id);
        }

        public void Desactivar(int id)
        {
            if (id <= 0) throw new Exception("Id inválido.");
            datos.Desactivar(id);
        }

        private void Validar(TurnoTrabajo t)
        {
            if (t == null) throw new Exception("Turno de trabajo inválido.");
            if (string.IsNullOrWhiteSpace(t.Nombre))
                throw new Exception("El nombre es obligatorio.");
            t.Nombre = t.Nombre.Trim();

            if (t.HoraInicio == default(TimeSpan))
                throw new Exception("Hora inicio inválida.");
            if (t.HoraFin == default(TimeSpan))
                throw new Exception("Hora fin inválida.");

            if (t.HoraFin <= t.HoraInicio)
                throw new Exception("La hora fin debe ser mayor a la hora inicio.");
        }
    }
}
