using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using IsomanagerWeb.Models;
using System.Diagnostics;

namespace IsomanagerWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Registrar rutas y bundles al inicio
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            // Código que se ejecuta al inicio de cada solicitud
            var session = HttpContext.Current.Session;
            if (session != null && session["TipoBaseDatos"] != null)
            {
                string dbType = session["TipoBaseDatos"].ToString();
                IsomanagerContext.SetConnectionString(dbType);
                Debug.WriteLine($"=== Tipo de base de datos configurada: {dbType} ===");
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Debug.WriteLine($"=== ERROR GLOBAL ===");
            Debug.WriteLine($"*Tipo de error: {ex.GetType()}");
            Debug.WriteLine($"*Mensaje: {ex.Message}");
            Debug.WriteLine($"*Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Debug.WriteLine($"*Inner Exception: {ex.InnerException.Message}");
                Debug.WriteLine($"*Inner Stack Trace: {ex.InnerException.StackTrace}");
            }

            // Si es un error de sesión, redirigir al login
            if (ex is HttpException && ex.Message.Contains("estado de sesión no está disponible"))
            {
                Response.Redirect("~/Login.aspx");
            }
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