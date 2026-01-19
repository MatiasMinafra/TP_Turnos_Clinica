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
        PacienteNegocio negocio = new PacienteNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            
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
            List<Paciente> lista = negocio.Listar(filtro);
            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void Pacientes_ComandoPorFila(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Desactivar")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPaciente = Convert.ToInt32(gvPacientes.DataKeys[index].Value);

            negocio.Desactivar(idPaciente);
            CargarGrilla(txtBuscar.Text.Trim());
        }
    }
}