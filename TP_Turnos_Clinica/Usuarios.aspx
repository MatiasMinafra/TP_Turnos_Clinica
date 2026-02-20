<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs"
    Inherits="TP_Turnos_Clinica.Usuarios" %>

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

        .toolbar-actions .btn { border-radius: 10px; }

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
        .table thead th{
            position: sticky;
            top: 0;
            z-index: 1;
            white-space: nowrap;
        }
        .table td, .table th{ vertical-align: middle; }
        .table td{ font-size: .9rem; }

        .badge-soft{
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: .35rem .6rem;
            border-radius: 999px;
            font-weight: 700;
            font-size: .82rem;
            border: 1px solid rgba(0,0,0,.06);
            white-space: nowrap;
        }
        .badge-ok{ background: rgba(25,135,84,.12); color: #146c43; border-color: rgba(25,135,84,.20); }
        .badge-no{ background: rgba(220,53,69,.12); color: #b02a37; border-color: rgba(220,53,69,.20); }

        .action-btn{
            border-radius: 10px;
            font-weight: 600;
            padding: .25rem .55rem;
            font-size: .78rem;
            white-space: nowrap;
        }

      
        .col-actions{ min-width: 170px; }
    </style>

    <div class="full-bleed">
        <div class="page-wrap">

         
            <div class="d-flex flex-wrap align-items-start justify-content-between gap-2 mb-3">
                <div>
                    <h2 class="page-title">Usuarios</h2>
                    <div class="subtle">ABM de usuarios (solo Admin)</div>
                    <div class="pillbar">
                        <span class="pill">👤 Alta / baja</span>
                        <span class="pill">🔎 Filtros rápidos</span>
                        <span class="pill">🛡️ Roles</span>
                    </div>
                </div>

                <div class="toolbar-actions">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"
                        CssClass="btn btn-primary btn-chip shadow-sm"
                        OnClick="btnNuevo_Click" />
                </div>
            </div>

  
            <div class="card card-soft shadow-sm mb-3">
                <div class="card-header py-3">
                    <div class="fw-semibold">Filtros</div>
                    <small class="subtle">Buscá por usuario, nombre o apellido</small>
                </div>

                <div class="card-body">
                    <div class="row g-2 align-items-end">
                        <div class="col-12 col-lg-6">
                            <label class="form-label mb-1">Buscar</label>
                            <asp:TextBox ID="txtFiltro" runat="server"
                                CssClass="form-control"
                                placeholder="Usuario / nombre / apellido" />
                        </div>

                        <div class="col-12 col-lg-3">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkSoloActivos" runat="server"
                                    CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="<%= chkSoloActivos.ClientID %>">
                                    Solo activos
                                </label>
                            </div>
                        </div>

                        <div class="col-12 col-lg-3 d-grid">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                                CssClass="btn btn-outline-primary btn-chip"
                                OnClick="btnBuscar_Click" />
                        </div>
                    </div>

                 
                    <div id="msgWrap" runat="server" visible="false" class="mt-3">
                        <div id="msgAlert" class="alert alert-success alert-dismissible fade show mb-0" role="alert" style="border-radius:14px;">
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </div>
                    </div>

                </div>
            </div>

            
            <div class="card card-soft shadow-sm">
                <div class="card-header py-3 d-flex align-items-center justify-content-between">
                    <div>
                        <div class="fw-semibold">Listado</div>
                        <small class="subtle">Acciones rápidas por usuario</small>
                    </div>
                </div>

                <div class="card-body p-0">
                    <div class="table-responsive table-wrap">
                        <asp:GridView ID="dgvUsuarios" runat="server"
                            AutoGenerateColumns="false"
                            DataKeyNames="UsuarioID"
                            CssClass="table table-hover table-striped align-middle mb-0"
                            HeaderStyle-CssClass="table-dark"
                            OnRowCommand="dgvUsuarios_ComandoFila">

                            <Columns>
                                <asp:BoundField DataField="UsuarioNombre" HeaderText="Usuario" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                <asp:BoundField DataField="rol.Nombre" HeaderText="Rol" />

                                <asp:TemplateField HeaderText="Activo">
                                    <ItemTemplate>
                                        <%# (bool)Eval("Activo")
                                            ? "<span class='badge-soft badge-ok'>Sí</span>"
                                            : "<span class='badge-soft badge-no'>No</span>" %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones">
                                    <HeaderStyle CssClass="col-actions" />
                                    <ItemStyle CssClass="col-actions" />
                                    <ItemTemplate>
                                        <div class="d-flex gap-2 flex-wrap">
                                            <asp:LinkButton ID="btnEditar" runat="server"
                                                Text="Editar"
                                                CssClass="btn btn-sm btn-outline-primary action-btn"
                                                CommandName="Editar"
                                                CommandArgument="<%# Container.DataItemIndex %>" />

                                            <asp:LinkButton ID="btnBaja" runat="server"
                                                Text="Baja"
                                                CssClass="btn btn-sm btn-outline-danger action-btn"
                                                CommandName="Baja"
                                                CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClientClick="return confirm('¿Dar de baja este usuario?');" />
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

    <script>
        (function () {
            var el = document.getElementById('msgAlert');
            if (!el) return;
            setTimeout(function () {
                try {
                    if (window.bootstrap && bootstrap.Alert) {
                        bootstrap.Alert.getOrCreateInstance(el).close();
                    } else {
                        el.style.display = 'none';
                    }
                } catch (e) { el.style.display = 'none'; }
            }, 3000);
        })();
    </script>

</asp:Content>