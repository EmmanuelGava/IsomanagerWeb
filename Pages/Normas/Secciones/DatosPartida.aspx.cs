using System;
using System.Web.UI;
using System.Linq;
using IsomanagerWeb.Models;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace IsomanagerWeb.Pages.Normas.Secciones
{
    public partial class DatosPartida : Page
    {
        private IsomanagerContext db = new IsomanagerContext();
        private int normaId;
        private bool modoEdicion = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ObtenerNormaId())
            {
                MostrarMensaje("Error: ID de norma no válido", "danger");
                Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            CargarTiposFactores();
            CargarFactoresExternos();
        }

        private void CargarTiposFactores()
        {
            var tipos = db.TiposFactores.Where(t => t.Activo).OrderBy(t => t.Nombre).ToList();
            ddlTipoFactor.DataSource = tipos;
            ddlTipoFactor.DataTextField = "Nombre";
            ddlTipoFactor.DataValueField = "TipoFactorId";
            ddlTipoFactor.DataBind();
            ddlTipoFactor.Items.Insert(0, new ListItem("Seleccione...", ""));
        }

        protected void cvAreas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ucAreas.GetSelectedAreaIds().Length > 0;
        }

        private void CargarFactor(int factorId)
        {
            var factor = db.FactoresExternos
                .Include(f => f.TipoFactor)
                .Include(f => f.Areas)
                .FirstOrDefault(f => f.FactorId == factorId);

            if (factor != null)
            {
                ViewState["FactorId"] = factorId;
                txtNombreFactor.Text = factor.Nombre;
                txtDescripcionFactor.Text = factor.Descripcion;
                ddlTipoFactor.SelectedValue = factor.TipoFactorId.ToString();
                ddlImpacto.SelectedValue = factor.Impacto.ToString();
                ddlProbabilidad.SelectedValue = factor.Probabilidad.ToString();
                txtFechaIdentificacion.Text = factor.FechaIdentificacion.ToString("yyyy-MM-dd");
                txtAccionesRecomendadas.Text = factor.AccionesRecomendadas;

                // Seleccionar las áreas del factor
                ucAreas.SetSelectedAreaIds(factor.Areas.Select(a => a.AreaId).ToArray());
            }
        }

        protected void btnGuardarFactor_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int? factorId = ViewState["FactorId"] as int?;
                var factor = factorId.HasValue ? 
                    db.FactoresExternos.Include(f => f.Areas).FirstOrDefault(f => f.FactorId == factorId.Value) : 
                    new FactoresExternos { NormaId = normaId, Activo = true };

                factor.Nombre = txtNombreFactor.Text;
                factor.Descripcion = txtDescripcionFactor.Text;
                factor.TipoFactorId = Convert.ToInt32(ddlTipoFactor.SelectedValue);
                factor.Impacto = Convert.ToInt32(ddlImpacto.SelectedValue);
                factor.Probabilidad = Convert.ToInt32(ddlProbabilidad.SelectedValue);
                factor.FechaIdentificacion = Convert.ToDateTime(txtFechaIdentificacion.Text);
                factor.AccionesRecomendadas = txtAccionesRecomendadas.Text;

                // Actualizar áreas seleccionadas
                factor.Areas.Clear();
                foreach (int areaId in ucAreas.GetSelectedAreaIds())
                {
                    var area = db.Areas.Find(areaId);
                    if (area != null)
                        factor.Areas.Add(area);
                }

                if (!factorId.HasValue)
                    db.FactoresExternos.Add(factor);

                db.SaveChanges();
                CargarFactoresExternos();
                LimpiarFormularioFactor();
                MostrarMensaje("Factor guardado correctamente", "success");
                ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "hideForm();", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar el factor: " + ex.Message, "danger");
            }
        }

        private void LimpiarFormularioFactor()
        {
            ViewState["FactorId"] = null;
            txtNombreFactor.Text = string.Empty;
            txtDescripcionFactor.Text = string.Empty;
            ddlTipoFactor.SelectedIndex = 0;
            ddlImpacto.SelectedIndex = 1; // Medio por defecto
            ddlProbabilidad.SelectedIndex = 1; // Media por defecto
            txtFechaIdentificacion.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtAccionesRecomendadas.Text = string.Empty;
            ucAreas.SetSelectedAreaIds(new int[0]);
        }

        private void ConfigurarModoVisualizacion()
        {
            modoEdicion = false;
            txtFortalezas.Enabled = false;
            txtOportunidades.Enabled = false;
            txtDebilidades.Enabled = false;
            txtAmenazas.Enabled = false;
            btnGuardar.Visible = false;
            btnEditar.Visible = true;
            btnCancelar.Visible = false;
            btnBorrar.Visible = true;
        }

        private void ConfigurarModoEdicion()
        {
            modoEdicion = true;
            txtFortalezas.Enabled = true;
            txtOportunidades.Enabled = true;
            txtDebilidades.Enabled = true;
            txtAmenazas.Enabled = true;
            btnGuardar.Visible = true;
            btnEditar.Visible = false;
            btnCancelar.Visible = true;
            btnBorrar.Visible = false;
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            ConfigurarModoEdicion();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarFODA();
            ConfigurarModoVisualizacion();
        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                var foda = db.Fodas.FirstOrDefault(f => f.NormaId == normaId);
                if (foda != null)
                {
                    db.Fodas.Remove(foda);
                    db.SaveChanges();
                    LimpiarCampos();
                    MostrarMensaje("El FODA ha sido eliminado correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al eliminar el FODA: {ex.Message}", "danger");
                System.Diagnostics.Debug.WriteLine($"Error completo: {ex.ToString()}");
            }
        }

        private void LimpiarCampos()
        {
            txtFortalezas.Text = string.Empty;
            txtOportunidades.Text = string.Empty;
            txtDebilidades.Text = string.Empty;
            txtAmenazas.Text = string.Empty;
        }

        private bool ObtenerNormaId()
        {
            if (ViewState["NormaId"] != null)
            {
                normaId = (int)ViewState["NormaId"];
                return true;
            }

            if (int.TryParse(Request.QueryString["NormaId"], out normaId) && normaId > 0)
            {
                // Verificar que la norma existe
                if (db.Norma.Any(n => n.NormaId == normaId))
                {
                    ViewState["NormaId"] = normaId;
                    return true;
                }
            }
            return false;
        }

        private void CargarDatosNorma()
        {
            var norma = db.Norma.Find(normaId);
            if (norma != null)
            {
                litTituloNorma.Text = norma.Titulo;
                litDetallesNorma.Text = $"Versión {norma.Version} - {norma.UltimaActualizacion:dd/MM/yyyy}";
            }
        }

        private void CargarFODA()
        {
            var foda = db.Fodas.FirstOrDefault(f => f.NormaId == normaId);
            if (foda != null)
            {
                txtFortalezas.Text = foda.Fortalezas;
                txtOportunidades.Text = foda.Oportunidades;
                txtDebilidades.Text = foda.Debilidades;
                txtAmenazas.Text = foda.Amenazas;
                btnEditar.Visible = true;
                btnBorrar.Visible = true;
            }
            else
            {
                LimpiarCampos();
                ConfigurarModoEdicion();
                btnBorrar.Visible = false;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ObtenerNormaId())
            {
                MostrarMensaje("Error: ID de norma no válido", "danger");
                return;
            }

            try
            {
                var foda = db.Fodas.FirstOrDefault(f => f.NormaId == normaId);
                if (foda == null)
                {
                    foda = new Foda { NormaId = normaId };
                    db.Fodas.Add(foda);
                }

                foda.Fortalezas = txtFortalezas.Text;
                foda.Oportunidades = txtOportunidades.Text;
                foda.Debilidades = txtDebilidades.Text;
                foda.Amenazas = txtAmenazas.Text;

                db.SaveChanges();
                MostrarMensaje("Los cambios se guardaron correctamente", "success");
                ConfigurarModoVisualizacion();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar los cambios: {ex.Message}", "danger");
                System.Diagnostics.Debug.WriteLine($"Error completo: {ex.ToString()}");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            if (ObtenerNormaId())
            {
                Response.Redirect($"~/Pages/Normas/ComponentesNorma.aspx?NormaId={normaId}");
            }
            else
            {
                Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.CssClass = $"alert alert-{tipo} mt-3";
            litMensaje.Text = mensaje;
            pnlMensaje.Visible = true;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        #region Factores Externos
        protected void btnNuevoFactor_Click(object sender, EventArgs e)
        {
            LimpiarFormularioFactor();
            litTituloFactor.Text = "Nuevo Factor Externo";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "showModal();", true);
        }

        protected void gvFactores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarFactor")
            {
                int factorId = Convert.ToInt32(e.CommandArgument);
                CargarFactor(factorId);
                litTituloFactor.Text = "Editar Factor Externo";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "showModal();", true);
            }
            else if (e.CommandName == "EliminarFactor")
            {
                int factorId = Convert.ToInt32(e.CommandArgument);
                EliminarFactor(factorId);
            }
        }

        private void EliminarFactor(int factorId)
        {
            try
            {
                var factor = db.FactoresExternos.Find(factorId);
                if (factor != null)
                {
                    db.FactoresExternos.Remove(factor);
                    db.SaveChanges();
                    CargarFactoresExternos();
                    MostrarMensaje("Factor eliminado correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar el factor: " + ex.Message, "danger");
            }
        }

        private void CargarFactoresExternos()
        {
            var factores = db.FactoresExternos
                .Include(f => f.TipoFactor)
                .Where(f => f.NormaId == normaId)
                .OrderBy(f => f.TipoFactor.Categoria)
                .ThenBy(f => f.Nombre)
                .ToList();

            gvFactores.DataSource = factores;
            gvFactores.DataBind();
        }

        protected string GetCategoriaNombre(object categoria)
        {
            if (categoria == null) return string.Empty;
            switch (categoria.ToString())
            {
                case "P": return "Político";
                case "E": return "Económico";
                case "S": return "Social";
                case "T": return "Tecnológico";
                case "A": return "Ambiental";
                case "L": return "Legal";
                default: return string.Empty;
            }
        }

        protected string GetCategoriaBadgeClass(object categoria)
        {
            if (categoria == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (categoria.ToString())
            {
                case "P": return baseClass + "bg-primary";
                case "E": return baseClass + "bg-success";
                case "S": return baseClass + "bg-info";
                case "T": return baseClass + "bg-warning text-dark";
                case "A": return baseClass + "bg-success";
                case "L": return baseClass + "bg-secondary";
                default: return baseClass + "bg-secondary";
            }
        }

        protected string GetImpactoNombre(object impacto)
        {
            if (impacto == null) return string.Empty;
            switch (Convert.ToInt32(impacto))
            {
                case 0: return "Bajo";
                case 1: return "Medio";
                case 2: return "Alto";
                default: return string.Empty;
            }
        }

        protected string GetImpactoBadgeClass(object impacto)
        {
            if (impacto == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (Convert.ToInt32(impacto))
            {
                case 0: return baseClass + "bg-success";
                case 1: return baseClass + "bg-warning text-dark";
                case 2: return baseClass + "bg-danger";
                default: return baseClass + "bg-secondary";
            }
        }
        #endregion

        #region Alcance Sistema
        protected void btnEditarAlcance_Click(object sender, EventArgs e)
        {
            ConfigurarModoEdicionAlcance(true);
        }

        protected void btnBorrarAlcance_Click(object sender, EventArgs e)
        {
            try
            {
                var alcance = db.AlcanceSistemaGestion.FirstOrDefault(a => a.NormaId == normaId);
                if (alcance != null)
                {
                    db.AlcanceSistemaGestion.Remove(alcance);
                    db.SaveChanges();
                    MostrarMensaje("Alcance eliminado correctamente", "success");
                }
                LimpiarCamposAlcance();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar el alcance: " + ex.Message, "danger");
            }
        }

        protected void btnGuardarAlcance_Click(object sender, EventArgs e)
        {
            try
            {
                var alcance = db.AlcanceSistemaGestion.FirstOrDefault(a => a.NormaId == normaId) ?? new AlcanceSistemaGestion 
                { 
                    NormaId = normaId,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };
                
                alcance.Descripcion = txtAlcance.Text;
                alcance.UltimaModificacion = DateTime.Now;

                if (alcance.AlcanceId == 0)
                    db.AlcanceSistemaGestion.Add(alcance);

                db.SaveChanges();
                MostrarMensaje("Alcance guardado correctamente", "success");
                ConfigurarModoEdicionAlcance(false);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar el alcance: " + ex.Message, "danger");
            }
        }

        protected void btnCancelarAlcance_Click(object sender, EventArgs e)
        {
            CargarAlcance();
            ConfigurarModoEdicionAlcance(false);
        }

        private void ConfigurarModoEdicionAlcance(bool modoEdicion)
        {
            txtAlcance.Enabled = modoEdicion;

            btnEditarAlcance.Visible = !modoEdicion;
            btnBorrarAlcance.Visible = !modoEdicion;
            btnGuardarAlcance.Visible = modoEdicion;
            btnCancelarAlcance.Visible = modoEdicion;
        }

        private void LimpiarCamposAlcance()
        {
            txtAlcance.Text = string.Empty;
        }

        private void CargarAlcance()
        {
            try
            {
                var alcance = db.AlcanceSistemaGestion.FirstOrDefault(a => a.NormaId == normaId);
                if (alcance != null)
                {
                    txtAlcance.Text = alcance.Descripcion;
                }
                ConfigurarModoEdicionAlcance(false);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el alcance: " + ex.Message, "danger");
            }
        }
        #endregion

        protected void btnVolverAlcance_Click(object sender, EventArgs e)
        {
            if (ObtenerNormaId())
            {
                Response.Redirect($"~/Pages/Normas/ComponentesNorma.aspx?NormaId={normaId}");
            }
            else
            {
                Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
            }
        }

        protected void btnVolverFactores_Click(object sender, EventArgs e)
        {
            if (ObtenerNormaId())
            {
                Response.Redirect($"~/Pages/Normas/ComponentesNorma.aspx?NormaId={normaId}");
            }
            else
            {
                Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
            }
        }

        protected void btnCancelarFactores_Click(object sender, EventArgs e)
        {
            CargarFactoresExternos();
            ScriptManager.RegisterStartupScript(this, GetType(), "HideModal", "hideModal();", true);
        }

        protected void btnGuardarFactores_Click(object sender, EventArgs e)
        {
            try
            {
                db.SaveChanges();
                CargarFactoresExternos();
                MostrarMensaje("Los cambios se guardaron correctamente", "success");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar los cambios: {ex.Message}", "danger");
                System.Diagnostics.Debug.WriteLine($"Error completo: {ex.ToString()}");
            }
        }
    }
} 
