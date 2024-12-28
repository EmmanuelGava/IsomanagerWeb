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

namespace IsomanagerWeb.Pages
{
    public partial class Dashboard : Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Iniciando Page_Load del Dashboard");
                System.Diagnostics.Debug.WriteLine($"IsPostBack: {IsPostBack}");
                System.Diagnostics.Debug.WriteLine($"Request Path: {Request.Path}");
                System.Diagnostics.Debug.WriteLine($"Request Method: {Request.HttpMethod}");

                // Siempre cargar los datos, sin importar si es postback
                System.Diagnostics.Debug.WriteLine("Iniciando carga de datos...");
                CargarNormas();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\n=== ERROR EN PAGE_LOAD ===");
                System.Diagnostics.Debug.WriteLine($"Tipo de error: {ex.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"Mensaje: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("\n=== INNER EXCEPTION ===");
                    System.Diagnostics.Debug.WriteLine($"Tipo: {ex.InnerException.GetType().FullName}");
                    System.Diagnostics.Debug.WriteLine($"Mensaje: {ex.InnerException.Message}");
                }
            }
        }

        protected string GetStatusClass(string estado)
        {
            try
            {
                estado = estado?.ToLower() ?? "";
                if (estado == "en progreso") return "in-progress";
                if (estado == "en revisión") return "in-review";
                if (estado == "iniciado") return "initiated";
                if (estado == "planificación") return "planning";
                return "in-progress";
            }
            catch (Exception ex)
            {
                LogError("GetStatusClass", ex);
                return "in-progress";
            }
        }

        protected string GetStatusText(string estado)
        {
            try
            {
                estado = estado?.ToLower() ?? "";
                if (estado == "en progreso") return "En Progreso";
                if (estado == "en revisión") return "En Revisión";
                if (estado == "iniciado") return "Iniciado";
                if (estado == "planificación") return "Planificación";
                return estado;
            }
            catch (Exception ex)
            {
                LogError("GetStatusText", ex);
                return estado ?? "Desconocido";
            }
        }

        protected void CargarNormas()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Iniciando CargarNormas...");
                using (var db = new IsomanagerContext())
                {
                    System.Diagnostics.Debug.WriteLine("Conexión a base de datos establecida");

                    // Primero obtenemos las normas con sus datos básicos
                    System.Diagnostics.Debug.WriteLine("Consultando normas básicas...");
                    var normasBasicas = db.Norma
                        .Include(n => n.Responsable)
                        .AsNoTracking()
                        .OrderByDescending(n => n.UltimaActualizacion)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Normas básicas obtenidas: {normasBasicas?.Count ?? 0}");

                    if (normasBasicas == null || !normasBasicas.Any())
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontraron normas");
                        pnlNoData.Visible = true;
                        return;
                    }

                    // Luego procesamos los datos adicionales
                    var normasCompletas = new List<object>();
                    foreach (var n in normasBasicas)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"Procesando norma ID: {n.NormaId}, Título: {n.Titulo}");

                            // Obtener contexto y estadísticas en una sola consulta
                            var stats = (from p in db.Procesos
                                       where p.NormaId == n.NormaId
                                       group p by p.NormaId into g
                                       select new
                                       {
                                           TotalProcesos = g.Count(),
                                           ProcesosCumplidos = g.Count(p => p.Estado == "Completado")
                                       }).FirstOrDefault() ?? new { TotalProcesos = 0, ProcesosCumplidos = 0 };

                            var totalDocumentos = db.Documentos
                                .Count(d => d.SeccionId == n.NormaId && d.Seccion == "Norma" && d.Activo);

                            System.Diagnostics.Debug.WriteLine($"Estadísticas para norma {n.NormaId}: " +
                                $"Procesos={stats.TotalProcesos}, Cumplidos={stats.ProcesosCumplidos}, " +
                                $"Docs={totalDocumentos}");

                            var normaObj = new
                            {
                                n.NormaId,
                                Nombre = n.Titulo ?? "Sin título",
                                Version = n.Version ?? "1.0",
                                Estado = n.Estado ?? "En Progreso",
                                UltimaActualizacion = n.UltimaActualizacion.ToString("dd/MM/yyyy"),
                                Responsable = n.Responsable?.Nombre ?? "Sin asignar",
                                TotalProcesos = stats.TotalProcesos,
                                TotalDocumentos = totalDocumentos,
                                Progreso = stats.TotalProcesos == 0 ? 0 : 
                                    (int)((double)stats.ProcesosCumplidos / stats.TotalProcesos * 100)
                            };

                            System.Diagnostics.Debug.WriteLine($"Objeto norma creado: {normaObj.Nombre}, " +
                                $"Progreso: {normaObj.Progreso}%");
                            normasCompletas.Add(normaObj);
                        }
                        catch (Exception ex)
                        {
                            LogError($"Error procesando norma {n.NormaId}", ex);
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Total de normas procesadas: {normasCompletas.Count}");

                    if (normasCompletas.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Asignando datos al Repeater...");
                        rptNormas1.DataSource = normasCompletas;
                        rptNormas1.DataBind();
                        System.Diagnostics.Debug.WriteLine("DataBind completado");
                        pnlNoData.Visible = false;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No hay normas para mostrar");
                        pnlNoData.Visible = true;
                        MostrarMensaje("No se encontraron normas para mostrar.", TipoMensaje.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("CargarNormas", ex);
                MostrarMensaje("Error al cargar las normas: " + ex.Message, TipoMensaje.Error);
            }
        }

        private void LogError(string location, Exception ex, string additionalInfo = null)
        {
            try
            {
                var errorMessage = new System.Text.StringBuilder();
                errorMessage.AppendLine($"=== Error en {location} ===");
                errorMessage.AppendLine($"Timestamp: {DateTime.Now}");
                errorMessage.AppendLine($"Message: {ex.Message}");
                
                if (additionalInfo != null)
                    errorMessage.AppendLine($"Info Adicional: {additionalInfo}");
                    
                if (ex.InnerException != null)
                {
                    errorMessage.AppendLine("Inner Exception:");
                    errorMessage.AppendLine($"- Message: {ex.InnerException.Message}");
                    errorMessage.AppendLine($"- Type: {ex.InnerException.GetType().FullName}");
                    if (ex.InnerException.StackTrace != null)
                        errorMessage.AppendLine($"- Stack: {ex.InnerException.StackTrace}");
                }
                
                errorMessage.AppendLine($"Exception Type: {ex.GetType().FullName}");
                if (ex.StackTrace != null)
                    errorMessage.AppendLine($"Stack Trace: {ex.StackTrace}");
                    
                // Log del estado de los controles principales
                errorMessage.AppendLine("\nEstado de los controles:");
                errorMessage.AppendLine($"pnlData.Visible: {(pnlData?.Visible ?? false)}");
                errorMessage.AppendLine($"pnlNoData.Visible: {(pnlNoData?.Visible ?? false)}");
                errorMessage.AppendLine($"rptNormas.Items.Count: {(rptNormas?.Items?.Count ?? -1)}");
                
                System.Diagnostics.Debug.WriteLine(errorMessage.ToString());
                
                // También podríamos guardar en un archivo o en la base de datos
                // System.IO.File.AppendAllText(Server.MapPath("~/App_Data/error.log"), errorMessage.ToString());
            }
            catch (Exception logEx)
            {
                // Si falla el logging, al menos intentamos escribir algo básico
                System.Diagnostics.Debug.WriteLine($"Error al intentar registrar error: {logEx.Message}");
                System.Diagnostics.Debug.WriteLine($"Error original: {ex.Message}");
            }
        }

        private enum TipoMensaje
        {
            Success,
            Info,
            Warning,
            Error
        }

        private void MostrarMensaje(string mensaje, TipoMensaje tipo)
        {
            try
            {
                pnlMensajes.Visible = true;
                litMensaje.Text = mensaje;
                
                string cssClass;
                switch (tipo)
                {
                    case TipoMensaje.Success:
                        cssClass = "alert alert-success";
                        break;
                    case TipoMensaje.Info:
                        cssClass = "alert alert-info";
                        break;
                    case TipoMensaje.Warning:
                        cssClass = "alert alert-warning";
                        break;
                    case TipoMensaje.Error:
                        cssClass = "alert alert-danger";
                        break;
                    default:
                        cssClass = "alert alert-info";
                        break;
                }
                
                pnlMensajes.CssClass = cssClass;
                upDashboard.Update();
            }
            catch (Exception ex)
            {
                LogError("MostrarMensaje", ex);
            }
        }

        private void OcultarMensajes()
        {
            pnlMensajes.Visible = false;
            upDashboard.Update();
        }

        private void MostrarError(string mensaje)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"\n=== MOSTRANDO ERROR AL USUARIO ===");
                System.Diagnostics.Debug.WriteLine($"Mensaje: {mensaje}");
                
                // Asegurarnos de que los controles existen y están accesibles
                if (pnlMensajes == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: pnlMensajes es null");
                    return;
                }
                
                if (litMensaje == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: litMensaje es null");
                    return;
                }
                
                if (upDashboard == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: upDashboard es null");
                    return;
                }

                // Mostrar el mensaje
                pnlMensajes.Visible = true;
                pnlMensajes.CssClass = "alert alert-danger";
                litMensaje.Text = HttpUtility.HtmlEncode(mensaje);
                
                // Intentar actualizar el panel
                try
                {
                    upDashboard.Update();
                    System.Diagnostics.Debug.WriteLine("Panel actualizado correctamente");
                }
                catch (Exception updateEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al actualizar el panel: {updateEx.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\n=== ERROR AL MOSTRAR ERROR ===");
                System.Diagnostics.Debug.WriteLine($"Error original: {mensaje}");
                System.Diagnostics.Debug.WriteLine($"Error al mostrar: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        protected void OnNormasItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    // Ya no necesitamos manipular el progressBar aquí porque lo manejamos directamente en el markup
                    System.Diagnostics.Debug.WriteLine($"Item bound: {DataBinder.Eval(e.Item.DataItem, "NormaId")}");
                }
            }
            catch (Exception ex)
            {
                LogError("OnNormasItemDataBound", ex);
            }
        }

        protected void OnBuscarClick(object sender, EventArgs e)
        {
            try
            {
                using (var db = new IsomanagerContext())
                {
                    var busqueda = txtBuscar.Text?.Trim().ToLower() ?? "";
                    System.Diagnostics.Debug.WriteLine($"Iniciando búsqueda con término: {busqueda}");
                    
                    var normasBasicas = db.Norma
                        .Include(n => n.Responsable)
                        .AsNoTracking()
                        .Where(n => n.Titulo.ToLower().Contains(busqueda) ||
                                  (n.Descripcion != null && n.Descripcion.ToLower().Contains(busqueda)))
                        .ToList();

                    if (!normasBasicas.Any())
                    {
                        pnlData.Visible = false;
                        pnlNoData.Visible = true;
                        MostrarMensaje($"No se encontraron normas que coincidan con '{txtBuscar.Text}'", TipoMensaje.Info);
                        return;
                    }

                    System.Diagnostics.Debug.WriteLine($"Normas encontradas: {normasBasicas.Count}");

                    var normasFiltradas = new List<object>();
                    foreach (var n in normasBasicas)
                    {
                        try
                        {
                            var stats = (from p in db.Procesos
                                       where p.NormaId == n.NormaId
                                       group p by p.NormaId into g
                                       select new
                                       {
                                           TotalProcesos = g.Count(),
                                           ProcesosCumplidos = g.Count(p => p.Estado == "Completado")
                                       }).FirstOrDefault() ?? new { TotalProcesos = 0, ProcesosCumplidos = 0 };

                            var totalDocumentos = db.Documentos
                                .Count(d => d.SeccionId == n.NormaId && d.Seccion == "Norma" && d.Activo);

                            normasFiltradas.Add(new
                            {
                                n.NormaId,
                                Nombre = n.Titulo ?? "Sin título",
                                Version = n.Version ?? "1.0",
                                Estado = n.Estado ?? "En Progreso",
                                UltimaActualizacion = n.UltimaActualizacion.ToString("dd/MM/yyyy"),
                                Responsable = n.Responsable?.Nombre ?? "Sin asignar",
                                TotalProcesos = stats.TotalProcesos,
                                TotalDocumentos = totalDocumentos,
                                Progreso = stats.TotalProcesos == 0 ? 0 : 
                                    (int)((double)stats.ProcesosCumplidos / stats.TotalProcesos * 100)
                            });
                        }
                        catch (Exception ex)
                        {
                            LogError($"Procesando norma en búsqueda {n.NormaId}", ex);
                        }
                    }

                    if (normasFiltradas.Count > 0)
                    {
                        rptNormas.DataSource = normasFiltradas;
                        rptNormas.DataBind();
                        pnlData.Visible = true;
                        pnlNoData.Visible = false;
                    }
                    else
                    {
                        pnlData.Visible = false;
                        pnlNoData.Visible = true;
                        MostrarMensaje("No se encontraron resultados.", TipoMensaje.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("OnBuscarClick", ex);
                MostrarMensaje("Error al buscar las normas: " + ex.Message, TipoMensaje.Error);
            }
        }

        protected void OnIrProcesosClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (LinkButton)sender;
                int normaId = Convert.ToInt32(btn.CommandArgument);
                
                System.Diagnostics.Debug.WriteLine($"=== OnIrProcesosClick ===");
                System.Diagnostics.Debug.WriteLine($"NormaId: {normaId}");

                string url = $"~/Pages/Procesos/GestionProcesos.aspx?NormaId={normaId}";
                System.Diagnostics.Debug.WriteLine($"URL antes de resolver: {url}");
                
                string resolvedUrl = ResolveUrl(url);
                System.Diagnostics.Debug.WriteLine($"URL resuelta: {resolvedUrl}");
                
                System.Diagnostics.Debug.WriteLine($"Iniciando redirección...");
                Response.Redirect(resolvedUrl);
            }
            catch (ThreadAbortException)
            {
                // Esta excepción es normal durante una redirección
                System.Diagnostics.Debug.WriteLine("Redirección en progreso (ThreadAbortException esperada)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnIrProcesosClick: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                MostrarError("Error al navegar a la página de procesos.");
            }
        }

        protected void OnIrDocumentosClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (LinkButton)sender;
                var normaId = btn.CommandArgument;
                Response.Redirect($"~/Pages/Documentos/GestionArchivos.aspx?normaId={normaId}");
            }
            catch (Exception ex)
            {
                LogError("OnIrDocumentosClick", ex);
                MostrarMensaje("Error al navegar a la página de documentos.", TipoMensaje.Error);
            }
        }

        protected void rptNormas1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "IrProcesos")
                {
                    int normaId = Convert.ToInt32(e.CommandArgument);
                    System.Diagnostics.Debug.WriteLine($"=== rptNormas1_ItemCommand ===");
                    System.Diagnostics.Debug.WriteLine($"NormaId: {normaId}");

                    string url = $"~/Pages/Procesos/GestionProcesos.aspx?NormaId={normaId}";
                    System.Diagnostics.Debug.WriteLine($"URL antes de resolver: {url}");
                    
                    string resolvedUrl = ResolveUrl(url);
                    System.Diagnostics.Debug.WriteLine($"URL resuelta: {resolvedUrl}");
                    
                    System.Diagnostics.Debug.WriteLine($"Iniciando redirección...");
                    Response.Redirect(resolvedUrl);
                }
            }
            catch (ThreadAbortException)
            {
                // Esta excepción es normal durante una redirección
                System.Diagnostics.Debug.WriteLine("Redirección en progreso (ThreadAbortException esperada)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en rptNormas1_ItemCommand: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                MostrarError("Error al navegar a la página de procesos.");
            }
        }

        protected void OnIrComponentesClick(object sender, EventArgs e)
        {
            try
            {
                var btn = (LinkButton)sender;
                var normaId = btn.CommandArgument;
                Response.Redirect($"~/Pages/Normas/ComponentesNorma.aspx?NormaId={normaId}");
            }
            catch (Exception ex)
            {
                LogError("OnIrComponentesClick", ex);
                MostrarError("Error al navegar a la estructura de la norma.");
            }
        }
    }
} 