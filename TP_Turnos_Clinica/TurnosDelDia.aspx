<%@ Page Title="Turnos del día"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="TurnosDelDia.aspx.cs"
    Inherits="TP_Turnos_Clinica.TurnosDelDia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex align-items-center justify-content-between mb-3">
        <div>
            <h2 class="mb-0">Turnos del día</h2>
            <small class="text-muted">Gestión de turnos y pagos (Admin / Recepción)</small>
        </div>
        <asp:HyperLink runat="server"
            NavigateUrl="~/Home.aspx"
            CssClass="btn btn-outline-secondary">
            Volver
        </asp:HyperLink>
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
                    <small class="text-muted">Simulación para el TP</small>
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

    <!-- ✅ FILTROS -->
    <div class="card shadow-sm mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">

                <div class="col-12 col-md-3">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server"
                        CssClass="form-control"
                        TextMode="Date" />
                </div>

                <!-- ✅ NUEVO: DNI -->
                <div class="col-12 col-md-3">
                    <label class="form-label">DNI paciente</label>
                    <asp:TextBox ID="txtDni" runat="server"
                        CssClass="form-control"
                        MaxLength="15"
                        placeholder="Ej: 40123456" />
                </div>

                <div class="col-12 col-md-2">
                    <asp:Button ID="btnBuscar" runat="server"
                        Text="Buscar"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnBuscar_Click" />
                </div>

                <div class="col-12 col-md-4">
                    <div class="form-check mt-4">
                        <asp:CheckBox ID="chkMostrarCancelados" runat="server"
                            CssClass="form-check-input"
                            AutoPostBack="true"
                            OnCheckedChanged="chkMostrarCancelados_CheckedChanged" />
                        <label class="form-check-label">
                            Mostrar turnos cancelados
                        </label>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-body">

            <asp:GridView ID="dgvTurnos" runat="server"
                CssClass="table table-striped table-hover align-middle"
                AutoGenerateColumns="false"
                DataKeyNames="TurnoID,EstadoTurno,EstadoPago"
                OnRowCommand="dgvTurnos_ComandoPorFila"
                OnRowDataBound="dgvTurnos_FilaDataBound">

                <Columns>

                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                    <asp:BoundField DataField="Medico" HeaderText="Médico" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />

                    <asp:TemplateField HeaderText="Estado turno">
    <ItemTemplate>
        <span class='badge <%# 
            Eval("EstadoTurno").ToString().Equals("Cancelado", StringComparison.OrdinalIgnoreCase) ? "bg-danger" :
            Eval("EstadoTurno").ToString().Equals("Atendido", StringComparison.OrdinalIgnoreCase) ? "bg-success" :
            Eval("EstadoTurno").ToString().Equals("Agendado", StringComparison.OrdinalIgnoreCase) ? "bg-primary" :
            Eval("EstadoTurno").ToString().Equals("Reprogramado", StringComparison.OrdinalIgnoreCase) ? "bg-warning text-dark" :
            Eval("EstadoTurno").ToString().Equals("No asistió", StringComparison.OrdinalIgnoreCase) ? "bg-warning text-dark" :
            "bg-secondary"
        %>'>
            <%# Eval("EstadoTurno") %>
        </span>
    </ItemTemplate>
</asp:TemplateField>

                    <asp:TemplateField HeaderText="Estado pago">
                        <ItemTemplate>
                            <span class='badge
                                <%# Eval("EstadoPago").ToString() == "Confirmado" ? "bg-success" :
                                    "bg-warning text-dark" %>'>
                                <%# Eval("EstadoPago") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="MedioPago" HeaderText="Medio" />

                    <asp:ButtonField
                        Text="Confirmar pago"
                        CommandName="ConfirmarPago"
                        ButtonType="Button" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                Text="Cancelar"
                                CommandName="Cancelar"
                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                CssClass="btn btn-danger btn-sm"
                                OnClientClick="return confirm('¿Seguro que querés cancelar el turno?');" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                Text="No asistió"
                                CommandName="NoAsistio"
                                CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                CssClass="btn btn-warning btn-sm"
                                OnClientClick="return confirm('¿Marcar turno como NO ASISTIÓ?');" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:ButtonField
                        Text="Reprogramar"
                        CommandName="Reprogramar"
                        ButtonType="Button" />

                </Columns>
            </asp:GridView>

        </div>
    </div>

</asp:Content>