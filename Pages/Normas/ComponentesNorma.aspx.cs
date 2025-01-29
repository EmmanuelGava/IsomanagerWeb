using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;
using System.Linq;
using System.Data.Entity.SqlServer;

namespace IsomanagerWeb.Pages.Procesos.Normas
{
    public partial class ComponentesNorma : Page
    {
        private IsomanagerContext db = new IsomanagerContext();
        protected int NormaId { get; private set; }

        private int ObtenerNormaId()
        {
            int id;
            if (int.TryParse(Request.QueryString["NormaId"], out id))
            {
                return id;
            }
            return 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NormaId = ObtenerNormaId();
                CargarDatosNorma();
                this.DataBind();
            }
        }

        private void CargarDatosNorma()
        {
            try
            {
                var norma = db.Normas.FirstOrDefault(n => n.NormaId == NormaId);
                if (norma != null)
                {
                    litTituloNorma.Text = norma.Titulo ?? "Sin título";
                    litVersion.Text = norma.Version ?? "1.0";
                    litFecha.Text = norma.UltimaModificacion.ToString("dd/MM/yyyy");
                }
                else
                {
                    // Si no se encuentra la norma, mostrar valores por defecto
                    litTituloNorma.Text = "Norma no encontrada";
                    litVersion.Text = "N/A";
                    litFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    System.Diagnostics.Debug.WriteLine($"No se encontró la norma con ID: {NormaId}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos de la norma: {ex.Message}");
                litTituloNorma.Text = "Error al cargar la norma";
                litVersion.Text = "N/A";
                litFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Normas/ListaNormas.aspx");
        }

        protected void lnkDatosPartida_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/DatosPartida.aspx?NormaId={currentNormaId}");
        }

        protected void lnkContextoOrg_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/ContextoOrganizacion.aspx?NormaId={currentNormaId}");
        }

        protected void lnkRecursosHumanos_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/RecursosHumanos.aspx?NormaId={currentNormaId}");
        }

        protected void lnkInfraestructura_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Infraestructura.aspx?NormaId={currentNormaId}");
        }

        protected void lnkProcesosProductivos_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/ProcesosProductivos.aspx?NormaId={currentNormaId}");
        }

        protected void lnkControlOperacional_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/ControlOperacional.aspx?NormaId={currentNormaId}");
        }

        protected void lnkDocumentacion_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Documentacion.aspx?NormaId={currentNormaId}");
        }

        protected void lnkCompras_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Compras.aspx?NormaId={currentNormaId}");
        }

        protected void lnkIDI_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/IDI.aspx?NormaId={currentNormaId}");
        }

        protected void lnkPlanificacion_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Planificacion.aspx?NormaId={currentNormaId}");
        }

        protected void lnkIncidentes_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Incidentes.aspx?NormaId={currentNormaId}");
        }

        protected void lnkPlanesEmergencia_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/PlanesEmergencia.aspx?NormaId={currentNormaId}");
        }

        protected void lnkNoConformidades_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/NoConformidades.aspx?NormaId={currentNormaId}");
        }

        protected void lnkComunicacion_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Comunicacion.aspx?NormaId={currentNormaId}");
        }

        protected void lnkAuditoria_Click(object sender, EventArgs e)
        {
            int currentNormaId = NormaId > 0 ? NormaId : ObtenerNormaId();
            Response.Redirect($"~/Pages/Normas/Secciones/Auditoria.aspx?NormaId={currentNormaId}");
        }
    }
} 