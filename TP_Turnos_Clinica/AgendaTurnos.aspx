<%@ Page Title="Agenda - Alta de Turno"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="AgendaTurnos.aspx.cs"
    Inherits="TP_Turnos_Clinica.AgendaTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Agenda - Alta de Turno</h3>

    <asp:Label ID="lblMensaje" runat="server"
        CssClass="alert alert-danger d-block mb-3"
        Visible="false" />

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-3">

                <div class="col-md-6">
                    <label class="form-label">Paciente</label>
                    <asp:DropDownList ID="ddlPacientes" runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlPacientes_SelectedIndexChanged" />
                </div>

                <div class="col-md-6">
                    <label class="form-label">Especialidad</label>
                    <asp:DropDownList ID="ddlEspecialidades" runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged" />
                </div>

                <div class="col-12">
                    <label class="form-label">Motivo de la consulta</label>
                    <asp:TextBox ID="txtMotivo" runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="400" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Importe</label>
                    <asp:TextBox ID="txtImporte" runat="server"
                        CssClass="form-control"
                        placeholder="Ej: 15000" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Medio de pago</label>
                    <asp:DropDownList ID="ddlMedioPago" runat="server" CssClass="form-select">
                        <asp:ListItem Text="MERCADOPAGO" Value="MERCADOPAGO" Selected="True" />
                        <asp:ListItem Text="TRANSFERENCIA" Value="TRANSFERENCIA" />
                        <asp:ListItem Text="EFECTIVO" Value="EFECTIVO" />
                    </asp:DropDownList>
                </div>

                <div class="col-md-4">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server"
                        CssClass="form-control"
                        TextMode="Date" />
                </div>

                <div class="col-md-4">
                    <label class="form-label">Disponibilidad del paciente</label>
                   <asp:DropDownList ID="ddlFranja" runat="server" CssClass="form-select">
    <asp:ListItem Text="Mañana (08 a 12)" Value="MANIANA" Selected="True" />
    <asp:ListItem Text="Tarde (14 a 18)" Value="TARDE" />
    <asp:ListItem Text="Noche (19 a 22)" Value="NOCHE" />
</asp:DropDownList>
                </div>

            </div>

            <hr class="my-3" />

            <asp:Button ID="btnSugerir" runat="server"
                Text="Ver disponibilidad"
                CssClass="btn btn-primary"
                OnClick="btnSugerir_Click" />

            <a class="btn btn-link ms-2" href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>

        </div>
    </div>

    <asp:Panel ID="pnlSugerencias" runat="server" Visible="false" CssClass="card">
        <div class="card-body">
            <h5>Disponibilidad</h5>

            <asp:GridView ID="gvSugerencias" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="false"
                OnRowCommand="gvSugerencias_ComandoPorFila">

                <Columns>
                    <asp:BoundField DataField="Medico" HeaderText="Médico" />
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />

                    <asp:TemplateField HeaderText="Hora">
                        <ItemTemplate>
                            <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %> -
                            <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# (bool)Eval("Ocupado") ? "badge bg-danger" : "badge bg-success" %>'>
                                <%# Eval("Estado") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                Text="Elegir"
                                CommandName="Elegir"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                CssClass="btn btn-success btn-sm"
                                Enabled='<%# !(bool)Eval("Ocupado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </asp:Panel>

</asp:Content>