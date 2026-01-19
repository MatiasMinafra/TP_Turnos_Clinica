<%@ Page Title="Paciente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PacienteForm.aspx.cs"
    Inherits="TP_Turnos_Clinica.PacienteForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-6">

                <h3 class="mb-4">
                    <asp:Label ID="lblTitulo" runat="server" Text="Nuevo Paciente"></asp:Label>
                </h3>

                <asp:Label ID="lblMensaje" runat="server"
                    CssClass="alert alert-danger d-block mb-3"
                    Visible="false"></asp:Label>

                <div class="mb-3">
                    <label class="form-label fw-bold">DNI *</label>
                    <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" placeholder="Ingrese DNI" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Nombre *</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese nombre" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Apellido *</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ingrese apellido" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Sexo</label>
                    <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione..." Value="" />
                        <asp:ListItem Text="M" Value="M" />
                        <asp:ListItem Text="F" Value="F" />
                        <asp:ListItem Text="X" Value="X" />
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Nacionalidad</label>
                    <asp:TextBox ID="txtNacionalidad" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Email *</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="correo@ejemplo.com" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 11 5555-5555" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-bold">Ciudad</label>
                    <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-4">
                    <label class="form-label fw-bold">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                </div>

                <div class="d-flex gap-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" />

                    <asp:Button ID="btnVolver" runat="server" Text="Volver"
                        CssClass="btn btn-secondary"
                        OnClick="btnVolver_Click"
                        CausesValidation="false" />
                </div>

            </div>
        </div>
    </div>

</asp:Content>