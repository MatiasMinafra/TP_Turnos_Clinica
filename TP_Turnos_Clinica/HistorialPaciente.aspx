<%@ Page Title="Historial del Paciente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HistorialPaciente.aspx.cs"
    Inherits="TP_Turnos_Clinica.HistorialPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Historial Clínico del Paciente</h3>

    <asp:Label ID="lblMsg" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <div class="mb-3">
        <asp:HyperLink ID="lnkVolver" runat="server"
            CssClass="btn btn-outline-secondary"
            NavigateUrl="~/MisTurnos.aspx"
            Text="← Volver" />
    </div>

    <asp:Panel ID="pnlPaciente" runat="server" CssClass="card mb-3" Visible="false">
    <div class="card-body">
        <h5 class="mb-1">
            Paciente: <asp:Label ID="lblPacienteNombre" runat="server" />
        </h5>

        <div class="text-muted">
            DNI: <asp:Label ID="lblPacienteDni" runat="server" /> |
            Nacimiento: <asp:Label ID="lblPacienteNac" runat="server" /> |
            Edad: <asp:Label ID="lblPacienteEdad" runat="server" /> |
            Email: <asp:Label ID="lblPacienteEmail" runat="server" />
        </div>
    </div>
</asp:Panel>


    <asp:GridView ID="gvHistorial" runat="server"
        CssClass="table table-striped table-bordered align-middle"
        AutoGenerateColumns="false"
        EmptyDataText="No hay historial para este paciente.">

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
                    <span class='badge
                        <%# Eval("EstadoTurno").ToString() == "Cancelado" ? "bg-danger" :
                            Eval("EstadoTurno").ToString() == "Confirmado" ? "bg-success" :
                            Eval("EstadoTurno").ToString() == "Atendido" ? "bg-success" :
                            Eval("EstadoTurno").ToString() == "Reprogramado" ? "bg-warning text-dark" :
                            (Eval("EstadoTurno").ToString() == "No Asistió" || Eval("EstadoTurno").ToString() == "No Asistio") ? "bg-dark" :
                            Eval("EstadoTurno").ToString() == "Cerrado" ? "bg-secondary" :
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
                            : "bg-danger" %>'>
                        <%# Eval("EstadoPago") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>

           
            <asp:TemplateField HeaderText="Evolución">
                <ItemTemplate>
                    <%# (bool)Eval("TieneEvolucion")
                        ? "<span class='badge bg-success'>Sí</span>"
                        : "<span class='badge bg-secondary'>No</span>" %>
                </ItemTemplate>
            </asp:TemplateField>

           
            <asp:TemplateField HeaderText="Detalle clínico">
                <ItemTemplate>
                    <%# (bool)Eval("TieneEvolucion")
                        ? (Eval("DescripcionEvolucion").ToString().Length > 120
                            ? Eval("DescripcionEvolucion").ToString().Substring(0, 120) + "..."
                            : Eval("DescripcionEvolucion").ToString())
                        : "-" %>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>
