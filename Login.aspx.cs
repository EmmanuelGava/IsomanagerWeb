using System;
using System.Web.UI;
using System.Linq;
using System.Web.Security;
using IsomanagerWeb.Models;
using System.Diagnostics;
using System.Data.Entity;

namespace IsomanagerWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiar cualquier sesión existente
                Session.Clear();
                FormsAuthentication.SignOut();

                // Verificar si hay un mensaje de error en la URL
                string error = Request.QueryString["error"];
                if (!string.IsNullOrEmpty(error))
                {
                    lblError.Text = Server.UrlDecode(error);
                    lblError.Visible = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                Debug.WriteLine($"Intento de login para email: {email}");

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MostrarError("Por favor, ingrese su correo electrónico y contraseña.");
                    return;
                }

                using (var context = new IsomanagerContext())
                {
                    var usuario = context.Usuarios
                        .Include("Area")
                        .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

                    if (usuario == null)
                    {
                        Debug.WriteLine("Usuario no encontrado en la base de datos");
                        MostrarError("Usuario no encontrado.");
                        return;
                    }

                    Debug.WriteLine($"Usuario encontrado: {usuario.Nombre}, Estado: {usuario.Estado}");

                    // Verificar contraseña
                    string hashedPassword = GeneratePasswordHash(password);
                    Debug.WriteLine($"Hash de contraseña ingresada: {hashedPassword}");
                    Debug.WriteLine($"Hash almacenado en BD: {usuario.Password}");

                    if (usuario.Password != hashedPassword)
                    {
                        Debug.WriteLine("Contraseña incorrecta");
                        ManejarIntentoFallido(usuario);
                        return;
                    }

                    // Verificar estado del usuario
                    if (usuario.Estado != "Activo")
                    {
                        Debug.WriteLine($"Usuario inactivo. Estado actual: {usuario.Estado}");
                        MostrarError("Su cuenta está desactivada. Por favor, contacte al administrador.");
                        return;
                    }

                    Debug.WriteLine("Login exitoso, actualizando última conexión");

                    // Actualizar última conexión
                    usuario.UltimaConexion = DateTime.Now;
                    usuario.ContadorIntentos = 0; // Resetear contador de intentos
                    context.SaveChanges();

                    // Guardar información en sesión
                    Session["Usuario"] = usuario;
                    Session["UsuarioId"] = usuario.UsuarioId;
                    Session["NombreUsuario"] = usuario.Nombre;
                    Session["EmailUsuario"] = usuario.Email;
                    Session["RolUsuario"] = usuario.Rol;
                    Session["DepartamentoUsuario"] = usuario.Area?.Nombre ?? "Sin departamento";

                    Debug.WriteLine("Información guardada en sesión");

                    // Autenticar usuario
                    FormsAuthentication.SetAuthCookie(usuario.Email, false);

                    Debug.WriteLine("Redirigiendo al dashboard");

                    // Redirigir al dashboard
                    Response.Redirect("~/Pages/Dashboard.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en login: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                MostrarError("Error al intentar iniciar sesión. Por favor, intente nuevamente.");
            }
        }

        private void ManejarIntentoFallido(Usuario usuario)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    // Incrementar contador de intentos
                    usuario.ContadorIntentos++;
                    
                    // Si excede el máximo de intentos, bloquear la cuenta
                    if (usuario.ContadorIntentos >= 3)
                    {
                        usuario.Estado = "Inactivo";
                        context.SaveChanges();
                        MostrarError("Su cuenta ha sido bloqueada por múltiples intentos fallidos. Por favor, contacte al administrador.");
                    }
                    else
                    {
                        context.SaveChanges();
                        MostrarError($"Contraseña incorrecta. Intentos restantes: {3 - usuario.ContadorIntentos}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al manejar intento fallido: {ex.Message}");
                MostrarError("Error al procesar la solicitud. Por favor, intente nuevamente.");
            }
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }

        private string GeneratePasswordHash(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombreRegistro.Text.Trim();
                string email = txtEmailRegistro.Text.Trim();
                string password = txtPasswordRegistro.Text;

                using (var context = new IsomanagerContext())
                {
                    // Verificar si el email ya existe
                    if (context.Usuarios.Any(u => u.Email.ToLower() == email.ToLower()))
                    {
                        lblErrorRegistro.Text = "El correo electrónico ya está registrado.";
                        pnlErrorRegistro.Visible = true;
                        return;
                    }

                    // Crear nuevo usuario
                    var usuario = new Usuario
                    {
                        Nombre = nombre,
                        Email = email,
                        Password = GeneratePasswordHash(password),
                        Estado = "Activo",
                        Rol = "Usuario", // Rol por defecto
                        FechaRegistro = DateTime.Now,
                        ContadorIntentos = 0
                    };

                    context.Usuarios.Add(usuario);
                    context.SaveChanges();

                    // Limpiar campos y mostrar mensaje de éxito
                    txtNombreRegistro.Text = "";
                    txtEmailRegistro.Text = "";
                    txtPasswordRegistro.Text = "";
                    txtConfirmPasswordRegistro.Text = "";

                    // Ejecutar script de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "RegistroExitoso", 
                        "showSuccessAndHideModal();", true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en registro: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                lblErrorRegistro.Text = "Error al crear el usuario. Por favor, intente nuevamente.";
                pnlErrorRegistro.Visible = true;
            }
        }

        protected void btnRecuperarPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RecuperarPassword.aspx");
        }
    }
} 
