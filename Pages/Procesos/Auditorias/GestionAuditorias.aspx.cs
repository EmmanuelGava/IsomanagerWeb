using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Data.Entity;
using IsomanagerWeb.Models;
using System.Collections.Generic;
using System.Web;

namespace IsomanagerWeb.Pages.Procesos.Auditorias
{
    [Serializable]
    public class AuditoriaViewModel
    {
        public int AuditoriaInternaProcesoId { get; set; }
        public int ProcesoId { get; set; }
        public string Proceso { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaAuditoria { get; set; }
        public string Auditor { get; set; }
        public int AsignadoId { get; set; }
        public string Estado { get; set; }
        public string Alcance { get; set; }
        public string Hallazgos { get; set; }
        public string Recomendaciones { get; set; }
    }

    public partial class GestionAuditorias : Page
    {
        protected UpdatePanel upModal;
        
        private int NormaId
        {
            get
            {
                try
                {
                    if (ViewState["NormaId"] == null)
                    {
                        string normaIdStr = Request.QueryString["NormaId"];
                        System.Diagnostics.Debug.WriteLine($"NormaId from QueryString: {normaIdStr}");

                        if (string.IsNullOrEmpty(normaIdStr))
                        {
                            System.Diagnostics.Debug.WriteLine("NormaId is null or empty");
                            Response.Redirect("~/Default.aspx");
                            return 0;
                        }

                        if (!int.TryParse(normaIdStr, out int normaId))
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to parse NormaId");
                            Response.Redirect("~/Default.aspx");
                            return 0;
                        }

                        using (var context = new IsomanagerContext())
                        {
                            var norma = context.Normas.FirstOrDefault(n => n.NormaId == normaId);
                            if (norma == null)
                            {
                                System.Diagnostics.Debug.WriteLine($"Norma not found for ID: {normaId}");
                                Response.Redirect("~/Default.aspx");
                                return 0;
                            }
                        }

                        ViewState["NormaId"] = normaId;
                        System.Diagnostics.Debug.WriteLine($"NormaId stored in ViewState: {normaId}");
                    }
                    return (int)ViewState["NormaId"];
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in NormaId getter: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    Response.Redirect("~/Default.aspx");
                    return 0;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Page_Load Started ===");
                
                // Verificar sesión primero
                var usuario = UsuarioActual;
                if (usuario == null)
                {
                    System.Diagnostics.Debug.WriteLine("Usuario is null - redirecting");
                    return; // La redirección ya fue manejada en la propiedad
                }

                System.Diagnostics.Debug.WriteLine($"Usuario actual ID: {usuario.UsuarioId}");

                if (!IsPostBack)
                {
                    int normaId = NormaId;
                    if (normaId == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("NormaId is 0 - redirecting");
                        return; // La redirección ya fue manejada en la propiedad
                    }

                    System.Diagnostics.Debug.WriteLine($"NormaId: {normaId}");
                    
                    System.Diagnostics.Debug.WriteLine("Loading initial data...");
                    CargarDatosNorma();
                    CargarProcesos();
                    CargarContadores();
                    CargarAuditorias();

                    // Configurar el control de usuario
                    System.Diagnostics.Debug.WriteLine("Configuring user control...");
                    ucUsuarioAsignado.Required = true;
                    ucUsuarioAsignado.ValidationGroup = "Auditoria";
                }
                System.Diagnostics.Debug.WriteLine("=== Page_Load Completed ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in Page_Load: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar la página: {ex.Message}");
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Normas.FirstOrDefault(n => n.NormaId == NormaId);
                    if (norma != null)
                    {
                        litTituloPagina.Text = "Gestión de Auditorías";
                        litNormaTitulo.Text = norma.Titulo;
                    }
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
                        System.Diagnostics.Debug.WriteLine("Session[UsuarioId] is null");
                        Response.Redirect("~/Login.aspx");
                        return null;
                    }

                    using (var context = new IsomanagerContext())
                    {
                        int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                        System.Diagnostics.Debug.WriteLine($"Looking for user with ID: {usuarioId}");
                        
                        var usuario = context.Usuarios
                            .FirstOrDefault(u => u.UsuarioId == usuarioId && u.Estado == "Activo");
                            
                        if (usuario == null)
                        {
                            System.Diagnostics.Debug.WriteLine("User not found or not active");
                            Session.Clear();
                            Response.Redirect("~/Login.aspx");
                            return null;
                        }

                        System.Diagnostics.Debug.WriteLine($"User found: {usuario.Nombre}");
                        return usuario;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in UsuarioActual getter: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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
                
                if (litAprobadas == null || litPendientes == null || litNoConformes == null || litEnProceso == null)
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

                    var query = context.AuditoriasInternaProceso
                        .Include(a => a.Proceso)
                        .Where(a => a.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    System.Diagnostics.Debug.WriteLine("Contando registros por estado...");

                    litAprobadas.Text = query.Count(a => a.Estado != null && a.Estado.ToLower() == "aprobada").ToString();
                    litPendientes.Text = query.Count(a => a.Estado != null && a.Estado.ToLower() == "pendiente").ToString();
                    litNoConformes.Text = query.Count(a => a.Estado != null && a.Estado.ToLower() == "no conforme").ToString();
                    litEnProceso.Text = query.Count(a => a.Estado != null && a.Estado.ToLower() == "en proceso").ToString();

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

        private void CargarAuditorias()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var query = context.AuditoriasInternaProceso
                        .Include(a => a.Proceso)
                        .Include(a => a.Creador)
                        .Include(a => a.Asignado)
                        .Where(a => a.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(a =>
                            a.Proceso.Nombre.ToLower().Contains(filtro) ||
                            a.Creador.Nombre.ToLower().Contains(filtro) ||
                            a.Asignado.Nombre.ToLower().Contains(filtro) ||
                            a.Estado.ToLower().Contains(filtro) ||
                            a.Titulo.ToLower().Contains(filtro) ||
                            a.Alcance.ToLower().Contains(filtro) ||
                            a.Hallazgos.ToLower().Contains(filtro) ||
                            a.Recomendaciones.ToLower().Contains(filtro));
                    }

                    // Obtener los datos
                    var auditoriasList = query
                        .OrderByDescending(a => a.FechaAuditoria)
                        .Select(a => new
                        {
                            a.AuditoriaInternaProcesoId,
                            a.ProcesoId,
                            Proceso = a.Proceso.Nombre,
                            Titulo = a.Titulo,
                            FechaAuditoria = a.FechaAuditoria,
                            Auditor = a.Asignado.Nombre,
                            a.AsignadoId,
                            a.Estado,
                            a.Descripcion,
                            a.Alcance,
                            a.Hallazgos,
                            a.Recomendaciones
                        })
                        .ToList();

                    gvAuditorias.DataSource = auditoriasList;
                    gvAuditorias.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las auditorías: {ex.Message}");
            }
        }

        private string ExtraerAlcance(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion)) return string.Empty;
            var partes = descripcion.Split(new[] { "\n\nHallazgos:\n" }, StringSplitOptions.None);
            return partes[0].Trim();
        }

