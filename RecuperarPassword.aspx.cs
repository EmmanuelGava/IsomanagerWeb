using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI;

namespace IsomanagerWeb
{
    public partial class RecuperarPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización si es necesaria
            }
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                
                // Aquí deberías verificar si el email existe en tu base de datos
                // Por ahora, simularemos que el email existe
                
                // Generar token único para recuperación (esto debería guardarse en la base de datos)
                string token = Guid.NewGuid().ToString();
                
                // Construir el enlace de recuperación
                string resetLink = $"{Request.Url.Scheme}://{Request.Url.Authority}/ResetPassword.aspx?token={token}";
                
                // Enviar email
                EnviarEmailRecuperacion(email, resetLink);
                
                // Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                    "toastr.success('Se han enviado las instrucciones a su correo electrónico.');", true);
                
                // Limpiar el campo de email
                txtEmail.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // Registrar el error para debugging
                System.Diagnostics.Debug.WriteLine($"Error en recuperación de contraseña: {ex.Message}");
                
                // Mostrar mensaje de error al usuario
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "toastr.error('Ocurrió un error al procesar su solicitud. Por favor, intente nuevamente.');", true);
            }
        }

        private void EnviarEmailRecuperacion(string emailDestino, string resetLink)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SmtpEmail"], "ISOManager");
                    mail.To.Add(emailDestino);
                    mail.Subject = "Recuperación de Contraseña - ISOManager";
                    mail.Body = $@"
                        <h2>Recuperación de Contraseña</h2>
                        <p>Hemos recibido una solicitud para restablecer su contraseña.</p>
                        <p>Si usted no realizó esta solicitud, puede ignorar este mensaje.</p>
                        <p>Para restablecer su contraseña, haga clic en el siguiente enlace:</p>
                        <p><a href='{resetLink}' style='padding: 10px 20px; background-color: #212529; color: white; text-decoration: none; border-radius: 5px;'>
                            Restablecer Contraseña
                        </a></p>
                        <p>Este enlace expirará en 24 horas.</p>
                        <p>Si el botón no funciona, copie y pegue el siguiente enlace en su navegador:</p>
                        <p>{resetLink}</p>
                        <br>
                        <p>Saludos cordiales,<br>Equipo de ISOManager</p>";
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.Host = ConfigurationManager.AppSettings["SmtpHost"];
                        smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                        smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpSsl"]);
                        smtp.Credentials = new System.Net.NetworkCredential(
                            ConfigurationManager.AppSettings["SmtpEmail"],
                            ConfigurationManager.AppSettings["SmtpPassword"]);
                        
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el email de recuperación", ex);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
} 