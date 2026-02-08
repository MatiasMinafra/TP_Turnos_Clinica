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
    public partial class TurnosDelDia : System.Web.UI.Page
    {
        private TurnosNegocio negocio = new TurnosNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var u = (Usuario)Session["usuario"];
            if (u.RolID != RolesIds.ADMIN && u.RolID != RolesIds.RECEPCIONISTA)
            {
                Response.Redirect("~/Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
               
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                CargarGrilla();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "d-block mb-3";
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            DateTime fecha = DateTime.Today;

            if (!string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                
                fecha = DateTime.ParseExact(txtFecha.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            dgvTurnos.DataSource = negocio.ListarDelDia(fecha);
            dgvTurnos.DataBind();
        }

        protected void dgvTurnos_ComandoPorFila(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int turnoId = (int)dgvTurnos.DataKeys[rowIndex].Value;

                if (e.CommandName == "ConfirmarPago")
                {
                   
                    hfTurnoIdPago.Value = turnoId.ToString();
                    txtComprobante.Text = "";
                    pnlPago.Visible = true;

                    lblMsg.CssClass = "alert alert-info d-block mb-3";
                    lblMsg.Text = $"Confirmar pago del turno #{turnoId}.";
                    return;
                }

                if (e.CommandName == "Cancelar")
                {
                    
                    return;
                }

                if (e.CommandName == "Reprogramar")
                {
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "alert alert-danger d-block mb-3";
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnConfirmarPagoFinal_Click(object sender, EventArgs e)
        {
            try
            {
                int turnoId = int.Parse(hfTurnoIdPago.Value);
                string comprobante = txtComprobante.Text?.Trim();

                TurnosNegocio negocio = new TurnosNegocio();
                negocio.ConfirmarPago(turnoId, comprobante);

                pnlPago.Visible = false;

                lblMsg.CssClass = "alert alert-success d-block mb-3";
                lblMsg.Text = "Pago confirmado correctamente.";

                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "alert alert-danger d-block mb-3";
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnCancelarPago_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = false;
            hfTurnoIdPago.Value = "";
            txtComprobante.Text = "";
        }

    }
}