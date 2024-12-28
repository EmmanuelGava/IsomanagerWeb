using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Pages.Documentos
{
    public partial class GestionArchivos : Page
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
            if (!IsPostBack)
            {
                CargarDatosNorma();
                CargarContadores();
                CargarArchivos();
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
                    litTituloPagina.Text = "Gestión de Archivos";
                    litNormaTitulo.Text = norma.Titulo;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los datos de la norma: {ex.Message}");
            }
        }

        private void CargarContadores()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var archivos = context.Archivos.Where(a => a.Proceso.NormaId == NormaId);
                    litActivos.Text = archivos.Count(a => a.Estado == "Activo").ToString();
                    litEnRevision.Text = archivos.Count(a => a.Estado == "En Revisión").ToString();
                    litObsoletos.Text = archivos.Count(a => a.Estado == "Obsoleto").ToString();
                    litPendientes.Text = archivos.Count(a => a.Estado == "Pendiente").ToString();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar contadores: {ex.Message}");
            }
        }

        private void CargarArchivos()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var query = context.Archivos
                        .Include(a => a.Proceso)
                        .Include(a => a.Creador)
                        .Where(a => a.Proceso.NormaId == NormaId)
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLowerInvariant();
                        query = query.Where(a =>
                            a.Nombre.ToLower().Contains(filtro) ||
                            a.Proceso.Nombre.ToLower().Contains(filtro) ||
                            a.Version.ToLower().Contains(filtro) ||
                            a.Creador.Nombre.ToLower().Contains(filtro) ||
                            a.Estado.ToLower().Contains(filtro));
                    }

                    var archivos = query
                        .OrderByDescending(a => a.FechaCreacion)
                        .Select(a => new
                        {
                            a.ArchivoId,
                            Proceso = a.Proceso.Nombre,
                            a.Nombre,
                            a.Version,
                            a.FechaCreacion,
                            Creador = a.Creador.Nombre,
                            a.Estado
                        })
                        .ToList();

                    gvArchivos.DataSource = archivos;
                    gvArchivos.DataBind();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los archivos: {ex.Message}");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Procesos/GestionProcesos.aspx?NormaId={NormaId}");
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarArchivos();
        }

        protected void btnNuevoArchivo_Click(object sender, EventArgs e)
        {
            // Implementar lógica para nuevo archivo
        }

        protected void gvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                // Implementar lógica para editar
            }
            else if (e.CommandName == "Descargar")
            {
                // Implementar lógica para descargar
            }
            else if (e.CommandName == "Ver")
            {
                // Implementar lógica para ver detalles
            }
            else if (e.CommandName == "Eliminar")
            {
                // Implementar lógica para eliminar
            }
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado.ToLowerInvariant())
            {
                case "activo":
                    return "bg-success";
                case "en revisión":
                    return "bg-warning";
                case "obsoleto":
                    return "bg-danger";
                case "pendiente":
                    return "bg-info";
                default:
                    return "bg-secondary";
            }
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-danger";
            lblMensaje.Visible = true;
        }
    }
} 