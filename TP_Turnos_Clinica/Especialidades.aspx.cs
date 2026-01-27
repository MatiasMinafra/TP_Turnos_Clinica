using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TP_Turnos_Clinica
{
    public partial class Especialidades : System.Web.UI.Page
    {
        private readonly EspecialidadNegocio negocio = new EspecialidadNegocio();

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

        private void CargarGrilla()
        {
            string filtro = (txtBuscar.Text ?? "").Trim();
            bool soloActivos = !chkInactivos.Checked;

            List<Especialidad> lista = negocio.Listar(filtro, soloActivos);
            gvEspecialidades.DataSource = lista;
            gvEspecialidades.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void Especialidades_ComandoPorFila(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(gvEspecialidades.DataKeys[index].Value);

            if (e.CommandName == "Desactivar")
                negocio.Desactivar(id);
            else if (e.CommandName == "Activar")
                negocio.Activar(id);

            CargarGrilla();
        }
    }
}