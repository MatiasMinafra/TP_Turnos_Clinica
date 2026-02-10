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
    public partial class MisTurnos : System.Web.UI.Page
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
            {
                txtDesde.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtHasta.Text = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
                CargarGrilla();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
        protected void Filtros_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
        private void CargarGrilla()
        {
            OcultarMensaje();

            var u = (Usuario)Session["usuario"];
            int medicoId = u.MedicoID.Value;

            DateTime desde, hasta;
            if (!DateTime.TryParse(txtDesde.Text, out desde)) desde = DateTime.Today;
            if (!DateTime.TryParse(txtHasta.Text, out hasta)) hasta = DateTime.Today.AddDays(7);

            if (hasta < desde)
            {
                MostrarError("La fecha 'Hasta' no puede ser menor que 'Desde'.");
                gvMisTurnos.DataSource = null;
                gvMisTurnos.DataBind();
                return;
            }

            
            if (chkSoloProximos.Checked && desde.Date < DateTime.Today)
                desde = DateTime.Today;

            try
            {
                var lista = turnosNegocio.ListarMisTurnos(medicoId, desde, hasta);

                
                if (chkOcultarCancelados.Checked)
                    lista = lista.Where(t => !string.Equals(t.EstadoTurno, "Cancelado", StringComparison.OrdinalIgnoreCase)).ToList();

                gvMisTurnos.DataSource = lista;
                gvMisTurnos.DataBind();

                if (lista == null || lista.Count == 0)
                {
                    MostrarInfo("No hay turnos para el rango seleccionado.");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
                gvMisTurnos.DataSource = null;
                gvMisTurnos.DataBind();
            }
        }

        private void OcultarMensaje()
        {
            lblMensaje.Visible = false;
            lblMensaje.Text = "";
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
        }

        private void MostrarError(string msg)
        {
            lblMensaje.Text = msg;
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
            lblMensaje.Visible = true;
        }

        private void MostrarInfo(string msg)
        {
            lblMensaje.Text = msg;
            lblMensaje.CssClass = "alert alert-info d-block mb-3";
            lblMensaje.Visible = true;
        }

    }
}