        private string ExtraerHallazgos(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion) || !descripcion.Contains("\n\nHallazgos:\n")) return string.Empty;
            var partes = descripcion.Split(new[] { "\n\nHallazgos:\n", "\n\nRecomendaciones:\n" }, StringSplitOptions.None);
            return partes.Length > 1 ? partes[1].Trim() : string.Empty;
        }

        private string ExtraerRecomendaciones(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion) || !descripcion.Contains("\n\nRecomendaciones:\n")) return string.Empty;
            var partes = descripcion.Split(new[] { "\n\nRecomendaciones:\n" }, StringSplitOptions.None);
            return partes.Length > 1 ? partes[1].Trim() : string.Empty;
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

        protected void btnGuardarAuditoria_Click(object sender, EventArgs e)
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

                    bool esNueva = string.IsNullOrEmpty(hdnAuditoriaId.Value);
                    AuditoriasInternaProceso auditoria;

                    if (esNueva)
                    {
                        auditoria = new AuditoriasInternaProceso
                        {
                            ProcesoId = procesoId,
                            CreadorId = UsuarioActual.UsuarioId,
                            AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? UsuarioActual.UsuarioId,
                            FechaCreacion = DateTime.Now,
                            UltimaActualizacion = DateTime.Now,
                            FechaAuditoria = DateTime.Parse(txtFechaAuditoria.Text)
                        };
                        context.AuditoriasInternaProceso.Add(auditoria);
                    }
                    else
                    {
                        int auditoriaId = Convert.ToInt32(hdnAuditoriaId.Value);
                        auditoria = context.AuditoriasInternaProceso.Find(auditoriaId);
                        if (auditoria == null)
                        {
                            MostrarErrorModal("No se encontró la auditoría a editar.");
                            return;
                        }
                        auditoria.UltimaActualizacion = DateTime.Now;
                        auditoria.AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? auditoria.AsignadoId;
                        auditoria.FechaAuditoria = DateTime.Parse(txtFechaAuditoria.Text);
                    }

                    // Actualizar propiedades
                    auditoria.Titulo = ddlTipoAuditoria.SelectedValue;
                    auditoria.Estado = ddlEstado.SelectedValue;
                    auditoria.Alcance = txtAlcance.Text.Trim();
                    auditoria.Hallazgos = txtHallazgos.Text.Trim();
                    auditoria.Recomendaciones = txtRecomendaciones.Text.Trim();
                    auditoria.Descripcion = txtDescripcion.Text.Trim();

                    // Guardar los cambios
                    context.SaveChanges();

                    CargarAuditorias();
                    CargarContadores();
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                        "hideModal();", true);

                    string mensaje = esNueva ? "Auditoría creada exitosamente." : "Auditoría actualizada exitosamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                        $"showSuccessMessage('{mensaje}');", true);
                }
            }
            catch (Exception ex)
            {
                MostrarErrorModal($"Error al guardar la auditoría: {ex.Message}");
            }
        }

        protected void gvAuditorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int auditoriaId = Convert.ToInt32(e.CommandArgument);
                CargarAuditoriaParaEditar(auditoriaId);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    int auditoriaId = Convert.ToInt32(e.CommandArgument);
                    using (var context = new IsomanagerContext())
                    {
                        var auditoria = context.AuditoriasInternaProceso.Find(auditoriaId);
                        if (auditoria != null)
                        {
                            context.AuditoriasInternaProceso.Remove(auditoria);
                            context.SaveChanges();

                            CargarAuditorias();
                            CargarContadores();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                                "showSuccessMessage('Auditoría eliminada exitosamente.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al eliminar la auditoría: {ex.Message}");
                }
            }
            else if (e.CommandName == "VerDetalles")
            {
                int auditoriaId = Convert.ToInt32(e.CommandArgument);
                ScriptManager.RegisterStartupScript(this, GetType(), "toggleDetalles",
                    $"toggleDetalles({auditoriaId});", true);
            }
        }

        private void CargarAuditoriaParaEditar(int auditoriaId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var auditoria = context.AuditoriasInternaProceso
                        .Include(a => a.Proceso)
                        .Include(a => a.Asignado)
                        .FirstOrDefault(a => a.AuditoriaInternaProcesoId == auditoriaId);

                    if (auditoria != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Cargando auditoría ID: {auditoriaId}");
                        
                        // Limpiar y cargar el modal
                        LimpiarModal();
                        
                        // Aseguramos que el proceso está en la lista
                        if (!ddlProceso.Items.Cast<ListItem>().Any(i => i.Value == auditoria.ProcesoId.ToString()))
                        {
                            CargarProcesos();
                        }

                        // Establecer valores
                        hdnAuditoriaId.Value = auditoria.AuditoriaInternaProcesoId.ToString();
                        ddlProceso.SelectedValue = auditoria.ProcesoId.ToString();
                        
                        // Seleccionar el tipo de auditoría
                        System.Diagnostics.Debug.WriteLine($"Tipo de auditoría a seleccionar: {auditoria.Titulo}");
                        ddlTipoAuditoria.ClearSelection();
                        var tipoItem = ddlTipoAuditoria.Items.FindByText(auditoria.Titulo);
                        if (tipoItem != null)
                        {
                            tipoItem.Selected = true;
                            System.Diagnostics.Debug.WriteLine($"Tipo de auditoría seleccionado: {tipoItem.Text}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"No se encontró el tipo de auditoría: {auditoria.Titulo}");
                        }

                        var estadoItem = ddlEstado.Items.FindByValue(auditoria.Estado);
                        if (estadoItem != null)
                        {
                            ddlEstado.ClearSelection();
                            estadoItem.Selected = true;
                        }

                        txtFechaAuditoria.Text = auditoria.FechaAuditoria.ToString("yyyy-MM-dd");
                        txtAlcance.Text = auditoria.Alcance;
                        txtHallazgos.Text = auditoria.Hallazgos;
                        txtRecomendaciones.Text = auditoria.Recomendaciones;
                        txtDescripcion.Text = auditoria.Descripcion;
                        ucUsuarioAsignado.SelectedUsuarioId = auditoria.AsignadoId;
                        
                        // Actualizar título del modal
                        litTituloModal.Text = "EDITAR AUDITORÍA";
                        
                        System.Diagnostics.Debug.WriteLine("Datos cargados en el modal");
                        
                        // Actualizar el UpdatePanel
                        upModal.Update();
                        
                        // Mostrar el modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "setTimeout(function() { showModal(); }, 100);", true);
                            
                        System.Diagnostics.Debug.WriteLine("Modal mostrado");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la auditoría para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al cargar la auditoría para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private void EliminarAuditoria(int auditoriaId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var auditoria = context.AuditoriasInternaProceso.Find(auditoriaId);
                    if (auditoria != null)
                    {
                        context.AuditoriasInternaProceso.Remove(auditoria);
                        context.SaveChanges();
                        CargarAuditorias();
                        CargarContadores();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "showSuccessMessage('Auditoría eliminada exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar la auditoría: {ex.Message}");
            }
        }

        private void LimpiarModal()
        {
            try
            {
                hdnAuditoriaId.Value = string.Empty;
                ddlProceso.SelectedIndex = 0;
                ddlTipoAuditoria.SelectedIndex = 0;
                txtFechaAuditoria.Text = DateTime.Today.ToString("yyyy-MM-dd");
                ddlEstado.SelectedIndex = 0;
                txtAlcance.Text = string.Empty;
                txtHallazgos.Text = string.Empty;
                txtRecomendaciones.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                ucUsuarioAsignado.SelectedUsuarioId = null;
                lblErrorModal.Visible = false;
                litTituloModal.Text = "NUEVA AUDITORÍA";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                "setTimeout(function() { showModal(); }, 100);", true);
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado.ToLower())
            {
                case "aprobada":
                    return "bg-success";
                case "pendiente":
                    return "bg-warning";
                case "no conforme":
                    return "bg-danger";
                case "en proceso":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarAuditorias();
        }

        protected void btnNuevaAuditoria_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== INICIANDO NUEVA AUDITORÍA ===");
                
                // 1. Limpiar el modal
                LimpiarModal();
                
                // 2. Establecer el título
                litTituloModal.Text = "NUEVA AUDITORÍA";
                
                // 3. Limpiar el ID
                hdnAuditoriaId.Value = "";
                
                System.Diagnostics.Debug.WriteLine("Título del modal establecido: NUEVA AUDITORÍA");
                
                // 4. Mostrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                    "setTimeout(function() { showModal(); }, 100);", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en btnNuevaAuditoria_Click: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError("Error al preparar el formulario para nueva auditoría.");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={NormaId}");
        }

        protected void gvAuditorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el ID de la auditoría
                var auditoriaId = DataBinder.Eval(e.Row.DataItem, "AuditoriaInternaProcesoId");
                
                // Establecer el atributo data-auditoria-id
                e.Row.Attributes["data-auditoria-id"] = auditoriaId.ToString();
            }
        }
    }
} 