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
    public partial class TurnoTrabajoForm : System.Web.UI.Page
    {
        private readonly TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
        private int? TurnoTrabajoId
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
                if (TurnoTrabajoId.HasValue)
                {
                    lblTitulo.Text = "Editar Turno de Trabajo";
                    Cargar(TurnoTrabajoId.Value);
                }
                else
                {
                    lblTitulo.Text = "Nuevo Turno de Trabajo";
                }
            }
        }
            private void Cargar(int id)
        {
            TurnoTrabajo t = negocio.ObtenerPorId(id);
            if (t == null)
            {
                MostrarError("Turno de trabajo inexistente.");
                btnGuardar.Enabled = false;
                return;
            }

            txtNombre.Text = t.Nombre;
            txtHoraInicio.Text = t.HoraInicio.ToString(@"hh\:mm");
            txtHoraFin.Text = t.HoraFin.ToString(@"hh\:mm");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            OcultarMensaje();

            try
            {
                if (!TimeSpan.TryParse(txtHoraInicio.Text.Trim(), out TimeSpan inicio))
                    throw new Exception("Hora inicio inválida. Formato esperado: HH:mm");

                if (!TimeSpan.TryParse(txtHoraFin.Text.Trim(), out TimeSpan fin))
                    throw new Exception("Hora fin inválida. Formato esperado: HH:mm");

                TurnoTrabajo t = new TurnoTrabajo
                {
                    Nombre = txtNombre.Text.Trim(),
                    HoraInicio = inicio,
                    HoraFin = fin,
                    Activo = true
                };

                if (TurnoTrabajoId.HasValue)
                {
                    t.TurnoTrabajoID = TurnoTrabajoId.Value;
                    negocio.Modificar(t);
                }
                else
                {
                    negocio.Agregar(t);
                }

                Response.Redirect("~/TurnosTrabajo.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TurnosTrabajo.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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
