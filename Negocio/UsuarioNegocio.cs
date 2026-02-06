using ConexionesBD;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioNegocio
    {
        private UsuarioDatos datos = new UsuarioDatos();

        public Usuario Login(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                return null;

            return datos.Login(usuario.Trim(), password.Trim());
        }


        public Usuario ObtenerPorId(int id)
        {
            return datos.ObtenerPorId(id);
        }

        public List<Usuario> Listar(string filtro = "", bool soloActivos = true)
        {
            return datos.Listar(filtro, soloActivos);
        }

        public void BajaLogica(int id)
        {
            datos.BajaLogica(id);
        }

        public void Guardar(Usuario u)
        {
            Validar(u);

            if (u.UsuarioID > 0)
                datos.Modificar(u);
            else
                datos.Agregar(u);
        }

        private void Validar(Usuario u)
        {
            if (u == null) throw new Exception("Usuario inválido.");

            u.UsuarioNombre = (u.UsuarioNombre ?? "").Trim();
            if (string.IsNullOrWhiteSpace(u.UsuarioNombre))
                throw new Exception("Debe ingresar el nombre de usuario.");

            if (u.UsuarioID == 0 && string.IsNullOrWhiteSpace(u.Password))
                throw new Exception("Debe ingresar la contraseña.");

            
            if (datos.ExisteUsuario(u.UsuarioNombre, u.UsuarioID > 0 ? u.UsuarioID : (int?)null))
                throw new Exception("Ya existe un usuario con ese nombre.");

            bool esMedico = (u.RolID == RolesIds.MEDICO);

            if (esMedico)
            {
                if (!u.MedicoID.HasValue || u.MedicoID.Value <= 0)
                    throw new Exception("Si el rol es Médico, debe asociarse a un médico.");

                if (datos.ExisteUsuarioMedicoActivo(u.MedicoID.Value, u.UsuarioID > 0 ? u.UsuarioID : (int?)null))
                    throw new Exception("Ese médico ya tiene un usuario activo asignado.");
            }
            else
            {
                u.MedicoID = null;
            }
        }
    }
}
