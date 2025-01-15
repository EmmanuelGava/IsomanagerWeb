using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace IsomanagerWeb.Pages.Normas
{
    public partial class GestionNormas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarNormas();
            }
        }

        private void CargarNormas()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    IQueryable<Norma> query = context.Normas
                        .Include(n => n.Responsable)
                        .OrderByDescending(n => n.UltimaActualizacion);

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(n => 
                            n.Titulo.ToLower().Contains(filtro) ||
                            n.Version.ToLower().Contains(filtro) ||
                            n.Estado.ToLower().Contains(filtro) ||
                            n.Responsable.Nombre.ToLower().Contains(filtro)
                        ).OrderByDescending(n => n.UltimaActualizacion);
                    }

                    var normas = query.Select(n => new
                    {
                        n.NormaId,
                        n.Titulo,
                        n.Descripcion,
                        n.Version,
                        n.Estado,
                        FechaImplementacion = n.FechaCreacion,
                        ProximaAuditoria = SqlFunctions.DateAdd("month", 6, n.UltimaActualizacion),
                        Responsable = n.Responsable.Nombre
                    }).ToList();

                    gvNormas.DataSource = normas;
                    gvNormas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar las normas: " + ex.Message;
                lblMensaje.CssClass = "alert alert-danger";
            }
        }

        protected void btnNuevaNorma_Click(object sender, EventArgs e)
        {
            // Limpiar campos del modal
            LimpiarModal();
            litTituloModal.Text = "Nueva Norma";
            hdnNormaId.Value = "";

            // Mostrar el modal
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                "setTimeout(function() { showModal(); }, 100);", true);
        }

        protected void btnGuardarNorma_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var context = new IsomanagerContext())
                    {
                        if (!ucResponsable.SelectedUsuarioId.HasValue)
                        {
                            lblErrorModal.Text = "Error: Debe seleccionar un responsable.";
                            lblErrorModal.Visible = true;
                            return;
                        }

                        bool esNueva = string.IsNullOrEmpty(hdnNormaId.Value);
                        Norma norma;

                        if (esNueva)
                        {
                            norma = new Norma
                            {
                                FechaCreacion = DateTime.Now
                            };
                            context.Normas.Add(norma);
                        }
                        else
                        {
                            int normaId = Convert.ToInt32(hdnNormaId.Value);
                            norma = context.Normas.Find(normaId);
                            if (norma == null)
                            {
                                lblErrorModal.Text = "Error: No se encontró la norma a editar.";
                                lblErrorModal.Visible = true;
                                return;
                            }
                        }

                        // Actualizar propiedades
                        norma.Titulo = txtTitulo.Text.Trim();
                        norma.Descripcion = txtDescripcion.Text.Trim();
                        norma.Version = txtVersion.Text.Trim();
                        norma.Estado = ddlEstado.SelectedValue;
                        norma.ResponsableId = ucResponsable.SelectedUsuarioId.Value;
                        norma.UltimaActualizacion = DateTime.Now;

                        context.SaveChanges();

                        // Actualizar contador en sesión para el dashboard
                        Session["NormasCount"] = context.Normas.Count();
                        Session["LastNormaUpdate"] = DateTime.Now;

                        // Cerrar modal y mostrar mensaje de éxito
                        string mensaje = esNueva ? "Norma creada exitosamente." : "Norma actualizada exitosamente.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "hideModal",
                            $"hideModal(); toastr.success('{mensaje}');", true);

                        // Recargar la grilla
                        CargarNormas();
                    }
                }
                catch (Exception ex)
                {
                    lblErrorModal.Text = "Error al guardar la norma: " + ex.Message;
                    lblErrorModal.Visible = true;
                }
            }
        }

        protected void gvNormas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int normaId = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Editar":
                    CargarNormaParaEditar(normaId);
                    break;

                case "Eliminar":
                    try
                    {
                        using (var context = new IsomanagerContext())
                        {
                            var norma = context.Normas.Find(normaId);
                            if (norma != null)
                            {
                                context.Normas.Remove(norma);
                                context.SaveChanges();
                                CargarNormas();
                                
                                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                                    "toastr.success('Norma eliminada exitosamente.');", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.Text = "Error al eliminar la norma: " + ex.Message;
                        lblMensaje.CssClass = "alert alert-danger";
                    }
                    break;
            }
        }

        private void CargarNormaParaEditar(int normaId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Normas  
                        .Include(n => n.Responsable)
                        .FirstOrDefault(n => n.NormaId == normaId);

                    if (norma != null)
                    {
                        // Limpiar y cargar el modal
                        LimpiarModal();
                        
                        // Establecer valores
                        hdnNormaId.Value = norma.NormaId.ToString();
                        txtTitulo.Text = norma.Titulo;
                        txtDescripcion.Text = norma.Descripcion;
                        txtVersion.Text = norma.Version;
                        ddlEstado.SelectedValue = norma.Estado;
                        ucResponsable.SelectedUsuarioId = norma.ResponsableId;
                        
                        // Actualizar título del modal
                        litTituloModal.Text = "Editar Norma";

                        // Mostrar el modal
                        ScriptManager.RegisterStartupScript(this, GetType(), "showModal",
                            "showModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar la norma para editar: " + ex.Message;
                lblMensaje.CssClass = "alert alert-danger";
            }
        }

        private void LimpiarModal()
        {
            txtTitulo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtVersion.Text = string.Empty;
            ddlEstado.SelectedIndex = 0;
            ucResponsable.SelectedUsuarioId = null;
            lblErrorModal.Visible = false;
            hdnNormaId.Value = "";
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado?.ToLower())
            {
                case "borrador":
                    return "bg-secondary";
                case "en revisión":
                    return "bg-warning";
                case "aprobado":
                    return "bg-success";
                case "obsoleto":
                    return "bg-danger";
                default:
                    return "bg-secondary";
            }
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarNormas();
        }
    }
} 