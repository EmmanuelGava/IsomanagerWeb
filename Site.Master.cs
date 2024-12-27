using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;

namespace IsomanagerWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Actualizar el nombre del usuario si está autenticado
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    litUserName.Text = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Redirigir al login si no está autenticado
                    Response.Redirect("~/Login.aspx", true);
                }
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // Eliminar la cookie de autenticación
            FormsAuthentication.SignOut();

            // Redirigir al login
            Response.Redirect("~/Login.aspx", true);
        }
    }
}