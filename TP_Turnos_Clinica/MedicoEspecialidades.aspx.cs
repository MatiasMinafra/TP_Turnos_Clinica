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
    public partial class MedicoEspecialidades : System.Web.UI.Page
    {
        private readonly EspecialidadNegocio espNegocio = new EspecialidadNegocio();
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();

        private int MedicoId
        {
            get
            {
                if (int.TryParse(Request.QueryString["id"], out int id) && id > 0)
                    return id;
                return 0;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (MedicoId == 0)
            {
                MostrarError("Falta el id del médico en la URL.");
                btnAsignar.Enabled = false;
                return;
            }

            if (!IsPostBack)
            {
                CargarHeaderMedico();
                CargarComboEspecialidades();
                CargarGrillaAsignadas();
            }
        }

        private void CargarHeaderMedico()
        {
            Medico m = medicoNegocio.ObtenerPorId(MedicoId);
            if (m == null)
            {
                MostrarError("Médico inexistente.");
                btnAsignar.Enabled = false;   // OJO: es btnAsignar
                return;
            }

            lblMedico.Text = $"Médico: {m.Apellido}, {m.Nombre} (Matrícula: {m.Matricula})";
        }

        private void CargarComboEspecialidades()
        {
            var lista = espNegocio.ListarActivas();
            ddlEspecialidades.DataSource = lista;
            ddlEspecialidades.DataTextField = "Nombre";
            ddlEspecialidades.DataValueField = "EspecialidadID";
            ddlEspecialidades.DataBind();
        }

        private void CargarGrillaAsignadas()
        {
            List<Especialidad> asignadas = espNegocio.ListarPorMedico(MedicoId);
            gvAsignadas.DataSource = asignadas;
            gvAsignadas.DataBind();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            OcultarMensaje();

            try
            {
                int espId = int.Parse(ddlEspecialidades.SelectedValue);

                // Acá llamás al negocio que asigna
                espNegocio.AsignarAMedico(MedicoId, espId);

                CargarComboEspecialidades();
                CargarGrillaAsignadas();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        protected void gvAsignadas_ComandoFila(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Quitar")
                return;

            OcultarMensaje();

            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int especialidadId = Convert.ToInt32(gvAsignadas.DataKeys[index].Value);

                espNegocio.QuitarDeMedico(MedicoId, especialidadId);
                CargarGrillaAsignadas();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
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