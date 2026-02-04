using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP_Turnos_Clinica
{
    public partial class AgendaTurnos : System.Web.UI.Page
    {
        private readonly TurnosNegocio turnosNegocio = new TurnosNegocio();
        private readonly PacienteNegocio pacientesNegocio = new PacienteNegocio();
        private readonly EspecialidadNegocio especialidadesNegocio = new EspecialidadNegocio();

        private class ComboItem
        {
            public int Id { get; set; }
            public string Texto { get; set; }
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
                txtFechaDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");
                CargarCombos();
            }
        }
        private void CargarCombos()
        {
            // PACIENTES
            // Ajuste: si tu Listar no tiene el bool soloActivos, dejalo como Listar("")
            var pacientes = pacientesNegocio.Listar("", true);

            var pacCombo = new List<ComboItem>();
            pacCombo.Add(new ComboItem { Id = 0, Texto = "-- Seleccionar --" });

            foreach (var p in pacientes)
            {
                // Ajustá propiedades si tu entidad se llama distinto
                string texto = $"{p.Apellido}, {p.Nombre} (DNI: {p.DNI})";
                pacCombo.Add(new ComboItem { Id = p.PacienteID, Texto = texto });
            }

            ddlPacientes.DataSource = pacCombo;
            ddlPacientes.DataTextField = "Texto";
            ddlPacientes.DataValueField = "Id";
            ddlPacientes.DataBind();

            // ESPECIALIDADES
            var esp = especialidadesNegocio.Listar("", true);

            var espCombo = new List<ComboItem>();
            espCombo.Add(new ComboItem { Id = 0, Texto = "-- Seleccionar --" });

            foreach (var e in esp)
                espCombo.Add(new ComboItem { Id = e.EspecialidadID, Texto = e.Nombre });

            ddlEspecialidades.DataSource = espCombo;
            ddlEspecialidades.DataTextField = "Texto";
            ddlEspecialidades.DataValueField = "Id";
            ddlEspecialidades.DataBind();

            pnlSugerencias.Visible = false;
            lblMensaje.Visible = false;
        }

        protected void ddlPacientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;
        }

        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;
        }

        protected void btnSugerir_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;

            int pacienteId = Convert.ToInt32(ddlPacientes.SelectedValue);
            int especialidadId = Convert.ToInt32(ddlEspecialidades.SelectedValue);

            if (pacienteId <= 0 || especialidadId <= 0)
            {
                MostrarError("Seleccioná paciente y especialidad.");
                return;
            }

            DateTime fechaDesde;
            if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                fechaDesde = DateTime.Today;

            try
            {
                var sugerencias = turnosNegocio.SugerirTurnos(especialidadId, fechaDesde, 3);

                if (sugerencias == null || sugerencias.Count == 0)
                {
                    MostrarError("No se encontraron horarios disponibles para esa especialidad.");
                    return;
                }

                
                Session["Sugerencias"] = sugerencias;

                gvSugerencias.DataSource = sugerencias;
                gvSugerencias.DataBind();
                pnlSugerencias.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        protected void gvSugerencias_ComandoPorFila(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Elegir")
                return;

            var sugerencias = Session["Sugerencias"] as List<OpcionTurno>;
            if (sugerencias == null || sugerencias.Count == 0)
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            if (index < 0 || index >= sugerencias.Count)
                return;

            int pacienteId = Convert.ToInt32(ddlPacientes.SelectedValue);
            int especialidadId = Convert.ToInt32(ddlEspecialidades.SelectedValue);

            var op = sugerencias[index];

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MostrarError("Debés cargar el motivo de la consulta.");
                return;
            }

            decimal importe;
            string impTxt = (txtImporte.Text ?? "").Trim();

            if (!decimal.TryParse(impTxt, NumberStyles.Number, CultureInfo.GetCultureInfo("es-AR"), out importe) &&
                !decimal.TryParse(impTxt, NumberStyles.Number, CultureInfo.InvariantCulture, out importe))
            {
                MostrarError("Importe inválido.");
                return;
            }

            try
            {
                int turnoId = turnosNegocio.AltaTurno(
                    pacienteId,
                    especialidadId,
                    op.MedicoID,
                    op.Fecha,
                    op.HoraInicio,
                    txtMotivo.Text.Trim(),
                    importe,
                    ddlMedioPago.SelectedValue
                );

                
                LimpiarFormulario();

                
                lblMensaje.CssClass = "alert alert-success d-block mb-3";
                lblMensaje.Text = $"Turno creado correctamente. N° {turnoId}";
                lblMensaje.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private void MostrarError(string msg)
        {
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
            lblMensaje.Text = msg;
            lblMensaje.Visible = true;
        }

        private void LimpiarFormulario()
        {
            ddlPacientes.SelectedIndex = 0;
            ddlEspecialidades.SelectedIndex = 0;

            txtMotivo.Text = string.Empty;
            txtImporte.Text = string.Empty;
            txtFechaDesde.Text = string.Empty;

            pnlSugerencias.Visible = false;

            gvSugerencias.DataSource = null;
            gvSugerencias.DataBind();

            Session.Remove("Sugerencias");
        }
    }
}