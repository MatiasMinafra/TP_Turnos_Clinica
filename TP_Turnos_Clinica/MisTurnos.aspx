<%@ Page Title="Mis Turnos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs"
    Inherits="TP_Turnos_Clinica.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Mis turnos</h3>

    <asp:Label ID="lblMensaje" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-3 align-items-end">

                <div class="col-md-4">
                    <label class="form-label">Desde</label>
                    <asp:TextBox ID="txtDesde" runat="server"
                        CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server"
                        CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-md-4">
                    <asp:Button ID="btnBuscar" runat="server"
                        Text="Buscar"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnBuscar_Click" />
                </div>

            </div>
        </div>
    </div>

    <asp:GridView ID="gvMisTurnos" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false">
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
            <asp:BoundField DataField="EstadoTurno" HeaderText="Estado" />
            <asp:BoundField DataField="MotivoConsulta" HeaderText="Motivo" />

        </Columns>
    </asp:GridView>

</asp:Content>