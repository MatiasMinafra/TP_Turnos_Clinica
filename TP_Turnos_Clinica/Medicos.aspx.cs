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
    public partial class Medicos : System.Web.UI.Page
    {
        private MedicoNegocio negocio = new MedicoNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
                CargarGrilla();
        }

        private void CargarGrilla(string filtro = "")
        {
            bool soloActivos = !chkInactivos.Checked;

            List<Medico> lista = negocio.Listar(filtro, soloActivos); // ✅ acá está la clave
            gvMedicos.DataSource = lista;
            gvMedicos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void Medicos_ComandoPorFila(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Desactivar")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idMedico = Convert.ToInt32(gvMedicos.DataKeys[index].Value);

            negocio.Desactivar(idMedico);
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }


    }
}