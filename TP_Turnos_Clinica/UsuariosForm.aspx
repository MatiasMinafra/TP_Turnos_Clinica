<%@ Page Title="Usuario" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="UsuariosForm.aspx.cs"
    Inherits="TP_Turnos_Clinica.UsuariosForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-12 col-lg-8">

            <div class="d-flex align-items-center justify-content-between mb-3">
                <div>
                    <h2 class="mb-0">Usuario</h2>
                    <small class="text-muted">Alta / edición de usuarios</small>
                </div>

                <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/Usuarios.aspx"
                    CssClass="btn btn-outline-secondary">
                    Volver
                </asp:HyperLink>
            </div>

            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

            <div class="card shadow-sm">
                <div class="card-body">

                    <div class="row g-3">

                        <div class="col-12 col-md-6">
                            <label class="form-label">Usuario</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            <div class="form-text">
                                En edición: si lo dejás vacío, no se cambia.
                            </div>
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label">Apellido</label>
                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-12">
                            <label class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label">Rol</label>
                            <asp:DropDownList ID="ddlRol" runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlRol_SelectedIndexChanged" />
                        </div>

                        <div class="col-12 col-md-6">
                            <label class="form-label">Médico (solo si rol = Médico)</label>
                            <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" />
                        </div>

                        <div class="col-12">
                            <div class="form-check">
                                <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="<%= chkActivo.ClientID %>">Activo</label>
                            </div>
                        </div>

                        <div class="col-12 d-flex gap-2 mt-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                                CssClass="btn btn-primary"
                                OnClick="btnGuardar_Click" />

                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                                CssClass="btn btn-outline-secondary"
                                OnClick="btnCancelar_Click" />
                        </div>

                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>
