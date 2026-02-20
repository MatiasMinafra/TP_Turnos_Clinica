<%@ Page Title="Historial del Paciente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HistorialPaciente.aspx.cs"
    Inherits="TP_Turnos_Clinica.HistorialPaciente" %>

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
    .page-title{ font-weight:800; letter-spacing:-.2px; margin:0; }
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
        white-space:nowrap;
    }

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
        border:1px solid rgba(0,0,0,.06);
        white-space:nowrap;
    }
    .badge-ok{ background:rgba(25,135,84,.12); color:#146c43; }
    .badge-warn{ background:rgba(255,193,7,.18); color:#8a6d00; }
    .badge-bad{ background:rgba(220,53,69,.12); color:#b02a37; }
    .badge-info{ background:rgba(13,110,253,.12); color:#0b5ed7; }
    .badge-muted{ background:rgba(108,117,125,.12); color:#495057; }

    .truncate{
        max-width: 520px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        display: block;
    }
    @media (max-width: 992px){
        .truncate{ max-width: 280px; }
    }

    .alert{ border-radius:14px; }
</style>

<div class="full-bleed">
    <div class="page-wrap">

     
        <div class="d-flex flex-wrap align-items-start justify-content-between gap-2 mb-3">
            <div>
                <h2 class="page-title">Historial Clínico del Paciente</h2>
                <div class="subtle">Turnos + evoluciones registradas</div>
            </div>

          
            <asp:HyperLink ID="lnkVolver" runat="server"
                CssClass="btn btn-outline-secondary btn-chip"
                NavigateUrl="~/MisTurnos.aspx"
                Text="← Volver" />
        </div>

        <asp:Label ID="lblMsg" runat="server"
            CssClass="alert alert-danger d-block mb-3"
            Visible="false" />

    
        <asp:Panel ID="pnlPaciente" runat="server" CssClass="card card-soft shadow-sm mb-3" Visible="false">
            <div class="card-header">
                <div class="fw-semibold">Datos del paciente</div>
                <small class="subtle">Información básica</small>
            </div>
            <div class="card-body">
                <h5 class="mb-1">
                    Paciente: <asp:Label ID="lblPacienteNombre" runat="server" />
                </h5>

                <div class="subtle">
                    DNI: <asp:Label ID="lblPacienteDni" runat="server" />
                    &nbsp;•&nbsp; Nacimiento: <asp:Label ID="lblPacienteNac" runat="server" />
                    &nbsp;•&nbsp; Edad: <asp:Label ID="lblPacienteEdad" runat="server" />
                    &nbsp;•&nbsp; Email: <asp:Label ID="lblPacienteEmail" runat="server" />
                </div>
            </div>
        </asp:Panel>

       
        <div class="card card-soft shadow-sm">
            <div class="card-header">
                <div class="fw-semibold">Historial</div>
                <small class="subtle">Listado de consultas registradas</small>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-wrap">

                    <asp:GridView ID="gvHistorial" runat="server"
                        CssClass="table table-hover table-striped align-middle mb-0"
                        AutoGenerateColumns="false"
                        EmptyDataText="No hay historial para este paciente."
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

                            <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                            <asp:BoundField DataField="Medico" HeaderText="Médico" />

                 
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge-soft <%#
                                        Eval("EstadoTurno").ToString() == "Cancelado" ? "badge-bad" :
                                        Eval("EstadoTurno").ToString() == "Atendido" ? "badge-ok" :
                                        Eval("EstadoTurno").ToString() == "Confirmado" ? "badge-ok" :
                                        Eval("EstadoTurno").ToString() == "Reprogramado" ? "badge-warn" :
                                        (Eval("EstadoTurno").ToString() == "No Asistió" || Eval("EstadoTurno").ToString() == "No Asistio") ? "badge-muted" :
                                        Eval("EstadoTurno").ToString() == "Cerrado" ? "badge-muted" :
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
                                            : "badge-bad"
                                    %>'>
                                        <%# Eval("EstadoPago") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                           
                            <asp:TemplateField HeaderText="Evolución">
                                <ItemTemplate>
                                    <%# (bool)Eval("TieneEvolucion")
                                        ? "<span class='badge-soft badge-ok'>Sí</span>"
                                        : "<span class='badge-soft badge-muted'>No</span>" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            
                            <asp:TemplateField HeaderText="Detalle clínico">
                                <ItemTemplate>
                                    <%# (bool)Eval("TieneEvolucion")
                                        ? "<span class='truncate' title=\"" + Server.HtmlEncode(Eval("DescripcionEvolucion").ToString()) + "\">" +
                                            Server.HtmlEncode(
                                                Eval("DescripcionEvolucion").ToString().Length > 140
                                                ? Eval("DescripcionEvolucion").ToString().Substring(0, 140) + "..."
                                                : Eval("DescripcionEvolucion").ToString()
                                            ) +
                                          "</span>"
                                        : "-" %>
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