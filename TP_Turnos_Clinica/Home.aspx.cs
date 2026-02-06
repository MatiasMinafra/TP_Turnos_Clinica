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
            
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var u = (Usuario)Session["usuario"];

            if (!IsPostBack)
            {
                lblUsuario.Text = u.UsuarioNombre;
                lblRol.Text = u.rol?.Nombre ?? "Sin rol";

                AplicarPermisos(u);
            }
        }

        private void AplicarPermisos(Usuario u)
        {
            bool esAdmin = (u.RolID == RolesIds.ADMIN);
            bool esRecep = (u.RolID == RolesIds.RECEPCIONISTA);
            bool esMedico = (u.RolID == RolesIds.MEDICO);

           
            lnkAsignarTurno.Visible = false;
            lnkTurnosDia.Visible = false;
            lnkPacientes.Visible = false;
            lnkMedicos.Visible = false;
            lnkEspecialidades.Visible = false;
            lnkAgenda.Visible = false;
            lnkUsuarios.Visible = false;

            lnkMisTurnos.Visible = false;
            lnkEvoluciones.Visible = false;

            
            if (esAdmin)
            {
                lnkAsignarTurno.Visible = true;
                lnkTurnosDia.Visible = true;
                lnkPacientes.Visible = true;
                lnkMedicos.Visible = true;
                lnkEspecialidades.Visible = true;
                lnkAgenda.Visible = true;
                lnkUsuarios.Visible = true; 
                return;
            }

            
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

           
            if (esMedico)
            {
               
                if (!u.MedicoID.HasValue)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                lnkMisTurnos.Visible = true;
                lnkEvoluciones.Visible = true;
                return;
            }

            Response.Redirect("~/Login.aspx");
        }

    }
}