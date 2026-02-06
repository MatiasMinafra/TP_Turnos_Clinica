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
    public partial class UsuariosForm : System.Web.UI.Page
    {
        private UsuarioNegocio negocio = new UsuarioNegocio();
        private RolesNegocio rolesNegocio = new RolesNegocio();
        private MedicoNegocio medicosNegocio = new MedicoNegocio();

        private int? UsuarioId
        {
            get
            {
                if (int.TryParse(Request.QueryString["id"], out int id) && id > 0)
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad: solo Admin puede entrar
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var ses = (Usuario)Session["usuario"];
            if (ses.RolID != RolesIds.ADMIN)
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarRoles();
                CargarMedicos();

                if (UsuarioId.HasValue)
                    CargarEdicion(UsuarioId.Value);

               
            }
            AplicarHabilitacionMedico();
        }

        private void CargarRoles()
        {
            ddlRol.DataSource = rolesNegocio.Listar();
            ddlRol.DataTextField = "Nombre";
            ddlRol.DataValueField = "RolID";
            ddlRol.DataBind();
        }

        private void CargarMedicos()
        {
            ddlMedico.DataSource = medicosNegocio.Listar("", true);

            // Si tu clase Medico NO tiene "Apellido", cambiá DataTextField al que tengas (por ej: "Nombre")
            ddlMedico.DataTextField = "Apellido";
            ddlMedico.DataValueField = "MedicoID";

            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarHabilitacionMedico();
        }

        private void AplicarHabilitacionMedico()
        {
            int rolId = int.Parse(ddlRol.SelectedValue);
            bool esMedico = (rolId == RolesIds.MEDICO);

            ddlMedico.Enabled = esMedico;

            if (!esMedico)
                ddlMedico.SelectedValue = "0";
        }

        private void CargarEdicion(int id)
        {
            Usuario u = negocio.ObtenerPorId(id);

            txtUsuario.Text = u.UsuarioNombre;
            txtPassword.Text = ""; // no mostrar password

            txtNombre.Text = u.Nombre;
            txtApellido.Text = u.Apellido;
            txtEmail.Text = u.Email;

            ddlRol.SelectedValue = u.RolID.ToString();
            chkActivo.Checked = u.Activo;

            AplicarHabilitacionMedico();

            if (u.MedicoID.HasValue)
                ddlMedico.SelectedValue = u.MedicoID.Value.ToString();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario u = new Usuario();
                u.UsuarioID = UsuarioId ?? 0;
                u.UsuarioNombre = txtUsuario.Text.Trim();
                u.Password = txtPassword.Text.Trim(); // si está vacío en edición, no se actualiza

                u.Nombre = txtNombre.Text.Trim();
                u.Apellido = txtApellido.Text.Trim();
                u.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();

                u.RolID = int.Parse(ddlRol.SelectedValue);
                u.Activo = chkActivo.Checked;

                if (u.RolID == RolesIds.MEDICO)
                {
                    int mid = int.Parse(ddlMedico.SelectedValue);
                    u.MedicoID = (mid > 0) ? (int?)mid : null;
                }
                else
                {
                    u.MedicoID = null;
                }

                negocio.Guardar(u);

                Response.Redirect("~/Usuarios.aspx");
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Usuarios.aspx");
        }
    }
}