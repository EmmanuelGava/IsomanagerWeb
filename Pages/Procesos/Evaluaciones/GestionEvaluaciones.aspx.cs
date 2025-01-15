using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Procesos.Evaluaciones
{
    public partial class GestionEvaluaciones : Page
    {
        private int NormaId
        {
            get
            {
                if (ViewState["NormaId"] == null)
                {
                    string normaIdStr = Request.QueryString["NormaId"];
                    if (string.IsNullOrEmpty(normaIdStr))
                    {
                        Response.Redirect("~/Default.aspx");
                        return 0;
                    }

                    if (!int.TryParse(normaIdStr, out int normaId))
                    {
                        Response.Redirect("~/Default.aspx");
                        return 0;
                    }

                    using (var context = new IsomanagerContext())
                    {
                        if (!context.Normas.Any(n => n.NormaId == normaId))
                        {
                            Response.Redirect("~/Default.aspx");
                            return 0;
                        }
                    }

                    ViewState["NormaId"] = normaId;
                }
                return (int)ViewState["NormaId"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosNorma();
                CargarContadores();
                CargarEvaluaciones();
                CargarProcesos();

                // Configurar el control de usuario
                ucUsuarioAsignado.Required = true;
                ucUsuarioAsignado.ValidationGroup = "Evaluacion";
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Normas.FirstOrDefault(n => n.NormaId == NormaId);
                    if (norma == null)
                    {
                        Response.Redirect("~/Default.aspx");
                        return;
                    }
                    litTituloPagina.Text = "Gestión de Evaluaciones";
                    litNormaTitulo.Text = norma.Titulo;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los datos de la norma: {ex.Message}");
            }
        }

        private Usuario UsuarioActual
        {
            get
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return null;
                }
                return (Usuario)Session["Usuario"];
            }
        }

        private void CargarContadores()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Iniciando CargarContadores ===");
                
                if (litAprobadas == null || litPendientes == null || litRechazadas == null || litEnRevision == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Uno o más controles Literal son null");
                    return;
                }

                using (var context = new IsomanagerContext())
                {
                    if (context == null)
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Contexto es null");
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine($"NormaId: {NormaId}");

                    var query = context.EvaluacionProcesos
                        .Include(e => e.Proceso)
                        .Where(e => e.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    System.Diagnostics.Debug.WriteLine("Contando registros por estado...");

                    litAprobadas.Text = query.Count(e => e.Estado != null && e.Estado.ToLower() == "aprobada").ToString();
                    litPendientes.Text = query.Count(e => e.Estado != null && e.Estado.ToLower() == "pendiente").ToString();
                    litRechazadas.Text = query.Count(e => e.Estado != null && e.Estado.ToLower() == "rechazada").ToString();
                    litEnRevision.Text = query.Count(e => e.Estado != null && e.Estado.ToLower() == "en revisión").ToString();

                    System.Diagnostics.Debug.WriteLine("=== CargarContadores completado ===");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en CargarContadores: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar los contadores: {ex.Message}");
            }
        }

        private void CargarEvaluaciones()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var query = context.EvaluacionProcesos
                        .Include(e => e.Proceso)
                        .Include(e => e.Creador)
                        .Include(e => e.Asignado)
                        .Where(e => e.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLowerInvariant();
                        query = query.Where(e =>
                            e.Proceso.Nombre.ToLowerInvariant().Contains(filtro) ||
                            e.Descripcion.ToLowerInvariant().Contains(filtro) ||
                            e.Creador.Nombre.ToLowerInvariant().Contains(filtro) ||
                            e.Asignado.Nombre.ToLowerInvariant().Contains(filtro) ||
                            e.Estado.ToLowerInvariant().Contains(filtro));
                    }

                    var evaluacionesList = query
                        .OrderByDescending(e => e.FechaCreacion)
                        .Select(e => new
                        {
                            e.EvaluacionId,
                            e.ProcesoId,
                            Proceso = e.Proceso.Nombre,
                            e.Descripcion,
                            FechaEvaluacion = e.FechaEvaluacion,
                            Evaluador = e.Asignado.Nombre,
                            e.AsignadoId,
                            e.Estado,
                            e.Calificacion,
                            e.Observaciones,
                            e.Comentarios,
                            e.Recomendaciones
                        })
                        .ToList();

                    gvEvaluaciones.DataSource = evaluacionesList;
                    gvEvaluaciones.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las evaluaciones: {ex.Message}");
            }
        }

        private void CargarProcesos()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var procesos = context.Procesos
                        .Where(p => p.NormaId == NormaId && p.Activo)
                        .OrderBy(p => p.Nombre)
                        .Select(p => new
                        {
                            p.ProcesoId,
                            p.Nombre
                        })
                        .ToList();

                    ddlProceso.DataSource = procesos;
                    ddlProceso.DataTextField = "Nombre";
                    ddlProceso.DataValueField = "ProcesoId";
                    ddlProceso.DataBind();

                    ddlProceso.Items.Insert(0, new ListItem("Seleccione...", ""));
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los procesos: {ex.Message}");
            }
        }

        protected void btnGuardarEvaluacion_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || UsuarioActual == null)
                return;

            try
            {
                using (var context = new IsomanagerContext())
                {
                    if (!int.TryParse(ddlProceso.SelectedValue, out int procesoId))
                    {
                        MostrarErrorModal("Debe seleccionar un proceso.");
                        return;
                    }

                    if (ucUsuarioAsignado.SelectedUsuarioId == null)
                    {
                        MostrarErrorModal("Debe seleccionar un usuario asignado.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                    {
                        MostrarErrorModal("La descripción es requerida.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ddlEstado.SelectedValue))
                    {
                        MostrarErrorModal("Debe seleccionar un estado.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtFechaEvaluacion.Text))
                    {
                        MostrarErrorModal("La fecha de evaluación es requerida.");
                        return;
                    }

                    if (!int.TryParse(ddlCalificacion.SelectedValue, out int calificacion))
                    {
                        MostrarErrorModal("Debe seleccionar una calificación válida.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtComentarios.Text))
                    {
                        MostrarErrorModal("Los comentarios son requeridos.");
                        return;
                    }

                    bool esNueva = string.IsNullOrEmpty(hdnEvaluacionId.Value);
                    EvaluacionProcesos evaluacion;

                    if (esNueva)
                    {
                        evaluacion = new EvaluacionProcesos
                        {
                            ProcesoId = procesoId,
                            CreadorId = UsuarioActual.UsuarioId,
                            AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? UsuarioActual.UsuarioId,
                            FechaCreacion = DateTime.Now,
                            UltimaModificacion = DateTime.Now,
                            Activo = true
                        };
                        context.EvaluacionProcesos.Add(evaluacion);
                    }
                    else
                    {
                        int evaluacionId = Convert.ToInt32(hdnEvaluacionId.Value);
                        evaluacion = context.EvaluacionProcesos.Find(evaluacionId);
                        if (evaluacion == null)
                        {
                            MostrarErrorModal("No se encontró la evaluación a editar.");
                            return;
                        }
                        evaluacion.UltimaModificacion = DateTime.Now;
                        evaluacion.ProcesoId = procesoId;
                        evaluacion.AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? evaluacion.AsignadoId;
                    }

                    // Actualizar propiedades
                    evaluacion.Descripcion = txtDescripcion.Text.Trim();
                    evaluacion.Estado = ddlEstado.SelectedValue;
                    evaluacion.FechaEvaluacion = DateTime.Parse(txtFechaEvaluacion.Text);
                    evaluacion.Calificacion = calificacion;
                    evaluacion.Observaciones = txtObservaciones.Text?.Trim();
                    evaluacion.Comentarios = txtComentarios.Text.Trim();
                    evaluacion.Recomendaciones = txtRecomendaciones.Text?.Trim();

                    context.SaveChanges();

                    CargarEvaluaciones();
                    CargarContadores();
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                        "hideModal();", true);

                    string mensaje = esNueva ? "Evaluación creada exitosamente." : "Evaluación actualizada exitosamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                        $"showSuccessMessage('{mensaje}');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarErrorModal($"Error al guardar la evaluación: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al guardar la evaluación: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        protected void gvEvaluaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int evaluacionId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "Editar":
                        CargarEvaluacionParaEditar(evaluacionId);
                        break;

                    case "Ver":
                        // Implementar vista detallada si es necesario
                        break;

                    case "Eliminar":
                        EliminarEvaluacion(evaluacionId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarEvaluacionParaEditar(int evaluacionId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var evaluacion = context.EvaluacionProcesos
                        .Include(e => e.Proceso)
                        .FirstOrDefault(e => e.EvaluacionId == evaluacionId);

                    if (evaluacion != null)
                    {
                        LimpiarModal();
                        
                        hdnEvaluacionId.Value = evaluacion.EvaluacionId.ToString();
                        
                        // Seleccionar proceso
                        ddlProceso.SelectedIndex = -1; // Deseleccionar todo primero
                        ddlProceso.SelectedValue = evaluacion.ProcesoId.ToString();

                        txtDescripcion.Text = evaluacion.Descripcion;
                        
                        // Seleccionar estado
                        ddlEstado.SelectedIndex = -1; // Deseleccionar todo primero
                        ddlEstado.SelectedValue = evaluacion.Estado;

                        txtFechaEvaluacion.Text = evaluacion.FechaEvaluacion.ToString("yyyy-MM-dd");
                        
                        // Seleccionar calificación
                        ddlCalificacion.SelectedIndex = -1; // Deseleccionar todo primero
                        ddlCalificacion.SelectedValue = evaluacion.Calificacion.ToString();

                        txtObservaciones.Text = evaluacion.Observaciones;
                        txtComentarios.Text = evaluacion.Comentarios;
                        txtRecomendaciones.Text = evaluacion.Recomendaciones;
                        ucUsuarioAsignado.SelectedUsuarioId = evaluacion.AsignadoId;
                        
                        litTituloModal.Text = "Editar Evaluación";

                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la evaluación para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al cargar la evaluación para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void EliminarEvaluacion(int evaluacionId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var evaluacion = context.EvaluacionProcesos.Find(evaluacionId);
                    if (evaluacion != null)
                    {
                        context.EvaluacionProcesos.Remove(evaluacion);
                        context.SaveChanges();
                        CargarEvaluaciones();
                        CargarContadores();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "showSuccessMessage('Evaluación eliminada exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar la evaluación: {ex.Message}");
            }
        }

        private void LimpiarModal()
        {
            hdnEvaluacionId.Value = string.Empty;
            ddlProceso.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            txtFechaEvaluacion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            ddlCalificacion.SelectedIndex = 0;
            txtComentarios.Text = string.Empty;
            txtRecomendaciones.Text = string.Empty;
            lblErrorModal.Visible = false;
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-danger";
            lblMensaje.Visible = true;
        }

        private void MostrarErrorModal(string mensaje)
        {
            lblErrorModal.Text = mensaje;
            lblErrorModal.Visible = true;
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado.ToLower())
            {
                case "aprobada":
                    return "bg-success";
                case "pendiente":
                    return "bg-warning";
                case "rechazada":
                    return "bg-danger";
                case "en revisión":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarEvaluaciones();
        }

        protected void btnNuevaEvaluacion_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            litTituloModal.Text = "Nueva Evaluación";
            hdnEvaluacionId.Value = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                "showModal();", true);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={NormaId}");
        }

        protected void gvEvaluaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el ID de la evaluación
                var evaluacionId = DataBinder.Eval(e.Row.DataItem, "EvaluacionId");
                
                // Establecer el atributo data-evaluacion-id
                e.Row.Attributes["data-evaluacion-id"] = evaluacionId.ToString();
            }
        }
    }
} 