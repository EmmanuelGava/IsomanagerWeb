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
                        .OrderByDescending(n => n.UltimaModificacion);

                    // Aplicar filtro de búsqueda si existe
                    if (!string.IsNullOrEmpty(txtBuscar.Text))
                    {
                        string filtro = txtBuscar.Text.ToLower();
                        query = query.Where(n => 
                            n.Titulo.ToLower().Contains(filtro) ||
                            n.Version.ToLower().Contains(filtro) ||
                            n.Estado.ToLower().Contains(filtro) ||
                            n.Responsable.Nombre.ToLower().Contains(filtro)
                        ).OrderByDescending(n => n.UltimaModificacion);
                    }

                    var normas = query.Select(n => new
                    {
                        n.NormaId,
                        n.TipoNorma,
                        n.Titulo,
                        n.Descripcion,
                        n.Version,
                        n.Estado,
                        FechaImplementacion = n.FechaCreacion,
                        ProximaAuditoria = SqlFunctions.DateAdd("month", 6, n.UltimaModificacion),
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

        protected void ddlTipoNorma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTipoNorma.SelectedValue))
            {
                // Establecer descripción predeterminada según la norma seleccionada
                switch (ddlTipoNorma.SelectedValue)
                {
                    case "ISO 9001":
                        txtDescripcion.Text = "Sistema de Gestión de Calidad que proporciona la infraestructura, procedimientos, procesos y recursos necesarios para ayudar a las organizaciones a controlar y mejorar su rendimiento y conducirles hacia la eficiencia.";
                        break;
                    case "ISO 14001":
                        txtDescripcion.Text = "Sistema de Gestión Ambiental que ayuda a identificar, priorizar y gestionar los riesgos ambientales como parte de sus prácticas habituales.";
                        break;
                    case "ISO 45001":
                        txtDescripcion.Text = "Sistema de Gestión de Seguridad y Salud en el Trabajo que ayuda a las organizaciones a mejorar su desempeño en prevención de lesiones y enfermedades.";
                        break;
                }
            }
        }

        protected void btnGuardarNorma_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var db = new IsomanagerContext())
                    {
                        bool esNuevo = string.IsNullOrEmpty(hdnNormaId.Value);
                        Norma norma;

                        if (esNuevo)
                        {
                            norma = new Norma
                            {
                                Titulo = ddlTipoNorma.SelectedItem.Text,
                                Descripcion = txtDescripcion.Text,
                                Version = txtVersion.Text,
                                Estado = ddlEstado.SelectedValue,
                                ResponsableId = ucResponsable.SelectedUsuarioId.Value,
                                FechaCreacion = DateTime.Now,
                                UltimaModificacion = DateTime.Now,
                                TipoNorma = ddlTipoNorma.SelectedValue
                            };
                            db.Normas.Add(norma);
                        }
                        else
                        {
                            int normaId = Convert.ToInt32(hdnNormaId.Value);
                            norma = db.Normas.Find(normaId);
                            if (norma == null)
                            {
                                MostrarError("La norma no fue encontrada.");
                                return;
                            }

                            norma.Titulo = ddlTipoNorma.SelectedItem.Text;
                            norma.Descripcion = txtDescripcion.Text;
                            norma.Version = txtVersion.Text;
                            norma.Estado = ddlEstado.SelectedValue;
                            norma.ResponsableId = ucResponsable.SelectedUsuarioId.Value;
                            norma.UltimaModificacion = DateTime.Now;
                            norma.TipoNorma = ddlTipoNorma.SelectedValue;
                        }

                        db.SaveChanges();
                        CargarNormas();
                        LimpiarModal();

                        // Cerrar el modal y mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "hideModal", "hideModal();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", 
                            $"showSuccessMessage('Norma {(esNuevo ? "creada" : "actualizada")} exitosamente.');", true);
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al {(string.IsNullOrEmpty(hdnNormaId.Value) ? "crear" : "actualizar")} la norma: {ex.Message}");
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
                        ddlTipoNorma.SelectedValue = norma.TipoNorma;
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
            hdnNormaId.Value = "";
            ddlTipoNorma.SelectedIndex = 0;
            txtDescripcion.Text = "";
            txtVersion.Text = "";
            ddlEstado.SelectedIndex = 0;
            ucResponsable.SelectedUsuarioId = null;
            lblErrorModal.Visible = false;
        }

        private void MostrarError(string mensaje)
        {
            lblErrorModal.Text = mensaje;
            lblErrorModal.Visible = true;
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