using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP_Turnos_Clinica
{
    public partial class MedicoForm : System.Web.UI.Page
    {
        private MedicoNegocio negocio = new MedicoNegocio();

        private int? MedicoId
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
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (MedicoId.HasValue)
                {
                    lblTitulo.Text = "Editar Médico";
                    CargarMedico(MedicoId.Value);
                    txtDNI.Enabled = false;
                    txtMatricula.Enabled = false;
                }
                else
                {
                    lblTitulo.Text = "Nuevo Médico";
                    txtDNI.Enabled = true;
                    txtMatricula.Enabled = true;
                }
            }
        }

        private void CargarMedico(int id)
        {
            Medico m = negocio.ObtenerPorId(id);

            if (m == null)
            {
                MostrarError("Médico inexistente.");
                btnGuardar.Enabled = false;
                return;
            }

            txtDNI.Text = m.DNI;
            txtMatricula.Text = m.Matricula;
            txtNombre.Text = m.Nombre;
            txtApellido.Text = m.Apellido;
            txtEmail.Text = m.Email;
            txtTelefono.Text = m.Telefono;

            txtNacionalidad.Text = m.Nacionalidad;
            txtCiudad.Text = m.Ciudad;
            txtDireccion.Text = m.Direccion;

            if (m.Sexo.HasValue)
                ddlSexo.SelectedValue = m.Sexo.Value.ToString();
            else
                ddlSexo.SelectedValue = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            OcultarMensaje();

            try
            {
                char? sexo = null;
                if (!string.IsNullOrWhiteSpace(ddlSexo.SelectedValue))
                    sexo = Convert.ToChar(ddlSexo.SelectedValue);

                Medico m = new Medico
                {
                    DNI = txtDNI.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Sexo = sexo,
                    Nacionalidad = string.IsNullOrWhiteSpace(txtNacionalidad.Text) ? null : txtNacionalidad.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Ciudad = string.IsNullOrWhiteSpace(txtCiudad.Text) ? null : txtCiudad.Text.Trim(),
                    Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                    Activo = true
                };

                if (MedicoId.HasValue)
                {
                    
                    m.MedicoID = MedicoId.Value;
                    negocio.Modificar(m);

                    Response.Redirect("~/Medicos.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    
                    int nuevoId = negocio.Agregar(m);
                    m.MedicoID = nuevoId;

                    lblMensaje.Visible = true;
                    lblMensaje.CssClass = "alert alert-success d-block mb-3";
                    lblMensaje.Text = "Médico guardado. Ahora asignale sus especialidades.";

                    lnkVolverLista.Visible = true;
                    lnkAsignarEspecialidades.Visible = true;
                    lnkAsignarEspecialidades.NavigateUrl = "~/MedicoEspecialidades.aspx?id=" + nuevoId;

                    
                    btnGuardar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Medicos.aspx");
        }
        private void MostrarError(string mensaje)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = mensaje;
        }
        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
            lblMensaje.Text = "";
        }
    }
}