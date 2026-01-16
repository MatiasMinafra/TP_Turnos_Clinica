<%@ Page Title="Pacientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Pacientes.aspx.cs"
    Inherits="TP_Turnos_Clinica.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Pacientes</h3>

    <!-- BUSCADOR -->
    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                placeholder="Buscar por DNI o Nombre"></asp:TextBox>
        </div>

        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                CssClass="btn btn-primary w-100"
                OnClick="btnBuscar_Click" />
        </div>

        <div class="col-md-3 offset-md-3 text-end">
            <asp:HyperLink ID="lnkNuevo" runat="server"
                NavigateUrl="~/PacienteForm.aspx"
                CssClass="btn btn-success">
                Nuevo Paciente
            </asp:HyperLink>
        </div>
    </div>

    <!-- GRILLA -->
    <asp:GridView ID="gvPacientes" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="PacienteID"
        OnRowCommand="gvPacientes_RowCommand">

        <Columns>
            <asp:BoundField DataField="Dni" HeaderText="DNI" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />

            <asp:ButtonField
                Text="Editar"
                CommandName="Editar"
                ButtonType="Button"
                ControlStyle-CssClass="btn btn-warning btn-sm" />

            <asp:ButtonField
                Text="Desactivar"
                CommandName="Desactivar"
                ButtonType="Button"
                ControlStyle-CssClass="btn btn-danger btn-sm" />
        </Columns>

    </asp:GridView>

</asp:Content>