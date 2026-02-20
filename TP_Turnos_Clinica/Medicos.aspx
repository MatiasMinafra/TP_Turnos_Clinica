<%@ Page Title="Médicos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Medicos.aspx.cs"
    Inherits="TP_Turnos_Clinica.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
       
        .full-bleed{
            width: 100vw;
            margin-left: calc(50% - 50vw);
            margin-right: calc(50% - 50vw);
            padding-left: 26px;
            padding-right: 26px;
        }
        @media (min-width: 1400px){
            .full-bleed{ padding-left: 38px; padding-right: 38px; }
        }

        .page-wrap { padding-top: 14px; padding-bottom: 24px; }
        .page-title { font-weight: 800; letter-spacing: -.2px; margin-bottom: 0; }
        .subtle { color: #6c757d; }

        .card-soft{
            border: 1px solid rgba(0,0,0,.08);
            border-radius: 14px;
            overflow: hidden;
            background: #fff;
        }
        .card-soft .card-header{
            background: linear-gradient(90deg, rgba(13,110,253,.12), rgba(13,110,253,.05));
            border-bottom: 1px solid rgba(0,0,0,.06);
        }

        .btn-chip{
            border-radius: 10px;
            padding-left: .85rem;
            padding-right: .85rem;
            font-weight: 600;
            white-space: nowrap;
        }

        .pillbar{ display:flex; flex-wrap:wrap; gap:.5rem; margin-top:.35rem; }
        .pill{
            display:inline-flex;
            align-items:center;
            gap:.4rem;
            padding:.35rem .6rem;
            border-radius:999px;
            border:1px solid rgba(0,0,0,.08);
            background:#fff;
            font-size:.85rem;
        }

        .form-label{ font-weight: 600; }
        .form-control, .form-select{ border-radius: 12px; }

        .table-wrap{
            border-radius: 14px;
            overflow: hidden;
            border: 1px solid rgba(0,0,0,.08);
        }
        .table thead th{ white-space: nowrap; }
        .table td, .table th{ vertical-align: middle; }
        .table td{ font-size: .9rem; }

        .badge-soft{
            display: inline-flex;
            align-items: center;
            padding: .35rem .6rem;
            border-radius: 999px;
            font-weight: 700;
            font-size: .82rem;
            border: 1px solid rgba(0,0,0,.06);
            white-space: nowrap;
        }
        .badge-ok { background: rgba(25,135,84,.12); color: #146c43; border-color: rgba(25,135,84,.20); }
        .badge-no { background: rgba(220,53,69,.12); color: #b02a37; border-color: rgba(220,53,69,.20); }

        .actions{ display:flex; gap:.5rem; flex-wrap:wrap; }

        .action-btn{
            border-radius: 10px;
            font-weight: 600;
            padding: .25rem .55rem;
            font-size: .78rem;
            white-space: nowrap;
        }
        .col-actions{ min-width: 320px; }
    </style>

    <div class="full-bleed">
        <div class="page-wrap">

            
            <div class="d-flex flex-wrap align-items-start justify-content-between gap-2 mb-3">
                <div>
                    <h2 class="page-title">Médicos</h2>
                    <div class="subtle">Administración de médicos, especialidades y disponibilidad</div>
                    <div class="pillbar">
                        <span class="pill">🧑‍⚕️ ABM</span>
                        <span class="pill">🗂️ Especialidades</span>
                        <span class="pill">📅 Disponibilidad</span>
                    </div>
                </div>

                <a class="btn btn-success btn-chip shadow-sm"
                   href="<%= ResolveUrl("~/MedicoForm.aspx") %>">
                    Nuevo Médico
                </a>
            </div>

        
            <div class="card card-soft shadow-sm mb-3">
                <div class="card-header py-3">
                    <div class="fw-semibold">Filtros</div>
                    <small class="subtle">Buscá por DNI, matrícula, nombre o apellido</small>
                </div>

                <div class="card-body">
                    <div class="row g-2 align-items-end">

                        <div class="col-12 col-lg-6">
                            <label class="form-label mb-1">Buscar</label>
                            <asp:TextBox ID="txtBuscar" runat="server"
                                CssClass="form-control"
                                placeholder="Buscar por DNI, Matrícula, Nombre o Apellido" />
                        </div>

                        <div class="col-12 col-lg-3">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkInactivos" runat="server"
                                    CssClass="form-check-input"
                                    AutoPostBack="true"
                                    OnCheckedChanged="chkInactivos_CheckedChanged" />
                                <label class="form-check-label" for="<%= chkInactivos.ClientID %>">
                                    Mostrar inactivos
                                </label>
                            </div>
                        </div>

                        <div class="col-12 col-lg-3 d-grid">
                            <asp:Button ID="btnBuscar" runat="server"
                                Text="Buscar"
                                CssClass="btn btn-outline-primary btn-chip"
                                OnClick="btnBuscar_Click" />
                        </div>

                    </div>
                </div>
            </div>

           
            <div class="card card-soft shadow-sm">
                <div class="card-header py-3 d-flex align-items-center justify-content-between">
                    <div>
                        <div class="fw-semibold">Listado</div>
                        <small class="subtle">Médicos cargados en el sistema</small>
                    </div>
                </div>

                <div class="card-body p-0">
                    <div class="table-responsive table-wrap">
                        <asp:GridView ID="gvMedicos" runat="server"
                            CssClass="table table-hover table-striped align-middle mb-0"
                            AutoGenerateColumns="false"
                            DataKeyNames="MedicoID"
                            HeaderStyle-CssClass="table-dark"
                            OnRowCommand="Medicos_ComandoPorFila">

                            <Columns>

                                <asp:BoundField DataField="DNI" HeaderText="DNI" />
                                <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />

                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />

                                <asp:BoundField DataField="CantidadEspecialidades" HeaderText="# Esp." />

                                <asp:TemplateField HeaderText="Activo">
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("Activo"))
                                            ? "<span class='badge-soft badge-ok'>Sí</span>"
                                            : "<span class='badge-soft badge-no'>No</span>" %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones">
                                    <HeaderStyle CssClass="col-actions" />
                                    <ItemStyle CssClass="col-actions" />
                                    <ItemTemplate>
                                        <div class="actions">

                                            <asp:HyperLink ID="hlEditar" runat="server"
                                                Text="Editar"
                                                NavigateUrl='<%# ResolveUrl("~/MedicoForm.aspx?id=" + Eval("MedicoID")) %>'
                                                CssClass="btn btn-sm btn-outline-primary action-btn" />

                                            <asp:HyperLink ID="hlAgenda" runat="server"
                                                Text="Disponibilidad"
                                                NavigateUrl='<%# ResolveUrl("~/AsignacionTurnoMedico.aspx?id=" + Eval("MedicoID")) %>'
                                                CssClass="btn btn-sm btn-outline-secondary action-btn" />

                                            <asp:LinkButton ID="btnToggleActivo" runat="server"
                                                Text='<%# (Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar") %>'
                                                CommandName="ToggleActivo"
                                                CommandArgument="<%# Container.DataItemIndex %>"
                                                CssClass='<%# (Convert.ToBoolean(Eval("Activo"))
                                                    ? "btn btn-sm btn-outline-danger action-btn"
                                                    : "btn btn-sm btn-outline-success action-btn") %>'
                                                CausesValidation="false"
                                                OnClientClick='<%# (Convert.ToBoolean(Eval("Activo"))
                                                    ? "return confirm(\"¿Seguro que querés desactivar este médico?\");"
                                                    : "return confirm(\"¿Seguro que querés activar este médico?\");") %>' />

                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>