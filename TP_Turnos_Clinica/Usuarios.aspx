<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs"
    Inherits="TP_Turnos_Clinica.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-12 col-lg-10">

            <div class="d-flex align-items-center justify-content-between mb-3">
                <div>
                    <h2 class="mb-0">Usuarios</h2>
                    <small class="text-muted">ABM de usuarios (solo Admin)</small>
                </div>

                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"
                    CssClass="btn btn-primary"
                    OnClick="btnNuevo_Click" />
            </div>

            <div class="card shadow-sm mb-3">
                <div class="card-body">

                    <div class="row g-2 align-items-end">
                        <div class="col-12 col-md-6">
                            <label class="form-label mb-1">Buscar</label>
                            <asp:TextBox ID="txtFiltro" runat="server"
                                CssClass="form-control"
                                placeholder="Usuario / nombre / apellido" />
                        </div>

                        <div class="col-12 col-md-3">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkSoloActivos" runat="server"
                                    CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="<%= chkSoloActivos.ClientID %>">
                                    Solo activos
                                </label>
                            </div>
                        </div>

                        <div class="col-12 col-md-3 d-grid">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                                CssClass="btn btn-outline-secondary"
                                OnClick="btnBuscar_Click" />
                        </div>
                    </div>

                    
                    <div id="msgWrap" runat="server" visible="false" class="mt-3">
                        <div id="msgAlert" class="alert alert-success alert-dismissible fade show mb-0" role="alert">
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </div>
                    </div>

                </div>
            </div>

            <asp:GridView ID="dgvUsuarios" runat="server"
                AutoGenerateColumns="false"
                DataKeyNames="UsuarioID"
                CssClass="table table-striped table-hover table-bordered align-middle"
                HeaderStyle-CssClass="table-dark"
                OnRowCommand="dgvUsuarios_ComandoFila">

                <Columns>
                    <asp:BoundField DataField="UsuarioNombre" HeaderText="Usuario" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="rol.Nombre" HeaderText="Rol" />

                    <asp:TemplateField HeaderText="Activo">
                        <ItemTemplate>
                            <%# (bool)Eval("Activo") ? "Sí" : "No" %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <div class="d-flex gap-2">
                                <asp:LinkButton ID="btnEditar" runat="server"
                                    Text="Editar"
                                    CssClass="btn btn-sm btn-outline-primary"
                                    CommandName="Editar"
                                    CommandArgument="<%# Container.DataItemIndex %>" />

                                <asp:LinkButton ID="btnBaja" runat="server"
                                    Text="Baja"
                                    CssClass="btn btn-sm btn-outline-danger"
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