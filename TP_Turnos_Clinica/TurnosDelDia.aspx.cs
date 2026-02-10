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
                fecha = DateTime.Parse(txtFecha.Text);

            var lista = negocio.ListarDelDia(fecha);

       
            if (!chkMostrarCancelados.Checked)
            {
                lista = lista
                    .Where(x => x.EstadoTurno != "Cancelado")
                    .ToList();
            }

            dgvTurnos.DataSource = lista;
            dgvTurnos.DataBind();
        }

        protected void dgvTurnos_ComandoPorFila(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                int turnoId = Convert.ToInt32(dgvTurnos.DataKeys[rowIndex].Values["TurnoID"]);
                string estadoTurno = dgvTurnos.DataKeys[rowIndex].Values["EstadoTurno"]?.ToString() ?? "";
                string estadoPago = dgvTurnos.DataKeys[rowIndex].Values["EstadoPago"]?.ToString() ?? "";

                if (e.CommandName == "ConfirmarPago")
                {
                  
                    if (estadoTurno.Equals("Cancelado", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("No se puede confirmar el pago: el turno está cancelado.");

                    if (!estadoPago.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("El pago no está pendiente.");

                    hfTurnoIdPago.Value = turnoId.ToString();
                    txtComprobante.Text = "";
                    pnlPago.Visible = true;

                    lblMsg.CssClass = "alert alert-info d-block mb-3";
                    lblMsg.Text = $"Confirmar pago del turno #{turnoId}.";
                    return;
                }


                if (e.CommandName == "NoAsistio")
                {
                    negocio.MarcarNoAsistio(turnoId);

                    lblMsg.CssClass = "alert alert-success d-block mb-3";
                    lblMsg.Text = "Turno marcado como NO ASISTIÓ.";

                    CargarGrilla();
                    return;
                }

                if (e.CommandName == "Cancelar")
                {
                    
                    negocio.CancelarTurno(turnoId);

                    pnlPago.Visible = false;

                    lblMsg.CssClass = "alert alert-success d-block mb-3";
                    lblMsg.Text = "Turno cancelado.";

                    CargarGrilla();
                    return;
                }

                if (e.CommandName == "Reprogramar")
                {
                    
                    if (estadoTurno.Equals("Cancelado", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("No se puede reprogramar: el turno está cancelado.");

                    
                    if (estadoTurno.Equals("Cerrado", StringComparison.OrdinalIgnoreCase))
                        throw new Exception("No se puede reprogramar: el turno está cerrado.");

                   
                    if (estadoTurno.Equals("No asistió", StringComparison.OrdinalIgnoreCase) &&
                        !estadoPago.Equals("Confirmado", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception("No se puede reprogramar un 'No asistió' si el pago está pendiente.");
                    }

                    Response.Redirect($"~/AgendaTurnos.aspx?reprog=1&turnoId={turnoId}", false);
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

        protected void chkMostrarCancelados_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void dgvTurnos_FilaDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            string estadoTurno = DataBinder.Eval(e.Row.DataItem, "EstadoTurno")?.ToString();

            if (estadoTurno == "Cancelado")
            {
               
                e.Row.BackColor = System.Drawing.Color.LightGray;
                e.Row.ForeColor = System.Drawing.Color.Gray;

                
                foreach (TableCell celda in e.Row.Cells)
                {
                    foreach (Control ctrl in celda.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            btn.Enabled = false;
                            btn.CssClass += " disabled";
                        }
                        else if (ctrl is LinkButton lnk)
                        {
                            lnk.Enabled = false;
                            lnk.CssClass += " disabled";
                        }
                    }
                }
            }
        }

    }
}