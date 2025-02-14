using System;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;
using System.Globalization;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Data.Entity.SqlServer;

namespace IsomanagerWeb.Pages
{
    public partial class Dashboard : Page
    {
        #region Campos Privados
        private IsomanagerContext db = new IsomanagerContext();

        // Controles del Modal de Calendario
        protected DropDownList ddlVista;
        protected Literal litCalendarioModal;
        protected Repeater rptAuditoriasModal;
        #endregion

        #region Eventos de Página
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                // Agregar manejador de errores no manejados
                this.Error += new EventHandler(Page_Error);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnInit: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurarSaludo();
                CargarDatos();
                CargarNormas();
                CargarTareas();
                CargarAuditorias();
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            System.Diagnostics.Debug.WriteLine("=== ERROR NO MANEJADO DETECTADO ===");
            System.Diagnostics.Debug.WriteLine($"Tipo de excepción: {ex.GetType().FullName}");
            System.Diagnostics.Debug.WriteLine($"Mensaje: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Source: {ex.Source}");
            System.Diagnostics.Debug.WriteLine($"TargetSite: {ex.TargetSite}");
            
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine("=== INNER EXCEPTION ===");
                System.Diagnostics.Debug.WriteLine($"Tipo: {ex.InnerException.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"Mensaje: {ex.InnerException.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.InnerException.StackTrace}");
            }
            
            System.Diagnostics.Debug.WriteLine("=== STACK TRACE ===");
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
        }
        #endregion

