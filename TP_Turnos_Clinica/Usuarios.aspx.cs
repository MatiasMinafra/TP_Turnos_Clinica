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
    public partial class Usuarios : System.Web.UI.Page
    {
        private UsuarioNegocio negocio = new UsuarioNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad: solo Admin
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var u = (Usuario)Session["usuario"];
            if (u.RolID != RolesIds.ADMIN)
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!IsPostBack)
                CargarGrilla();
        }

        private void CargarGrilla()
        {
            string filtro = txtFiltro.Text.Trim();
            bool soloActivos = chkSoloActivos.Checked;

            List<Usuario> lista = negocio.Listar(filtro, soloActivos);

            dgvUsuarios.DataSource = lista;
            dgvUsuarios.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "d-block mt-3";
            CargarGrilla();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UsuariosForm.aspx");
        }

        protected void dgvUsuarios_ComandoFila(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(dgvUsuarios.DataKeys[index].Value);

            if (e.CommandName == "Editar")
            {
                Response.Redirect("~/UsuariosForm.aspx?id=" + id);
                return;
            }

            if (e.CommandName == "Baja")
            {
                try
                {
                    negocio.BajaLogica(id);
                    lblMsg.CssClass = "alert alert-success mt-3";
                    lblMsg.Text = "Usuario dado de baja correctamente.";
                    CargarGrilla();
                }
                catch (Exception ex)
                {
                    lblMsg.CssClass = "alert alert-danger mt-3";
                    lblMsg.Text = ex.Message;
                }
            }
        }
    }
}