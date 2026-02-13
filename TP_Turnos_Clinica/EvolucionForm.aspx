<%@ Page Title="Evolución" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EvolucionForm.aspx.cs"
    Inherits="TP_Turnos_Clinica.EvolucionForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Registrar evolución</h3>

    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert alert-danger d-block mb-3" />

    <div class="card mb-3">
        <div class="card-body">

            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Paciente</label>
                    <asp:TextBox ID="txtPaciente" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="col-md-6">
                    <label class="form-label">Médico</label>
                    <asp:TextBox ID="txtMedico" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Fecha turno</label>
                    <asp:TextBox ID="txtFechaTurno" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Horario</label>
                    <asp:TextBox ID="txtHorario" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>

                <div class="col-md-4 d-flex align-items-end justify-content-end">
                    <asp:HyperLink ID="lnkHistorial" runat="server"
                        CssClass="btn btn-outline-secondary w-100"
                        Text="Ver historial del paciente" />
                </div>

                <div class="col-12">
                    <label class="form-label">Evolución / Diagnóstico / Indicaciones</label>
                    <asp:TextBox ID="txtDescripcion" runat="server"
                        CssClass="form-control" TextMode="MultiLine" Rows="8"
                        placeholder="Escriba la evolución de la consulta..." />
                </div>

                <div class="col-12 d-flex gap-2">
                    <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar evolución"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" />

                    <asp:HyperLink ID="lnkVolver" runat="server"
                        Text="Volver a Mis Turnos"
                        NavigateUrl="~/MisTurnos.aspx"
                        CssClass="btn btn-outline-secondary" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>
