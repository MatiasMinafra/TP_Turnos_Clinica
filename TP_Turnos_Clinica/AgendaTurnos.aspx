<%@ Page Title="Agenda - Alta de Turno"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="AgendaTurnos.aspx.cs"
    Inherits="TP_Turnos_Clinica.AgendaTurnos" %>

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

    .page-wrap{ padding-top:14px; padding-bottom:24px; }

    .page-title{ font-weight:800; margin:0; letter-spacing:-.2px; }
    .subtle{ color:#6c757d; }

    .card-soft{
        border:1px solid rgba(0,0,0,.08);
        border-radius:14px;
        overflow:hidden;
        background:#fff;
    }
    .card-soft .card-header{
        background:linear-gradient(90deg, rgba(13,110,253,.12), rgba(13,110,253,.05));
        border-bottom:1px solid rgba(0,0,0,.06);
        padding:14px 18px;
    }
    .card-soft .card-body{ padding:18px; }

    .btn-chip{
        border-radius:10px;
        padding-left:.85rem;
        padding-right:.85rem;
        font-weight:600;
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

    .field-hint{ font-size:.85rem; color:#6c757d; margin-top:.15rem; }

    .form-label{ font-weight:600; }
    .form-control, .form-select{
        border-radius:12px;
    }

    .section-title{ font-weight:700; margin:0; }
    .divider{
        height:1px;
        background:rgba(0,0,0,.08);
        margin:14px 0;
    }


    .table-wrap{
        border-radius:14px;
        overflow:hidden;
        border:1px solid rgba(0,0,0,.08);
    }
    .table thead th{ white-space:nowrap; }
    .table td, .table th{ vertical-align:middle; }
    .table td{ font-size:.9rem; }

  
    .badge-soft{
        display:inline-flex;
        align-items:center;
        padding:.35rem .6rem;
        border-radius:999px;
        font-weight:700;
        font-size:.82rem;
        white-space:nowrap;
        border:1px solid rgba(0,0,0,.06);
    }
    .badge-ok{ background:rgba(25,135,84,.12); color:#146c43; }
    .badge-bad{ background:rgba(220,53,69,.12); color:#b02a37; }

    .alert{ border-radius:14px; }
</style>

<div class="full-bleed">
    <div class="page-wrap">

        <div class="d-flex align-items-start justify-content-between mb-3">
            <div>
                <h2 class="page-title">Agenda - Alta de Turno</h2>
                <div class="subtle">Seleccioná paciente, especialidad y consultá disponibilidad</div>
                <div class="pillbar">
                    <span class="pill">🧑‍⚕️ Elegí especialidad</span>
                    <span class="pill">📅 Fecha + franja</span>
                    <span class="pill">✅ Elegí un horario libre</span>
                </div>
            </div>

            <a class="btn btn-outline-secondary btn-chip" href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>
        </div>

        <asp:HiddenField ID="hfTurnoIdReprog" runat="server" />

        
        <asp:Panel ID="pnlReprog" runat="server" Visible="false" CssClass="alert alert-info d-flex align-items-center justify-content-between mb-3">
            <div>
                <strong>Modo REPROGRAMAR:</strong>
                elegí un nuevo horario para el turno <asp:Label ID="lblTurnoReprog" runat="server" />.
            </div>

            <asp:LinkButton ID="btnSalirReprog" runat="server"
                CssClass="btn btn-sm btn-outline-dark btn-chip"
                OnClick="btnSalirReprog_Click"
                CausesValidation="false">
                Salir
            </asp:LinkButton>
        </asp:Panel>

     
        <asp:Label ID="lblMensaje" runat="server"
            CssClass="alert alert-danger d-block mb-3"
            Visible="false" />

      
        <div class="card card-soft shadow-sm mb-3">
            <div class="card-header">
                <div class="d-flex align-items-center justify-content-between">
                    <div>
                        <div class="section-title">Datos del turno</div>
                        <small class="subtle">Completá los datos básicos antes de ver disponibilidad</small>
                    </div>
                </div>
            </div>

            <div class="card-body">
                <div class="row g-3">

                    <div class="col-12 col-lg-6">
                        <label class="form-label">Paciente</label>
                        <asp:DropDownList ID="ddlPacientes" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPacientes_SelectedIndexChanged" />
                        <div class="field-hint">Elegí el paciente para asociar el turno.</div>
                    </div>

                    <div class="col-12 col-lg-6">
                        <label class="form-label">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidades" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged" />
                        <div class="field-hint">Filtra médicos y horarios disponibles.</div>
                    </div>

                    <div class="col-12">
                        <label class="form-label">Motivo de la consulta</label>
                        <asp:TextBox ID="txtMotivo" runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="3"
                            MaxLength="400" />
                        <div class="field-hint">Opcional. Máx 400 caracteres.</div>
                    </div>

                    <div class="col-12 col-md-4">
                        <label class="form-label">Importe</label>
                        <asp:TextBox ID="txtImporte" runat="server"
                            CssClass="form-control"
                            placeholder="Ej: 15000" />
                    </div>

                    <div class="col-12 col-md-4">
                        <label class="form-label">Medio de pago</label>
                        <asp:DropDownList ID="ddlMedioPago" runat="server" CssClass="form-select">
                            <asp:ListItem Text="MERCADOPAGO" Value="MERCADOPAGO" Selected="True" />
                            <asp:ListItem Text="TRANSFERENCIA" Value="TRANSFERENCIA" />
                            <asp:ListItem Text="EFECTIVO" Value="EFECTIVO" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 col-md-4">
                        <label class="form-label">Fecha</label>
                        <asp:TextBox ID="txtFechaDesde" runat="server"
                            CssClass="form-control"
                            TextMode="Date" />
                    </div>

                    <div class="col-12 col-md-5">
                        <label class="form-label">Disponibilidad del paciente</label>
                        <asp:DropDownList ID="ddlFranja" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFranja_SelectedIndexChanged">
                            <asp:ListItem Text="Mañana (08 a 12)" Value="MANANA" Selected="True" />
                            <asp:ListItem Text="Tarde (14 a 18)" Value="TARDE" />
                            <asp:ListItem Text="Noche (19 a 22)" Value="NOCHE" />
                        </asp:DropDownList>
                        <div class="field-hint">Define el rango horario en el que buscás turnos.</div>
                    </div>

                </div>

                <div class="divider"></div>

                <div class="d-flex flex-wrap gap-2">
                    <asp:Button ID="btnSugerir" runat="server"
                        Text="Ver disponibilidad"
                        CssClass="btn btn-primary btn-chip"
                        OnClick="btnSugerir_Click" />

                    <a class="btn btn-outline-secondary btn-chip" href="<%= ResolveUrl("~/Home.aspx") %>">Volver</a>
                </div>

            </div>
        </div>

     
        <asp:Panel ID="pnlSugerencias" runat="server" Visible="false" CssClass="card card-soft shadow-sm">
            <div class="card-header">
                <div class="d-flex align-items-center justify-content-between">
                    <div>
                        <div class="section-title">Disponibilidad</div>
                        <small class="subtle">Elegí un horario libre para agendar el turno</small>
                    </div>
                </div>
            </div>

            <div class="card-body p-0">
                <div class="table-responsive table-wrap">
                    <asp:GridView ID="gvSugerencias" runat="server"
                        CssClass="table table-hover table-striped align-middle mb-0"
                        AutoGenerateColumns="false"
                        HeaderStyle-CssClass="table-dark"
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
                                    <span class='badge-soft <%# (bool)Eval("Ocupado") ? "badge-bad" : "badge-ok" %>'>
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
                                        CssClass="btn btn-success btn-sm btn-chip"
                                        Enabled='<%# !(bool)Eval("Ocupado") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>

    </div>
</div>

</asp:Content>