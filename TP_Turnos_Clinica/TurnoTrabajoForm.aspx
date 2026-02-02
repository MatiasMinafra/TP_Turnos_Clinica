<%@ Page Title="Turno de Trabajo" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TurnoTrabajoForm.aspx.cs"
    Inherits="TP_Turnos_Clinica.TurnoTrabajoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-6">

                <h3 class="mb-4">
                    <asp:Label ID="lblTitulo" runat="server" Text="Nuevo Turno de Trabajo"></asp:Label>
                </h3>

                <asp:Label ID="lblMensaje" runat="server"
                    CssClass="alert alert-danger d-block mb-3"
                    Visible="false"></asp:Label>

                <div class="mb-3">
                    <label class="form-label fw-bold">Nombre *</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"
                        placeholder="Ej: Mañana" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Hora Inicio *</label>
                    <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control"
                        placeholder="08:00" />
                </div>

                <div class="mb-4">
                    <label class="form-label fw-bold">Hora Fin *</label>
                    <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control"
                        placeholder="12:00" />
                </div>

                <div class="d-flex gap-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" />

                    <asp:Button ID="btnVolver" runat="server" Text="Volver"
                        CssClass="btn btn-secondary"
                        OnClick="btnVolver_Click"
                        CausesValidation="false" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>
