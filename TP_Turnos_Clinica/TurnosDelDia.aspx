<%@ Page Title="Turnos del día" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="TurnosDelDia.aspx.cs"
    Inherits="TP_Turnos_Clinica.TurnosDelDia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <div>
            <h2 class="mb-0">Turnos del día</h2>
            <small class="text-muted">Gestión de turnos y pagos (Admin / Recepción)</small>
        </div>
        <asp:HyperLink runat="server" NavigateUrl="~/Home.aspx" CssClass="btn btn-outline-secondary">Volver</asp:HyperLink>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

   
    <asp:Panel ID="pnlPago" runat="server" Visible="false" CssClass="card shadow-sm mb-3">
        <div class="card-body">
            <h5 class="mb-2">Confirmar pago</h5>

            <asp:HiddenField ID="hfTurnoIdPago" runat="server" />

            <div class="row g-2 align-items-end">
                <div class="col-12 col-md-6">
                    <label class="form-label">Comprobante (opcional)</label>
                    <asp:TextBox ID="txtComprobante" runat="server"
                        CssClass="form-control"
                        MaxLength="200"
                        placeholder="Ej: MP-8439201 / TRX-12345" />
                    <small class="text-muted">Simulación para el TP (número de operación)</small>
                </div>

                <div class="col-12 col-md-3">
                    <asp:Button ID="btnConfirmarPagoFinal" runat="server"
                        Text="Confirmar"
                        CssClass="btn btn-success w-100"
                        OnClick="btnConfirmarPagoFinal_Click" />
                </div>

                <div class="col-12 col-md-3">
                    <asp:Button ID="btnCancelarPago" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-outline-secondary w-100"
                        OnClick="btnCancelarPago_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="card shadow-sm mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-12 col-md-4">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-12 col-md-3">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-body">

            <asp:GridView ID="dgvTurnos" runat="server"
                CssClass="table table-striped table-hover align-middle"
                AutoGenerateColumns="false"
                DataKeyNames="TurnoID"
                OnRowCommand="dgvTurnos_ComandoPorFila">

                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                    <asp:BoundField DataField="Medico" HeaderText="Médico" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                    <asp:BoundField DataField="EstadoTurno" HeaderText="Estado turno" />
                    <asp:BoundField DataField="EstadoPago" HeaderText="Estado pago" />
                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="MedioPago" HeaderText="Medio" />

                    
                    <asp:ButtonField Text="Confirmar pago" CommandName="ConfirmarPago" ButtonType="Button" />
                    <asp:ButtonField Text="Cancelar" CommandName="Cancelar" ButtonType="Button" />
                    <asp:ButtonField Text="Reprogramar" CommandName="Reprogramar" ButtonType="Button" />
                </Columns>

            </asp:GridView>

        </div>
    </div>

</asp:Content>