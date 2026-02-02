<%@ Page Title="Turnos de Trabajo"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="TurnosTrabajo.aspx.cs"
    Inherits="TP_Turnos_Clinica.TurnosTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Turnos de Trabajo</h3>

    <div class="row mb-3 align-items-end">
        <div class="col-md-5">
            <asp:TextBox ID="txtBuscar" runat="server"
                CssClass="form-control"
                placeholder="Buscar por nombre" />
        </div>

        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server"
                Text="Buscar"
                CssClass="btn btn-primary w-100"
                OnClick="btnBuscar_Click" />
        </div>

        <div class="col-md-3">
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

        <div class="col-md-2 text-end">
            <a class="btn btn-success w-100"
               href="<%= ResolveUrl("~/TurnoTrabajoForm.aspx") %>">
                Nuevo Turno
            </a>
        </div>
    </div>

    <asp:Label ID="lblMensaje" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <asp:GridView ID="gvTurnosTrabajo" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="TurnoTrabajoID"
        OnRowCommand="TurnosTrabajo_ComandoPorFila">

        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

            <asp:TemplateField HeaderText="Hora Inicio">
                <ItemTemplate>
                    <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Hora Fin">
                <ItemTemplate>
                    <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:HyperLink ID="hlEditar" runat="server"
                        Text="Editar"
                        NavigateUrl='<%# ResolveUrl("~/TurnoTrabajoForm.aspx?id=" + Eval("TurnoTrabajoID")) %>'
                        CssClass="btn btn-warning btn-sm me-1" />

                    <asp:LinkButton ID="btnToggleActivo" runat="server"
                        Text='<%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar" %>'
                        CommandName="ToggleActivo"
                        CommandArgument='<%# Eval("TurnoTrabajoID") %>'
                        CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>'
                        CausesValidation="false"
                        OnClientClick='<%# Convert.ToBoolean(Eval("Activo"))
                            ? "return confirm(\"¿Seguro que querés desactivar este turno?\");"
                            : "return confirm(\"¿Seguro que querés activar este turno?\");" %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>