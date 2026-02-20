<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Home.aspx.cs"
    Inherits="TP_Turnos_Clinica.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>


.home-bg {
    min-height: calc(100vh - 140px);
    background:
        radial-gradient(900px 350px at 55% 0%, rgba(120, 160, 255, 0.18), transparent 60%),
        radial-gradient(900px 350px at 95% 25%, rgba(255, 220, 120, 0.20), transparent 60%),
        linear-gradient(180deg, #f7f9ff 0%, #f3f6fb 100%);
    border-radius: 18px;
    padding: 22px;
    position: relative;
    overflow: hidden;
}

.home-bg::after {
    content: "CLÍNICA TURNOS";
    position: absolute;
    right: 22px;
    bottom: 8px;
    font-weight: 900;
    font-size: 56px;
    color: rgba(17,24,39,.06);
    letter-spacing: .03em;
    pointer-events: none;
    user-select: none;
}


.home-grid {
    display: grid;
    grid-template-columns: 260px 1fr;
    gap: 18px;
}


.sidebar {
    border-radius: 16px;
    box-shadow: 0 18px 40px rgba(17,24,39,0.25);
    background: linear-gradient(180deg, #1f242b 0%, #171b21 100%);
    padding: 16px;
    min-height: 78vh;
    color: white;
}

.sidebar .btn {
    border-radius: 12px;
    font-weight: 600;
}

.sidebar hr {
    opacity: .25;
}


.hero {
    background: rgba(255,255,255,.8);
    border: 1px solid rgba(15,23,42,.08);
    border-radius: 22px;
    box-shadow: 0 30px 60px rgba(17,24,39,0.10);
    padding: 40px;
    position: relative;
    overflow: hidden;
}

.title {
    font-weight: 900;
    font-size: 44px;
    color: #2b3442;
    margin-bottom: 10px;
}

.subtitle {
    color: #6b7280;
    font-size: 16px;
    margin-bottom: 20px;
}

.tags {
    display: flex;
    gap: 10px;
    flex-wrap: wrap;
}

.tag {
    background: rgba(255,255,255,.9);
    border: 1px solid rgba(15,23,42,.10);
    padding: 10px 14px;
    border-radius: 12px;
    font-weight: 700;
    font-size: 14px;
}


.medical-cross {
    position: absolute;
    right: 60px;
    top: 70px;
    width: 180px;
    height: 180px;
    background: rgba(220, 38, 38, 0.08);
    border-radius: 40px;
    box-shadow: 0 35px 80px rgba(220,38,38,0.18);
}

.medical-cross::before,
.medical-cross::after {
    content: "";
    position: absolute;
    background: #dc2626;
    border-radius: 12px;
}


.medical-cross::before {
    width: 50px;
    height: 120px;
    left: 65px;
    top: 30px;
}


.medical-cross::after {
    width: 120px;
    height: 50px;
    left: 30px;
    top: 65px;
}

.panel-bottom {
    margin-top: 20px;
    background: rgba(255,255,255,.75);
    border-radius: 18px;
    padding: 20px;
    box-shadow: 0 20px 40px rgba(17,24,39,0.08);
}

.panel-bottom ul {
    list-style: none;
    padding: 0;
}

.panel-bottom li {
    padding: 8px 0;
    font-weight: 700;
    color: #374151;
}

@media (max-width: 992px){
    .home-grid { grid-template-columns: 1fr; }
    .medical-cross { display:none; }
}
</style>

<div class="home-bg">
    <div class="home-grid">

    
        <aside class="sidebar">

            <div class="fw-bold mb-3 fs-5">CLÍNICA</div>

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
                    Pacientes
                </asp:HyperLink>

                <asp:HyperLink ID="lnkMedicos" runat="server"
                    NavigateUrl="~/Medicos.aspx"
                    CssClass="btn btn-outline-light">
                    Médicos
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

                <asp:HyperLink ID="lnkPanelMedico" runat="server"
                    NavigateUrl="~/PanelMedico.aspx"
                    CssClass="btn btn-outline-info">
                    Panel Médico
                </asp:HyperLink>

                <asp:HyperLink ID="lnkMisEstadisticas" runat="server"
                    NavigateUrl="~/MisEstadisticas.aspx"
                    CssClass="btn btn-outline-info">
                    Mis estadísticas
                </asp:HyperLink>

                <asp:HyperLink ID="lnkUsuarios" runat="server"
                    NavigateUrl="~/Usuarios.aspx"
                    CssClass="btn btn-outline-light">
                    Usuarios (Admin)
                </asp:HyperLink>

                <asp:HyperLink ID="lnkLogout" runat="server"
                    NavigateUrl="~/Logout.aspx"
                    CssClass="btn btn-danger mt-2">
                    Cerrar sesión
                </asp:HyperLink>

            </div>
        </aside>

        
        <main>

            <section class="hero">

                <div class="medical-cross"></div>

                <h1 class="title">Sistema de Gestión Médica</h1>

                <p class="subtitle">
                    Sistema para gestionar turnos, pacientes y médicos desde una misma plataforma Dinamica.
                </p>

                <div class="tags">
                    <span class="tag">Clínica Turnos</span>
                    <span class="tag">UTN • WebForms</span>
                    <span class="tag">C# • SQL Server</span>
                </div>

                <div class="mt-4 fw-bold text-muted">
                    Proyecto UTN<br />
                    Hecho por Matías Minafra
                </div>

            </section>

            <section class="panel-bottom mt-4">
                <ul>
                    <li>✔ Clínica Turnos</li>
                    <li>✔ ASP.NET WebForms</li>
                    <li>✔ C# | SQL Server</li>
                </ul>

                <div class="mt-3 text-muted fw-semibold">
                    © <%: DateTime.Now.Year %> CLÍNICA TURNOS
                </div>
            </section>

        </main>
    </div>
</div>

</asp:Content>