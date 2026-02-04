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
                        MaxLength="400"
                        placeholder="Ej: Dolor, control, etc." />
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
                    <label class="form-label">Buscar desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server"
                        CssClass="form-control"
                        TextMode="Date" />
                </div>

            </div>

            <hr class="my-4" />

            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <asp:Button ID="btnSugerir" runat="server"
                        Text="Sugerir 3 opciones"
                        CssClass="btn btn-primary"
                        OnClick="btnSugerir_Click" />

                    <a class="btn btn-outline-secondary ms-2"
                       href="<%= ResolveUrl("~/TurnoForm.aspx") %>">
                        Cargar manual
                    </a>
                </div>

                <a class="btn btn-link" href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>
            </div>

        </div>
    </div>

    <asp:Panel ID="pnlSugerencias" runat="server" Visible="false" CssClass="card">
        <div class="card-body">
            <h5 class="mb-3">Opciones sugeridas</h5>

            <asp:GridView ID="gvSugerencias" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="false"
                OnRowCommand="gvSugerencias_ComandoPorFila">
                <Columns>

                    <asp:BoundField DataField="Medico" HeaderText="Médico" />
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                    <asp:TemplateField HeaderText="Hora">
                        <ItemTemplate>
                            <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %> -
                            <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElegir" runat="server"
                                Text="Elegir"
                                CommandName="Elegir"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                CssClass="btn btn-success btn-sm"
                                CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </asp:Panel>

</asp:Content>