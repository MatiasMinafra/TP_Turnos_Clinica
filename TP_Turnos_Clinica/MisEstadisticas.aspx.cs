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
    public partial class MisEstadisticas : System.Web.UI.Page
    {
        private readonly TurnosNegocio turnosNegocio = new TurnosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var u = (Usuario)Session["usuario"];
            if (!u.MedicoID.HasValue || u.MedicoID.Value <= 0)
            {
                MostrarError("Este usuario no tiene médico asociado.");
                return;
            }

            if (!IsPostBack)
                Cargar();
        }

        private void Cargar()
        {
            try
            {
                OcultarMensaje();

                var u = (Usuario)Session["usuario"];
                int medicoId = u.MedicoID.Value;

                lblMesActual.Text = DateTime.Today.ToString("MMMM yyyy");

                
                var listaHoy = turnosNegocio.ListarPorMedico(medicoId, DateTime.Today, DateTime.Today);

                lblHoy.Text = listaHoy.Count.ToString();

                lblAtendidosHoy.Text = listaHoy
                    .Count(x => string.Equals(x.EstadoTurno, "Atendido", StringComparison.OrdinalIgnoreCase))
                    .ToString();

                lblPendientesHoy.Text = listaHoy
                    .Count(x =>
                        !string.Equals(x.EstadoTurno, "Atendido", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(x.EstadoTurno, "Cancelado", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(x.EstadoTurno, "No Asistió", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(x.EstadoTurno, "No Asistio", StringComparison.OrdinalIgnoreCase))
                    .ToString();

                
                var estMes = turnosNegocio.ObtenerEstadisticasMes(medicoId, DateTime.Today.Year, DateTime.Today.Month);

                lblAtendidosMes.Text = estMes.Atendidos.ToString();
                lblNoAsistioMes.Text = estMes.NoAsistio.ToString();
                lblReprogramadosMes.Text = estMes.Reprogramados.ToString();

                
                int diasTranscurridos = DateTime.Today.Day; 
                double promedio = 0;

                if (diasTranscurridos > 0)
                    promedio = estMes.Atendidos / (double)diasTranscurridos;

                lblPromedioDia.Text = promedio.ToString("0.00");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }
        private void OcultarMensaje()
        {
            lblMsg.Visible = false;
            lblMsg.Text = "";
            lblMsg.CssClass = "alert alert-danger d-block mb-3";
        }

        private void MostrarError(string msg)
        {
            lblMsg.Text = msg;
            lblMsg.CssClass = "alert alert-danger d-block mb-3";
            lblMsg.Visible = true;
        }
    }
}