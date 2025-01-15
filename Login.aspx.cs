using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using System.Diagnostics;
using System.Configuration;
using System.Web.Configuration;
using IsomanagerWeb.Models;
using IsomanagerWeb.Utils;

namespace IsomanagerWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlGenericControl localConfig;
        protected System.Web.UI.HtmlControls.HtmlGenericControl remoteConfig;
        protected System.Web.UI.HtmlControls.HtmlGenericControl azureConfig;
        protected System.Web.UI.HtmlControls.HtmlGenericControl localCredentials;
        protected System.Web.UI.HtmlControls.HtmlGenericControl connectionHelp;
        
        protected System.Web.UI.WebControls.TextBox txtLocalInstance;
        protected System.Web.UI.WebControls.TextBox txtLocalDatabase;
        protected System.Web.UI.WebControls.CheckBox chkLocalIntegratedSecurity;
        protected System.Web.UI.WebControls.TextBox txtLocalUser;
        protected System.Web.UI.WebControls.TextBox txtLocalPassword;
        
        protected System.Web.UI.WebControls.TextBox txtRemoteServer;
        protected System.Web.UI.WebControls.TextBox txtRemotePort;
        protected System.Web.UI.WebControls.TextBox txtRemoteDatabase;
        protected System.Web.UI.WebControls.TextBox txtRemoteUser;
        protected System.Web.UI.WebControls.TextBox txtRemotePassword;
        
        protected System.Web.UI.WebControls.TextBox txtAzureServer;
        protected System.Web.UI.WebControls.TextBox txtAzureDatabase;
        protected System.Web.UI.WebControls.TextBox txtAzureUser;
        protected System.Web.UI.WebControls.TextBox txtAzurePassword;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si hay una cookie de autenticación
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    try
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        if (!ticket.Expired)
                        {
                            string dbType = ticket.UserData; // Recuperar el tipo de base de datos del ticket
                            
                            // Establecer el tipo de base de datos en la sesión
                            Session["TipoBaseDatos"] = dbType;
                            
                            // Configurar la cadena de conexión
                            IsomanagerContext.SetConnectionString(dbType);
                            
                            // Intentar autenticar al usuario
                            using (var context = new IsomanagerContext())
                            {
                                var usuario = context.Usuarios
                                    .Include("Area")
                                    .FirstOrDefault(u => u.Email == ticket.Name);

                                if (usuario != null && usuario.Estado == "Activo")
                                {
                                    // Guardar información en sesión
                                    Session["Usuario"] = usuario;
                                    Session["UsuarioId"] = usuario.UsuarioId;
                                    Session["NombreUsuario"] = usuario.Nombre;
                                    Session["EmailUsuario"] = usuario.Email;
                                    Session["RolUsuario"] = usuario.Rol;
                                    Session["DepartamentoUsuario"] = usuario.Area?.Nombre ?? "Sin departamento";

                                    // Redirigir al dashboard
                                    Response.Redirect("~/Pages/Dashboard.aspx", false);
                                    Context.ApplicationInstance.CompleteRequest();
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Si hay algún error con la cookie, la eliminamos
                        FormsAuthentication.SignOut();
                        Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                    }
                }

                // Si no hay cookie válida o hubo un error, mostrar el formulario de login
                Session.Clear();
                FormsAuthentication.SignOut();

                // Verificar si hay un mensaje de error en la URL
                string error = Request.QueryString["error"];
                if (!string.IsNullOrEmpty(error))
                {
                    lblError.Text = Server.UrlDecode(error);
                    lblError.Visible = true;
                }

                LoadCurrentDbConfig();
            }
        }

        private void LoadCurrentDbConfig()
        {
            try
            {
                string defaultType = ConfigurationManager.AppSettings["DefaultDatabaseType"] ?? "local";
                ddlDbTypeConfig.SelectedValue = defaultType;
                ddlDbType.SelectedValue = defaultType;

                switch (defaultType)
                {
                    case "local":
                        txtLocalInstance.Text = ConfigurationManager.AppSettings["LocalInstance"] ?? @".\SQLEXPRESS";
                        txtLocalDatabase.Text = ConfigurationManager.AppSettings["LocalDatabase"] ?? "IsomanagerDB";
                        bool useIntegratedSecurity = bool.Parse(ConfigurationManager.AppSettings["LocalUseIntegratedSecurity"] ?? "true");
                        chkLocalIntegratedSecurity.Checked = useIntegratedSecurity;
                        localCredentials.Visible = !useIntegratedSecurity;
                        if (!useIntegratedSecurity)
                        {
                            txtLocalUser.Text = ConfigurationManager.AppSettings["LocalUser"];
                            txtLocalPassword.Text = ConfigurationManager.AppSettings["LocalPassword"];
                        }
                        break;

                    case "remote":
                        txtRemoteServer.Text = ConfigurationManager.AppSettings["RemoteServer"];
                        txtRemotePort.Text = ConfigurationManager.AppSettings["RemotePort"] ?? "1433";
                        txtRemoteDatabase.Text = ConfigurationManager.AppSettings["RemoteDatabase"];
                        txtRemoteUser.Text = ConfigurationManager.AppSettings["RemoteUser"];
                        txtRemotePassword.Text = ConfigurationManager.AppSettings["RemotePassword"];
                        break;

                    case "azure":
                        txtAzureServer.Text = ConfigurationManager.AppSettings["AzureServer"];
                        txtAzureDatabase.Text = ConfigurationManager.AppSettings["AzureDatabase"];
                        txtAzureUser.Text = ConfigurationManager.AppSettings["AzureUser"];
                        txtAzurePassword.Text = ConfigurationManager.AppSettings["AzurePassword"];
                        break;
                }

                // Mostrar el panel correspondiente
                ddlDbType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar la configuración de BD: {ex.Message}");
                // Configuración por defecto en caso de error
                ddlDbTypeConfig.SelectedValue = "local";
                ddlDbType.SelectedValue = "local";
                localConfig.Visible = true;
            }
        }

        protected void ddlDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = ddlDbTypeConfig.SelectedValue;
            
            // Ocultar todos los paneles primero
            localConfig.Visible = false;
            remoteConfig.Visible = false;
            azureConfig.Visible = false;
            
            // Mostrar el panel correspondiente
            switch (selectedType)
            {
                case "local":
                    localConfig.Visible = true;
                    connectionHelp.InnerText = "Configure la conexión a SQL Server local. Si usa seguridad integrada, no necesita especificar usuario y contraseña.";
                    break;
                case "remote":
                    remoteConfig.Visible = true;
                    connectionHelp.InnerText = "Configure la conexión a SQL Server remoto. Asegúrese de que el servidor esté accesible y el puerto esté abierto.";
                    break;
                case "azure":
                    azureConfig.Visible = true;
                    connectionHelp.InnerText = "Configure la conexión a Azure SQL Database. Use las credenciales proporcionadas en el portal de Azure.";
                    break;
            }
        }

        protected void chkIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            localCredentials.Visible = !chkLocalIntegratedSecurity.Checked;
        }

        protected void btnSaveDbConfig_Click(object sender, EventArgs e)
        {
            try
            {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                var settings = config.AppSettings.Settings;

                // Limpiar configuraciones existentes
                settings.Remove("LocalInstance");
                settings.Remove("LocalDatabase");
                settings.Remove("LocalUseIntegratedSecurity");
                settings.Remove("LocalUser");
                settings.Remove("LocalPassword");
                settings.Remove("RemoteServer");
                settings.Remove("RemotePort");
                settings.Remove("RemoteDatabase");
                settings.Remove("RemoteUser");
                settings.Remove("RemotePassword");
                settings.Remove("AzureServer");
                settings.Remove("AzureDatabase");
                settings.Remove("AzureUser");
                settings.Remove("AzurePassword");

                string selectedType = ddlDbTypeConfig.SelectedValue;
                switch (selectedType)
                {
                    case "local":
                        settings.Add("LocalInstance", txtLocalInstance.Text);
                        settings.Add("LocalDatabase", txtLocalDatabase.Text);
                        settings.Add("LocalUseIntegratedSecurity", chkLocalIntegratedSecurity.Checked.ToString());
                        if (!chkLocalIntegratedSecurity.Checked)
                        {
                            settings.Add("LocalUser", txtLocalUser.Text);
                            settings.Add("LocalPassword", txtLocalPassword.Text);
                        }
                        break;

                    case "remote":
                        settings.Add("RemoteServer", txtRemoteServer.Text);
                        settings.Add("RemotePort", txtRemotePort.Text);
                        settings.Add("RemoteDatabase", txtRemoteDatabase.Text);
                        settings.Add("RemoteUser", txtRemoteUser.Text);
                        settings.Add("RemotePassword", txtRemotePassword.Text);
                        break;

                    case "azure":
                        settings.Add("AzureServer", txtAzureServer.Text);
                        settings.Add("AzureDatabase", txtAzureDatabase.Text);
                        settings.Add("AzureUser", txtAzureUser.Text);
                        settings.Add("AzurePassword", txtAzurePassword.Text);
                        break;
                }

                // Guardar el tipo de base de datos seleccionado
                settings["DefaultDatabaseType"].Value = selectedType;
                
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", 
                    "toastr.success('La configuración de la base de datos se ha guardado correctamente.');", true);

                // Actualizar el dropdown de la página principal
                ddlDbType.SelectedValue = selectedType;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al guardar configuración: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", 
                    $"toastr.error('Error al guardar la configuración: {ex.Message}');", true);
            }
        }

        protected async void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                string dbType = ddlDbTypeConfig.SelectedValue;
                var result = await ConnectionManager.TestConnection(dbType);
                
                if (result.success)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", 
                        $"toastr.success('{result.message}');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", 
                        $"toastr.error('{result.message}');", true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al probar conexión: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", 
                    $"toastr.error('Error al probar la conexión: {ex.Message}');", true);
            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;
                string dbType = ddlDbType.SelectedValue;

                Debug.WriteLine($"Intento de login para email: {email} usando base de datos: {dbType}");

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MostrarError("Por favor, ingrese su correo electrónico y contraseña.");
                    return;
                }

                // Establecer el tipo de base de datos en la sesión
                Session["TipoBaseDatos"] = dbType;
                Debug.WriteLine($"=== Tipo de base de datos seleccionado: {dbType} ===");

                // Probar la conexión antes de intentar el login
                var connectionTest = await ConnectionManager.TestConnection(dbType);
                if (!connectionTest.success)
                {
                    MostrarError($"Error de conexión a la base de datos: {connectionTest.message}");
                    return;
                }

                // Configurar la cadena de conexión según el tipo de base de datos
                IsomanagerContext.SetConnectionString(dbType);

                using (var context = new IsomanagerContext())
                {
                    var usuario = await context.Usuarios
                        .Include("Area")
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

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
                    await context.SaveChangesAsync();

                    // Guardar información en sesión
                    Session["Usuario"] = usuario;
                    Session["UsuarioId"] = usuario.UsuarioId;
                    Session["NombreUsuario"] = usuario.Nombre;
                    Session["EmailUsuario"] = usuario.Email;
                    Session["RolUsuario"] = usuario.Rol;
                    Session["DepartamentoUsuario"] = usuario.Area?.Nombre ?? "Sin departamento";
                    Session["ConnectionString"] = ConnectionManager.GetConnectionString(dbType);

                    Debug.WriteLine("Información guardada en sesión");

                    // Autenticar usuario y crear cookie persistente si "Recordarme" está marcado
                    if (chkRecordarme.Checked)
                    {
                        // Crear ticket con información de conexión
                        var authTicket = new FormsAuthenticationTicket(
                            1,
                            email,
                            DateTime.Now,
                            DateTime.Now.AddDays(14),
                            true,
                            dbType, // Guardamos el tipo de base de datos en el ticket
                            FormsAuthentication.FormsCookiePath);

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authCookie.Expires = authTicket.Expiration;
                        Response.Cookies.Add(authCookie);
                    }

                    FormsAuthentication.SetAuthCookie(email, chkRecordarme.Checked);
                    Debug.WriteLine("Redirigiendo al dashboard");

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
            Debug.WriteLine($"Error de login: {mensaje}");
            
            if (lblError != null)
            {
                lblError.Text = mensaje;
                lblError.Visible = true;
            }
            else
            {
                // Si lblError es null, mostrar el error usando toastr
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", 
                    $"toastr.error('{mensaje}');", true);
            }
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
