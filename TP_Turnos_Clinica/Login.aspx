<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="TP_Turnos_Clinica.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
      

        html, body { height: 100%; }

        body {
            margin: 0;
            display: grid;
            place-items: center;
            background:
                radial-gradient(1100px 520px at 18% 12%, rgba(120,160,255,.30), transparent 60%),
                radial-gradient(900px 420px at 88% 82%, rgba(180,200,255,.25), transparent 60%),
                linear-gradient(180deg, #f4f7fb 0%, #eaf0f8 100%);
            font-family: "Segoe UI", system-ui, -apple-system, Arial, sans-serif;
        }

     
        .login-wrapper {
            width: 100%;
            padding: 24px;
            display: flex;
            justify-content: center;
        }

       
        .login-card {
            width: 440px;             
            max-width: 440px;
            background: rgba(255,255,255,.88);
            border: 1px solid rgba(15,23,42,.08);
            border-radius: 22px;
            box-shadow: 0 28px 70px rgba(0,0,0,0.14);
            padding: 34px 34px 26px;   
            backdrop-filter: blur(10px);
        }

        .login-title {
            font-size: 28px;           
            font-weight: 800;
            text-align: center;
            margin-bottom: 18px;
            color: #2b3442;
            letter-spacing: -0.02em;
        }

      
        input, select, textarea {
            max-width: none !important;
            width: 100% !important;
        }

        .form-control {
            width: 100% !important;
            display: block !important;
            border-radius: 14px;
            padding: 12px 14px;     
            border: 1px solid #d6dbe4;
            background-color: #f7f9fc;
            font-size: 15px;
        }

        .form-control:focus {
            border-color: #3ea39a;
            box-shadow: 0 0 0 0.18rem rgba(62,163,154,.22);
        }

        .btn-login {
            margin-top: 8px;
            background: linear-gradient(90deg, #2f8f7a, #3ea39a);
            border: none;
            border-radius: 14px;
            padding: 12px;
            font-weight: 700;
            letter-spacing: .01em;
            transition: .25s ease;
        }

        .btn-login:hover {
            transform: translateY(-2px);
            box-shadow: 0 12px 28px rgba(47,143,122,.32);
        }

        .copyright {
            text-align: center;
            margin-top: 16px;
            font-size: 13px;
            color: #6b7280;
            font-weight: 600;
        }

       
        @media (max-width: 480px){
            .login-card{
                width: 92vw;
                max-width: 92vw;
                padding: 28px 22px 22px;
            }
            .login-title{ font-size: 26px; }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">

            <div class="login-card">

                <div class="login-title">Iniciar sesión</div>

                <asp:Label ID="lblError" runat="server"
                    CssClass="text-danger d-block mb-3 text-center" />

                <div class="mb-3">
                    <asp:TextBox ID="txtUsuario" runat="server"
                        CssClass="form-control"
                        placeholder="Usuario" />
                </div>

                <div class="mb-3">
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password"
                        CssClass="form-control"
                        placeholder="Contraseña" />
                </div>

                <asp:Button ID="btnLogin" runat="server"
                    Text="Ingresar"
                    CssClass="btn btn-login w-100 text-white"
                    OnClick="btnLogin_Click" />

                <div class="copyright">
                    © <%: DateTime.Now.Year %> Clínica Turnos
                </div>

            </div>

        </div>
    </form>
</body>
</html>