using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Normas.Secciones
{
    public partial class RecursosHumanos : System.Web.UI.Page
    {
        private int normaId;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(Request.QueryString["normaId"], out normaId))
                {
                    Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    CargarDatosNorma();
                    CargarRoles();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar la página: " + ex.Message, "danger");
            }
        }

        protected void btnGuardarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ucUsuarioSelectorRol.SelectedUsuarioId.HasValue)
                {
                    MostrarMensaje("Debe seleccionar un usuario", "warning");
                    return;
                }

                using (var db = new IsomanagerContext())
                {
                    var usuario = db.Usuarios.Find(ucUsuarioSelectorRol.SelectedUsuarioId.Value);
                    if (usuario != null)
                    {
                        usuario.Cargo = txtCargo.Text.Trim();
                        usuario.Responsabilidades = txtResponsabilidadesNuevo.Text.Trim();
                        db.SaveChanges();

                        LimpiarFormularioRol();
                        CargarRoles();
                        MostrarMensaje("Rol actualizado exitosamente", "success");
                        
                        // Actualizar los UpdatePanels
                        upRoles.Update();
                        upPrincipal.Update();
                    }
                    else
                    {
                        MostrarMensaje("Usuario no encontrado", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, "danger");
            }
        }

        protected void btnLimpiarRol_Click(object sender, EventArgs e)
        {
            LimpiarFormularioRol();
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarRol")
                {
                    int rolId = Convert.ToInt32(e.CommandArgument);
                    CargarRolParaEditar(rolId);
                }
                else if (e.CommandName == "EliminarRol")
                {
                    int rolId = Convert.ToInt32(e.CommandArgument);
                    EliminarRol(rolId);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al procesar la acción: " + ex.Message, "danger");
            }
        }

        private void CargarRolParaEditar(int usuarioId)
        {
            using (var db = new IsomanagerContext())
            {
                var usuario = db.Usuarios.Find(usuarioId);
                if (usuario != null)
                {
                    ucUsuarioSelectorRol.SelectedUsuarioId = usuario.UsuarioId;
                    txtCargo.Text = usuario.Cargo;
                    txtResponsabilidadesNuevo.Text = usuario.Responsabilidades ?? "";
                    ViewState["RolId"] = usuarioId;
                }
            }
        }

        private void EliminarRol(int usuarioId)
        {
            using (var db = new IsomanagerContext())
            {
                var usuario = db.Usuarios.Find(usuarioId);
                if (usuario != null)
                {
                    usuario.Cargo = "Sin asignar";
                    db.SaveChanges();
                    CargarRoles();
                    MostrarMensaje("Cargo eliminado exitosamente", "success");
                    
                    // Actualizar los UpdatePanels
                    upRoles.Update();
                    upPrincipal.Update();
                }
            }
        }

        private void LimpiarFormularioRol()
        {
            ucUsuarioSelectorRol.SelectedUsuarioId = null;
            txtCargo.Text = string.Empty;
            txtResponsabilidadesNuevo.Text = string.Empty;
            ViewState["RolId"] = null;
        }

        private void CargarRoles()
        {
            using (var db = new IsomanagerContext())
            {
                var usuarios = db.Usuarios
                    .Include(u => u.Area)
                    .Where(u => u.Estado == "Activo")
                    .Select(u => new
                    {
                        RolId = u.UsuarioId,
                        NombreUsuario = u.Nombre,
                        Cargo = u.Cargo,
                        Area = u.Area.Nombre,
                        Responsabilidad = u.Responsabilidades ?? ""
                    })
                    .ToList();

                gvRoles.DataSource = usuarios;
                gvRoles.DataBind();
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                using (var db = new IsomanagerContext())
                {
                    var norma = db.Normas.Find(normaId);
                    if (norma != null)
                    {
                        litTituloNorma.Text = norma.Titulo;
                        litDetallesNorma.Text = $"Versión {norma.Version} - {norma.FechaCreacion:dd/MM/yyyy}";
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar datos de la norma: " + ex.Message, "danger");
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            litMensaje.Text = mensaje;
            pnlMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show";
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Normas/GestionNormas.aspx");
        }

        protected void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new IsomanagerContext())
                {
                    var usuario = new Usuario
                    {
                        Nombre = txtNombreUsuario.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Password = HashPassword(txtPassword.Text),
                        Rol = ddlRol.SelectedValue,
                        AreaId = ucDepartamento.SelectedDepartamentoId.Value,
                        Estado = "Activo",
                        FechaRegistro = DateTime.Now,
                        UltimaConexion = DateTime.Now,
                        ContadorIntentos = 0,
                        Cargo = "Sin asignar",
                        Responsabilidades = "Pendiente de asignar"
                    };

                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    // Actualizar el selector de usuarios
                    ucUsuarioSelectorRol.SelectedUsuarioId = usuario.UsuarioId;
                    
                    // Actualizar la lista de roles
                    CargarRoles();
                    
                    MostrarMensaje("Usuario creado exitosamente", "success");

                    // Actualizar los UpdatePanels
                    upRoles.Update();
                    upPrincipal.Update();

                    // Cerrar el modal y mostrar mensaje de éxito
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModalScript",
                        "hideModal(); showSuccessMessage('Usuario creado exitosamente');", true);
                }
            }
            catch (Exception ex)
            {
                lblErrorModal.Text = "Error al crear usuario: " + ex.Message;
                lblErrorModal.Visible = true;
            }
        }

        private string HashPassword(string password)
        {
            // TODO: Implementar un método seguro de hash de contraseñas
            return password;
        }

        protected void gvCapacitaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Formatear la fecha
                if (e.Row.Cells[1].Text != null)
                {
                    DateTime fecha;
                    if (DateTime.TryParse(e.Row.Cells[1].Text, out fecha))
                    {
                        e.Row.Cells[1].Text = fecha.ToString("dd/MM/yyyy");
                    }
                }

                // Formatear la duración
                if (e.Row.Cells[3].Text != null)
                {
                    e.Row.Cells[3].Text = $"{e.Row.Cells[3].Text} horas";
                }
            }
        }

        protected void gvCompetencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Formatear el nivel requerido
                if (e.Row.Cells[1].Text != null)
                {
                    int nivel;
                    if (int.TryParse(e.Row.Cells[1].Text, out nivel))
                    {
                        string nivelTexto;
                        switch (nivel)
                        {
                            case 1:
                                nivelTexto = "Básico";
                                break;
                            case 2:
                                nivelTexto = "Intermedio";
                                break;
                            case 3:
                                nivelTexto = "Avanzado";
                                break;
                            case 4:
                                nivelTexto = "Experto";
                                break;
                            default:
                                nivelTexto = "No definido";
                                break;
                        }
                        e.Row.Cells[1].Text = nivelTexto;
                    }
                }
            }
        }

        protected void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Aplicar formato condicional basado en el cargo
                string cargo = e.Row.Cells[1].Text;
                if (!string.IsNullOrEmpty(cargo))
                {
                    if (cargo.ToLower().Contains("responsable") || cargo.ToLower().Contains("jefe"))
                    {
                        e.Row.CssClass += " table-primary";
                    }
                    else if (cargo.ToLower().Contains("auditor"))
                    {
                        e.Row.CssClass += " table-info";
                    }
                }
            }
        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar campos del modal
                txtNombreUsuario.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPassword.Text = string.Empty;
                ddlRol.SelectedIndex = 0;
                ucDepartamento.SelectedDepartamentoId = null;
                lblErrorModal.Visible = false;

                // Mostrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                    "setTimeout(function() { showModal(); }, 100);", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al preparar nuevo usuario: " + ex.Message, "danger");
            }
        }

        protected void btnGuardarCompetencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ucUsuarioSelectorCompetencia.SelectedUsuarioId.HasValue)
                {
                    MostrarMensaje("Debe seleccionar un usuario", "warning");
                    return;
                }

                using (var db = new IsomanagerContext())
                {
                    var competencia = new CompetenciaNorma
                    {
                        NormaId = normaId,
                        UsuarioId = ucUsuarioSelectorCompetencia.SelectedUsuarioId.Value,
                        Nombre = txtCompetencia.Text.Trim(),
                        NivelRequerido = Convert.ToInt32(ddlNivel.SelectedValue),
                        FechaCreacion = DateTime.Now,
                        UltimaActualizacion = DateTime.Now
                    };

                    db.CompetenciasNorma.Add(competencia);
                    db.SaveChanges();

                    CargarCompetencias();
                    MostrarMensaje("Competencia guardada exitosamente", "success");

                    // Cerrar el modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModalCompetencia",
                        "hideCompetenciaModal(); showSuccessMessage('Competencia guardada exitosamente');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar competencia: " + ex.Message, "danger");
            }
        }

        protected void btnGuardarCapacitacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }

                if (!ucUsuarioSelectorCapacitacion.SelectedUsuarioId.HasValue)
                {
                    MostrarMensaje("Debe seleccionar un usuario", "warning");
                    return;
                }

                using (var db = new IsomanagerContext())
                {
                    var capacitacion = new Formacion
                    {
                        UsuarioId = ucUsuarioSelectorCapacitacion.SelectedUsuarioId.Value,
                        NormaId = normaId,
                        Nombre = txtNombreCapacitacion.Text.Trim(),
                        FechaObtencion = DateTime.Parse(txtFechaCapacitacion.Text),
                        Duracion = int.Parse(txtDuracion.Text),
                        Estado = "Activo",
                        TipoFormacion = "Capacitación",
                        Descripcion = "Capacitación para la norma"
                    };

                    db.Formaciones.Add(capacitacion);
                    db.SaveChanges();

                    CargarCapacitaciones();
                    MostrarMensaje("Capacitación guardada exitosamente", "success");

                    // Cerrar el modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModalCapacitacion",
                        "hideCapacitacionModal(); showSuccessMessage('Capacitación guardada exitosamente');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar capacitación: " + ex.Message, "danger");
            }
        }

        protected void btnNuevaCompetencia_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar campos del modal
                ucUsuarioSelectorCompetencia.SelectedUsuarioId = null;
                txtCompetencia.Text = string.Empty;
                ddlNivel.SelectedIndex = 0;

                // Mostrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalCompetencia",
                    "setTimeout(function() { showCompetenciaModal(); }, 100);", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al preparar nueva competencia: " + ex.Message, "danger");
            }
        }

        protected void btnNuevaCapacitacion_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar campos del modal
                ucUsuarioSelectorCapacitacion.SelectedUsuarioId = null;
                txtNombreCapacitacion.Text = string.Empty;
                txtDuracion.Text = string.Empty;
                txtFechaCapacitacion.Text = string.Empty;

                // Mostrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalCapacitacion",
                    "setTimeout(function() { showCapacitacionModal(); }, 100);", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al preparar nueva capacitación: " + ex.Message, "danger");
            }
        }

        protected void gvCompetencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarCompetencia")
                {
                    int competenciaId = Convert.ToInt32(e.CommandArgument);
                    using (var db = new IsomanagerContext())
                    {
                        var competencia = db.CompetenciasNorma.Find(competenciaId);
                        if (competencia != null)
                        {
                            ucUsuarioSelectorCompetencia.SelectedUsuarioId = competencia.UsuarioId;
                            txtCompetencia.Text = competencia.Nombre;
                            ddlNivel.SelectedValue = competencia.NivelRequerido.ToString();
                            ViewState["CompetenciaId"] = competenciaId;

                            // Mostrar el modal
                            ScriptManager.RegisterStartupScript(this, GetType(), "showModalCompetencia",
                                "setTimeout(function() { showCompetenciaModal(); }, 100);", true);
                        }
                    }
                }
                else if (e.CommandName == "EliminarCompetencia")
                {
                    int competenciaId = Convert.ToInt32(e.CommandArgument);
                    EliminarCompetencia(competenciaId);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al procesar la acción: " + ex.Message, "danger");
            }
        }

        protected void gvCapacitaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarCapacitacion")
                {
                    int capacitacionId = Convert.ToInt32(e.CommandArgument);
                    using (var db = new IsomanagerContext())
                    {
                        var capacitacion = db.Formaciones.Find(capacitacionId);
                        if (capacitacion != null)
                        {
                            ucUsuarioSelectorCapacitacion.SelectedUsuarioId = capacitacion.UsuarioId;
                            txtNombreCapacitacion.Text = capacitacion.Nombre;
                            txtFechaCapacitacion.Text = capacitacion.FechaObtencion.ToString("yyyy-MM-dd");
                            txtDuracion.Text = capacitacion.Duracion.ToString();
                            ViewState["CapacitacionId"] = capacitacionId;

                            // Mostrar el modal
                            ScriptManager.RegisterStartupScript(this, GetType(), "showModalCapacitacion",
                                "setTimeout(function() { showCapacitacionModal(); }, 100);", true);
                        }
                    }
                }
                else if (e.CommandName == "EliminarCapacitacion")
                {
                    int capacitacionId = Convert.ToInt32(e.CommandArgument);
                    using (var db = new IsomanagerContext())
                    {
                        var capacitacion = db.Formaciones.Find(capacitacionId);
                        if (capacitacion != null)
                        {
                            capacitacion.Estado = "Inactivo";
                            db.SaveChanges();
                            CargarCapacitaciones();
                            MostrarMensaje("Capacitación eliminada exitosamente", "success");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al procesar la acción: " + ex.Message, "danger");
            }
        }

        private void CargarCompetencia(int competenciaId)
        {
            using (var db = new IsomanagerContext())
            {
                var competencia = db.CompetenciasNorma.Find(competenciaId);
                if (competencia != null)
                {
                    ucUsuarioSelectorCompetencia.SelectedUsuarioId = competencia.UsuarioId;
                    txtCompetencia.Text = competencia.Nombre;
                    ddlNivel.SelectedValue = competencia.NivelRequerido.ToString();
                    ViewState["CompetenciaId"] = competenciaId;

                    // Mostrar el modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "showModalCompetencia",
                        "handleModal('modalCompetencia', 'show');", true);
                }
            }
        }

        private void EliminarCompetencia(int competenciaId)
        {
            using (var db = new IsomanagerContext())
            {
                var competencia = db.CompetenciasNorma.Find(competenciaId);
                if (competencia != null)
                {
                    db.CompetenciasNorma.Remove(competencia);
                    db.SaveChanges();
                    CargarCompetencias();
                    MostrarMensaje("Competencia eliminada exitosamente", "success");
                }
            }
        }

        private void CargarCompetencias()
        {
            using (var db = new IsomanagerContext())
            {
                var competencias = db.CompetenciasNorma
                    .Include(c => c.Usuario)
                    .Where(c => c.NormaId == normaId)
                    .Select(c => new
                    {
                        CompetenciaId = c.CompetenciaId,
                        Nombre = c.Nombre,
                        NivelRequerido = c.NivelRequerido,
                        PersonalAsignado = c.Usuario.Nombre
                    })
                    .ToList();

                gvCompetencias.DataSource = competencias;
                gvCompetencias.DataBind();
            }
        }

        private void CargarCapacitaciones()
        {
            using (var db = new IsomanagerContext())
            {
                var capacitaciones = db.Formaciones
                    .Include(c => c.Usuario)
                    .Where(c => c.TipoFormacion == "Capacitación" && c.Estado == "Activo" && c.NormaId == normaId)
                    .Select(c => new
                    {
                        CapacitacionId = c.FormacionId,
                        Nombre = c.Nombre,
                        Fecha = c.FechaObtencion,
                        Participantes = c.Usuario.Nombre,
                        Duracion = c.Duracion
                    })
                    .ToList();

                gvCapacitaciones.DataSource = capacitaciones;
                gvCapacitaciones.DataBind();
            }
        }

        protected void btnDescargarReporte_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Implementar la generación y descarga del reporte
                MostrarMensaje("Funcionalidad de reporte en desarrollo", "info");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al generar reporte: " + ex.Message, "danger");
            }
        }
    }
} 