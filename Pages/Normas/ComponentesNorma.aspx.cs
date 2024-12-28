using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;
using System.Linq;

namespace IsomanagerWeb.Pages.Procesos.Normas
{
    public partial class ComponentesNorma : Page
    {
        private IsomanagerContext db = new IsomanagerContext();
        private int normaId;

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
                int normaId = ObtenerNormaId();
                if (normaId > 0)
                {
                    CargarDatosNorma(normaId);
                }
                else
                {
                    Response.Redirect("~/Pages/Procesos/GestionProcesos.aspx");
                }
            }
        }

        private void CargarDatosNorma(int normaId)
        {
            try
            {
                var norma = db.Norma.FirstOrDefault(n => n.NormaId == normaId);
                if (norma != null)
                {
                    // Mostrar los datos en la UI
                    litTituloNorma.Text = norma.Titulo ?? "Sin título";
                    litVersion.Text = norma.Version ?? "1.0";
                    litFecha.Text = norma.UltimaActualizacion.ToString("dd/MM/yyyy");

                    // Solo actualizamos si realmente hay campos que necesitan ser actualizados
                    bool requiereActualizacion = false;

                    if (string.IsNullOrEmpty(norma.Estado))
                    {
                        norma.Estado = "Activo";
                        requiereActualizacion = true;
                    }

                    if (norma.FechaCreacion == default(DateTime))
                    {
                        norma.FechaCreacion = DateTime.Now;
                        requiereActualizacion = true;
                    }

                    if (norma.UltimaActualizacion == default(DateTime))
                    {
                        norma.UltimaActualizacion = DateTime.Now;
                        requiereActualizacion = true;
                    }

                    // Solo guardamos si hubo cambios
                    if (requiereActualizacion)
                    {
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                        {
                            // Si falla la actualización, no es crítico para mostrar la página
                            System.Diagnostics.Debug.WriteLine($"Error al actualizar campos por defecto: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Pages/Procesos/GestionProcesos.aspx");
                }
            }
            catch (Exception ex)
            {
                // Registrar el error para debugging
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos de norma: {ex.Message}");
                Response.Redirect("~/Pages/Procesos/GestionProcesos.aspx");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Procesos/GestionProcesos.aspx");
        }

        protected void lnkDatosPartida_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/DatosPartida.aspx?NormaId={normaId}");
            }
        }

        protected void lnkRecursosHumanos_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/RecursosHumanos.aspx?NormaId={normaId}");
            }
        }

        protected void lnkInfraestructura_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Infraestructura.aspx?NormaId={normaId}");
            }
        }

        protected void lnkRealizacion_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Realizacion.aspx?NormaId={normaId}");
            }
        }

        protected void lnkDocumentacion_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Documentacion.aspx?NormaId={normaId}");
            }
        }

        protected void lnkCompras_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Compras.aspx?NormaId={normaId}");
            }
        }

        protected void lnkIDI_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/IDI.aspx?NormaId={normaId}");
            }
        }

        protected void lnkPlanificacion_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Planificacion.aspx?NormaId={normaId}");
            }
        }

        protected void lnkControlOperacional_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/ControlOperacional.aspx?NormaId={normaId}");
            }
        }

        protected void lnkPlanesEmergencia_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/PlanesEmergencia.aspx?NormaId={normaId}");
            }
        }

        protected void lnkIncidentes_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Incidentes.aspx?NormaId={normaId}");
            }
        }

        protected void lnkComunicacion_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Comunicacion.aspx?NormaId={normaId}");
            }
        }

        protected void lnkNoConformidades_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/NoConformidades.aspx?NormaId={normaId}");
            }
        }

        protected void lnkAuditoria_Click(object sender, EventArgs e)
        {
            int normaId = ObtenerNormaId();
            if (normaId > 0)
            {
                Response.Redirect($"~/Pages/Normas/Secciones/Auditoria.aspx?NormaId={normaId}");
            }
        }
    }
} 