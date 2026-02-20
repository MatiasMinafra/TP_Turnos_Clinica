<%@ Page Title="Mis Estadísticas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MisEstadisticas.aspx.cs"
    Inherits="TP_Turnos_Clinica.MisEstadisticas" %>

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

    
    .kpi-card{
        border:1px solid rgba(0,0,0,.08);
        border-radius:14px;
        overflow:hidden;
        background:#fff;
        box-shadow: 0 .25rem .75rem rgba(0,0,0,.06);
    }
    .kpi-card .kpi-top{
        padding:14px 16px;
        border-bottom:1px solid rgba(0,0,0,.06);
        background: rgba(13,110,253,.06);
    }
    .kpi-card.kpi-success .kpi-top{ background: rgba(25,135,84,.08); }
    .kpi-card.kpi-warning .kpi-top{ background: rgba(255,193,7,.14); }

    .kpi-card .kpi-body{ padding:16px; text-align:center; }
    .kpi-label{ color:#6c757d; font-weight:600; }
    .kpi-value{ font-size:2rem; font-weight:800; line-height:1; margin-top:.35rem; }

    .kpi-value.primary{ color:#0b5ed7; }
    .kpi-value.success{ color:#146c43; }
    .kpi-value.warning{ color:#8a6d00; }

   
    .stat-card{
        border:1px solid rgba(0,0,0,.08);
        border-radius:14px;
        background:#fff;
        overflow:hidden;
    }
    .stat-card .stat-body{ padding:16px; text-align:center; }
    .stat-title{ color:#6c757d; font-weight:600; }
    .stat-value{ font-size:1.6rem; font-weight:800; margin-top:.25rem; }

    .stat-card.info{
        border-color: rgba(13,110,253,.18);
    }
    .stat-card.info .stat-value{ color:#0b5ed7; }

    .note{ color:#6c757d; font-size:.9rem; }
    .alert{ border-radius:14px; }
</style>

<div class="full-bleed">
    <div class="page-wrap">

     
        <div class="d-flex align-items-start justify-content-between mb-3">
            <div>
                <h2 class="page-title">Mis Estadísticas</h2>
                <div class="subtle">Resumen de turnos del médico logueado</div>
                <div class="pillbar">
                    <span class="pill">📅 Hoy</span>
                    <span class="pill">📊 Mes en curso</span>
                    <span class="pill">👨‍⚕️ Solo tu cuenta</span>
                </div>
            </div>

            <a class="btn btn-outline-secondary" style="border-radius:10px; font-weight:600;"
               href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>
        </div>

        <asp:Label ID="lblMsg" runat="server"
            CssClass="alert alert-danger d-block mb-3"
            Visible="false" />

      
        <div class="row g-3 mb-4">

            <div class="col-12 col-md-4">
                <div class="kpi-card">
                    <div class="kpi-top">
                        <div class="kpi-label">Turnos hoy</div>
                    </div>
                    <div class="kpi-body">
                        <div class="kpi-value primary">
                            <asp:Label ID="lblHoy" runat="server" Text="0" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-4">
                <div class="kpi-card kpi-success">
                    <div class="kpi-top">
                        <div class="kpi-label">Atendidos hoy</div>
                    </div>
                    <div class="kpi-body">
                        <div class="kpi-value success">
                            <asp:Label ID="lblAtendidosHoy" runat="server" Text="0" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-4">
                <div class="kpi-card kpi-warning">
                    <div class="kpi-top">
                        <div class="kpi-label">Pendientes hoy</div>
                    </div>
                    <div class="kpi-body">
                        <div class="kpi-value warning">
                            <asp:Label ID="lblPendientesHoy" runat="server" Text="0" />
                        </div>
                    </div>
                </div>
            </div>

        </div>

    
        <div class="card card-soft shadow-sm mb-3">
            <div class="card-header d-flex align-items-center justify-content-between">
                <div>
                    <div class="fw-semibold">Estadísticas del mes</div>
                    <small class="subtle">Totales del mes en curso</small>
                </div>
                <span class="text-muted">
                    <asp:Label ID="lblMesActual" runat="server" />
                </span>
            </div>

            <div class="card-body">
                <div class="row g-3">

                    <div class="col-12 col-md-3">
                        <div class="stat-card">
                            <div class="stat-body">
                                <div class="stat-title">Atendidos</div>
                                <div class="stat-value">
                                    <asp:Label ID="lblAtendidosMes" runat="server" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-3">
                        <div class="stat-card">
                            <div class="stat-body">
                                <div class="stat-title">No asistió</div>
                                <div class="stat-value">
                                    <asp:Label ID="lblNoAsistioMes" runat="server" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-3">
                        <div class="stat-card">
                            <div class="stat-body">
                                <div class="stat-title">Reprogramados</div>
                                <div class="stat-value">
                                    <asp:Label ID="lblReprogramadosMes" runat="server" Text="0" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-3">
                        <div class="stat-card info">
                            <div class="stat-body">
                                <div class="stat-title">Promedio atendidos / día</div>
                                <div class="stat-value">
                                    <asp:Label ID="lblPromedioDia" runat="server" Text="0.00" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="note mt-3">
                    *Datos calculados con los turnos del médico logueado (día actual y mes en curso).
                </div>
            </div>
        </div>

    </div>
</div>

</asp:Content>