<%@ Page Title="Login"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="TP_Turnos_Clinica.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-12 col-md-6 col-lg-4">

                <div class="card shadow-sm">
                    <div class="card-body p-4">

                        <h3 class="text-center mb-4">Iniciar sesión</h3>

                       
                        <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mb-3" />

                        
                        <div class="mb-3">
                            <label class="form-label">Usuario</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
                        </div>

                        
                        <div class="mb-3">
                            <label class="form-label">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        </div>

                        <asp:Button ID="btnLogin" runat="server"
                            Text="Ingresar"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnLogin_Click" />

                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>