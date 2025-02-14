using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Controls
{
    public partial class ucCalendario : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEventosCalendario();
            }
        }

        private void CargarEventosCalendario()
        {
            try
            {
                using (var db = new IsomanagerContext())
                {
                    var eventos = ObtenerAuditorias(db).Concat(ObtenerProcesos(db));

                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        DateFormatString = "yyyy-MM-dd",
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    var eventosJson = JsonConvert.SerializeObject(eventos, settings);
                    RegisterCalendarScripts(eventosJson);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar eventos: {ex.Message}");
            }
        }

        private IEnumerable<object> ObtenerAuditorias(IsomanagerContext db)
        {
            return db.AuditoriasInternaProceso
                .Include(a => a.Proceso)
                .Include(a => a.Asignado)
                .AsNoTracking()
                .ToList()
                .Select(a => new
                {
                    id = $"auditoria_{a.AuditoriaInternaProcesoId}",
                    title = a.Titulo,
                    start = a.FechaAuditoria.ToString("yyyy-MM-dd"),
                    className = $"fc-event-auditoria fc-event-auditoria-{a.Estado.ToLower().Replace(" ", "-")}",
                    extendedProps = new
                    {
                        tipo = "auditoria",
                        estado = a.Estado,
                        proceso = a.Proceso?.Nombre,
                        responsable = a.Asignado?.Nombre,
                        descripcion = a.Descripcion
                    }
                });
        }

        private IEnumerable<object> ObtenerProcesos(IsomanagerContext db)
        {
            return db.Procesos
                .Include(p => p.Responsable)
                .AsNoTracking()
                .ToList()
                .Select(p => new
                {
                    id = $"proceso_{p.ProcesoId}",
                    title = p.Nombre,
                    start = p.FechaCreacion.ToString("yyyy-MM-dd"),
                    className = "fc-event-proceso",
                    extendedProps = new
                    {
                        tipo = "proceso",
                        estado = p.Estado,
                        responsable = p.Responsable?.Nombre,
                        descripcion = p.Descripcion
                    }
                });
        }

        private void RegisterCalendarScripts(string eventosJson)
        {
            try
            {
                // Registrar los datos del calendario
                var dataScript = $@"
                    if (typeof window.calendarData === 'undefined') {{
                        window.calendarData = {eventosJson};
                        console.log('Datos del calendario cargados:', window.calendarData);
                    }}
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "CalendarData", dataScript, true);

                // Registrar el script de inicialización después de los datos
                var initScript = @"
                    if (typeof initializeCalendars === 'function') {
                        console.log('Inicializando calendarios...');
                        initializeCalendars();
                    } else {
                        console.error('La función initializeCalendars no está definida');
                    }
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "InitCalendars", initScript, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al registrar scripts del calendario: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
} 