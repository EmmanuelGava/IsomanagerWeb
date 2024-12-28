using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Procesos.Cambios
{
    public partial class GestionCambios : Page
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
                        if (!context.Norma.Any(n => n.NormaId == normaId))
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
            try
            {
                // Verificar sesión primero
                if (UsuarioActual == null)
                    return; // La redirección ya fue manejada en la propiedad

                if (!IsPostBack)
                {
                    if (NormaId == 0)
                        return; // La redirección ya fue manejada en la propiedad

                    CargarDatosNorma();
                    CargarProcesos();
                    CargarContadores();
                    CargarCambios();

                    // Configurar el control de usuario
                    ucUsuarioAsignado.Required = true;
                    ucUsuarioAsignado.ValidationGroup = "Cambio";
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la página: {ex.Message}");
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Norma.FirstOrDefault(n => n.NormaId == NormaId);
                    if (norma == null)
                    {
                        Response.Redirect("~/Default.aspx");
                        return;
                    }
                    litTituloPagina.Text = "Gestión de Cambios";
                    litNormaTitulo.Text = norma.Titulo;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los datos de la norma: {ex.Message}");
            }
        }

        public Usuario UsuarioActual
        {
            get
            {
                try
                {
                    if (Session["UsuarioId"] == null)
                    {
                        Response.Redirect("~/Login.aspx");
                        return null;
                    }

                    using (var context = new IsomanagerContext())
                    {
                        int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                        var usuario = context.Usuarios.Find(usuarioId);
                        if (usuario == null)
                        {
                            Session.Clear();
                            Response.Redirect("~/Login.aspx");
                            return null;
                        }
                        return usuario;
                    }
                }
                catch (Exception)
                {
                    Session.Clear();
                    Response.Redirect("~/Login.aspx");
                    return null;
                }
            }
        }

        private void CargarContadores()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Iniciando CargarContadores ===");
                
                if (litAprobados == null || litPendientes == null || litRechazados == null || litEnRevision == null)
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

                    var query = context.CambiosProceso
                        .Include(c => c.Proceso)
                        .Where(c => c.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    System.Diagnostics.Debug.WriteLine("Contando registros por estado...");

                    litAprobados.Text = query.Count(c => c.Estado != null && c.Estado.ToLower() == "aprobado").ToString();
                    litPendientes.Text = query.Count(c => c.Estado != null && c.Estado.ToLower() == "pendiente").ToString();
                    litRechazados.Text = query.Count(c => c.Estado != null && c.Estado.ToLower() == "rechazado").ToString();
                    litEnRevision.Text = query.Count(c => c.Estado != null && c.Estado.ToLower() == "en revisión").ToString();

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

        private void CargarCambios()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Iniciando CargarCambios ===");
                using (var context = new IsomanagerContext())
                {
                    var query = context.CambiosProceso
                        .Include(c => c.Proceso)
                        .Include(c => c.Creador)
                        .Include(c => c.Asignado)
                        .Where(c => c.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLowerInvariant();
                        query = query.Where(c =>
                            c.Proceso.Nombre.ToLowerInvariant().Contains(filtro) ||
                            c.Titulo.ToLowerInvariant().Contains(filtro) ||
                            c.Creador.Nombre.ToLowerInvariant().Contains(filtro) ||
                            c.Asignado.Nombre.ToLowerInvariant().Contains(filtro) ||
                            (c.Estado != null && c.Estado.ToLowerInvariant().Contains(filtro)) ||
                            (c.ImpactoEstimado != null && c.ImpactoEstimado.ToLowerInvariant().Contains(filtro)));
                    }

                    var cambiosList = query
                        .OrderByDescending(c => c.FechaCreacion)
                        .Select(c => new
                        {
                            c.CambioId,
                            c.ProcesoId,
                            Proceso = c.Proceso.Nombre,
                            c.Titulo,
                            c.FechaCreacion,
                            Responsable = c.Asignado.Nombre,
                            c.AsignadoId,
                            c.Estado,
                            c.Descripcion,
                            c.ImpactoEstimado,
                            c.Justificacion,
                            c.RiesgosAsociados
                        })
                        .ToList();

                    gvCambios.DataSource = cambiosList;
                    gvCambios.DataBind();
                    System.Diagnostics.Debug.WriteLine("=== CargarCambios completado ===");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en CargarCambios: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar los cambios: {ex.Message}");
            }
        }

        private void CargarProcesos()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var procesos = context.Procesos
                        .Where(p => p.NormaId == NormaId)
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

        protected void btnGuardarCambio_Click(object sender, EventArgs e)
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

                    bool esNuevo = string.IsNullOrEmpty(hdnCambioId.Value);
                    CambiosProceso cambio;

                    if (esNuevo)
                    {
                        cambio = new CambiosProceso
                        {
                            ProcesoId = procesoId,
                            CreadorId = UsuarioActual.UsuarioId,
                            AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? UsuarioActual.UsuarioId,
                            FechaCreacion = DateTime.Now,
                            UltimaModificacion = DateTime.Now,
                            Activo = true
                        };
                        context.CambiosProceso.Add(cambio);
                    }
                    else
                    {
                        int cambioId = Convert.ToInt32(hdnCambioId.Value);
                        cambio = context.CambiosProceso.Find(cambioId);
                        if (cambio == null)
                        {
                            MostrarErrorModal("No se encontró el cambio a editar.");
                            return;
                        }
                        cambio.UltimaModificacion = DateTime.Now;
                        cambio.AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? cambio.AsignadoId;
                    }

                    // Actualizar propiedades
                    cambio.Titulo = txtTitulo.Text.Trim();
                    cambio.Estado = ddlEstado.SelectedValue;
                    cambio.ImpactoEstimado = ddlImpacto.SelectedValue;
                    cambio.Descripcion = txtDescripcion.Text.Trim();
                    cambio.Justificacion = txtJustificacion.Text.Trim();
                    cambio.RiesgosAsociados = txtRiesgos.Text.Trim();

                    context.SaveChanges();

                    CargarCambios();
                    CargarContadores();
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                        "hideModal();", true);

                    string mensaje = esNuevo ? "Cambio creado exitosamente." : "Cambio actualizado exitosamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                        $"showSuccessMessage('{mensaje}');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarErrorModal($"Error al guardar el cambio: {ex.Message}");
            }
        }

        protected void gvCambios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int cambioId = Convert.ToInt32(e.CommandArgument);
                CargarCambioParaEditar(cambioId);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int cambioId = Convert.ToInt32(e.CommandArgument);
                    using (var context = new IsomanagerContext())
                    {
                        var cambio = context.CambiosProceso.Find(cambioId);
                        if (cambio != null)
                        {
                            context.CambiosProceso.Remove(cambio);
                            context.SaveChanges();

                            CargarCambios();
                            CargarContadores();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                                "showSuccessMessage('Cambio eliminado exitosamente.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al eliminar el cambio: {ex.Message}");
                }
            }
        }

        private void CargarCambioParaEditar(int cambioId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var cambio = context.CambiosProceso
                        .Include(c => c.Proceso)
                        .FirstOrDefault(c => c.CambioId == cambioId);

                    if (cambio != null)
                    {
                        // Limpiar y cargar el modal
                        LimpiarModal();
                        
                        // Aseguramos que el proceso esté en la lista
                        if (!ddlProceso.Items.Cast<ListItem>().Any(i => i.Value == cambio.ProcesoId.ToString()))
                        {
                            CargarProcesos();
                        }

                        // Establecer valores
                        hdnCambioId.Value = cambio.CambioId.ToString();
                        ddlProceso.SelectedValue = cambio.ProcesoId.ToString();
                        ddlEstado.SelectedValue = cambio.Estado ?? "";
                        ddlImpacto.SelectedValue = cambio.ImpactoEstimado ?? "";
                        txtTitulo.Text = cambio.Titulo;
                        txtDescripcion.Text = cambio.Descripcion;
                        txtJustificacion.Text = cambio.Justificacion;
                        txtRiesgos.Text = cambio.RiesgosAsociados;
                        ucUsuarioAsignado.SelectedUsuarioId = cambio.AsignadoId;
                        
                        // Actualizar título del modal
                        litTituloModal.Text = "Editar Cambio";

                        // Mostrar el modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar el cambio para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al cargar el cambio para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void EliminarCambio(int cambioId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var cambio = context.CambiosProceso.Find(cambioId);
                    if (cambio != null)
                    {
                        context.CambiosProceso.Remove(cambio);
                        context.SaveChanges();
                        CargarCambios();
                        CargarContadores();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "showSuccessMessage('Cambio eliminado exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar el cambio: {ex.Message}");
            }
        }

        private void LimpiarModal()
        {
            try
            {
                hdnCambioId.Value = string.Empty;
                ddlProceso.SelectedIndex = 0;
                ddlEstado.SelectedIndex = 0;
                ddlImpacto.SelectedIndex = 0;
                txtTitulo.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                txtJustificacion.Text = string.Empty;
                txtRiesgos.Text = string.Empty;
                ucUsuarioAsignado.SelectedUsuarioId = null;
                lblErrorModal.Visible = false;
                litTituloModal.Text = "Nuevo Cambio";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                "showModal();", true);
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado.ToLower())
            {
                case "aprobado":
                    return "bg-success";
                case "pendiente":
                    return "bg-warning";
                case "rechazado":
                    return "bg-danger";
                case "en revisión":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarCambios();
        }

        protected void btnNuevoCambio_Click(object sender, EventArgs e)
        {
            LimpiarModal();
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                "showModal();", true);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={NormaId}");
        }

        protected void gvCambios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el ID del cambio
                var cambioId = DataBinder.Eval(e.Row.DataItem, "CambioId");
                
                // Establecer el atributo data-cambio-id
                e.Row.Attributes["data-cambio-id"] = cambioId.ToString();
            }
        }
    }
} 