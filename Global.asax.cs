using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using IsomanagerWeb.Models;
using System.Diagnostics;

namespace IsomanagerWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new CreateDatabaseIfNotExists<IsomanagerContext>());
            using (var context = new IsomanagerContext())
            {
                context.Database.Initialize(force: false);
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Debug.WriteLine($"=== ERROR GLOBAL ===");
            Debug.WriteLine($"Tipo de error: {ex.GetType().Name}");
            Debug.WriteLine($"Mensaje: {ex.Message}");
            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                Debug.WriteLine($"Inner Stack Trace: {ex.InnerException.StackTrace}");
            }

            // Si es un error de sesión, redirigir al login
            if (ex is HttpException && ex.Message.Contains("estado de sesión no está disponible"))
            {
                Server.ClearError();
                Response.Redirect("~/Login.aspx", true);
                return;
            }

            // Para otros errores, usar la página de error
            string errorUrl = "~/Pages/Error.aspx";
            if (HttpContext.Current != null && HttpContext.Current.Request.IsLocal)
            {
                errorUrl += $"?message={HttpUtility.UrlEncode(ex.Message)}";
            }

            Server.ClearError();
            Response.Redirect(errorUrl, true);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Debug.WriteLine("=== NUEVA SESIÓN INICIADA ===");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Debug.WriteLine("=== SESIÓN FINALIZADA ===");
        }
    }
}