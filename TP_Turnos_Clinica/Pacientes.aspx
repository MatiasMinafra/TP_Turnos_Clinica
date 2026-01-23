<%@ Page Title="Pacientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Pacientes.aspx.cs"
    Inherits="TP_Turnos_Clinica.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Pacientes</h3>

    <!-- BUSCADOR + CHECKBOX + NUEVO -->
    <div class="row mb-3 align-items-end">

        <div class="col-md-5">
            <asp:TextBox ID="txtBuscar" runat="server"
                CssClass="form-control"
                placeholder="Buscar por DNI, Nombre, Apellido o Email">
            </asp:TextBox>
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
               href="<%= ResolveUrl("~/PacienteForm.aspx") %>">
                Nuevo Paciente
            </a>
        </div>

    </div>

    <!-- GRILLA -->
    <asp:GridView ID="gvPacientes" runat="server"
        CssClass="table table-striped table-bordered table-sm"
        AutoGenerateColumns="false"
        DataKeyNames="PacienteID"
        OnRowCommand="Pacientes_ComandoPorFila">

        <Columns>
            <asp:BoundField DataField="DNI" HeaderText="DNI" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:HyperLink ID="hlEditar" runat="server"
                        Text="Editar"
                        NavigateUrl='<%# ResolveUrl("~/PacienteForm.aspx?id=" + Eval("PacienteID")) %>'
                        CssClass="btn btn-warning btn-sm me-1" />

                    <asp:LinkButton ID="btnDesactivar" runat="server"
                        Text="Desactivar"
                        CommandName="Desactivar"
                        CommandArgument="<%# Container.DataItemIndex %>"
                        CssClass="btn btn-danger btn-sm"
                        CausesValidation="false"
                        OnClientClick="return confirm('¿Seguro que querés desactivar este paciente?');" />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

</asp:Content>