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
    public partial class TurnosTrabajo : System.Web.UI.Page
    {
        private readonly TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
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

            List<TurnoTrabajo> lista = negocio.Listar(filtro, soloActivos);
            gvTurnosTrabajo.DataSource = lista;
            gvTurnosTrabajo.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla(txtBuscar.Text.Trim());
        }

        protected void TurnosTrabajo_ComandoPorFila(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ToggleActivo")
                return;

            int id;
            if (!int.TryParse(e.CommandArgument?.ToString(), out id) || id <= 0)
                return;

            var tt = negocio.ObtenerPorId(id);
            if (tt == null) return;

            if (tt.Activo) negocio.Desactivar(id);
            else negocio.Activar(id);

            CargarGrilla(txtBuscar.Text.Trim());
        }

    }
}