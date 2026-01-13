using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace TP_Turnos_Clinica
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            UsuarioNegocio neg = new UsuarioNegocio();
            Usuario u = neg.Login(txtUsuario.Text, txtPassword.Text);

            if (u == null)
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                return;
            }

            Session["usuario"] = u;

            Response.Redirect("~/Home.aspx");
        }
        
    }
}