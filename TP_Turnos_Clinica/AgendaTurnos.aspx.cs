using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
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

                pnlSugerencias.Visible = false;
                lblMensaje.Visible = false;

                if (Request.QueryString["reprog"] == "1" && int.TryParse(Request.QueryString["turnoId"], out int turnoIdReprog))
                {
                    hfTurnoIdReprog.Value = turnoIdReprog.ToString();

                    pnlReprog.Visible = true;
                    lblTurnoReprog.Text = turnoIdReprog.ToString();

                   
                    ddlPacientes.Enabled = false;
                    ddlEspecialidades.Enabled = false;
                }
            }
        }

        protected void btnSalirReprog_Click(object sender, EventArgs e)
        {
            hfTurnoIdReprog.Value = "";
            pnlReprog.Visible = false;
            lblTurnoReprog.Text = "";

            ddlPacientes.Enabled = true;
            ddlEspecialidades.Enabled = true;

            pnlSugerencias.Visible = false;
            gvSugerencias.DataSource = null;
            gvSugerencias.DataBind();
            Session.Remove("Sugerencias");

            lblMensaje.Visible = false;
        }

        private void CargarCombos()
        {
            
            var pacientes = pacientesNegocio.Listar("", true);

            var pacCombo = new List<ComboItem>
            {
                new ComboItem { Id = 0, Texto = "-- Seleccionar --" }
            };

            foreach (var p in pacientes)
            {
                string texto = $"{p.Apellido}, {p.Nombre} (DNI: {p.DNI})";
                pacCombo.Add(new ComboItem { Id = p.PacienteID, Texto = texto });
            }

            ddlPacientes.DataSource = pacCombo;
            ddlPacientes.DataTextField = "Texto";
            ddlPacientes.DataValueField = "Id";
            ddlPacientes.DataBind();

            
            var esp = especialidadesNegocio.Listar("", true);

            var espCombo = new List<ComboItem>
            {
                new ComboItem { Id = 0, Texto = "-- Seleccionar --" }
            };

            foreach (var e in esp)
                espCombo.Add(new ComboItem { Id = e.EspecialidadID, Texto = e.Nombre });

            ddlEspecialidades.DataSource = espCombo;
            ddlEspecialidades.DataTextField = "Texto";
            ddlEspecialidades.DataValueField = "Id";
            ddlEspecialidades.DataBind();
        }

        protected void ddlPacientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;

            LimpiarSugerencias();
        }

        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;

            LimpiarSugerencias();
        }

        protected void btnSugerir_Click(object sender, EventArgs e)
        {

            LimpiarSugerencias();
            lblMensaje.Visible = false;
            pnlSugerencias.Visible = false;

            bool esReprogramacion = !string.IsNullOrWhiteSpace(hfTurnoIdReprog.Value);

            int pacienteId = Convert.ToInt32(ddlPacientes.SelectedValue);
            int especialidadId;

          
            if (esReprogramacion)
            {
                int turnoIdReprog = int.Parse(hfTurnoIdReprog.Value);
                especialidadId = turnosNegocio.ObtenerEspecialidadDelTurno(turnoIdReprog);
            }
            else
            {
                especialidadId = Convert.ToInt32(ddlEspecialidades.SelectedValue);

                if (pacienteId <= 0 || especialidadId <= 0)
                {
                    MostrarError("Seleccioná paciente y especialidad.");
                    return;
                }
            }

            DateTime fecha;
            if (!DateTime.TryParse(txtFechaDesde.Text, out fecha))
                fecha = DateTime.Today;

            try
            {
                var sugerencias = turnosNegocio.SugerirTurnos(
                    especialidadId,
                    fecha.Date,
                    ddlFranja.SelectedValue
                );

                if (sugerencias == null || sugerencias.Count == 0)
                {
                    MostrarError("No se encontraron horarios para esa fecha y franja.");
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

            var lista = Session["Sugerencias"] as List<OpcionTurno>;
            if (lista == null || lista.Count == 0)
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            if (index < 0 || index >= lista.Count)
                return;

            var op = lista[index];

            
            if (op.Ocupado)
            {
                MostrarError("Ese horario está ocupado.");
                return;
            }

            try
            {
                bool esReprogramacion = !string.IsNullOrWhiteSpace(hfTurnoIdReprog.Value);

                
                if (esReprogramacion)
                {
                    int turnoIdReprog = int.Parse(hfTurnoIdReprog.Value);

                    turnosNegocio.ReprogramarTurno(
                        turnoIdReprog,
                        op.Fecha,
                        op.HoraInicio,
                        op.MedicoID
                    );

                    
                    hfTurnoIdReprog.Value = "";
                    pnlReprog.Visible = false;
                    lblTurnoReprog.Text = "";

                    
                    ddlPacientes.Enabled = true;
                    ddlEspecialidades.Enabled = true;

                    
                    LimpiarSugerencias();

                    lblMensaje.CssClass = "alert alert-success d-block mb-3";
                    lblMensaje.Text = $"Turno reprogramado correctamente. N° {turnoIdReprog}";
                    lblMensaje.Visible = true;

                    return;
                }

               
                if (string.IsNullOrWhiteSpace(txtMotivo.Text))
                {
                    MostrarError("Debés cargar el motivo.");
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

                int turnoId = turnosNegocio.AltaTurno(
                    Convert.ToInt32(ddlPacientes.SelectedValue),
                    Convert.ToInt32(ddlEspecialidades.SelectedValue),
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
        private void LimpiarSugerencias()
        {
            pnlSugerencias.Visible = false;

            gvSugerencias.DataSource = null;
            gvSugerencias.DataBind();

            Session.Remove("Sugerencias");

            lblMensaje.Visible = false;
            lblMensaje.Text = "";
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
        }

        protected void ddlFranja_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarSugerencias();
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

            txtMotivo.Text = "";
            txtImporte.Text = "";
            txtFechaDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");

            pnlSugerencias.Visible = false;

            gvSugerencias.DataSource = null;
            gvSugerencias.DataBind();

            Session.Remove("Sugerencias");
        }

        
       
    }
}