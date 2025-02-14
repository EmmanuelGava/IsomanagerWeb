using System;
using System.Linq;
using System.Web.UI;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Controls
{
    public partial class ucProximasAuditorias : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProximasAuditorias();
            }
        }

        private void CargarProximasAuditorias()
        {
            try
            {
                using (var db = new IsomanagerContext())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;

                    var fechaActual = DateTime.Today;
                    var fechaLimite = fechaActual.AddMonths(1);

                    var auditorias = db.AuditoriasInternaProceso
                        .Include(a => a.Proceso)
                        .Include(a => a.Asignado)
                        .Where(a => a.FechaAuditoria >= fechaActual && 
                               a.FechaAuditoria <= fechaLimite &&
                               a.Estado != "Completada" && 
                               a.Estado != "Cancelada")
                        .OrderBy(a => a.FechaAuditoria)
                        .Take(5)
                        .AsNoTracking()
                        .ToList();

                    rptAuditorias.DataSource = auditorias;
                    rptAuditorias.DataBind();

                    if (!auditorias.Any())
                    {
                        pnlNoAuditorias.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar auditorías: {ex.Message}");
                litError.Text = "Error al cargar las próximas auditorías";
                pnlError.Visible = true;
            }
        }

        protected string FormatearFecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy HH:mm");
        }

        protected string ObtenerClaseEstado(string estado)
        {
            return $"badge badge-{(estado?.ToLower() ?? "pendiente")}";
        }
    }
} 