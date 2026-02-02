<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Home.aspx.cs"
    Inherits="TP_Turnos_Clinica.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid p-0">
        <div class="row g-0">

            <!-- SIDEBAR -->
            <aside class="col-12 col-md-3 col-lg-2 bg-dark text-white min-vh-100 p-3">

                <div class="fw-bold mb-3">CLÍNICA</div>

                <div class="small text-white-50 mb-3">
                    <div>Bienvenido,</div>
                    <asp:Label ID="lblUsuario" runat="server" CssClass="fw-semibold d-block"></asp:Label>

                    <div class="mt-1">
                        Rol:
                        <asp:Label ID="lblRol" runat="server" CssClass="fw-semibold"></asp:Label>
                    </div>
                </div>

                <hr class="border-secondary" />

                <div class="d-grid gap-2">

                    <!-- Operación (Admin/Recep) -->
                    <asp:HyperLink ID="lnkAsignarTurno" runat="server"
                        NavigateUrl="~/AgendaTurnos.aspx"
                        CssClass="btn btn-warning text-dark fw-semibold">
                        Asignar turno
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkTurnosDia" runat="server"
                        NavigateUrl="~/TurnosDelDia.aspx"
                        CssClass="btn btn-outline-light">
                        Turnos del día
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkPacientes" runat="server"
                        NavigateUrl="~/Pacientes.aspx"
                        CssClass="btn btn-outline-light">
                        Pacientes (ABM)
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkMedicos" runat="server"
                        NavigateUrl="~/Medicos.aspx"
                        CssClass="btn btn-outline-light">
                        Médicos (ABM)
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkEspecialidades" runat="server"
                        NavigateUrl="~/Especialidades.aspx"
                        CssClass="btn btn-outline-light">
                        Especialidades
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkAgenda" runat="server"
                        NavigateUrl="~/TurnosTrabajo.aspx"
                        CssClass="btn btn-outline-light">
                        Turnos de trabajo
                    </asp:HyperLink>

                    <!-- Médico -->
                    <asp:HyperLink ID="lnkMisTurnos" runat="server"
                        NavigateUrl="~/MisTurnos.aspx"
                        CssClass="btn btn-outline-info">
                        Mis turnos
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkEvoluciones" runat="server"
                        NavigateUrl="~/Evoluciones.aspx"
                        CssClass="btn btn-outline-info">
                        Diagnóstico / Evoluciones
                    </asp:HyperLink>

                    <!-- Admin -->
                    <asp:HyperLink ID="lnkUsuarios" runat="server"
                        NavigateUrl="~/Usuarios.aspx"
                        CssClass="btn btn-outline-secondary">
                        Usuarios (Admin)
                    </asp:HyperLink>

                    <asp:HyperLink ID="lnkLogout" runat="server"
                        NavigateUrl="~/Logout.aspx"
                        CssClass="btn btn-danger mt-2">
                        Cerrar sesión
                    </asp:HyperLink>

                </div>
            </aside>

            <!-- CONTENIDO -->
            <main class="col-12 col-md-9 col-lg-10 p-4">
                <h3 class="mb-2">Panel principal</h3>
                <p class="text-muted mb-4">
                    Usá el menú de la izquierda para navegar por el sistema.
                </p>

                <div class="alert alert-info">
                    Estado: login + roles + sesión OK.
                </div>

            </main>

        </div>
    </div>

</asp:Content>