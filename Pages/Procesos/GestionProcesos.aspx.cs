using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Procesos
{
    public partial class GestionProcesos : Page
    {
        private int NormaId
        {
            get
            {
                if (ViewState["NormaId"] == null)
                {
                    string normaIdStr = Request.QueryString["NormaId"];
                    int normaId;
                    if (!int.TryParse(normaIdStr, out normaId))
                    {
                        Response.Redirect("~/Default.aspx");
                        return 0;
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
                CargarProcesos();
            }
        }

        private void CargarDatosNorma()
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Normas
                        .FirstOrDefault(n => n.NormaId == NormaId);
                    if (norma == null)
                    {
                        Response.Redirect("~/Default.aspx");
                        return;
                    }
                    litTituloPagina.Text = $"Procesos de {norma.Titulo}";
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
                using (var context = new IsomanagerContext())
                {
                    // Obtener contadores solo para la norma actual
                    litEvaluaciones.Text = context.EvaluacionProcesos
                        .Count(e => e.Proceso.NormaId == NormaId).ToString();
                    litCambios.Text = context.CambiosProceso
                        .Count(c => c.Proceso.NormaId == NormaId).ToString();
                    litMejoras.Text = context.MejoraProceso
                        .Count(m => m.Proceso.NormaId == NormaId).ToString();
                    litAuditorias.Text = context.AuditoriasInternaProceso
                        .Count(a => a.Proceso.NormaId == NormaId).ToString();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar contadores: {ex.Message}");
            }
        }

        private void CargarProcesos()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    IQueryable<Proceso> query = context.Procesos
                        .Include(p => p.Area)
                        .Include(p => p.Responsable)
                        .Where(p => p.NormaId == NormaId)
                        .OrderByDescending(p => p.UltimaActualizacion);

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(p => 
                            p.Nombre.ToLower().Contains(filtro) ||
                            p.Descripcion.ToLower().Contains(filtro) ||
                            p.Area.Nombre.ToLower().Contains(filtro) ||
                            p.Responsable.Nombre.ToLower().Contains(filtro));
                    }

                    gvProcesos.DataSource = query.ToList();
                    gvProcesos.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los procesos: {ex.Message}");
            }
        }

        protected void btnGuardarProceso_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || UsuarioActual == null)
                return;

            try
            {
                using (var context = new IsomanagerContext())
                {
                    if (!ucArea.SelectedDepartamentoId.HasValue)
                    {
                        MostrarErrorModal("Debe seleccionar un área.");
                        return;
                    }

                    if (!ucResponsable.SelectedUsuarioId.HasValue)
                    {
                        MostrarErrorModal("Debe seleccionar un responsable.");
                        return;
                    }

                    bool esNuevo = string.IsNullOrEmpty(hdnProcesoId.Value);
                    Proceso proceso;

                    if (esNuevo)
                    {
                        proceso = new Proceso
                        {
                            FechaCreacion = DateTime.Now,
                            Activo = true,
                            NormaId = NormaId
                        };
                        context.Procesos.Add(proceso);
                    }
                    else
                    {
                        int procesoId = Convert.ToInt32(hdnProcesoId.Value);
                        proceso = context.Procesos.Find(procesoId);
                        if (proceso == null || proceso.NormaId != NormaId)
                        {
                            MostrarErrorModal("No se encontró el proceso a editar.");
                            return;
                        }
                    }

                    // Actualizar propiedades
                    proceso.Nombre = txtNombre.Text.Trim();
                    proceso.Objetivo = txtObjetivo.Text.Trim();
                    proceso.Descripcion = txtDescripcion.Text.Trim();
                    proceso.Estado = ddlEstado.SelectedValue;
                    proceso.AreaId = ucArea.SelectedDepartamentoId.Value;
                    proceso.ResponsableId = ucResponsable.SelectedUsuarioId.Value;
                    proceso.AdministradorId = UsuarioActual.UsuarioId;
                    proceso.UltimaActualizacion = DateTime.Now;
                    
                    // Fechas
                    if (!string.IsNullOrEmpty(txtFechaInicio.Text))
                        proceso.FechaInicio = DateTime.Parse(txtFechaInicio.Text);
                    
                    if (!string.IsNullOrEmpty(txtFechaFin.Text))
                        proceso.FechaFin = DateTime.Parse(txtFechaFin.Text);

                    // El progreso se calculará automáticamente
                    proceso.Progreso = 0;

                    context.SaveChanges();

                    CargarProcesos();
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                        "hideModal();", true);

                    string mensaje = esNuevo ? "Proceso creado exitosamente." : "Proceso actualizado exitosamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                        $"showSuccessMessage('{mensaje}');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarErrorModal($"Error al guardar el proceso: {ex.Message}");
            }
        }

        protected void gvProcesos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int procesoId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "Editar":
                        CargarProcesoParaEditar(procesoId);
                        break;

                    case "Eliminar":
                        EliminarProceso(procesoId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarProcesoParaEditar(int procesoId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var proceso = context.Procesos
                        .Include(p => p.Area)
                        .Include(p => p.Responsable)
                        .Include(p => p.Administrador)
                        .FirstOrDefault(p => p.ProcesoId == procesoId);

                    if (proceso != null)
                    {
                        LimpiarModal();

                        hdnProcesoId.Value = proceso.ProcesoId.ToString();
                        
                        // Establecer valores directamente
                        ddlEstado.SelectedValue = proceso.Estado;
                        txtNombre.Text = proceso.Nombre;
                        txtObjetivo.Text = proceso.Objetivo;
                        txtDescripcion.Text = proceso.Descripcion;
                        ucArea.SelectedDepartamentoId = proceso.AreaId;
                        ucResponsable.SelectedUsuarioId = proceso.ResponsableId;

                        if (proceso.FechaInicio != DateTime.MinValue)
                            txtFechaInicio.Text = proceso.FechaInicio.ToString("yyyy-MM-dd");
                        
                        if (proceso.FechaFin.HasValue)
                            txtFechaFin.Text = proceso.FechaFin.Value.ToString("yyyy-MM-dd");
                        
                        litTituloModal.Text = "Editar Proceso";

                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar el proceso para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al cargar el proceso para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void EliminarProceso(int procesoId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var proceso = context.Procesos.Find(procesoId);
                    if (proceso != null)
                    {
                        context.Procesos.Remove(proceso);
                        context.SaveChanges();
                        CargarProcesos();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "showSuccessMessage('Proceso eliminado exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar el proceso: {ex.Message}");
            }
        }

        private void LimpiarModal()
        {
            try
            {
                hdnProcesoId.Value = string.Empty;
                ddlEstado.SelectedIndex = 0;
                txtNombre.Text = string.Empty;
                txtObjetivo.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                ucArea.SelectedDepartamentoId = null;
                ucResponsable.SelectedUsuarioId = null;
                txtFechaInicio.Text = string.Empty;
                txtFechaFin.Text = string.Empty;
                lblErrorModal.Visible = false;
                litTituloModal.Text = "Nuevo Proceso";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en LimpiarModal: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
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
                case "completado":
                    return "bg-success";
                case "en progreso":
                    return "bg-warning";
                case "pendiente":
                    return "bg-secondary";
                default:
                    return "bg-info";
            }
        }

        protected void btnIrEvaluaciones_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/Evaluaciones/GestionEvaluaciones.aspx?NormaId={NormaId}");
        }

        protected void btnIrCambios_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/Cambios/GestionCambios.aspx?NormaId={NormaId}");
        }

        protected void btnIrMejoras_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/Mejoras/GestionMejoras.aspx?NormaId={NormaId}");
        }

        protected void btnIrAuditorias_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/Auditorias/GestionAuditorias.aspx?NormaId={NormaId}");
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarProcesos();
        }

        protected void btnNuevoProceso_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            litTituloModal.Text = "Nuevo Proceso";
            hdnProcesoId.Value = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                "showModal();", true);
        }
    }
} 