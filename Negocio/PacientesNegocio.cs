using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PacientesNegocio
    {
        private readonly PacientesDatos datos = new PacientesDatos();

        public List<Pacientes> Listar(string filtro = "")
        {
            return datos.Listar(filtro);
        }

        public void Desactivar(int idPaciente)
        {
            if (idPaciente <= 0)
                throw new Exception("Id inválido");

            datos.Desactivar(idPaciente);
        }

    }
}
