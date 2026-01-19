using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

namespace TP_Turnos_Clinica
{
    public partial class PacienteForm : System.Web.UI.Page
    {
        private PacienteNegocio negocio = new PacienteNegocio();

        private int? PacienteId
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
                if (PacienteId.HasValue)
                {
                    lblTitulo.Text = "Editar Paciente";
                    CargarPaciente(PacienteId.Value);

                    
                    txtDNI.Enabled = false;
                }
                else
                {
                    lblTitulo.Text = "Nuevo Paciente";
                    txtDNI.Enabled = true;
                }
            }
        }

        private void CargarPaciente(int id)
        {
            var p = negocio.ObtenerPorId(id);

            if (p == null)
            {
                MostrarError("Paciente inexistente.");
                btnGuardar.Enabled = false;
                return;
            }

            txtDNI.Text = p.DNI;
            txtNombre.Text = p.Nombre;
            txtApellido.Text = p.Apellido;
            txtEmail.Text = p.Email;
            txtTelefono.Text = p.Telefono;

            txtNacionalidad.Text = p.Nacionalidad;
            txtCiudad.Text = p.Ciudad;
            txtDireccion.Text = p.Direccion;

            
            if (p.Sexo.HasValue)
            {
                string sexo = p.Sexo.Value.ToString();
                if (ddlSexo.Items.FindByValue(sexo) != null)
                    ddlSexo.SelectedValue = sexo;
            }
            else
            {
                ddlSexo.SelectedValue = "";
            }

            
            if (p.FechaNacimiento.HasValue)
                txtFechaNacimiento.Text = p.FechaNacimiento.Value.ToString("yyyy-MM-dd");
            else
                txtFechaNacimiento.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            OcultarMensaje();

            try
            {
               
                string dni = txtDNI.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string email = txtEmail.Text.Trim();

                
                DateTime? fechaNac = null;
                if (!string.IsNullOrWhiteSpace(txtFechaNacimiento.Text))
                {
                    if (DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fn))
                        fechaNac = fn.Date;
                    else
                        throw new Exception("Fecha de nacimiento inválida.");
                }

               
                char? sexo = null;
                if (!string.IsNullOrWhiteSpace(ddlSexo.SelectedValue))
                    sexo = Convert.ToChar(ddlSexo.SelectedValue);

                Paciente p = new Paciente
                {
                    DNI = dni,
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,

                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Sexo = sexo,
                    Nacionalidad = string.IsNullOrWhiteSpace(txtNacionalidad.Text) ? null : txtNacionalidad.Text.Trim(),
                    FechaNacimiento = fechaNac,
                    Ciudad = string.IsNullOrWhiteSpace(txtCiudad.Text) ? null : txtCiudad.Text.Trim(),
                    Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),

                    Activo = true
                };

                if (PacienteId.HasValue)
                {
                    p.PacienteID = PacienteId.Value;
                    negocio.Modificar(p);
                }
                else
                {
                    negocio.Agregar(p);
                }

                Response.Redirect("~/Pacientes.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pacientes.aspx");
        }

        private void MostrarError(string msg)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = msg;
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
        }

        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
            lblMensaje.Text = "";
        }
    }
}