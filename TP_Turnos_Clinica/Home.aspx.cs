using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP_Turnos_Clinica
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Si no hay sesión, afuera
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var u = (Usuario)Session["usuario"];

            if (!IsPostBack)
            {
                lblUsuario.Text = u.UsuarioNombre; // ajustá propiedad
                lblRol.Text = u.rol?.Nombre ?? "Sin rol";

                AplicarPermisos(u.rol?.Nombre);
            }
        }

        private void AplicarPermisos(string rol)
        {
            // Normalizá por las dudas
            rol = (rol ?? "").Trim().ToLower();

            bool esAdmin = rol == "administrador" || rol == "admin";
            bool esRecep = rol == "recepcionista" || rol == "recep";
            bool esMedico = rol == "médico" || rol == "medico";

            // Por defecto, oculto todo "específico"
            lnkAsignarTurno.Visible = false;
            lnkTurnosDia.Visible = false;
            lnkPacientes.Visible = false;
            lnkMedicos.Visible = false;
            lnkEspecialidades.Visible = false;
            lnkAgenda.Visible = false;
            lnkUsuarios.Visible = false;

            lnkMisTurnos.Visible = false;
            lnkEvoluciones.Visible = false;

            // Admin ve todo
            if (esAdmin)
            {
                lnkAsignarTurno.Visible = true;
                lnkTurnosDia.Visible = true;
                lnkPacientes.Visible = true;
                lnkMedicos.Visible = true;
                lnkEspecialidades.Visible = true;
                lnkAgenda.Visible = true;

                // opcional
                lnkUsuarios.Visible = true;
                return;
            }

            // Recepcionista: operación + ABMs
            if (esRecep)
            {
                lnkAsignarTurno.Visible = true;
                lnkTurnosDia.Visible = true;
                lnkPacientes.Visible = true;
                lnkMedicos.Visible = true;
                lnkEspecialidades.Visible = true;
                lnkAgenda.Visible = true;
                return;
            }

            // Médico: solo lo suyo
            if (esMedico)
            {
                lnkMisTurnos.Visible = true;
                lnkEvoluciones.Visible = true;
                return;
            }

            // Si llega un rol raro, lo mando al login (o mostrás mensaje)
            Response.Redirect("~/Login.aspx");
        }

    }
}