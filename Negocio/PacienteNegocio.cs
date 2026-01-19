using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PacienteNegocio
    {
        private readonly PacienteDatos datos = new PacienteDatos();

        public List<Paciente> Listar(string filtro = "")
        {
            return datos.Listar(filtro);
        }

        public void Desactivar(int idPaciente)
        {
            if (idPaciente <= 0)
                throw new Exception("Id inválido");

            datos.Desactivar(idPaciente);
        }
        public Paciente ObtenerPorId(int id)
        {
            if (id <= 0) throw new Exception("Id inválido.");
            return datos.ObtenerPorId(id);
        }
        public void Agregar(Paciente p)
        {
            Validar(p);
            datos.Agregar(p);
        }
        public void Modificar(Paciente p)
        {
            if (p.PacienteID <= 0) throw new Exception("Id inválido.");
            Validar(p);
            datos.Modificar(p);
        }
        private void Validar(Paciente p)
        {
            if (p == null) throw new Exception("Paciente inválido.");
            if (string.IsNullOrWhiteSpace(p.DNI)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.Nombre)) throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.Apellido)) throw new Exception("El Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.Email)) throw new Exception("El Email es obligatorio.");
        }
    }
}
