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
    public partial class EspecialidadesForm : System.Web.UI.Page
    {
        private readonly EspecialidadNegocio negocio = new EspecialidadNegocio();

        private int? EspecialidadId
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
                if (EspecialidadId.HasValue)
                {
                    lblTitulo.Text = "Editar Especialidad";
                    CargarEspecialidad(EspecialidadId.Value);
                }
                else
                {
                    lblTitulo.Text = "Nueva Especialidad";
                }
            }
        }
        private void CargarEspecialidad(int id)
        {
            Especialidad e = negocio.ObtenerPorId(id);

            if (e == null)
            {
                MostrarError("Especialidad inexistente.");
                btnGuardar.Enabled = false;
                return;
            }

            txtNombre.Text = e.Nombre;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            OcultarMensaje();

            try
            {
                Especialidad esp;

                if (EspecialidadId.HasValue)
                {
                   
                    esp = negocio.ObtenerPorId(EspecialidadId.Value);
                    if (esp == null) throw new Exception("Especialidad inexistente.");

                    esp.Nombre = txtNombre.Text.Trim();
                    negocio.Modificar(esp);
                }
                else
                {
                    esp = new Especialidad
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Activo = true
                    };

                    negocio.Agregar(esp);
                }

                Response.Redirect("~/Especialidades.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Especialidades.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.Visible = true;
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
            lblMensaje.Text = mensaje;
        }

        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
            lblMensaje.Text = "";
        }


    }
}