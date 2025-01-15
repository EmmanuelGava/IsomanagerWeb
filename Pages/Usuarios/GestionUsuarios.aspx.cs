using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Usuarios
{
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var query = context.Usuarios
                        .Include(u => u.Area)
                        .OrderByDescending(u => u.FechaRegistro)
                        .AsQueryable();

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(u =>
                            u.Nombre.ToLower().Contains(filtro) ||
                            u.Email.ToLower().Contains(filtro) ||
                            u.Rol.ToLower().Contains(filtro) ||
                            (u.Area != null && u.Area.Nombre.ToLower().Contains(filtro))
                        );
                    }

                    var usuarios = query.ToList().Select(u => new
                    {
                        u.UsuarioId,
                        u.Nombre,
                        u.Email,
                        u.Rol,
                        Estado = u.Estado,
                        Area = u.Area != null ? u.Area.Nombre : "Sin área",
                        FechaRegistro = u.FechaRegistro.ToString("dd/MM/yyyy")
                    });

                    gvUsuarios.DataSource = usuarios;
                    gvUsuarios.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar los usuarios: " + ex.Message);
            }
        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            // Limpiar campos del modal
            LimpiarModal();
            litTituloModal.Text = "Nuevo Usuario";
            hdnUsuarioId.Value = "";
            divPassword.Visible = true;

            // Mostrar el modal
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                "setTimeout(function() { showModal(); }, 100);", true);
        }

        private void LimpiarModal()
        {
            hdnUsuarioId.Value = "";
            txtNombre.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            ddlRol.SelectedIndex = 0;
            ucDepartamento.SelectedDepartamentoId = null;
            litTituloModal.Text = "Nuevo Usuario";
            divPassword.Visible = true;
            lblErrorModal.Visible = false;
        }

        protected void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var context = new IsomanagerContext())
                    {
                        bool esNuevo = string.IsNullOrEmpty(hdnUsuarioId.Value);
                        Usuario usuario;

                        if (esNuevo)
                        {
                            // Verificar que el email no exista
                            if (context.Usuarios.Any(u => u.Email.ToLower() == txtEmail.Text.Trim().ToLower()))
                            {
                                MostrarError("El correo electrónico ya está registrado.");
                                return;
                            }

                            usuario = new Usuario
                            {
                                FechaRegistro = DateTime.Now,
                                Estado = "Activo",
                                ContadorIntentos = 0
                            };
                            context.Usuarios.Add(usuario);
                        }
                        else
                        {
                            int usuarioId = Convert.ToInt32(hdnUsuarioId.Value);
                            usuario = context.Usuarios.Find(usuarioId);
                            if (usuario == null)
                            {
                                MostrarError("Error: No se encontró el usuario a editar.");
                                return;
                            }

                            // Verificar que el email no exista (excepto para el mismo usuario)
                            if (context.Usuarios.Any(u => u.Email.ToLower() == txtEmail.Text.Trim().ToLower() && u.UsuarioId != usuarioId))
                            {
                                MostrarError("El correo electrónico ya está registrado por otro usuario.");
                                return;
                            }
                        }

                        // Si los campos están habilitados, actualizar la información del usuario
                        if (txtNombre.Enabled)
                        {
                            usuario.Nombre = txtNombre.Text.Trim();
                            usuario.Email = txtEmail.Text.Trim();
                            usuario.Rol = ddlRol.SelectedValue;
                            usuario.AreaId = ucDepartamento.SelectedDepartamentoId;
                        }

                        // Si hay contraseña nueva, actualizarla
                        if (!string.IsNullOrEmpty(txtPassword.Text))
                        {
                            usuario.Password = HashPassword(txtPassword.Text);
                        }

                        context.SaveChanges();

                        // Cerrar modal y mostrar mensaje de éxito
                        string mensaje = esNuevo ? "Usuario creado exitosamente." : "Usuario actualizado exitosamente.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "hideModalScript",
                            $"hideModal(); toastr.success('{mensaje}');", true);

                        // Recargar la grilla
                        CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    lblErrorModal.Text = "Error al guardar el usuario: " + ex.Message;
                    lblErrorModal.Visible = true;
                }
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                CargarUsuarioParaEditar(usuarioId);
            }
            else if (e.CommandName == "VerPerfil")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Usuarios/PerfilUsuario.aspx?id={usuarioId}");
            }
            else if (e.CommandName == "CambiarPassword")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                CambiarPasswordUsuario(usuarioId);
            }
            else if (e.CommandName == "Desactivar")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                CambiarEstadoUsuario(usuarioId, false);
            }
            else if (e.CommandName == "Activar")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                CambiarEstadoUsuario(usuarioId, true);
            }
            else if (e.CommandName == "Eliminar")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                EliminarUsuario(usuarioId);
            }
        }

        private void CargarUsuarioParaEditar(int usuarioId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var usuario = context.Usuarios
                        .Include(u => u.Area)
                        .FirstOrDefault(u => u.UsuarioId == usuarioId);

                    if (usuario != null)
                    {
                        hdnUsuarioId.Value = usuario.UsuarioId.ToString();
                        txtNombre.Text = usuario.Nombre;
                        txtEmail.Text = usuario.Email;
                        ddlRol.SelectedValue = usuario.Rol;
                        ucDepartamento.SelectedDepartamentoId = usuario.AreaId;
                        
                        divPassword.Visible = false;
                        litTituloModal.Text = "Editar Usuario";

                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar el usuario para editar: " + ex.Message);
            }
        }

        private void CambiarPasswordUsuario(int usuarioId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var usuario = context.Usuarios.Find(usuarioId);
                    if (usuario != null)
                    {
                        hdnUsuarioId.Value = usuario.UsuarioId.ToString();
                        litTituloModal.Text = $"Cambiar Contraseña - {usuario.Nombre}";
                        
                        // Mostrar solo la sección de contraseña
                        divPassword.Visible = true;
                        txtNombre.Enabled = false;
                        txtEmail.Enabled = false;
                        ddlRol.Enabled = false;
                        ucDepartamento.Enabled = false;

                        // Limpiar campos de contraseña
                        txtPassword.Text = string.Empty;
                        txtConfirmPassword.Text = string.Empty;

                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar el usuario para cambiar contraseña: " + ex.Message);
            }
        }

        private void CambiarEstadoUsuario(int usuarioId, bool activar)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var usuario = context.Usuarios.Find(usuarioId);
                    if (usuario != null)
                    {
                        // No permitir desactivar al administrador principal
                        if (usuario.Email == "admin@isomanager.com" && !activar)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                                "toastr.error('No se puede desactivar al administrador principal.');", true);
                            return;
                        }

                        usuario.Estado = activar ? "Activo" : "Inactivo";
                        context.SaveChanges();
                        CargarUsuarios();
                        
                        string mensaje = activar ? "Usuario activado exitosamente." : "Usuario desactivado exitosamente.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            $"toastr.success('{mensaje}');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al {(activar ? "activar" : "desactivar")} el usuario: " + ex.Message);
            }
        }

        private void EliminarUsuario(int usuarioId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var usuario = context.Usuarios.Find(usuarioId);
                    if (usuario != null)
                    {
                        // No permitir eliminar al administrador principal
                        if (usuario.Email == "admin@isomanager.com")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                                "toastr.error('No se puede eliminar al administrador principal.');", true);
                            return;
                        }

                        // Verificar si el usuario tiene referencias
                        var tieneNormas = context.Normas.Any(n => n.ResponsableId == usuarioId);
                        var tieneProcesos = context.Procesos.Any(p => p.ResponsableId == usuarioId || p.AdministradorId == usuarioId);

                        if (tieneNormas || tieneProcesos)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                                "toastr.error('No se puede eliminar el usuario porque tiene normas o procesos asociados. Considere desactivarlo en su lugar.');", true);
                            return;
                        }

                        context.Usuarios.Remove(usuario);
                        context.SaveChanges();
                        CargarUsuarios();
                        
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "toastr.success('Usuario eliminado exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al eliminar el usuario: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            lblErrorModal.Text = mensaje;
            lblErrorModal.Visible = true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        protected string GetEstadoBadgeClass(bool estado)
        {
            return estado ? "bg-success" : "bg-danger";
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        protected string GetAvatarColorClass(string name)
        {
            if (string.IsNullOrEmpty(name)) return "default";
            
            int hash = name.GetHashCode();
            int colorIndex = Math.Abs(hash % 6);
            
            switch (colorIndex)
            {
                case 0: return "blue";
                case 1: return "green";
                case 2: return "purple";
                case 3: return "orange";
                case 4: return "red";
                case 5: return "teal";
                default: return "default";
            }
        }

        protected string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name)) return "??";
            
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
            }
            return name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
        }
    }
} 