        #region Configuración Inicial
        private void ConfigurarSaludo()
        {
            try
            {
                if (litSaludo == null)
                {
                    System.Diagnostics.Debug.WriteLine("Error: litSaludo es null");
                    return;
                }

                string saludo = "Buenos días";
                int hora = DateTime.Now.Hour;
                
                if (hora >= 12 && hora < 20)
                    saludo = "Buenas tardes";
                else if (hora >= 20 || hora < 6)
                    saludo = "Buenas noches";

                string nombreUsuario = string.Empty;
                if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                {
                    // Intentar obtener el nombre después de la barra invertida si existe
                    var partes = User.Identity.Name.Split('\\');
                    nombreUsuario = partes.Length > 1 ? partes[1] : partes[0];

                    // Si el nombre está en formato de email, tomar la parte antes del @
                    if (nombreUsuario.Contains("@"))
                    {
                        nombreUsuario = nombreUsuario.Split('@')[0];
                    }
                }

                litSaludo.Text = $"{saludo}, {nombreUsuario}";
                
                // Manejar las iniciales del usuario de forma segura
                if (!string.IsNullOrEmpty(nombreUsuario))
                {
                    string iniciales = string.Join("", nombreUsuario.Split(' ')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Take(2)
                        .Select(x => x[0].ToString().ToUpper()));
                    
                    if (string.IsNullOrEmpty(iniciales))
                    {
                        iniciales = "U"; // Usuario por defecto si no se pueden obtener iniciales
                    }
                    
                    if (litUserInitials != null)
                    {
                        litUserInitials.Text = iniciales;
                    }
                }
                else
                {
                    if (litUserInitials != null)
                    {
                        litUserInitials.Text = "U";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error
                System.Diagnostics.Debug.WriteLine($"Error en ConfigurarSaludo: {ex.Message}");
                
                // Valores por defecto en caso de error
                if (litSaludo != null)
                {
                    litSaludo.Text = "Bienvenido";
                }
                if (litUserInitials != null)
                {
                    litUserInitials.Text = "U";
                }
            }
        }

        private void CargarDatos()
        {
            CargarNormas();
            // Aquí puedes cargar datos específicos del usuario como notificaciones, etc.
        }
        #endregion

        #region Carga de Datos Principales
        private void CargarNormas()
        {
            try
            {
                if (rptNormas1 == null)
                {
                    System.Diagnostics.Debug.WriteLine("Error: rptNormas1 es null");
                    return;
                }

                // Construir la consulta base
                var query = db.Normas
                    .Include(n => n.Documentos)
                    .Include(n => n.Responsable)
                    .AsQueryable();

                // Aplicar filtros
                AplicarFiltros(ref query);

                // Obtener los resultados
                var normas = query
                    .AsEnumerable()
                    .Select(n => new
                    {
                        n.NormaId,
                        n.TipoNorma,
                        n.Titulo,
                        n.Version,
                        n.Estado,
                        ResponsableNombre = n.Responsable != null ? n.Responsable.Nombre : "Sin asignar",
                        TotalDocumentos = n.Documentos != null ? n.Documentos.Count : 0,
                        TotalProcesos = db.Procesos.Count(p => p.NormaId == n.NormaId),
                        UltimaModificacion = n.UltimaModificacion,
                        Progreso = CalcularProgresoNorma(n.NormaId)
                    })
                    .ToList();

                // Asignar los datos al repeater
                rptNormas1.DataSource = normas;
                rptNormas1.DataBind();

                // Mostrar mensaje si no hay resultados
                if (pnlNoData != null)
                {
                    pnlNoData.Visible = !normas.Any();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en CargarNormas: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError("Error al cargar las normas: " + ex.Message);
            }
        }

        private int CalcularProgresoNorma(int normaId)
        {
            try
            {
                int totalItems = 0;
                int itemsCompletados = 0;

                // Contar documentos
                var documentos = db.Documentos.Where(d => d.SeccionId == normaId && d.Seccion == "Norma");
                int totalDocs = documentos.Count();
                int docsCompletados = documentos.Count(d => d.Activo);
                
                if (totalDocs > 0)
                {
                    totalItems += totalDocs;
                    itemsCompletados += docsCompletados;
                }

                // Contar procesos
                var procesos = db.Procesos.Where(p => p.NormaId == normaId);
                int totalProc = procesos.Count();
                int procCompletados = procesos.Count(p => p.Estado == "Implementado" || p.Estado == "Activo");

                if (totalProc > 0)
                {
                    totalItems += totalProc;
                    itemsCompletados += procCompletados;
                }

                return totalItems > 0 ? (itemsCompletados * 100) / totalItems : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void CargarTareas()
        {
            try
            {
                if (rptTareas == null)
                {
                    System.Diagnostics.Debug.WriteLine("Warning: rptTareas es null");
                    return;
                }

                // Crear lista de tareas de ejemplo
                var tareas = new List<object>
                {
                    new {
                        TaskId = "TASK-8782",
                        Titulo = "Actualizar manual de calidad",
                        TipoTarea = "documento",
                        Prioridad = "alta",
                        ResponsableNombre = "Ana García"
                    },
                    new {
                        TaskId = "TASK-7878",
                        Titulo = "Revisar procedimiento de auditoría interna",
                        TipoTarea = "auditoria",
                        Prioridad = "media",
                        ResponsableNombre = "Carlos López"
                    },
                    new {
                        TaskId = "TASK-7839",
                        Titulo = "Actualizar registro de formación del personal",
                        TipoTarea = "registro",
                        Prioridad = "baja",
                        ResponsableNombre = "María Rodríguez"
                    },
                    new {
                        TaskId = "TASK-7825",
                        Titulo = "Preparar informe de revisión por la dirección",
                        TipoTarea = "informe",
                        Prioridad = "alta",
                        ResponsableNombre = "Juan Pérez"
                    }
                };

                rptTareas.DataSource = tareas;
                rptTareas.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en CargarTareas: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MostrarError("Error al cargar las tareas: " + ex.Message);
            }
        }

        private void CargarAuditorias()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var auditorias = context.AuditoriasInternaProceso
                        .Include(a => a.Proceso)
                        .Where(a => a.FechaAuditoria >= DateTime.Today)
                        .OrderBy(a => a.FechaAuditoria)
                        .Take(5)
                        .Select(a => new
                        {
                            a.Titulo,
                            a.FechaAuditoria,
                            a.Estado
                        })
                        .ToList();

                    if (rptAuditorias != null)
                    {
                        rptAuditorias.DataSource = auditorias;
                        rptAuditorias.DataBind();

                        // Mostrar u ocultar el panel según si hay auditorías
                        if (pnlNoAuditorias != null)
                        {
                            pnlNoAuditorias.Visible = !auditorias.Any();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en CargarAuditorias: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                
                if (pnlMensajes != null && litMensaje != null)
                {
                    pnlMensajes.Visible = true;
                    litMensaje.Text = "Error al cargar las auditorías: " + ex.Message;
                }
                // Manejar el error apropiadamente
                pnlMensajes.Visible = true;
                litMensaje.Text = "Error al cargar las auditorías: " + ex.Message;
            }
        }
        #endregion

        #region Utilidades y Helpers
        public string GetEstadoClass(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "iso-card-borrador";

            switch (estado.ToLower())
            {
                case "borrador":
                    return "iso-card-borrador";
                case "en revisión":
                case "en revision":
                    return "iso-card-revision";
                case "aprobado":
                    return "iso-card-aprobado";
                case "obsoleto":
                    return "iso-card-obsoleto";
                default:
                    return "iso-card-borrador";
            }
        }

        public string GetStatusBadgeClass(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "status-borrador";

            switch (estado.ToLower())
            {
                case "borrador":
                    return "status-borrador";
                case "en revisión":
                case "en revision":
                    return "status-revision";
                case "aprobado":
                    return "status-aprobado";
                case "obsoleto":
                    return "status-obsoleto";
                default:
                    return "status-borrador";
            }
        }

        public string GetStatusText(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "Borrador";

            switch (estado.ToLower())
            {
                case "borrador":
                    return "Borrador";
                case "en revisión":
                case "en revision":
                    return "En Revisión";
                case "aprobado":
                    return "Aprobado";
                case "obsoleto":
                    return "Obsoleto";
                default:
                    return "Borrador";
            }
        }

        public string GetTaskBadgeStyle(object status)
        {
            if (status == null)
                return "task-status-default";

            switch (status.ToString().ToLower())
            {
                case "alta":
                    return "task-status-high";
                case "media":
                    return "task-status-medium";
                case "baja":
                    return "task-status-low";
                default:
                    return "task-status-default";
            }
        }

        public string GetTaskIcon(string tipoTarea)
        {
            if (string.IsNullOrEmpty(tipoTarea))
                return "bi-check-circle";

            switch (tipoTarea.ToLower())
            {
                case "documento":
                    return "bi-file-text";
                case "auditoria":
                    return "bi-clipboard-check";
                case "registro":
                    return "bi-journal-text";
                case "informe":
                    return "bi-graph-up";
                default:
                    return "bi-check-circle";
            }
        }

        public string GetTaskPriorityClass(string prioridad)
        {
            if (string.IsNullOrEmpty(prioridad))
                return "baja";

            switch (prioridad.ToLower())
            {
                case "alta":
                    return "alta";
                case "media":
                    return "media";
                case "baja":
                    return "baja";
                default:
                    return "baja";
            }
        }

        public string GetAuditoriaBadgeClass(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "auditoria-badge pendiente";

            switch (estado.ToLower())
            {
                case "pendiente":
                    return "auditoria-badge pendiente";
                case "programada":
                    return "auditoria-badge programada";
                case "confirmada":
                    return "auditoria-badge confirmada";
                case "completada":
                    return "auditoria-badge completada";
                case "cancelada":
                    return "auditoria-badge cancelada";
                default:
                    return "auditoria-badge pendiente";
            }
        }

        private void MostrarError(string mensaje)
        {
            if (pnlMensajes != null && litMensaje != null)
            {
                pnlMensajes.Visible = true;
                litMensaje.Text = mensaje;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Error al mostrar mensaje: {mensaje}");
            }
        }
        #endregion

        #region Eventos de Controles
        protected void OnBuscarClick(object sender, EventArgs e)
        {
            try
            {
                CargarNormas();
            }
            catch (Exception ex)
            {
                MostrarError("Error al aplicar el filtro de búsqueda: " + ex.Message);
            }
        }

        protected void OnIrDocumentosClick(object sender, EventArgs e)
        {
            var linkButton = (LinkButton)sender;
            var normaId = Convert.ToInt32(linkButton.CommandArgument);
            Response.Redirect($"~/Pages/Documentos/ListaDocumentos.aspx?NormaId={normaId}");
        }

        protected void OnIrProcesosClick(object sender, EventArgs e)
        {
            var linkButton = (LinkButton)sender;
            var normaId = Convert.ToInt32(linkButton.CommandArgument);
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={normaId}");
        }

        protected void OnIrComponentesClick(object sender, EventArgs e)
        {
            var linkButton = (LinkButton)sender;
            var normaId = Convert.ToInt32(linkButton.CommandArgument);
            Response.Redirect($"~/Pages/Normas/ComponentesNorma.aspx?NormaId={normaId}");
        }

        protected void rptNormas1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Maneja comandos adicionales del repeater si es necesario
        }

        protected void OnNormasItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Personaliza el binding de datos si es necesario
        }

        protected void btnVerCalendario_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Calendario.aspx");
        }
        #endregion

        #region Calendario
        #region Propiedades del Calendario
        protected DateTime CurrentDate
        {
            get
            {
                if (ViewState["CurrentDate"] == null)
                {
                    ViewState["CurrentDate"] = DateTime.Today;
                }
                return (DateTime)ViewState["CurrentDate"];
            }
            set
            {
                ViewState["CurrentDate"] = value;
            }
        }

        public string CurrentMonth => CurrentDate.ToString("MMMM", new CultureInfo("es-ES"));
        public string CurrentYear => CurrentDate.Year.ToString();

        protected void btnPrevWeek_Click(object sender, EventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(-7);
            // Actualizar la vista del calendario se maneja en el frontend
        }

        protected void btnNextWeek_Click(object sender, EventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(7);
            // Actualizar la vista del calendario se maneja en el frontend
        }
        #endregion
        #endregion

        #region Eventos de Filtros
        protected void ddlEstadoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CargarNormas();
            }
            catch (Exception ex)
            {
                MostrarError("Error al filtrar por estado: " + ex.Message);
            }
        }

