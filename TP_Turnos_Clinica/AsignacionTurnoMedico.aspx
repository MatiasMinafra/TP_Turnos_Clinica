<%@ Page Title="Asignación de turnos al médico" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AsignacionTurnoMedico.aspx.cs"
    Inherits="TP_Turnos_Clinica.AsignacionTurnoMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Asignación de turnos al médico</h3>

    <asp:Label ID="lblMedico" runat="server" CssClass="text-muted d-block mb-3" />

    <asp:Label ID="lblMensaje" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <div class="card mb-3">
        <div class="card-body">

            <div class="row g-3 align-items-end">

                <div class="col-md-4">
                    <label class="form-label">Día</label>
                    <asp:DropDownList ID="ddlDia" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Lunes" Value="1" />
                        <asp:ListItem Text="Martes" Value="2" />
                        <asp:ListItem Text="Miércoles" Value="3" />
                        <asp:ListItem Text="Jueves" Value="4" />
                        <asp:ListItem Text="Viernes" Value="5" />
                        <asp:ListItem Text="Sábado" Value="6" />
                        <asp:ListItem Text="Domingo" Value="7" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-5">
                    <label class="form-label">Turno de trabajo</label>
                    <asp:DropDownList ID="ddlTurnoTrabajo" runat="server" CssClass="form-select" />
                </div>

                <div class="col-md-3">
                    <asp:Button ID="btnAsignar" runat="server"
                        Text="Asignar"
                        CssClass="btn btn-success w-100"
                        OnClick="btnAsignar_Click" />
                </div>

                <div class="col-md-4">
                    <div class="form-check mt-2">
                        <asp:CheckBox ID="chkInactivos" runat="server"
                            CssClass="form-check-input"
                            AutoPostBack="true"
                            OnCheckedChanged="chkInactivos_CheckedChanged" />
                        <label class="form-check-label" for="<%= chkInactivos.ClientID %>">
                            Mostrar inactivos
                        </label>
                    </div>
                </div>

                <div class="col-md-8 text-end">
                    <a class="btn btn-link" href="<%= ResolveUrl("~/Medicos.aspx") %>">Volver a Médicos</a>
                </div>

            </div>

        </div>
    </div>

    <asp:GridView ID="gvAsignaciones" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="MedicoTurnoID"
        OnRowCommand="gvAsignaciones_ComandoFila">

        <Columns>

            <asp:BoundField DataField="DiaNombre" HeaderText="Día" />
            <asp:BoundField DataField="TurnoNombre" HeaderText="Turno" />

            <asp:TemplateField HeaderText="Horario">
                <ItemTemplate>
                    <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %> -
                    <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton ID="btnCambiarEstado" runat="server"
                        Text='<%# (Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar") %>'
                        CommandName="CambiarEstado"
                        CommandArgument="<%# Container.DataItemIndex %>"
                        CssClass='<%# (Convert.ToBoolean(Eval("Activo")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm") %>'
                        CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>
