using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.Entity;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Usuarios
{
    public partial class PerfilUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosUsuario();
                CargarCalificaciones();
                CargarTareas();
                CargarMensajes();
            }
        }

        private void CargarDatosUsuario()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                    using (var context = new IsomanagerContext())
                    {
                        var usuario = context.Usuarios
                            .Include(u => u.Area)
                            .FirstOrDefault(u => u.UsuarioId == usuarioId);

                        if (usuario != null)
                        {
                            litNombreUsuario.Text = usuario.Nombre;
                            litEmail.Text = usuario.Email;
                            litArea.Text = (usuario.Area != null) ? usuario.Area.Nombre : "Sin área";
                            litRol.Text = usuario.Rol;
                            litFechaRegistro.Text = usuario.FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss");
                            litEstado.Text = usuario.Estado;

                            // Cargar procesos del usuario
                            var procesos = context.Procesos
                                .Include(p => p.Area)
                                .Where(p => p.ResponsableId == usuarioId || p.AdministradorId == usuarioId)
                                .ToList()
                                .Select(p => new
                                {
                                    p.ProcesoId,
                                    p.Nombre,
                                    p.Estado,
                                    Area = p.Area?.Nombre ?? "Sin área",
                                    Rol = p.ResponsableId == usuarioId ? "Responsable" : "Administrador"
                                });

                            gvProcesos.DataSource = procesos;
                            gvProcesos.DataBind();

                            // Inicializar las estadísticas
                            litRendimiento.Text = "85";
                            litValoracion.Text = "4.5";
                            litProyectos.Text = "12";
                            litCertificaciones.Text = "3";
                            litFormacion.Text = "120";

                            // Configurar el avatar
                            string avatarClass = GetAvatarColorClass(usuario.Nombre);
                            avatarCircle.Attributes["class"] = $"avatar-circle avatar-lg {avatarClass}";
                            litInitials.Text = GetInitials(usuario.Nombre);
                        }
                        else
                        {
                            MostrarError("No se encontró el usuario especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar los datos del usuario: " + ex.Message);
            }
        }

        private void CargarCalificaciones()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                using (var context = new IsomanagerContext())
                {
                    var calificaciones = context.Calificaciones
                        .Where(c => c.UsuarioId == usuarioId)
                        .OrderByDescending(c => c.Fecha)
                        .Select(c => new
                        {
                            c.Fecha,
                            Evaluador = c.Evaluador.Nombre,
                            c.Categoria,
                            Calificacion = c.Valor,
                            c.Comentarios
                        })
                        .ToList();

                    gvCalificaciones.DataSource = calificaciones;
                    gvCalificaciones.DataBind();
                }
            }
        }

        private void CargarTareas()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                using (var context = new IsomanagerContext())
                {
                    var tareas = context.Tareas
                        .Where(t => t.UsuarioAsignadoId == usuarioId)
                        .OrderByDescending(t => t.FechaCreacion)
                        .Select(t => new
                        {
                            t.Titulo,
                            t.FechaCreacion,
                            t.FechaVencimiento,
                            t.Estado,
                            t.Prioridad,
                            AsignadoPor = t.UsuarioCreador.Nombre
                        })
                        .ToList();

                    gvTareas.DataSource = tareas;
                    gvTareas.DataBind();
                }
            }
        }

        private void CargarMensajes()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                using (var context = new IsomanagerContext())
                {
                    var mensajes = context.Mensajes
                        .Where(m => m.DestinatarioId == usuarioId || m.RemitenteId == usuarioId)
                        .OrderByDescending(m => m.FechaEnvio)
                        .Select(m => new
                        {
                            FromUser = m.Remitente.Nombre,
                            m.FechaEnvio,
                            m.Contenido,
                            IsOutgoing = m.RemitenteId == usuarioId
                        })
                        .ToList();

                    rptMensajes.DataSource = mensajes;
                    rptMensajes.DataBind();
                }
            }
        }

        protected void btnGuardarCalificacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                    using (var context = new IsomanagerContext())
                    {
                        var calificacion = new Calificacion
                        {
                            UsuarioId = usuarioId,
                            EvaluadorId = Convert.ToInt32(Session["UsuarioId"]),
                            Categoria = ddlCategoria.SelectedValue,
                            Valor = Convert.ToInt32(ddlCalificacion.SelectedValue),
                            Comentarios = txtComentarios.Text,
                            Fecha = DateTime.Now
                        };

                        context.Calificaciones.Add(calificacion);
                        context.SaveChanges();

                        CargarCalificaciones();
                        MostrarExito("Calificación guardada exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar la calificación: " + ex.Message);
            }
        }

        protected void btnGuardarTarea_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                    using (var context = new IsomanagerContext())
                    {
                        var tarea = new Tarea
                        {
                            Titulo = txtTituloTarea.Text,
                            Descripcion = txtDescripcionTarea.Text,
                            FechaCreacion = DateTime.Now,
                            FechaVencimiento = Convert.ToDateTime(txtFechaVencimiento.Text),
                            Estado = "Pendiente",
                            Prioridad = ddlPrioridad.SelectedValue,
                            UsuarioAsignadoId = usuarioId,
                            UsuarioCreadorId = Convert.ToInt32(Session["UsuarioId"])
                        };

                        context.Tareas.Add(tarea);
                        context.SaveChanges();

                        CargarTareas();
                        MostrarExito("Tarea asignada exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al asignar la tarea: " + ex.Message);
            }
        }

        protected void btnEnviarMensaje_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                    using (var context = new IsomanagerContext())
                    {
                        var mensaje = new Mensaje
                        {
                            Asunto = txtAsunto.Text,
                            Contenido = txtMensaje.Text,
                            FechaEnvio = DateTime.Now,
                            RemitenteId = Convert.ToInt32(Session["UsuarioId"]),
                            DestinatarioId = usuarioId,
                            Estado = "No leído"
                        };

                        context.Mensajes.Add(mensaje);
                        context.SaveChanges();

                        CargarMensajes();
                        MostrarExito("Mensaje enviado exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al enviar el mensaje: " + ex.Message);
            }
        }

        protected string GetStarRating(int rating)
        {
            string stars = string.Empty;
            for (int i = 0; i < rating; i++)
            {
                stars += "<i class='bi bi-star-fill'></i>";
            }
            for (int i = rating; i < 5; i++)
            {
                stars += "<i class='bi bi-star'></i>";
            }
            return stars;
        }

        protected string GetTareaStatusBadgeClass(string estado)
        {
            switch (estado?.ToLower())
            {
                case "completada":
                    return "bg-success";
                case "en proceso":
                    return "bg-primary";
                case "pendiente":
                    return "bg-warning";
                case "retrasada":
                    return "bg-danger";
                default:
                    return "bg-secondary";
            }
        }

        protected string GetTareaPrioridadBadgeClass(string prioridad)
        {
            switch (prioridad?.ToLower())
            {
                case "alta":
                    return "bg-danger";
                case "media":
                    return "bg-warning";
                case "baja":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        protected string GetFormacionBadgeClass(string estado)
        {
            switch (estado?.ToLower())
            {
                case "completado":
                    return "bg-success";
                case "en curso":
                    return "bg-primary";
                case "pendiente":
                    return "bg-warning";
                default:
                    return "bg-secondary";
            }
        }

        protected string GetProcesoBadgeClass(string estado)
        {
            switch (estado?.ToLower())
            {
                case "completado":
                    return "bg-success";
                case "en proceso":
                    return "bg-primary";
                case "pendiente":
                    return "bg-warning";
                case "retrasado":
                    return "bg-danger";
                default:
                    return "bg-secondary";
            }
        }

        private void MostrarError(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"toastr.error('{mensaje}');", true);
        }

        private void MostrarExito(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "success",
                $"toastr.success('{mensaje}');", true);
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

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(Request.QueryString["id"], out int usuarioId))
            {
                Response.Redirect($"~/Pages/Usuarios/GestionUsuarios.aspx?edit={usuarioId}");
            }
        }
    }
} 