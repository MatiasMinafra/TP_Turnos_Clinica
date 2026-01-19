<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="TP_Turnos_Clinica.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            min-height: 100vh;
            background-color: #f4f6f8; /* gris claro relajado */
        }

        .login-card {
            max-width: 420px;
            border-radius: 8px;
        }

        .login-title {
            color: #2f8f7a; /* verde suave */
            font-weight: 600;
        }

        .btn-login {
            background-color: #2f8f7a;
            border: none;
        }

        .btn-login:hover {
            background-color: #256f5f;
        }
    </style>
</head>

<body>

    <form id="form1" runat="server">

        <div class="container">
            <div class="row justify-content-center" style="padding-top: 12vh;">
                <div class="col-12">

                    <div class="card login-card mx-auto shadow-sm border-0">
                        <div class="card-body p-4">

                            <h4 class="text-center mb-4 login-title">
                                Iniciar sesión
                            </h4>

                            <asp:Label ID="lblError" runat="server"
                                CssClass="text-danger d-block mb-3 text-center" />

                            <div class="mb-3">
                                <asp:TextBox ID="txtUsuario" runat="server"
                                    CssClass="form-control"
                                    placeholder="Usuario" />
                            </div>

                            <div class="mb-4">
                                <asp:TextBox ID="txtPassword" runat="server"
                                    TextMode="Password"
                                    CssClass="form-control"
                                    placeholder="Contraseña" />
                            </div>

                            <asp:Button ID="btnLogin" runat="server"
                                Text="Ingresar"
                                CssClass="btn btn-login w-100 text-white"
                                OnClick="btnLogin_Click" />

                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>

</body>
</html>