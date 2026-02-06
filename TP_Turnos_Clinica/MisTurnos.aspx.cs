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
        private void CargarGrilla()
        {
            lblMensaje.Visible = false;

            var u = (Usuario)Session["usuario"];
            int medicoId = u.MedicoID.Value;

            DateTime desde, hasta;
            if (!DateTime.TryParse(txtDesde.Text, out desde)) desde = DateTime.Today;
            if (!DateTime.TryParse(txtHasta.Text, out hasta)) hasta = DateTime.Today.AddDays(7);

            if (hasta < desde)
            {
                MostrarError("La fecha 'Hasta' no puede ser menor que 'Desde'.");
                return;
            }

            try
            {
                var lista = turnosNegocio.ListarMisTurnos(medicoId, desde, hasta);
                gvMisTurnos.DataSource = lista;
                gvMisTurnos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }
        private void MostrarError(string msg)
        {
            lblMensaje.Text = msg;
            lblMensaje.CssClass = "alert alert-danger d-block mb-3";
            lblMensaje.Visible = true;
        }

    }
}