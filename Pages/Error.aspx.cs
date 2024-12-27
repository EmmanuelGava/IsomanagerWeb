using System;
using System.Web;
using System.Web.UI;
using System.Diagnostics;

namespace IsomanagerWeb.Pages
{
    public partial class Error : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Debug.WriteLine("=== PÁGINA DE ERROR ===");
                    // Obtener el mensaje de error de diferentes fuentes posibles
                    string errorMessage = GetErrorMessage();
                    Debug.WriteLine($"Mensaje de error: {errorMessage}");
                    lblError.Text = HttpUtility.HtmlEncode(errorMessage);

                    // En modo debug, mostrar detalles adicionales
                    if (HttpContext.Current.IsDebuggingEnabled)
                    {
                        var error = Session["LastError"] as Exception;
                        if (error != null)
                        {
                            lblErrorDetails.Visible = true;
                            lblErrorDetails.Text = $"Detalles técnicos: {error.Message}";
                            Debug.WriteLine($"Detalles del error: {error.Message}");
                            Debug.WriteLine($"Stack Trace: {error.StackTrace}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al procesar la página de error: {ex.Message}");
                    lblError.Text = "Ha ocurrido un error al procesar la página de error.";
                }
            }
        }

        private string GetErrorMessage()
        {
            // Intentar obtener el mensaje de error de diferentes fuentes
            string message = Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message))
            {
                Debug.WriteLine("Mensaje de error obtenido de QueryString");
                return message;
            }

            // Intentar obtener el error de la sesión
            if (Session["LastError"] != null)
            {
                var error = Session["LastError"] as Exception;
                Session["LastError"] = null; // Limpiar el error
                if (error != null)
                {
                    Debug.WriteLine("Mensaje de error obtenido de Session");
                    return error.Message;
                }
            }

            // Intentar obtener el error del servidor
            if (Server.GetLastError() != null)
            {
                var error = Server.GetLastError();
                Server.ClearError();
                Debug.WriteLine("Mensaje de error obtenido de Server.GetLastError");
                return error.Message;
            }

            // Mensaje por defecto
            Debug.WriteLine("Usando mensaje de error por defecto");
            return "Ha ocurrido un error inesperado. Por favor, inténtelo de nuevo más tarde.";
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al redirigir al inicio: {ex.Message}");
                Response.Redirect("~/");
            }
        }
    }
} 