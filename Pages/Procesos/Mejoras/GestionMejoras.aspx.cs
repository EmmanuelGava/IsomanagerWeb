using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Procesos.Mejoras
{
    public partial class GestionMejoras : Page
    {
        protected UpdatePanel upModal;
        
        private int NormaId
        {
            get
            {
                if (ViewState["NormaId"] == null)
                {
                    string normaIdStr = Request.QueryString["NormaId"];
                    if (string.IsNullOrEmpty(normaIdStr))
                    {
                        System.Diagnostics.Debug.WriteLine("NormaId no encontrado en QueryString");
                        Response.Redirect("~/Default.aspx", true);
                        return 0;
                    }

                    if (!int.TryParse(normaIdStr, out int normaId))
                    {
                        System.Diagnostics.Debug.WriteLine($"NormaId inválido: {normaIdStr}");
                        Response.Redirect("~/Default.aspx", true);
                        return 0;
                    }

                    using (var context = new IsomanagerContext())
                    {
                        var norma = context.Norma.FirstOrDefault(n => n.NormaId == normaId);
                        if (norma == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"No se encontró la norma con ID: {normaId}");
                            Response.Redirect("~/Default.aspx", true);
                            return 0;
                        }
                        ViewState["NormaId"] = normaId;
                        ViewState["NormaTitulo"] = norma.Titulo;
                    }
                }
                return (int)ViewState["NormaId"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Iniciando Page_Load de GestionMejoras ===");
                System.Diagnostics.Debug.WriteLine($"IsPostBack: {IsPostBack}");
                System.Diagnostics.Debug.WriteLine($"Session[UsuarioId]: {Session["UsuarioId"]}");
                System.Diagnostics.Debug.WriteLine($"QueryString[NormaId]: {Request.QueryString["NormaId"]}");

                // Verificar sesión primero
                if (UsuarioActual == null)
                {
                    System.Diagnostics.Debug.WriteLine("UsuarioActual es null - redirigiendo");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Usuario actual ID: {UsuarioActual.UsuarioId}");

                if (!IsPostBack)
                {
                    if (NormaId == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("NormaId es 0 - redirigiendo");
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine($"NormaId: {NormaId}");
                    
                    System.Diagnostics.Debug.WriteLine("Cargando datos iniciales...");
                    CargarDatosNorma();
                    CargarContadores();
                    CargarMejoras();
                    CargarProcesos();

                    // Configurar el control de usuario
                    System.Diagnostics.Debug.WriteLine("Configurando control de usuario...");
                    ucUsuarioAsignado.Required = true;
                    ucUsuarioAsignado.ValidationGroup = "Mejora";
                    System.Diagnostics.Debug.WriteLine("Control de usuario configurado");
                }
                System.Diagnostics.Debug.WriteLine("=== Page_Load completado ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en Page_Load: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar la página: {ex.Message}");
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Iniciando CargarDatosNorma ===");
                litTituloPagina.Text = "Gestión de Mejoras";
                litNormaTitulo.Text = ViewState["NormaTitulo"]?.ToString() ?? string.Empty;
                System.Diagnostics.Debug.WriteLine($"Título de la norma cargado: {litNormaTitulo.Text}");
                System.Diagnostics.Debug.WriteLine("=== CargarDatosNorma completado ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en CargarDatosNorma: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar los datos de la norma: {ex.Message}");
            }
        }

        public Usuario UsuarioActual
        {
            get
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("=== Obteniendo UsuarioActual ===");
                    if (Session["UsuarioId"] == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Session[UsuarioId] es null");
                        Response.Redirect("~/Login.aspx");
                        return null;
                    }

                    System.Diagnostics.Debug.WriteLine($"Session[UsuarioId]: {Session["UsuarioId"]}");

                    using (var context = new IsomanagerContext())
                    {
                        int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                        System.Diagnostics.Debug.WriteLine($"Buscando usuario con ID: {usuarioId}");
                        
                        var usuario = context.Usuarios.Find(usuarioId);
                        if (usuario == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Usuario no encontrado en la base de datos");
                            Session.Clear();
                            Response.Redirect("~/Login.aspx");
                            return null;
                        }
                        System.Diagnostics.Debug.WriteLine($"Usuario encontrado: {usuario.Nombre}");
                        return usuario;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR en UsuarioActual: {ex.Message}");
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
                using (var context = new IsomanagerContext())
                {
                    var mejoras = context.MejoraProceso
                        .Where(m => m.Proceso.NormaId == NormaId);

                    litImplementadas.Text = mejoras.Count(m => m.Estado == "Implementada").ToString();
                    litPendientes.Text = mejoras.Count(m => m.Estado == "Pendiente").ToString();
                    litRechazadas.Text = mejoras.Count(m => m.Estado == "Rechazada").ToString();
                    litEnAnalisis.Text = mejoras.Count(m => m.Estado == "En Análisis").ToString();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar contadores: {ex.Message}");
            }
        }

        private void CargarMejoras()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var query = context.MejoraProceso
                        .Include(m => m.Proceso)
                        .Include(m => m.Creador)
                        .Include(m => m.Asignado)
                        .Where(m => m.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(m =>
                            m.Proceso.Nombre.ToLower().Contains(filtro) ||
                            m.Titulo.ToLower().Contains(filtro) ||
                            m.Creador.Nombre.ToLower().Contains(filtro) ||
                            m.Asignado.Nombre.ToLower().Contains(filtro) ||
                            m.Estado.ToLower().Contains(filtro) ||
                            m.Prioridad.ToLower().Contains(filtro));
                    }

                    var mejoras = query
                        .OrderByDescending(m => m.FechaCreacion)
                        .Select(m => new
                        {
                            m.MejoraId,
                            m.ProcesoId,
                            Proceso = m.Proceso.Nombre,
                            m.Titulo,
                            FechaSolicitud = m.FechaCreacion,
                            Solicitante = m.Creador.Nombre,
                            Asignado = m.Asignado.Nombre,
                            m.Estado,
                            m.Prioridad,
                            m.Descripcion,
                            m.Justificacion,
                            m.BeneficiosEsperados,
                            m.RecursosNecesarios,
                            m.ResultadosEsperados
                        })
                        .ToList();

                    gvMejoras.DataSource = mejoras;
                    gvMejoras.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las mejoras: {ex.Message}");
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

        protected void btnGuardarMejora_Click(object sender, EventArgs e)
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

                    if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                    {
                        MostrarErrorModal("El título es requerido.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ddlEstado.SelectedValue))
                    {
                        MostrarErrorModal("Debe seleccionar un estado.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(ddlPrioridad.SelectedValue))
                    {
                        MostrarErrorModal("Debe seleccionar una prioridad.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                    {
                        MostrarErrorModal("La descripción es requerida.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtJustificacion.Text))
                    {
                        MostrarErrorModal("La justificación es requerida.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtBeneficiosEsperados.Text))
                    {
                        MostrarErrorModal("Los beneficios esperados son requeridos.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtRecursosNecesarios.Text))
                    {
                        MostrarErrorModal("Los recursos necesarios son requeridos.");
                        return;
                    }

                    bool esNuevo = string.IsNullOrEmpty(hdnMejoraId.Value);
                    MejoraProceso mejora;

                    if (esNuevo)
                    {
                        mejora = new MejoraProceso
                        {
                            ProcesoId = procesoId,
                            CreadorId = UsuarioActual.UsuarioId,
                            AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? UsuarioActual.UsuarioId,
                            FechaCreacion = DateTime.Now,
                            UltimaModificacion = DateTime.Now,
                            Activo = true
                        };
                        context.MejoraProceso.Add(mejora);
                    }
                    else
                    {
                        int mejoraId = Convert.ToInt32(hdnMejoraId.Value);
                        mejora = context.MejoraProceso.Find(mejoraId);
                        if (mejora == null)
                        {
                            MostrarErrorModal("No se encontró la mejora a editar.");
                            return;
                        }
                        mejora.UltimaModificacion = DateTime.Now;
                        mejora.AsignadoId = ucUsuarioAsignado.SelectedUsuarioId ?? mejora.AsignadoId;
                    }

                    // Actualizar propiedades
                    mejora.Titulo = txtTitulo.Text.Trim();
                    mejora.Estado = ddlEstado.SelectedValue;
                    mejora.Prioridad = ddlPrioridad.SelectedValue;
                    mejora.Descripcion = txtDescripcion.Text.Trim();
                    mejora.Justificacion = txtJustificacion.Text.Trim();
                    mejora.BeneficiosEsperados = txtBeneficiosEsperados.Text.Trim();
                    mejora.RecursosNecesarios = txtRecursosNecesarios.Text.Trim();
                    mejora.ResultadosEsperados = txtResultadosEsperados?.Text.Trim(); // Este campo es nullable

                    context.SaveChanges();

                    CargarMejoras();
                    CargarContadores();
                    ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                        "hideModal();", true);

                    string mensaje = esNuevo ? "Mejora creada exitosamente." : "Mejora actualizada exitosamente.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                        $"showSuccessMessage('{mensaje}');", true);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                MostrarErrorModal($"Error de validación: {fullErrorMessage}");
                System.Diagnostics.Debug.WriteLine($"Errores de validación: {fullErrorMessage}");
            }
            catch (Exception ex)
            {
                MostrarErrorModal($"Error al guardar la mejora: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Error al guardar la mejora: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        protected void gvMejoras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int mejoraId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "Editar":
                        CargarMejoraParaEditar(mejoraId);
                        break;

                    case "Ver":
                        // Implementar vista detallada si es necesario
                        break;

                    case "Eliminar":
                        EliminarMejora(mejoraId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarMejoraParaEditar(int mejoraId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== INICIANDO EDICIÓN DE MEJORA ID: {mejoraId} ===");
                using (var context = new IsomanagerContext())
                {
                    var mejora = context.MejoraProceso
                        .Include(m => m.Proceso)
                        .FirstOrDefault(m => m.MejoraId == mejoraId);

                    if (mejora != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Cargando mejora ID: {mejora.MejoraId}");
                        
                        // Limpiar el modal primero
                        LimpiarModal();

                        hdnMejoraId.Value = mejora.MejoraId.ToString();
                        
                        // Cargar procesos si es necesario
                        if (!ddlProceso.Items.Cast<ListItem>().Any(i => i.Value == mejora.ProcesoId.ToString()))
                        {
                            System.Diagnostics.Debug.WriteLine("Recargando lista de procesos");
                            CargarProcesos();
                        }

                        // Establecer valores en los dropdowns
                        var procesoItem = ddlProceso.Items.FindByValue(mejora.ProcesoId.ToString());
                        if (procesoItem != null)
                        {
                            ddlProceso.ClearSelection();
                            procesoItem.Selected = true;
                            System.Diagnostics.Debug.WriteLine($"Proceso seleccionado: {procesoItem.Text}");
                        }

                        var estadoItem = ddlEstado.Items.FindByValue(mejora.Estado);
                        if (estadoItem != null)
                        {
                            ddlEstado.ClearSelection();
                            estadoItem.Selected = true;
                            System.Diagnostics.Debug.WriteLine($"Estado seleccionado: {estadoItem.Text}");
                        }

                        var prioridadItem = ddlPrioridad.Items.FindByValue(mejora.Prioridad);
                        if (prioridadItem != null)
                        {
                            ddlPrioridad.ClearSelection();
                            prioridadItem.Selected = true;
                            System.Diagnostics.Debug.WriteLine($"Prioridad seleccionada: {prioridadItem.Text}");
                        }

                        // Establecer valores en los campos de texto
                        txtTitulo.Text = mejora.Titulo ?? string.Empty;
                        txtDescripcion.Text = mejora.Descripcion ?? string.Empty;
                        txtJustificacion.Text = mejora.Justificacion ?? string.Empty;
                        txtBeneficiosEsperados.Text = mejora.BeneficiosEsperados ?? string.Empty;
                        txtRecursosNecesarios.Text = mejora.RecursosNecesarios ?? string.Empty;
                        txtResultadosEsperados.Text = mejora.ResultadosEsperados ?? string.Empty;

                        // Establecer usuario asignado
                        ucUsuarioAsignado.SelectedUsuarioId = mejora.AsignadoId;
                        
                        litTituloModal.Text = "EDITAR MEJORA";
                        System.Diagnostics.Debug.WriteLine("Título del modal establecido: EDITAR MEJORA");
                        
                        System.Diagnostics.Debug.WriteLine("Mostrando modal de edición");
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "$('#modalMejora').modal('show');", true);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"No se encontró la mejora con ID: {mejoraId}");
                        MostrarError("No se encontró la mejora especificada.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar la mejora para editar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError($"Error al cargar la mejora para editar: {ex.Message}");
            }
        }

        private void EliminarMejora(int mejoraId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var mejora = context.MejoraProceso.Find(mejoraId);
                    if (mejora != null)
                    {
                        context.MejoraProceso.Remove(mejora);
                        context.SaveChanges();
                        CargarMejoras();
                        CargarContadores();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            "showSuccessMessage('Mejora eliminada exitosamente.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar la mejora: {ex.Message}");
            }
        }

        private void LimpiarModal()
        {
            try
            {
                hdnMejoraId.Value = string.Empty;
                
                // Limpiar DropDownLists
                foreach (ListItem item in ddlProceso.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in ddlEstado.Items)
                {
                    item.Selected = false;
                }
                foreach (ListItem item in ddlPrioridad.Items)
                {
                    item.Selected = false;
                }
                
                // Seleccionar los items por defecto
                if (ddlProceso.Items.Count > 0)
                    ddlProceso.Items[0].Selected = true;
                if (ddlEstado.Items.Count > 0)
                    ddlEstado.Items[0].Selected = true;
                if (ddlPrioridad.Items.Count > 0)
                    ddlPrioridad.Items[0].Selected = true;

                txtTitulo.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                txtJustificacion.Text = string.Empty;
                txtBeneficiosEsperados.Text = string.Empty;
                txtRecursosNecesarios.Text = string.Empty;
                txtResultadosEsperados.Text = string.Empty;
                ucUsuarioAsignado.SelectedUsuarioId = null;
                lblErrorModal.Visible = false;
                litTituloModal.Text = "NUEVA MEJORA";
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
            switch (estado.ToLowerInvariant())
            {
                case "implementada":
                    return "bg-success";
                case "pendiente":
                    return "bg-warning";
                case "rechazada":
                    return "bg-danger";
                case "en análisis":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarMejoras();
        }

        protected void btnNuevaMejora_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== INICIANDO NUEVA MEJORA ===");
                
                // 1. Limpiar el modal
                LimpiarModal();
                
                // 2. Establecer el título
                litTituloModal.Text = "NUEVA MEJORA";
                
                // 3. Limpiar el ID
                hdnMejoraId.Value = "";
                
                System.Diagnostics.Debug.WriteLine("Título del modal establecido: NUEVA MEJORA");
                
                // 4. Mostrar el modal con un pequeño delay
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                    "setTimeout(function() { showModal(); }, 100);", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en btnNuevaMejora_Click: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError("Error al preparar el formulario para nueva mejora.");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={NormaId}");
        }

        protected void gvMejoras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el ID de la mejora
                var mejoraId = DataBinder.Eval(e.Row.DataItem, "MejoraId");
                
                // Establecer el atributo data-mejora-id
                e.Row.Attributes["data-mejora-id"] = mejoraId.ToString();
            }
        }
    }
} 