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
    public partial class MedicoHorarios : System.Web.UI.Page
    {
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly TurnoTrabajoNegocio turnoTrabajoNegocio = new TurnoTrabajoNegocio();
        private readonly MedicoTurnoTrabajoNegocio mttNegocio = new MedicoTurnoTrabajoNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarCombos();
                CargarGrilla();
            }
        }

        private void CargarCombos()
        {
            var medicos = medicoNegocio.Listar("", true);
            ddlMedico.DataSource = medicos;
            ddlMedico.DataValueField = "MedicoID";
            ddlMedico.DataTextField = "Apellido";
            ddlMedico.DataBind();

            for (int i = 0; i < medicos.Count; i++)
            {
                var m = medicos[i];
                ddlMedico.Items[i].Text = $"{m.Apellido}, {m.Nombre} ({m.Matricula})";
            }

            ddlMedico.Items.Insert(0, new ListItem("-- Seleccionar --", ""));

            var turnos = turnoTrabajoNegocio.Listar("", true);
            ddlTurnoTrabajo.DataSource = turnos;
            ddlTurnoTrabajo.DataValueField = "TurnoTrabajoID";
            ddlTurnoTrabajo.DataTextField = "Nombre";
            ddlTurnoTrabajo.DataBind();

            ddlTurnoTrabajo.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
        }
        private int? MedicoSeleccionado()
        {
            if (int.TryParse(ddlMedico.SelectedValue, out int id) && id > 0)
                return id;
            return null;
        }

        private void CargarGrilla()
        {
            lblMensaje.Visible = false;

            var medicoId = MedicoSeleccionado();
            if (!medicoId.HasValue)
            {
                gvHorarios.DataSource = new List<Dto_MedicosTurnosTrabajo>();
                gvHorarios.DataBind();
                return;
            }

            bool soloActivos = !chkInactivos.Checked;
            var lista = mttNegocio.ListarPorMedico(medicoId.Value, soloActivos);

            gvHorarios.DataSource = lista;
            gvHorarios.DataBind();
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
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
                var medicoId = MedicoSeleccionado();
                if (!medicoId.HasValue)
                    throw new Exception("Seleccioná un médico.");

                if (!byte.TryParse(ddlDia.SelectedValue, out byte diaSemana) || diaSemana < 1 || diaSemana > 7)
                    throw new Exception("Día inválido.");

                if (!int.TryParse(ddlTurnoTrabajo.SelectedValue, out int turnoTrabajoId) || turnoTrabajoId <= 0)
                    throw new Exception("Seleccioná un turno de trabajo.");

                mttNegocio.Asignar(medicoId.Value, turnoTrabajoId, diaSemana);

                lblMensaje.Text = "Horario asignado correctamente.";
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

        protected void gvHorarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ToggleActivo")
                return;

            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id = Convert.ToInt32(gvHorarios.DataKeys[index].Value);

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