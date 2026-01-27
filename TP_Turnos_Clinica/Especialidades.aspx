<%@ Page Title="Especialidades" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs"
    Inherits="TP_Turnos_Clinica.Especialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Especialidades</h3>

    <div class="row mb-3 align-items-end">

        <div class="col-md-5">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                placeholder="Buscar por nombre"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
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
            <!-- CORREGIDO: va a EspecialidadesForm.aspx -->
            <a class="btn btn-success w-100" href="<%= ResolveUrl("~/EspecialidadesForm.aspx") %>">
                Nueva Especialidad
            </a>
        </div>
    </div>

    <asp:GridView ID="gvEspecialidades" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="EspecialidadID"
        OnRowCommand="Especialidades_ComandoPorFila">

        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    
                    <asp:HyperLink ID="hlEditar" runat="server"
                        Text="Editar"
                        NavigateUrl='<%# ResolveUrl("~/EspecialidadesForm.aspx?id=" + Eval("EspecialidadID")) %>'
                        CssClass="btn btn-warning btn-sm me-1" />

                    <asp:LinkButton ID="btnAccion" runat="server"
                        Text='<%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>'
                        CommandName='<%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>'
                        CommandArgument="<%# Container.DataItemIndex %>"
                        CssClass='<%# (bool)Eval("Activo") ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>'
                        CausesValidation="false"
                        OnClientClick='<%# (bool)Eval("Activo")
                            ? "return confirm(\"¿Seguro que querés desactivar esta especialidad?\");"
                            : "return confirm(\"¿Seguro que querés activar esta especialidad?\");" %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>