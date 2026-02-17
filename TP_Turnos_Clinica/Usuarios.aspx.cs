using System;
using System.Collections.Generic;
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
            {
                OcultarMsg();
                CargarGrilla();
            }
        }

        private void CargarGrilla()
        {
            string filtro = (txtFiltro.Text ?? "").Trim();
            bool soloActivos = chkSoloActivos.Checked;

            List<Usuario> lista = negocio.Listar(filtro, soloActivos);

            dgvUsuarios.DataSource = lista;
            dgvUsuarios.DataBind();
        }

        private void OcultarMsg()
        {
            lblMsg.Text = "";
            lblMsg.Visible = false;
            lblMsg.CssClass = "d-none";
        }

        private void MostrarMsg(string texto, string css)
        {
            lblMsg.Text = texto;
            lblMsg.Visible = true;
            lblMsg.CssClass = css + " d-block mt-3";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OcultarMsg();
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

                    MostrarMsg("Usuario dado de baja correctamente.", "alert alert-success");

                    CargarGrilla();


                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "hideMsg",
                        $"setTimeout(function(){{var el=document.getElementById('{lblMsg.ClientID}'); if(el) el.style.display='none';}}, 3000);",
                        true
                    );
                }
                catch (Exception ex)
                {
                    MostrarMsg(ex.Message, "alert alert-danger");
                }
            }
        }
    }
}
