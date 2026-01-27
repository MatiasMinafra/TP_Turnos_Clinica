<%@ Page Title="Asignar Especialidades" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="MedicoEspecialidades.aspx.cs"
    Inherits="TP_Turnos_Clinica.MedicoEspecialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Asignar especialidades al médico</h3>

    <asp:Label ID="lblMedico" runat="server"
        CssClass="alert alert-secondary d-block mb-3"
        Visible="true" />

    <asp:Label ID="lblMensaje" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <div class="row mb-3 align-items-end">
        <div class="col-md-6">
            <label class="form-label fw-bold">Especialidad</label>
            <asp:DropDownList ID="ddlEspecialidades" runat="server" CssClass="form-select" />
        </div>

        <div class="col-md-3">
            <asp:Button ID="btnAsignar" runat="server" Text="Asignar"
                CssClass="btn btn-primary w-100"
                OnClick="btnAsignar_Click" />
        </div>

        <div class="col-md-3 text-end">
            <a class="btn btn-secondary w-100" href="<%= ResolveUrl("~/Medicos.aspx") %>">
                Volver a Médicos
            </a>
        </div>
    </div>

    <asp:GridView ID="gvAsignadas" runat="server"
        CssClass="table table-striped table-bordered"
        AutoGenerateColumns="false"
        DataKeyNames="EspecialidadID"
        OnRowCommand="gvAsignadas_ComandoFila">

        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="Especialidad" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton ID="btnQuitar" runat="server"
                        Text="Quitar"
                        CommandName="Quitar"
                        CommandArgument="<%# Container.DataItemIndex %>"
                        CssClass="btn btn-danger btn-sm"
                        CausesValidation="false"
                        OnClientClick="return confirm('¿Seguro que querés quitar esta especialidad del médico?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>