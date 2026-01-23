using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MedicoNegocio
    {
        
            private readonly MedicoDatos datos = new MedicoDatos();

        public List<Medico> Listar(string filtro = "", bool soloActivos = true)
        {
            return datos.Listar(filtro, soloActivos);
        }

        public Medico ObtenerPorId(int id)
            {
                if (id <= 0) throw new Exception("Id inválido.");
                return datos.ObtenerPorId(id);
            }

            public void Agregar(Medico m)
            {
                Validar(m);
                datos.Agregar(m);
            }

            public void Modificar(Medico m)
            {
                if (m.MedicoID <= 0) throw new Exception("Id inválido.");
                Validar(m);
                datos.Modificar(m);
            }

            public void Desactivar(int id)
            {
                if (id <= 0) throw new Exception("Id inválido.");
                datos.Desactivar(id);
            }

            private void Validar(Medico m)
            {
            if (m == null) throw new Exception("Médico inválido.");

            
            m.DNI = (m.DNI ?? "").Trim();
            m.Matricula = (m.Matricula ?? "").Trim();
            m.Nombre = (m.Nombre ?? "").Trim();
            m.Apellido = (m.Apellido ?? "").Trim();
            m.Email = (m.Email ?? "").Trim();

            m.Telefono = string.IsNullOrWhiteSpace(m.Telefono) ? null : m.Telefono.Trim();
            m.Nacionalidad = string.IsNullOrWhiteSpace(m.Nacionalidad) ? null : m.Nacionalidad.Trim();
            m.Ciudad = string.IsNullOrWhiteSpace(m.Ciudad) ? null : m.Ciudad.Trim();
            m.Direccion = string.IsNullOrWhiteSpace(m.Direccion) ? null : m.Direccion.Trim();

            
            if (string.IsNullOrWhiteSpace(m.DNI)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(m.Matricula)) throw new Exception("La matrícula es obligatoria.");
            if (string.IsNullOrWhiteSpace(m.Nombre)) throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(m.Apellido)) throw new Exception("El Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(m.Email)) throw new Exception("El Email es obligatorio.");

            
            if (m.DNI.Length > 15) throw new Exception("El DNI no puede superar 15 caracteres.");
            if (m.Matricula.Length > 30) throw new Exception("La Matrícula no puede superar 30 caracteres.");
            if (m.Nombre.Length > 50) throw new Exception("El Nombre no puede superar 50 caracteres.");
            if (m.Apellido.Length > 50) throw new Exception("El Apellido no puede superar 50 caracteres.");
            if (m.Email.Length > 120) throw new Exception("El Email no puede superar 120 caracteres.");

            if (m.Telefono != null && m.Telefono.Length > 30) throw new Exception("El Teléfono no puede superar 30 caracteres.");
            if (m.Nacionalidad != null && m.Nacionalidad.Length > 60) throw new Exception("La Nacionalidad no puede superar 60 caracteres.");
            if (m.Ciudad != null && m.Ciudad.Length > 100) throw new Exception("La Ciudad no puede superar 100 caracteres.");
            if (m.Direccion != null && m.Direccion.Length > 150) throw new Exception("La Dirección no puede superar 150 caracteres.");

           
            if (!m.DNI.All(char.IsDigit))
                throw new Exception("El DNI debe contener solo números.");

         
            if (!m.Email.Contains("@") || !m.Email.Contains("."))
                throw new Exception("El Email no tiene un formato válido.");

            
            if (m.Sexo.HasValue)
            {
                char s = char.ToUpperInvariant(m.Sexo.Value);
                if (s != 'M' && s != 'F' && s != 'X')
                    throw new Exception("Sexo inválido. Valores válidos: M, F, X.");
                m.Sexo = s;
            }

            
            int? idExcluido = m.MedicoID > 0 ? (int?)m.MedicoID : null;

            if (datos.ExisteDni(m.DNI, idExcluido))
                throw new Exception("Ya existe un médico con ese DNI.");

            if (datos.ExisteMatricula(m.Matricula, idExcluido))
                throw new Exception("Ya existe un médico con esa Matrícula.");

            if (datos.ExisteEmail(m.Email, idExcluido))
                throw new Exception("Ya existe un médico con ese Email.");
        }
        }
    }

