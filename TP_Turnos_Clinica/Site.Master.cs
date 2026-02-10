using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP_Turnos_Clinica
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["usuario"] == null)
            {
                phMenuAdminRecep.Visible = false;
                phMenuMedico.Visible = false;
                phUser.Visible = false;
                return;
            }

            var u = (Usuario)Session["usuario"];

            
            litUsuario.Text = "Sesión iniciada";

            bool esAdminRecep = (u.RolID == RolesIds.ADMIN || u.RolID == RolesIds.RECEPCIONISTA);
            bool esMedico = (u.RolID == RolesIds.MEDICO);

            phMenuAdminRecep.Visible = esAdminRecep;
            phMenuMedico.Visible = esMedico;

            
        }
    }
}