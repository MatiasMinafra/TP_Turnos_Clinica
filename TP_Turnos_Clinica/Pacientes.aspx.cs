using Negocio;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP_Turnos_Clinica
{
    public partial class Pacientes : System.Web.UI.Page
    {
        PacientesNegocio negocio = new PacientesNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad: sesión obligatoria
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        private void CargarGrilla(string filtro = "")
        {
            List<Dominio.Pacientes> lista = negocio.Listar(filtro);
            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void gvPacientes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int idPaciente = Convert.ToInt32(gvPacientes.DataKeys[index].Value);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pacientes/Form.aspx?id={idPaciente}");
            }

            if (e.CommandName == "Desactivar")
            {
                negocio.Desactivar(idPaciente);
                CargarGrilla(txtBuscar.Text.Trim());
            }
        }
    }
}