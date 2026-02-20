<%@ Page Title="Panel Médico" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PanelMedico.aspx.cs"
    Inherits="TP_Turnos_Clinica.PanelMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
  
    .full-bleed{
        width: 100vw;
        margin-left: calc(50% - 50vw);
        margin-right: calc(50% - 50vw);
        padding-left: 26px;
        padding-right: 26px;
    }
    @media (min-width: 1400px){
        .full-bleed{ padding-left: 38px; padding-right: 38px; }
    }

    .page-wrap{ padding-top:14px; padding-bottom:24px; }
    .page-title{ font-weight:800; margin:0; letter-spacing:-.2px; }
    .subtle{ color:#6c757d; }

    .card-soft{
        border:1px solid rgba(0,0,0,.08);
        border-radius:14px;
        overflow:hidden;
        background:#fff;
    }
    .card-soft .card-header{
        background:linear-gradient(90deg, rgba(13,110,253,.12), rgba(13,110,253,.05));
        border-bottom:1px solid rgba(0,0,0,.06);
        padding:14px 18px;
    }
    .card-soft .card-body{ padding:18px; }

    .btn-chip{
        border-radius:10px;
        padding-left:.85rem;
        padding-right:.85rem;
        font-weight:600;
    }

    .pillbar{ display:flex; flex-wrap:wrap; gap:.5rem; margin-top:.35rem; }
    .pill{
        display:inline-flex;
        align-items:center;
        gap:.4rem;
        padding:.35rem .6rem;
        border-radius:999px;
        border:1px solid rgba(0,0,0,.08);
        background:#fff;
        font-size:.85rem;
    }

    .form-label{ font-weight:600; }
    .form-control, .form-select{ border-radius:12px; }

   
    .table-wrap{
        border-radius:14px;
        overflow:hidden;
        border:1px solid rgba(0,0,0,.08);
    }
    .table thead th{ white-space:nowrap; }
    .table td, .table th{ vertical-align:middle; }
    .table td{ font-size:.9rem; }

  
    .badge-soft{
        display:inline-flex;
        align-items:center;
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
        border-radius:10px;
        font-weight:600;
        padding:.25rem .55rem;
        font-size:.78rem;
        white-space:nowrap;
    }
    .col-actions{ min-width: 200px; }

    .alert{ border-radius:14px; }
</style>

<div class="full-bleed">
    <div class="page-wrap">

     
        <div class="d-flex align-items-start justify-content-between mb-3">
            <div>
                <h2 class="page-title">Panel del Médico</h2>
                <div class="subtle">Turnos asignados + acceso rápido a historial y evoluciones</div>
                <div class="pillbar">
                    <span class="pill">📅 Filtrá por fecha</span>
                    <span class="pill">✅ Solo hoy</span>
                    <span class="pill">🚫 Ocultar cancelados</span>
                </div>
            </div>

            <a class="btn btn-outline-secondary btn-chip" href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>
        </div>

        <asp:Label ID="lblMsg" runat="server"
            CssClass="alert alert-danger d-block mb-3"
            Visible="false" />

  
        <div class="card card-soft shadow-sm mb-3">
            <div class="card-header">
                <div class="fw-semibold">Filtros</div>
                <small class="subtle">Definí un rango o marcá “Solo hoy”</small>
            </div>

            <div class="card-body">
                <div class="row g-3 align-items-end">

                    <div class="col-12 col-md-3">
                        <label class="form-label">Desde</label>
                        <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <div class="col-12 col-md-3">
                        <label class="form-label">Hasta</label>
                        <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>

                    <div class="col-12 col-md-4">
                        <div class="d-flex flex-column gap-2 pt-1">
                            <div class="form-check">
                                <asp:CheckBox ID="chkSoloHoy" runat="server" CssClass="form-check-input"
                                    AutoPostBack="true" OnCheckedChanged="Filtros_CheckedChanged" />
                                <label class="form-check-label" for="<%= chkSoloHoy.ClientID %>">Solo hoy</label>
                            </div>

                            <div class="form-check">
                                <asp:CheckBox ID="chkOcultarCancelados" runat="server" CssClass="form-check-input"
                                    AutoPostBack="true" OnCheckedChanged="Filtros_CheckedChanged" />
                                <label class="form-check-label" for="<%= chkOcultarCancelados.ClientID %>">
                                    Ocultar cancelados
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-2 d-grid">
                        <asp:Button ID="btnBuscar" runat="server"
                            Text="Buscar"
                            CssClass="btn btn-primary btn-chip"
                            OnClick="btnBuscar_Click" />
                    </div>

                </div>
            </div>
        </div>

       
        <div class="card card-soft shadow-sm">
            <div class="card-header">
                <div class="fw-semibold">Turnos</div>
                <small class="subtle">Acciones: ver historial del paciente y cargar evolución</small>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-wrap">

                    <asp:GridView ID="gvTurnos" runat="server"
                        CssClass="table table-hover table-striped align-middle mb-0"
                        AutoGenerateColumns="false"
                        DataKeyNames="TurnoID,PacienteID,EstadoTurno,EstadoPago"
                        HeaderStyle-CssClass="table-dark">

                        <Columns>

                            <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %> -
                                    <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="PacienteNombre" HeaderText="Paciente" />
                            <asp:BoundField DataField="EspecialidadNombre" HeaderText="Especialidad" />
                            <asp:BoundField DataField="MotivoConsulta" HeaderText="Motivo" />

                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge-soft <%#
                                        Eval("EstadoTurno").ToString() == "Cancelado" ? "badge-bad" :
                                        Eval("EstadoTurno").ToString() == "Atendido" ? "badge-ok" :
                                        Eval("EstadoTurno").ToString() == "Reprogramado" ? "badge-warn" :
                                        Eval("EstadoTurno").ToString() == "Agendado" ? "badge-info" :
                                        "badge-muted"
                                    %>'>
                                        <%# Eval("EstadoTurno") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pago">
                                <ItemTemplate>
                                    <span class='badge-soft <%#
                                        Eval("EstadoPago").ToString() == "Confirmado" || Eval("EstadoPago").ToString() == "Aprobado"
                                            ? "badge-ok"
                                            : Eval("EstadoPago").ToString() == "Pendiente"
                                            ? "badge-warn"
                                            : "badge-muted"
                                    %>'>
                                        <%# Eval("EstadoPago") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones">
                                <HeaderStyle CssClass="col-actions" />
                                <ItemStyle CssClass="col-actions" />
                                <ItemTemplate>

                                    <asp:HyperLink runat="server"
                                        Text="Historial"
                                        CssClass="btn btn-sm btn-outline-secondary action-btn me-1"
                                        NavigateUrl='<%# ResolveUrl("~/HistorialPaciente.aspx?pacienteId=" + Eval("PacienteID")) %>' />

                                    <asp:HyperLink runat="server"
                                        Text="Evolucionar"
                                        CssClass="btn btn-sm btn-outline-primary action-btn"
                                        NavigateUrl='<%# ResolveUrl("~/EvolucionForm.aspx?turnoId=" + Eval("TurnoID")) %>' />

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