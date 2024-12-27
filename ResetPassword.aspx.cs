using System;
using System.Web.UI;

namespace IsomanagerWeb
{
    public partial class ResetPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (string.IsNullOrEmpty(token))
                {
                    MostrarError("El enlace de restablecimiento no es válido.");
                    return;
                }

                // Aquí deberías validar el token en la base de datos
                // Verificar que existe y no ha expirado
                // Por ahora, simularemos que el token es válido
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                string token = Request.QueryString["token"];
                string newPassword = txtNewPassword.Text;

                // Aquí deberías:
                // 1. Validar el token nuevamente
                // 2. Actualizar la contraseña en la base de datos
                // 3. Invalidar el token usado
                // Por ahora, simularemos que todo fue exitoso

                // Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                    "toastr.success('Su contraseña ha sido restablecida exitosamente.');", true);

                // Redirigir al login después de un breve delay
                ScriptManager.RegisterStartupScript(this, GetType(), "redirectToLogin",
                    "setTimeout(function() { window.location.href = 'Login.aspx'; }, 2000);", true);
            }
            catch (Exception ex)
            {
                // Registrar el error para debugging
                System.Diagnostics.Debug.WriteLine($"Error al restablecer contraseña: {ex.Message}");

                // Mostrar mensaje de error al usuario
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "toastr.error('Ocurrió un error al restablecer su contraseña. Por favor, intente nuevamente.');", true);
            }
        }

        protected void btnVolverLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        private void MostrarError(string mensaje)
        {
            pnlResetPassword.Visible = false;
            pnlError.Visible = true;
            litError.Text = mensaje;
        }
    }
} 