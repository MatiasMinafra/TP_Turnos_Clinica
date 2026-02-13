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
    public partial class EvolucionForm : System.Web.UI.Page
    {
        private readonly EvolucionNegocio negocio = new EvolucionNegocio();

        private int TurnoId
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["turnoId"], out id) && id > 0)
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

            var u = (Usuario)Session["usuario"];

            // Solo médicos
            if (u.RolID != RolesIds.MEDICO)
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!u.MedicoID.HasValue || u.MedicoID.Value <= 0)
            {
                MostrarError("Este usuario no tiene médico asociado.");
                return;
            }

            if (!IsPostBack)
                CargarDetalle();
        }

        private void CargarDetalle()
        {
            try
            {
                if (TurnoId <= 0)
                    throw new Exception("Turno inválido.");

                var det = negocio.ObtenerDetalleTurno(TurnoId);
                if (det == null)
                    throw new Exception("No se encontró el turno o está inactivo.");

                txtPaciente.Text = det.PacienteNombre;
                txtMedico.Text = det.MedicoNombre;
                txtFechaTurno.Text = det.TurnoFecha.ToString("dd/MM/yyyy");
                txtHorario.Text = $"{det.TurnoHoraInicio:hh\\:mm} - {det.TurnoHoraFin:hh\\:mm}";

                // Link al historial (B: turnos + evoluciones)
                lnkHistorial.NavigateUrl = "~/HistorialPaciente.aspx?pacienteId=" + det.PacienteID;
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var u = (Usuario)Session["usuario"];
                negocio.RegistrarEvolucion(u, TurnoId, txtDescripcion.Text);

                lblMsg.Visible = true;
                lblMsg.CssClass = "alert alert-success d-block mb-3";
                lblMsg.Text = "Evolución guardada. El turno quedó marcado como ATENDIDO.";

                btnGuardar.Enabled = false;
                txtDescripcion.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private void MostrarError(string msg)
        {
            lblMsg.Visible = true;
            lblMsg.CssClass = "alert alert-danger d-block mb-3";
            lblMsg.Text = msg;
        }
    }
}