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
    public partial class AsignacionTurnoMedico : System.Web.UI.Page
    {
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly TurnoTrabajoNegocio turnoTrabajoNegocio = new TurnoTrabajoNegocio();
        private readonly MedicoTurnoTrabajoNegocio mttNegocio = new MedicoTurnoTrabajoNegocio();

        private int MedicoId
        {
            get
            {
                if (int.TryParse(Request.QueryString["id"], out int id) && id > 0)
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

            if (MedicoId <= 0)
            {
                Response.Redirect("~/Medicos.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarCabecera();
                CargarComboTurnosTrabajo();
                CargarGrilla();
            }
        }

        private void CargarCabecera()
        {
            var m = medicoNegocio.ObtenerPorId(MedicoId);
            if (m == null)
            {
                Response.Redirect("~/Medicos.aspx");
                return;
            }

            lblMedico.Text = $"Médico: {m.Apellido}, {m.Nombre} (Matrícula: {m.Matricula})";
        }

        private void CargarComboTurnosTrabajo()
        {
            var turnos = turnoTrabajoNegocio.Listar("", true);

            ddlTurnoTrabajo.DataSource = turnos;
            ddlTurnoTrabajo.DataValueField = "TurnoTrabajoID";
            ddlTurnoTrabajo.DataTextField = "Nombre";
            ddlTurnoTrabajo.DataBind();

            ddlTurnoTrabajo.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
        }

        private void CargarGrilla()
        {
            lblMensaje.Visible = false;

            bool soloActivos = !chkInactivos.Checked;
            var lista = mttNegocio.ListarPorMedico(MedicoId, soloActivos);

            gvAsignaciones.DataSource = lista;
            gvAsignaciones.DataBind();
        }

        protected void chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            lblMensaje.Visible = false;

            try
            {
                if (!byte.TryParse(ddlDia.SelectedValue, out byte diaSemana) || diaSemana < 1 || diaSemana > 7)
                    throw new Exception("Día inválido.");

                if (!int.TryParse(ddlTurnoTrabajo.SelectedValue, out int turnoTrabajoId) || turnoTrabajoId <= 0)
                    throw new Exception("Seleccioná un turno de trabajo.");

                mttNegocio.Asignar(MedicoId, turnoTrabajoId, diaSemana);

                lblMensaje.Text = "Asignación guardada correctamente.";
                lblMensaje.CssClass = "alert alert-success d-block mb-3";
                lblMensaje.Visible = true;

                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                lblMensaje.CssClass = "alert alert-danger d-block mb-3";
                lblMensaje.Visible = true;
            }
        }

        protected void gvAsignaciones_ComandoFila(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "CambiarEstado")
                return;

            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvAsignaciones.DataKeys[index].Value);

                var item = mttNegocio.ObtenerPorId(id);
                if (item == null) return;

                if (item.Activo) mttNegocio.Desactivar(id);
                else mttNegocio.Activar(id);

                CargarGrilla();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                lblMensaje.CssClass = "alert alert-danger d-block mb-3";
                lblMensaje.Visible = true;
            }
        }
    }
}