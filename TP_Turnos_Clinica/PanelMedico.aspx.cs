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
    public partial class PanelMedico : System.Web.UI.Page
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

            // ✅ Solo MEDICO
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
            {
                txtDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtHasta.Text = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
                Cargar();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e) => Cargar();
        protected void Filtros_CheckedChanged(object sender, EventArgs e) => Cargar();

        private void Cargar()
        {
            try
            {
                OcultarMensaje();

                var u = (Usuario)Session["usuario"];
                int medicoId = u.MedicoID.Value;

                DateTime desde, hasta;
                if (!DateTime.TryParse(txtDesde.Text, out desde)) desde = DateTime.Today;
                if (!DateTime.TryParse(txtHasta.Text, out hasta)) hasta = DateTime.Today.AddDays(7);

                if (hasta < desde)
                    throw new Exception("La fecha 'Hasta' no puede ser menor que 'Desde'.");

                if (chkSoloHoy.Checked)
                {
                    desde = DateTime.Today;
                    hasta = DateTime.Today;
                    txtDesde.Text = desde.ToString("yyyy-MM-dd");
                    txtHasta.Text = hasta.ToString("yyyy-MM-dd");
                }

                // ✅ Grilla
                var lista = turnosNegocio.ListarPorMedico(medicoId, desde, hasta);

                if (chkOcultarCancelados.Checked)
                    lista = lista.Where(x => !string.Equals(x.EstadoTurno, "Cancelado", StringComparison.OrdinalIgnoreCase)).ToList();

                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();

                // ✅ Resumen HOY (como lo tenías)
                var hoy = DateTime.Today;
                var hoyList = lista.Where(x => x.Fecha.Date == hoy).ToList();

                lblHoy.Text = hoyList.Count.ToString();
                lblAtendidos.Text = hoyList.Count(x => Eq(x.EstadoTurno, "Atendido")).ToString();
                lblPendientes.Text = hoyList.Count(x =>
                    !Eq(x.EstadoTurno, "Atendido") &&
                    !Eq(x.EstadoTurno, "Cancelado") &&
                    !Eq(x.EstadoTurno, "No Asistió") &&
                    !Eq(x.EstadoTurno, "No Asistio")
                ).ToString();

                // ✅ Estadísticas del MES (NUEVO)
                lblMesActual.Text = DateTime.Today.ToString("MMMM yyyy"); // ej: "febrero 2026"

                var statsMes = turnosNegocio.StatsMedicoMes(medicoId, DateTime.Today.Year, DateTime.Today.Month);
                lblAtendidosMes.Text = statsMes.Atendidos.ToString();
                lblNoAsistioMes.Text = statsMes.NoAsistio.ToString();
                lblReprogramadosMes.Text = statsMes.Reprogramados.ToString();

                if (lista.Count == 0)
                    MostrarInfo("No hay turnos para el rango seleccionado.");
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                gvTurnos.DataSource = null;
                gvTurnos.DataBind();
            }
        }
        private bool Eq(string a, string b)
            => string.Equals((a ?? "").Trim(), (b ?? "").Trim(), StringComparison.OrdinalIgnoreCase);

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

        private void MostrarInfo(string msg)
        {
            lblMsg.Text = msg;
            lblMsg.CssClass = "alert alert-info d-block mb-3";
            lblMsg.Visible = true;
        }
    }
}