        protected void chkSoloActivas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CargarNormas();
            }
            catch (Exception ex)
            {
                MostrarError("Error al filtrar normas activas: " + ex.Message);
            }
        }

        private void AplicarFiltros(ref IQueryable<Norma> query)
        {
            try
            {
                // Filtro de búsqueda
                if (txtBuscarNormas != null && !string.IsNullOrEmpty(txtBuscarNormas.Text))
                {
                    string filtro = txtBuscarNormas.Text.Trim().ToLower();
                    query = query.Where(n =>
                        n.Titulo.ToLower().Contains(filtro) ||
                        n.TipoNorma.ToLower().Contains(filtro) ||
                        n.Estado.ToLower().Contains(filtro) ||
                        (n.Responsable != null && n.Responsable.Nombre.ToLower().Contains(filtro))
                    );
                }

                // Filtro por estado
                if (ddlEstadoFiltro != null && !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue))
                {
                    query = query.Where(n => n.Estado == ddlEstadoFiltro.SelectedValue);
                }

                // Filtro de normas activas
                if (chkSoloActivas != null && chkSoloActivas.Checked)
                {
                    query = query.Where(n => n.Estado == "Aprobado" || n.Estado == "En Revisión");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en AplicarFiltros: {ex.Message}");
                // No relanzamos la excepción para evitar que se rompa la carga de datos
            }
        }
        #endregion
    }
}