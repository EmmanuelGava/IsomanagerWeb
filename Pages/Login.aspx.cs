using System;
using System.Configuration;
using System.Web.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using IsomanagerWeb.Models;
using IsomanagerWeb.Utils;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Verificar si existe una cookie de autenticación
            HttpCookie authCookie = Request.Cookies["AuthCookie"];
            if (authCookie != null)
            {
                // Desencriptar la cookie y restaurar los datos de sesión
                string decryptedValue = Encryption.Decrypt(authCookie.Value);
                string[] userData = decryptedValue.Split('|');
                string email = userData[0];
                string dbType = userData[1];

                // Establecer el tipo de base de datos en la sesión
                Session["DatabaseType"] = dbType;
                Session["ConnectionString"] = ConnectionManager.GetConnectionString(dbType);

                try
                {
                    using (var db = new IsomanagerContext())
                    {
                        var usuario = db.Usuarios
                            .Include(u => u.Area)
                            .FirstOrDefault(u => u.Email == email);

                        if (usuario != null && usuario.Estado == "Activo")
                        {
                            // Establecer variables de sesión
                            Session["Usuario"] = usuario;
                            Session["UsuarioId"] = usuario.UsuarioId;
                            Session["NombreUsuario"] = usuario.Nombre;
                            Session["EmailUsuario"] = usuario.Email;
                            Session["RolUsuario"] = usuario.Rol;
                            Session["CargoUsuario"] = usuario.Cargo;
                            Session["ResponsabilidadesUsuario"] = usuario.Responsabilidades;
                            Session["DepartamentoUsuario"] = usuario.Area?.Nombre ?? "Sin departamento";
                            Session["Autenticado"] = true;

                            // Redirigir al dashboard
                            Response.Redirect("~/Pages/Dashboard.aspx");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Si hay un error con la cookie, cerrar la sesión
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    Response.Cookies["AuthCookie"].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error inesperado
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            if (Request.Cookies["AuthCookie"] != null)
            {
                Response.Cookies["AuthCookie"].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;

        try
        {
            using (var db = new IsomanagerContext())
            {
                var usuario = db.Usuarios
                    .Include(u => u.Area)
                    .FirstOrDefault(u => u.Email == email);

                if (usuario != null)
                {
                    if (usuario.Estado == "Bloqueado")
                    {
                        lblError.Text = "Usuario bloqueado. Contacte al administrador.";
                        return;
                    }

                    if (usuario.ContadorIntentos >= 3)
                    {
                        usuario.Estado = "Bloqueado";
                        db.SaveChanges();
                        lblError.Text = "Usuario bloqueado por múltiples intentos fallidos.";
                        return;
                    }

                    if (Encryption.VerifyPassword(password, usuario.Password))
                    {
                        // Restablecer contador de intentos
                        usuario.ContadorIntentos = 0;
                        usuario.UltimaConexion = DateTime.Now;
                        db.SaveChanges();

                        // Establecer variables de sesión
                        Session["Usuario"] = usuario;
                        Session["UsuarioId"] = usuario.UsuarioId;
                        Session["NombreUsuario"] = usuario.Nombre;
                        Session["EmailUsuario"] = usuario.Email;
                        Session["RolUsuario"] = usuario.Rol;
                        Session["CargoUsuario"] = usuario.Cargo;
                        Session["ResponsabilidadesUsuario"] = usuario.Responsabilidades;
                        Session["DepartamentoUsuario"] = usuario.Area?.Nombre ?? "Sin departamento";
                        Session["Autenticado"] = true;

                        // Si el checkbox de recordar está marcado, crear cookie
                        if (chkRecordarme.Checked)
                        {
                            string dbType = Session["DatabaseType"]?.ToString() ?? "local";
                            string userData = $"{email}|{dbType}";
                            string encryptedValue = Encryption.Encrypt(userData);

                            HttpCookie authCookie = new HttpCookie("AuthCookie", encryptedValue);
                            authCookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(authCookie);
                        }

                        Response.Redirect("~/Pages/Dashboard.aspx");
                    }
                    else
                    {
                        usuario.ContadorIntentos++;
                        db.SaveChanges();

                        int intentosRestantes = 3 - usuario.ContadorIntentos;
                        lblError.Text = $"Contraseña incorrecta. Intentos restantes: {intentosRestantes}";
                    }
                }
                else
                {
                    lblError.Text = "Usuario no encontrado.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al intentar iniciar sesión.";
            // Aquí podrías agregar logging del error
        }
    }
} 