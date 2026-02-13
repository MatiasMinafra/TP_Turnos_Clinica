<%@ Page Title="Panel Médico" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PanelMedico.aspx.cs"
    Inherits="TP_Turnos_Clinica.PanelMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Panel del Médico</h3>

    <asp:Label ID="lblMsg" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <!-- ✅ RESUMEN HOY -->
    <div class="row g-3 mb-3">
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <div class="text-muted">Turnos hoy</div>
                    <div class="fs-3 fw-bold">
                        <asp:Label ID="lblHoy" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <div class="text-muted">Atendidos hoy</div>
                    <div class="fs-3 fw-bold">
                        <asp:Label ID="lblAtendidos" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <div class="text-muted">Pendientes hoy</div>
                    <div class="fs-3 fw-bold">
                        <asp:Label ID="lblPendientes" runat="server" Text="0" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- ✅ ESTADÍSTICAS DEL MES (NUEVO) -->
    <div class="row g-3 mb-3">
        <div class="col-12">
            <div class="card border-0">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <div class="fw-semibold">Estadísticas del mes</div>
                        <div class="text-muted small">
                            <asp:Label ID="lblMesActual" runat="server" Text="" />
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-4">
                            <div class="card">
                                <div class="card-body">
                                    <div class="text-muted">Atendidos (mes)</div>
                                    <div class="fs-3 fw-bold">
                                        <asp:Label ID="lblAtendidosMes" runat="server" Text="0" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card">
                                <div class="card-body">
                                    <div class="text-muted">No asistió (mes)</div>
                                    <div class="fs-3 fw-bold">
                                        <asp:Label ID="lblNoAsistioMes" runat="server" Text="0" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card">
                                <div class="card-body">
                                    <div class="text-muted">Reprogramados (mes)</div>
                                    <div class="fs-3 fw-bold">
                                        <asp:Label ID="lblReprogramadosMes" runat="server" Text="0" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="text-muted small mt-2">
                        *Datos calculados con los turnos del médico logueado en el mes actual.
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- FILTROS -->
    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-3 align-items-end">

                <div class="col-md-3">
                    <label class="form-label">Desde</label>
                    <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-3">
                    <label class="form-label">Hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-3">
                    <div class="form-check mt-4">
                        <asp:CheckBox ID="chkSoloHoy" runat="server" CssClass="form-check-input"
                            AutoPostBack="true" OnCheckedChanged="Filtros_CheckedChanged" />
                        <label class="form-check-label" for="<%= chkSoloHoy.ClientID %>">Solo hoy</label>
                    </div>
                    <div class="form-check">
                        <asp:CheckBox ID="chkOcultarCancelados" runat="server" CssClass="form-check-input"
                            AutoPostBack="true" OnCheckedChanged="Filtros_CheckedChanged" />
                        <label class="form-check-label" for="<%= chkOcultarCancelados.ClientID %>">Ocultar cancelados</label>
                    </div>
                </div>

                <div class="col-md-3">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100"
                        OnClick="btnBuscar_Click" />
                </div>

            </div>
        </div>
    </div>

    <!-- GRILLA -->
    <asp:GridView ID="gvTurnos" runat="server"
        CssClass="table table-striped table-bordered align-middle"
        AutoGenerateColumns="false"
        DataKeyNames="TurnoID,PacienteID,EstadoTurno,EstadoPago">

        <Columns>
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

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
                    <span class='badge
                        <%# Eval("EstadoTurno").ToString() == "Cancelado" ? "bg-danger" :
                            Eval("EstadoTurno").ToString() == "Confirmado" ? "bg-success" :
                            Eval("EstadoTurno").ToString() == "Atendido" ? "bg-success" :
                            Eval("EstadoTurno").ToString() == "Reprogramado" ? "bg-warning text-dark" :
                            (Eval("EstadoTurno").ToString() == "No Asistió" || Eval("EstadoTurno").ToString() == "No Asistio") ? "bg-dark" :
                            "bg-secondary" %>'>
                        <%# Eval("EstadoTurno") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Pago">
                <ItemTemplate>
                    <span class='badge
                        <%# Eval("EstadoPago").ToString() == "Confirmado" || Eval("EstadoPago").ToString() == "Aprobado"
                            ? "bg-success"
                            : Eval("EstadoPago").ToString() == "Pendiente"
                            ? "bg-warning text-dark"
                            : "bg-secondary" %>'>
                        <%# Eval("EstadoPago") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>

                    <asp:HyperLink runat="server"
                        Text="Historial"
                        CssClass="btn btn-sm btn-outline-secondary me-1"
                        NavigateUrl='<%# ResolveUrl("~/HistorialPaciente.aspx?pacienteId=" + Eval("PacienteID")) %>' />

                    <asp:HyperLink runat="server"
                        Text="Evolucionar"
                        CssClass="btn btn-sm btn-outline-primary"
                        NavigateUrl='<%# ResolveUrl("~/EvolucionForm.aspx?turnoId=" + Eval("TurnoID")) %>' />

                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>