<%@ Page Title="Home"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Home.aspx.cs"
    Inherits="TP_Turnos_Clinica.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2>Login correcto</h2>

        <p>
            Usuario:
            <strong>
                <asp:Label ID="lblUsuario" runat="server" />
            </strong>
        </p>

        <p>
            Rol:
            <strong>
                <asp:Label ID="lblRol" runat="server" />
            </strong>
        </p>

        <a href="/Logout.aspx" class="btn btn-danger">Cerrar sesión</a>
    </div>

</asp:Content>