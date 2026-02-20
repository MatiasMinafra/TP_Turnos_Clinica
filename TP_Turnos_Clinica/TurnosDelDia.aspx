<%@ Page Title="Turnos del día"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="TurnosDelDia.aspx.cs"
    Inherits="TP_Turnos_Clinica.TurnosDelDia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    
    .full-bleed{
        width: 100vw;
        margin-left: calc(50% - 50vw);
        margin-right: calc(50% - 50vw);
        padding-left: 18px;
        padding-right: 18px;
    }
    @media (min-width: 1400px){
        .full-bleed{
            padding-left: 34px;
            padding-right: 34px;
        }
    }

    .page-wrap { padding-top:14px; padding-bottom:24px; }
    .page-title { font-weight:800; margin-bottom:0; letter-spacing:-.2px; }
    .subtle { color:#6c757d; }

    .card-soft{
        border:1px solid rgba(0,0,0,.08);
        border-radius:14px;
        overflow:hidden;
        background:#fff;
    }
    .card-soft .card-header{
        background:linear-gradient(90deg, rgba(13,110,253,.12), rgba(13,110,253,.05));
        border-bottom:1px solid rgba(0,0,0,.06);
    }

    .btn-chip{
        border-radius:10px;
        padding-left:.85rem;
        padding-right:.85rem;
        font-weight:600;
        white-space:nowrap;
    }

    .table-wrap{
        border-radius:14px;
        overflow:hidden;
        border:1px solid rgba(0,0,0,.08);
    }
    .table-wrap table{ width:100%; }

    .table td, .table th { vertical-align: middle; }
    .table thead th { white-space:nowrap; }
    .table td{ font-size:.88rem; }

    .badge-soft{
        display:inline-flex;
        align-items:center;
        gap:.35rem;
        padding:.35rem .6rem;
        border-radius:999px;
        font-weight:700;
        font-size:.82rem;
        white-space:nowrap;
        border:1px solid rgba(0,0,0,.06);
    }

    .badge-ok{ background:rgba(25,135,84,.12); color:#146c43; }
    .badge-warn{ background:rgba(255,193,7,.18); color:#8a6d00; }
    .badge-bad{ background:rgba(220,53,69,.12); color:#b02a37; }
    .badge-info{ background:rgba(13,110,253,.12); color:#0b5ed7; }
    .badge-muted{ background:rgba(108,117,125,.12); color:#495057; }

    .action-btn{
        border-radius: 8px;
        padding: .20rem .45rem;
        font-size: .75rem;
        font-weight: 600;
        white-space: nowrap;
    }

    .kpi{
        display:flex;
        flex-wrap:wrap;
        gap:.5rem;
        margin-top:.25rem;
    }
    .kpi .pill{
        display:inline-flex;
        align-items:center;
        padding:.35rem .6rem;
        border-radius:999px;
        border:1px solid rgba(0,0,0,.08);
        background:#fff;
        font-size:.85rem;
    }

    .col-actions{ min-width: 260px; }
</style>

<div class="full-bleed">
    <div class="page-wrap">

        <div class="d-flex align-items-start justify-content-between mb-3">
            <div>
                <h2 class="page-title">Turnos del día</h2>
                <div class="subtle">Gestión de turnos y pagos (Admin / Recepción)</div>
                <div class="kpi">
                    <span class="pill">📅 Filtrá por fecha</span>
                    <span class="pill">🪪 Filtrá por DNI</span>
                    <span class="pill">💳 Confirmá pagos</span>
                </div>
            </div>

            <asp:HyperLink runat="server"
                NavigateUrl="~/Home.aspx"
                CssClass="btn btn-outline-secondary btn-chip">
                Volver
            </asp:HyperLink>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

        <asp:Panel ID="pnlPago" runat="server" Visible="false" CssClass="card card-soft shadow-sm mb-3">
            <div class="card-header py-3">
                <div class="fw-semibold">Confirmar pago</div>
                <small class="subtle">Registro del comprobante (simulación TP)</small>
            </div>

            <div class="card-body">
                <asp:HiddenField ID="hfTurnoIdPago" runat="server" />

                <div class="row g-2 align-items-end">
                    <div class="col-12 col-md-6">
                        <label class="form-label mb-1">Comprobante (opcional)</label>
                        <asp:TextBox ID="txtComprobante" runat="server"
                            CssClass="form-control"
                            MaxLength="200"
                            placeholder="Ej: MP-8439201 / TRX-12345" />
                    </div>

                    <div class="col-12 col-md-3 d-grid">
                        <asp:Button ID="btnConfirmarPagoFinal" runat="server"
                            Text="Confirmar"
                            CssClass="btn btn-success btn-chip"
                            OnClick="btnConfirmarPagoFinal_Click" />
                    </div>

                    <div class="col-12 col-md-3 d-grid">
                        <asp:Button ID="btnCancelarPago" runat="server"
                            Text="Cancelar"
                            CssClass="btn btn-outline-secondary btn-chip"
                            OnClick="btnCancelarPago_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <div class="card card-soft shadow-sm mb-3">
            <div class="card-header py-3">
                <div class="fw-semibold">Filtros</div>
                <small class="subtle">Buscá por fecha / DNI y elegí si querés ver cancelados</small>
            </div>

            <div class="card-body">
                <div class="row g-2 align-items-end">

                    <div class="col-12 col-md-3">
                        <label class="form-label mb-1">Fecha</label>
                        <asp:TextBox ID="txtFecha" runat="server"
                            CssClass="form-control"
                            TextMode="Date" />
                    </div>

                    <div class="col-12 col-md-3">
                        <label class="form-label mb-1">DNI paciente</label>
                        <asp:TextBox ID="txtDni" runat="server"
                            CssClass="form-control"
                            MaxLength="15"
                            placeholder="Ej: 40123456" />
                    </div>

                    <div class="col-12 col-md-2 d-grid">
                        <asp:Button ID="btnBuscar" runat="server"
                            Text="Buscar"
                            CssClass="btn btn-primary btn-chip"
                            OnClick="btnBuscar_Click" />
                    </div>

                    <div class="col-12 col-md-4">
                        <div class="form-check mt-4">
                            <asp:CheckBox ID="chkMostrarCancelados" runat="server"
                                CssClass="form-check-input"
                                AutoPostBack="true"
                                OnCheckedChanged="chkMostrarCancelados_CheckedChanged" />
                            <label class="form-check-label" for="<%= chkMostrarCancelados.ClientID %>">
                                Mostrar turnos cancelados
                            </label>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="card card-soft shadow-sm">
            <div class="card-header py-3">
                <div class="fw-semibold">Listado de turnos</div>
                <small class="subtle">Acciones rápidas: pago / cancelar / no asistió / reprogramar</small>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-wrap" style="overflow-x:auto;">

                    <asp:GridView ID="dgvTurnos" runat="server"
                        CssClass="table table-hover table-striped align-middle mb-0"
                        AutoGenerateColumns="false"
                        DataKeyNames="TurnoID,EstadoTurno,EstadoPago"
                        HeaderStyle-CssClass="table-dark"
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
                                    <span class='badge-soft <%#
                                        Eval("EstadoTurno").ToString().Equals("Cancelado", StringComparison.OrdinalIgnoreCase) ? "badge-bad" :
                                        Eval("EstadoTurno").ToString().Equals("Atendido", StringComparison.OrdinalIgnoreCase) ? "badge-ok" :
                                        Eval("EstadoTurno").ToString().Equals("Agendado", StringComparison.OrdinalIgnoreCase) ? "badge-info" :
                                        Eval("EstadoTurno").ToString().Equals("Reprogramado", StringComparison.OrdinalIgnoreCase) ? "badge-warn" :
                                        Eval("EstadoTurno").ToString().Equals("No asistió", StringComparison.OrdinalIgnoreCase) ? "badge-warn" :
                                        "badge-muted"
                                    %>'>
                                        <%# Eval("EstadoTurno") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Estado pago">
                                <ItemTemplate>
                                    <span class='badge-soft <%# Eval("EstadoPago").ToString() == "Confirmado" ? "badge-ok" : "badge-warn" %>'>
                                        <%# Eval("EstadoPago") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="MedioPago" HeaderText="Medio" />

                            <asp:TemplateField HeaderText="Acciones">
                                <HeaderStyle CssClass="col-actions" />
                                <ItemStyle CssClass="col-actions" />
                                <ItemTemplate>
                                    <div class="d-flex flex-nowrap gap-2">
                                        <asp:LinkButton runat="server"
                                            Text="Pago"
                                            CommandName="ConfirmarPago"
                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                            CssClass="btn btn-outline-success btn-sm action-btn" />

                                        <asp:LinkButton runat="server"
                                            Text="Cancelar"
                                            CommandName="Cancelar"
                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                            CssClass="btn btn-outline-danger btn-sm action-btn"
                                            OnClientClick="return confirm('¿Seguro que querés cancelar el turno?');" />

                                        <asp:LinkButton runat="server"
                                            Text="No asistió"
                                            CommandName="NoAsistio"
                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                            CssClass="btn btn-outline-warning btn-sm action-btn"
                                            OnClientClick="return confirm('¿Marcar turno como NO ASISTIÓ?');" />

                                        <asp:LinkButton runat="server"
                                            Text="Reprog."
                                            CommandName="Reprogramar"
                                            CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                                            CssClass="btn btn-outline-primary btn-sm action-btn" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>
            </div>
        </div>

    </div>
</div>

</asp:Content>