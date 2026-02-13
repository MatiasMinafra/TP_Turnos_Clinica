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
    public partial class HistorialPaciente : System.Web.UI.Page
    {
        private readonly EvolucionNegocio negocio = new EvolucionNegocio();

        private int PacienteId
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["pacienteId"], out id) && id > 0)
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

            if (!IsPostBack)
                Cargar();
        }

        private void Cargar()
        {
            try
            {
                lblMsg.Visible = false;
                lblMsg.Text = "";

                if (PacienteId <= 0)
                    throw new Exception("Paciente inválido.");

                // 🔹 Cargar datos del paciente
                var pacNeg = new PacienteNegocio();
                var p = pacNeg.ObtenerPorId(PacienteId);

                if (p == null)
                    throw new Exception("El paciente no existe.");

                pnlPaciente.Visible = true;

                lblPacienteNombre.Text = $"{p.Apellido}, {p.Nombre}";
                lblPacienteDni.Text = p.DNI ?? "-";
                lblPacienteEmail.Text = p.Email ?? "-";

                if (p.FechaNacimiento.HasValue)
                {
                    var fn = p.FechaNacimiento.Value.Date;
                    lblPacienteNac.Text = fn.ToString("dd/MM/yyyy");

                    int edad = DateTime.Today.Year - fn.Year;
                    if (fn > DateTime.Today.AddYears(-edad)) edad--;

                    lblPacienteEdad.Text = edad.ToString();
                }
                else
                {
                    lblPacienteNac.Text = "-";
                    lblPacienteEdad.Text = "-";
                }

                // 🔹 Cargar historial
                var lista = negocio.HistorialPaciente(PacienteId);

                gvHistorial.DataSource = lista;
                gvHistorial.DataBind();

                if (lista == null || lista.Count == 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.CssClass = "alert alert-info d-block mb-3";
                    lblMsg.Text = "No hay historial registrado.";
                }
            }
            catch (Exception ex)
            {
                pnlPaciente.Visible = false;

                lblMsg.Visible = true;
                lblMsg.CssClass = "alert alert-danger d-block mb-3";
                lblMsg.Text = ex.Message;

                gvHistorial.DataSource = null;
                gvHistorial.DataBind();
            }
        }
    }
    
}