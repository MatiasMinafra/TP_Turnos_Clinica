<%@ Page Title="Mis Estadísticas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MisEstadisticas.aspx.cs"
    Inherits="TP_Turnos_Clinica.MisEstadisticas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Mis Estadísticas</h3>

    <asp:Label ID="lblMsg" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

   
    <div class="row g-3 mb-4">

        <div class="col-md-4">
            <div class="card border-primary">
                <div class="card-body text-center">
                    <div class="text-muted">Turnos hoy</div>
                    <div class="fs-3 fw-bold text-primary">
                        <asp:Label ID="lblHoy" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card border-success">
                <div class="card-body text-center">
                    <div class="text-muted">Atendidos hoy</div>
                    <div class="fs-3 fw-bold text-success">
                        <asp:Label ID="lblAtendidosHoy" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card border-warning">
                <div class="card-body text-center">
                    <div class="text-muted">Pendientes hoy</div>
                    <div class="fs-3 fw-bold text-warning">
                        <asp:Label ID="lblPendientesHoy" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

    </div>


  

    <div class="d-flex justify-content-between align-items-center mb-2">
        <strong>Estadísticas del mes</strong>
        <span class="text-muted">
            <asp:Label ID="lblMesActual" runat="server" />
        </span>
    </div>

    <div class="row g-3">

        <div class="col-md-3">
            <div class="card">
                <div class="card-body text-center">
                    <div class="text-muted">Atendidos</div>
                    <div class="fs-4 fw-bold">
                        <asp:Label ID="lblAtendidosMes" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card">
                <div class="card-body text-center">
                    <div class="text-muted">No asistió</div>
                    <div class="fs-4 fw-bold">
                        <asp:Label ID="lblNoAsistioMes" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card">
                <div class="card-body text-center">
                    <div class="text-muted">Reprogramados</div>
                    <div class="fs-4 fw-bold">
                        <asp:Label ID="lblReprogramadosMes" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card border-info">
                <div class="card-body text-center">
                    <div class="text-muted">Promedio atendidos / día</div>
                    <div class="fs-4 fw-bold text-info">
                        <asp:Label ID="lblPromedioDia" runat="server" Text="0.00" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <small class="text-muted d-block mt-3">
        *Datos calculados con los turnos del médico logueado (día actual y mes en curso).
    </small>

</asp:Content>
