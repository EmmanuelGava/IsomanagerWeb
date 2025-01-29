using System;
using System.Web.UI;
using System.Linq;
using IsomanagerWeb.Models;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace IsomanagerWeb.Pages.Normas.Secciones
{
    public partial class DatosPartida : Page
    {
        private IsomanagerContext db = new IsomanagerContext();
        private int normaId;
        private bool modoEdicion = false;
        protected Repeater rptDetalles;
        protected Repeater rptDetallesPartes;
        protected GridView gvPartesInteresadas;
        protected UpdatePanel upPartesInteresadas;
        protected Literal litTituloParte;
        protected TextBox txtNombreParte;
        protected DropDownList ddlTipoParte;
        protected DropDownList ddlCategoriaParte;
        protected TextBox txtIntereses;
        protected DropDownList ddlRelevancia;
        protected TextBox txtImpacto;
        protected TextBox txtAccionesParte;
        protected LinkButton btnEditar;
        protected LinkButton btnBorrar;

        // Controles de Factores Internos
        protected LinkButton btnNuevoFactorInterno;
        protected GridView gvFactoresInternos;
        protected Repeater rptDetallesFactoresInternos;
        protected UpdatePanel upFactoresInternos;
        protected UpdatePanel upFormFactorInterno;
        protected Literal litTituloFactorInterno;
        protected TextBox txtNombreFactorInterno;
        protected TextBox txtDescripcionFactorInterno;
        protected DropDownList ddlTipoFactorInterno;
        protected DropDownList ddlRelevanciaFactorInterno;
        protected TextBox txtComentariosFactorInterno;
        protected TextBox txtDesafiosFactorInterno;
        protected Controls.ucSeleccionAreas ucAreasFactorInterno;
        protected Button btnGuardarFactorInterno;

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
            CargarPartesInteresadas();
            CargarDatosNorma();
        }

        private void CargarTiposFactores()
        {
            var tipos = db.TiposFactores.Where(t => t.Activo && (
                t.Categoria == "R" || // Recursos Humanos
                t.Categoria == "I" || // Infraestructura
                t.Categoria == "P" || // Procesos
                t.Categoria == "F" || // Finanzas
                t.Categoria == "T"    // Tecnología
            ))
            .OrderBy(t => t.Nombre)
            .ToList();
            ddlTipoFactor.DataSource = tipos;
            ddlTipoFactor.DataTextField = "Nombre";
            ddlTipoFactor.DataValueField = "TipoFactorId";
            ddlTipoFactor.DataBind();
            ddlTipoFactor.Items.Insert(0, new ListItem("Seleccione...", ""));
        }

        protected void cvAreas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ucAreasFactorInterno.GetSelectedAreaIds().Length > 0;
        }

        private void CargarFactor(int factorId)
        {
            try
            {
                var factor = db.FactoresExternos
                    .Include(f => f.TipoFactor)
                    .Include(f => f.Areas)
                    .FirstOrDefault(f => f.FactorId == factorId);

                if (factor != null)
                {
                    ViewState["FactorId"] = factorId;
                    litTituloFactor.Text = "Editar Factor";
                    txtNombreFactor.Text = factor.Nombre;
                    txtDescripcionFactor.Text = factor.Descripcion;
                    ddlTipoFactor.SelectedValue = factor.TipoFactorId.ToString();
                    ddlImpacto.SelectedValue = factor.Impacto.ToString();
                    ddlProbabilidad.SelectedValue = factor.Probabilidad.ToString();
                    txtFechaIdentificacion.Text = factor.FechaIdentificacion.ToString("yyyy-MM-dd");
                    txtAccionesRecomendadas.Text = factor.AccionesRecomendadas;
                    ucAreas.SetSelectedAreaIds(factor.Areas.Select(a => a.AreaId).ToArray());

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowForm", "showForm();", true);
                }
                else
                {
                    MostrarError("No se encontró el factor especificado");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar el factor: {ex.Message}");
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

                factor.Nombre = txtNombreFactor.Text.Trim();
                factor.Descripcion = txtDescripcionFactor.Text.Trim();
                factor.TipoFactorId = Convert.ToInt32(ddlTipoFactor.SelectedValue);
                factor.Impacto = Convert.ToInt32(ddlImpacto.SelectedValue);
                factor.Probabilidad = Convert.ToInt32(ddlProbabilidad.SelectedValue);
                factor.FechaIdentificacion = Convert.ToDateTime(txtFechaIdentificacion.Text);
                factor.AccionesRecomendadas = txtAccionesRecomendadas.Text.Trim();

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

                // Limpiar y ocultar formulario
                LimpiarFormularioFactor();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "hideForm();", true);

                // Recargar grid
                CargarFactoresExternos();
                MostrarMensaje("Factor guardado correctamente", "success");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar el factor: {ex.Message}");
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
                if (db.Normas.Any(n => n.NormaId == normaId))
                {
                    ViewState["NormaId"] = normaId;
                    return true;
                }
            }
            return false;
        }

        private void CargarDatosNorma()
        {
            var norma = db.Normas.Find(normaId);
            if (norma != null)
            {
                litTituloNorma.Text = norma.Titulo;
                litDetallesNorma.Text = $"Versión {norma.Version} - {norma.UltimaModificacion:dd/MM/yyyy}";
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
            litTituloFactor.Text = "Nuevo Factor";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowForm", "showForm();", true);
        }

        protected void gvFactores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarFactor")
                {
                    int factorId = Convert.ToInt32(e.CommandArgument);
                    CargarFactor(factorId);
                }
                else if (e.CommandName == "EliminarFactor")
                {
                    int factorId = Convert.ToInt32(e.CommandArgument);
                    EliminarFactor(factorId);
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
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
            try
            {
                var factores = db.FactoresExternos
                    .Include(f => f.TipoFactor)
                    .Include(f => f.Areas)
                    .Where(f => f.NormaId == normaId)
                    .OrderBy(f => f.TipoFactor.Categoria)
                    .ThenBy(f => f.Nombre)
                    .ToList();

                gvFactores.DataSource = factores;
                gvFactores.DataBind();
                upFactores.Update();

                // Asignar la misma fuente de datos al Repeater
                rptDetalles.DataSource = factores;
                rptDetalles.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los factores: {ex.Message}");
            }
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

        protected string GetProbabilidadNombre(object probabilidad)
        {
            if (probabilidad == null) return string.Empty;
            switch (Convert.ToInt32(probabilidad))
            {
                case 0: return "Baja";
                case 1: return "Media";
                case 2: return "Alta";
                default: return string.Empty;
            }
        }

        protected string GetAreasAfectadas(object areas)
        {
            if (areas == null) return string.Empty;
            var areasList = areas as IEnumerable<Area>;
            if (areasList == null) return string.Empty;

            return string.Join("", areasList.Select(a => 
                $"<span class='badge bg-secondary me-1'>{a.Nombre}</span>"));
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

        protected void btnNuevaArea_Click(object sender, EventArgs e)
        {
            litTituloArea.Text = "Nueva Área";
            LimpiarFormularioArea();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowForm", "showFormArea();", true);
        }

        protected void btnCancelarArea_Click(object sender, EventArgs e)
        {
            LimpiarFormularioArea();
            ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "hideFormArea();", true);
        }

        protected void btnGuardarArea_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int ubicacionId;
                if (!int.TryParse(ddlUbicacion.SelectedValue, out ubicacionId))
                {
                    throw new Exception("Debe seleccionar una ubicación válida");
                }

                int? areaId = ViewState["AreaId"] as int?;
                var area = areaId.HasValue ? 
                    db.Areas.Find(areaId.Value) : 
                    new Area { 
                        Activo = true,
                        FechaCreacion = DateTime.Now
                    };

                area.Nombre = txtNombreArea.Text.Trim();
                area.Descripcion = txtDescripcionArea.Text.Trim();
                area.UbicacionId = ubicacionId;
                area.UltimaModificacion = DateTime.Now;

                if (!areaId.HasValue)
                    db.Areas.Add(area);

                db.SaveChanges();

                // Limpiar y ocultar formulario
                LimpiarFormularioArea();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "hideFormArea();", true);

                // Recargar grid
                CargarAreas();
                MostrarMensaje("Área guardada correctamente", "success");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar el área: {ex.Message}");
            }
        }

        protected void gvAreas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int areaId = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditarArea":
                        CargarArea(areaId);
                        break;

                    case "CambiarEstado":
                        CambiarEstadoArea(areaId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarArea(int areaId)
        {
            try 
            {
                var area = db.Areas
                    .Include(a => a.UbicacionGeografica)
                    .FirstOrDefault(a => a.AreaId == areaId);

                if (area != null)
                {
                    ViewState["AreaId"] = areaId;
                    litTituloArea.Text = "Editar Área";
                    txtNombreArea.Text = area.Nombre;
                    txtDescripcionArea.Text = area.Descripcion;
                    ddlUbicacion.SelectedValue = area.UbicacionId.ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowForm", "showFormArea();", true);
                }
                else 
                {
                    MostrarError("No se encontró el área especificada");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar el área: {ex.Message}");
            }
        }

        private void CambiarEstadoArea(int areaId)
        {
            var area = db.Areas.Find(areaId);
            if (area != null)
            {
                area.Activo = !area.Activo;
                area.UltimaModificacion = DateTime.Now;
                db.SaveChanges();
                CargarAreas();
            }
        }

        private void LimpiarFormularioArea()
        {
            txtNombreArea.Text = string.Empty;
            txtDescripcionArea.Text = string.Empty;
            ddlUbicacion.SelectedIndex = 0;
            ViewState["AreaId"] = null;
        }

        private void CargarAreas()
        {
            try
            {
                var areas = db.Areas
                    .Include(a => a.UbicacionGeografica)
                    .OrderBy(a => a.Nombre)
                    .ToList();

                gvAreas.DataSource = areas;
                gvAreas.DataBind();

                upAreas.Update();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las áreas: {ex.Message}");
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlMensaje.Visible = true;
            litMensaje.Text = $"<i class='bi bi-exclamation-triangle me-2'></i>{mensaje}";
            pnlMensaje.CssClass = "alert alert-danger";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                CargarUbicaciones();
                CargarAreas();
                CargarFactoresInternos();
            }
        }

        private void CargarUbicaciones()
        {
            try
            {
                var ubicaciones = db.UbicacionesGeograficas
                    .Where(u => u.Activo)
                    .OrderBy(u => u.Nombre)
                    .ToList();

                ddlUbicacion.DataSource = ubicaciones;
                ddlUbicacion.DataTextField = "Nombre";
                ddlUbicacion.DataValueField = "UbicacionId";
                ddlUbicacion.DataBind();

                ddlUbicacion.Items.Insert(0, new ListItem("Seleccione...", ""));
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las ubicaciones: {ex.Message}");
            }
        }

        #region Partes Interesadas
        protected void btnNuevaParte_Click(object sender, EventArgs e)
        {
            LimpiarFormularioParte();
            litTituloParte.Text = "Nueva Parte Interesada";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowFormParte", "showFormParte();", true);
        }

        protected void gvPartesInteresadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarParte")
                {
                    int parteId = Convert.ToInt32(e.CommandArgument);
                    CargarParte(parteId);
                }
                else if (e.CommandName == "EliminarParte")
                {
                    int parteId = Convert.ToInt32(e.CommandArgument);
                    EliminarParte(parteId);
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarParte(int parteId)
        {
            try
            {
                var parte = db.PartesInteresadas.Find(parteId);
                if (parte != null)
                {
                    ViewState["ParteID"] = parteId;
                    litTituloParte.Text = "Editar Parte Interesada";
                    txtNombreParte.Text = parte.Nombre;
                    ddlTipoParte.SelectedValue = parte.Tipo;
                    ddlCategoriaParte.SelectedValue = parte.Categoria;
                    txtIntereses.Text = parte.Intereses;
                    ddlRelevancia.SelectedValue = parte.Relevancia.ToString();
                    txtImpacto.Text = parte.Impacto;
                    txtAccionesParte.Text = parte.Acciones;

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowFormParte", "showFormParte();", true);
                }
                else
                {
                    MostrarError("No se encontró la parte interesada especificada");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la parte interesada: {ex.Message}");
            }
        }

        private void EliminarParte(int parteId)
        {
            try
            {
                var parte = db.PartesInteresadas.Find(parteId);
                if (parte != null)
                {
                    db.PartesInteresadas.Remove(parte);
                    db.SaveChanges();
                    CargarPartesInteresadas();
                    MostrarMensaje("Parte interesada eliminada correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al eliminar la parte interesada: {ex.Message}");
            }
        }

        protected void btnGuardarParte_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int? parteId = ViewState["ParteID"] as int?;
                var parte = parteId.HasValue ? 
                    db.PartesInteresadas.Find(parteId.Value) : 
                    new PartesInteresadas { NormaId = normaId };

                parte.Nombre = txtNombreParte.Text.Trim();
                parte.Tipo = ddlTipoParte.SelectedValue;
                parte.Categoria = ddlCategoriaParte.SelectedValue;
                parte.Intereses = txtIntereses.Text.Trim();
                parte.Relevancia = Convert.ToInt32(ddlRelevancia.SelectedValue);
                parte.Impacto = txtImpacto.Text.Trim();
                parte.Acciones = txtAccionesParte.Text.Trim();

                if (!parteId.HasValue)
                    db.PartesInteresadas.Add(parte);

                db.SaveChanges();

                LimpiarFormularioParte();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "hideFormParte();", true);
                CargarPartesInteresadas();
                MostrarMensaje("Parte interesada guardada correctamente", "success");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar la parte interesada: {ex.Message}");
            }
        }

        private void LimpiarFormularioParte()
        {
            ViewState["ParteID"] = null;
            txtNombreParte.Text = string.Empty;
            ddlTipoParte.SelectedIndex = 0;
            ddlCategoriaParte.SelectedIndex = 0;
            txtIntereses.Text = string.Empty;
            ddlRelevancia.SelectedIndex = 0;
            txtImpacto.Text = string.Empty;
            txtAccionesParte.Text = string.Empty;
        }

        private void CargarPartesInteresadas()
        {
            try
            {
                var partes = db.PartesInteresadas
                    .Where(p => p.NormaId == normaId)
                    .OrderBy(p => p.Nombre)
                    .ToList();

                gvPartesInteresadas.DataSource = partes;
                gvPartesInteresadas.DataBind();
                upPartesInteresadas.Update();

                // Asignar la misma fuente de datos al Repeater
                rptDetallesPartes.DataSource = partes;
                rptDetallesPartes.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar las partes interesadas: {ex.Message}");
            }
        }

        protected string GetTipoParteNombre(object tipo)
        {
            if (tipo == null) return string.Empty;
            switch (tipo.ToString())
            {
                case "I": return "Interna";
                case "E": return "Externa";
                default: return string.Empty;
            }
        }

        protected string GetTipoParteBadgeClass(object tipo)
        {
            if (tipo == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (tipo.ToString())
            {
                case "I": return baseClass + "bg-primary";
                case "E": return baseClass + "bg-info";
                default: return baseClass + "bg-secondary";
            }
        }

        protected string GetCategoriaParteNombre(object categoria)
        {
            if (categoria == null) return string.Empty;
            switch (categoria.ToString())
            {
                case "L": return "Legal";
                case "E": return "Económica";
                case "S": return "Social";
                case "T": return "Tecnológica";
                default: return string.Empty;
            }
        }

        protected string GetCategoriaParteBadgeClass(object categoria)
        {
            if (categoria == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (categoria.ToString())
            {
                case "L": return baseClass + "bg-warning text-dark";
                case "E": return baseClass + "bg-success";
                case "S": return baseClass + "bg-info";
                case "T": return baseClass + "bg-primary";
                default: return baseClass + "bg-secondary";
            }
        }

        protected string GetRelevanciaNombre(object relevancia)
        {
            if (relevancia == null) return string.Empty;
            switch (Convert.ToInt32(relevancia))
            {
                case 1: return "Baja";
                case 2: return "Media";
                case 3: return "Alta";
                default: return string.Empty;
            }
        }

        protected string GetRelevanciaBadgeClass(object relevancia)
        {
            if (relevancia == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (Convert.ToInt32(relevancia))
            {
                case 1: return baseClass + "bg-success";
                case 2: return baseClass + "bg-warning text-dark";
                case 3: return baseClass + "bg-danger";
                default: return baseClass + "bg-secondary";
            }
        }
        #endregion

        protected void btnEditarFoda_Click(object sender, EventArgs e)
        {
            ConfigurarModoEdicion();
        }

        protected void btnBorrarFoda_Click(object sender, EventArgs e)
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

        #region Factores Internos
        protected void btnNuevoFactorInterno_Click(object sender, EventArgs e)
        {
            LimpiarFormularioFactorInterno();
            litTituloFactorInterno.Text = "Nuevo Factor Interno";
            CargarTiposFactoresInternos();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowFormInterno", "showFormFactorInterno();", true);
        }

        private void CargarTiposFactoresInternos()
        {
            try
            {
                var tipos = db.TiposFactores
                    .Where(t => t.Activo)
                    .OrderBy(t => t.Nombre)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"Tipos de factores encontrados: {tipos.Count}");
                foreach (var tipo in tipos)
                {
                    System.Diagnostics.Debug.WriteLine($"Tipo: {tipo.Nombre}, Categoría: {tipo.Categoria}, Activo: {tipo.Activo}");
                }

                ddlTipoFactorInterno.DataSource = tipos;
                ddlTipoFactorInterno.DataTextField = "Nombre";
                ddlTipoFactorInterno.DataValueField = "TipoFactorId";
                ddlTipoFactorInterno.DataBind();
                ddlTipoFactorInterno.Items.Insert(0, new ListItem("Seleccione...", ""));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar tipos de factores: {ex.Message}");
                MostrarError($"Error al cargar los tipos de factores: {ex.Message}");
            }
        }

        protected void btnGuardarFactorInterno_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int? factorId = ViewState["FactorInternoId"] as int?;
                var factor = factorId.HasValue ? 
                    db.FactoresInternos.Include(f => f.Areas).FirstOrDefault(f => f.FactorInternoId == factorId.Value) : 
                    new FactoresInternos { 
                        NormaId = normaId,
                        CreadorId = ((Usuario)Session["Usuario"]).UsuarioId,
                        FechaCreacion = DateTime.Now,
                        Activo = true
                    };

                factor.Nombre = txtNombreFactorInterno.Text.Trim();
                factor.Descripcion = txtDescripcionFactorInterno.Text.Trim();
                factor.TipoFactorId = Convert.ToInt32(ddlTipoFactorInterno.SelectedValue);
                factor.Relevancia = Convert.ToInt32(ddlRelevanciaFactorInterno.SelectedValue);
                factor.Comentarios = txtComentariosFactorInterno.Text.Trim();
                factor.PosiblesDesafios = txtDesafiosFactorInterno.Text.Trim();
                factor.FechaIdentificacion = DateTime.Now;

                if (factorId.HasValue)
                {
                    factor.UltimoEditorId = ((Usuario)Session["Usuario"]).UsuarioId;
                    factor.UltimaModificacion = DateTime.Now;
                }

                // Actualizar áreas seleccionadas
                factor.Areas.Clear();
                foreach (int areaId in ucAreasFactorInterno.GetSelectedAreaIds())
                {
                    var area = db.Areas.Find(areaId);
                    if (area != null)
                        factor.Areas.Add(area);
                }

                if (!factorId.HasValue)
                    db.FactoresInternos.Add(factor);

                db.SaveChanges();

                // Limpiar y ocultar formulario
                LimpiarFormularioFactorInterno();
                ScriptManager.RegisterStartupScript(this, GetType(), "HideFormInterno", "hideFormFactorInterno();", true);

                // Recargar grid
                CargarFactoresInternos();
                MostrarMensaje("Factor interno guardado correctamente", "success");
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar el factor interno: {ex.Message}");
            }
        }

        protected void gvFactoresInternos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditarFactorInterno")
                {
                    int factorId = Convert.ToInt32(e.CommandArgument);
                    CargarFactorInterno(factorId);
                }
                else if (e.CommandName == "EliminarFactorInterno")
                {
                    int factorId = Convert.ToInt32(e.CommandArgument);
                    EliminarFactorInterno(factorId);
                }
                else if (e.CommandName == "CambiarEstadoFactorInterno")
                {
                    int factorId = Convert.ToInt32(e.CommandArgument);
                    CambiarEstadoFactorInterno(factorId);
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar la acción: {ex.Message}");
            }
        }

        private void CargarFactorInterno(int factorId)
        {
            try
            {
                var factor = db.FactoresInternos
                    .Include(f => f.TipoFactor)
                    .Include(f => f.Areas)
                    .FirstOrDefault(f => f.FactorInternoId == factorId);

                if (factor != null)
                {
                    ViewState["FactorInternoId"] = factorId;
                    litTituloFactorInterno.Text = "Editar Factor Interno";
                    
                    CargarTiposFactoresInternos();
                    
                    txtNombreFactorInterno.Text = factor.Nombre;
                    txtDescripcionFactorInterno.Text = factor.Descripcion;
                    ddlTipoFactorInterno.SelectedValue = factor.TipoFactorId.ToString();
                    ddlRelevanciaFactorInterno.SelectedValue = factor.Relevancia.ToString();
                    txtComentariosFactorInterno.Text = factor.Comentarios;
                    txtDesafiosFactorInterno.Text = factor.PosiblesDesafios;
                    ucAreasFactorInterno.SetSelectedAreaIds(factor.Areas.Select(a => a.AreaId).ToArray());

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowFormInterno", "showFormFactorInterno();", true);
                }
                else
                {
                    MostrarError("No se encontró el factor interno especificado");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar el factor interno: {ex.Message}");
            }
        }

        private void EliminarFactorInterno(int factorId)
        {
            try
            {
                var factor = db.FactoresInternos.Find(factorId);
                if (factor != null)
                {
                    db.FactoresInternos.Remove(factor);
                    db.SaveChanges();
                    CargarFactoresInternos();
                    MostrarMensaje("Factor interno eliminado correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar el factor interno: " + ex.Message, "danger");
            }
        }

        private void LimpiarFormularioFactorInterno()
        {
            ViewState["FactorInternoId"] = null;
            txtNombreFactorInterno.Text = string.Empty;
            txtDescripcionFactorInterno.Text = string.Empty;
            ddlTipoFactorInterno.SelectedIndex = 0;
            ddlRelevanciaFactorInterno.SelectedIndex = 0;
            txtComentariosFactorInterno.Text = string.Empty;
            txtDesafiosFactorInterno.Text = string.Empty;
            ucAreasFactorInterno.SetSelectedAreaIds(new int[0]);
        }

        private void CargarFactoresInternos()
        {
            try
            {
                var factores = db.FactoresInternos
                    .Include(f => f.TipoFactor)
                    .Include(f => f.Areas)
                    .Include(f => f.Creador)
                    .Include(f => f.UltimoEditor)
                    .Where(f => f.NormaId == normaId)
                    .OrderBy(f => f.TipoFactor.Categoria)
                    .ThenBy(f => f.Nombre)
                    .ToList();

                gvFactoresInternos.DataSource = factores;
                gvFactoresInternos.DataBind();
                upFactoresInternos.Update();

                // Asignar la misma fuente de datos al Repeater
                rptDetallesFactoresInternos.DataSource = factores;
                rptDetallesFactoresInternos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar los factores internos: {ex.Message}");
            }
        }

        protected string GetCategoriaInternaNombre(object categoria)
        {
            if (categoria == null) return string.Empty;
            switch (categoria.ToString())
            {
                case "R": return "Recursos Humanos";
                case "I": return "Infraestructura";
                case "P": return "Procesos";
                case "F": return "Finanzas";
                case "T": return "Tecnología";
                default: return string.Empty;
            }
        }

        protected string GetCategoriaInternaBadgeClass(object categoria)
        {
            if (categoria == null) return "badge bg-secondary";
            string baseClass = "badge ";
            switch (categoria.ToString())
            {
                case "R": return baseClass + "bg-primary";
                case "I": return baseClass + "bg-success";
                case "P": return baseClass + "bg-info";
                case "F": return baseClass + "bg-warning text-dark";
                case "T": return baseClass + "bg-danger";
                default: return baseClass + "bg-secondary";
            }
        }

        private void CambiarEstadoFactorInterno(int factorId)
        {
            try
            {
                var factor = db.FactoresInternos.Find(factorId);
                if (factor != null)
                {
                    factor.Activo = !factor.Activo;
                    factor.UltimoEditorId = ((Usuario)Session["Usuario"]).UsuarioId;
                    factor.UltimaModificacion = DateTime.Now;
                    db.SaveChanges();
                    CargarFactoresInternos();
                    MostrarMensaje($"Factor interno {(factor.Activo ? "activado" : "desactivado")} correctamente", "success");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cambiar el estado del factor interno: {ex.Message}");
            }
        }
        #endregion
    }
} 
