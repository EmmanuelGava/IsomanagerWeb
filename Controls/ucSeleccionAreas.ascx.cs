using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Controls
{
    public partial class ucSeleccionAreas : UserControl
    {
        private IsomanagerContext db;

        protected void Page_Init(object sender, EventArgs e)
        {
            db = new IsomanagerContext();
            if (!IsPostBack)
            {
                CargarAreas();
                CargarUbicaciones();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (lstAreas.Items.Count == 0)
                {
                    CargarAreas();
                }
                if (ddlUbicacion.Items.Count == 0)
                {
                    CargarUbicaciones();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos: {ex.Message}");
            }
        }

        private void CargarUbicaciones()
        {
            try
            {
                if (db == null) db = new IsomanagerContext();

                var ubicaciones = db.UbicacionesGeograficas
                    .Where(u => u.Activo)
                    .OrderBy(u => u.Nombre)
                    .ToList();

                ddlUbicacion.Items.Clear();
                ddlUbicacion.Items.Add(new ListItem("Seleccione una ubicación", ""));
                
                foreach (var ubicacion in ubicaciones)
                {
                    ddlUbicacion.Items.Add(new ListItem(ubicacion.Nombre, ubicacion.UbicacionId.ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar las ubicaciones: {ex.Message}");
            }
        }

        private void CargarAreas()
        {
            try
            {
                if (db == null) db = new IsomanagerContext();
                
                var areas = db.Areas
                    .OrderBy(a => a.Nombre)
                    .ToList();

                lstAreas.Items.Clear();
                foreach (var area in areas)
                {
                    var item = new ListItem(area.Nombre, area.AreaId.ToString());
                    lstAreas.Items.Add(item);
                }

                // Para debug
                System.Diagnostics.Debug.WriteLine($"Áreas cargadas: {areas.Count}");
                foreach (var area in areas)
                {
                    System.Diagnostics.Debug.WriteLine($"Área: {area.Nombre} (ID: {area.AreaId})");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar áreas: {ex.Message}");
                throw;
            }
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

                var area = new Area
                {
                    Nombre = txtNombreArea.Text.Trim(),
                    Descripcion = txtDescripcionArea.Text.Trim(),
                    UbicacionId = ubicacionId,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UltimaModificacion = DateTime.Now
                };

                db.Areas.Add(area);
                db.SaveChanges();

                // Limpiar formulario
                txtNombreArea.Text = string.Empty;
                txtDescripcionArea.Text = string.Empty;
                ddlUbicacion.SelectedIndex = 0;

                // Recargar lista de áreas
                CargarAreas();

                // Seleccionar la nueva área
                var item = lstAreas.Items.FindByValue(area.AreaId.ToString());
                if (item != null)
                    item.Selected = true;

                // Ocultar formulario
                ScriptManager.RegisterStartupScript(this, GetType(), "HideForm", "ocultarNuevaArea();", true);
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", 
                    $"alert('Error al guardar el área: {ex.Message}');", true);
            }
        }

        public int[] GetSelectedAreaIds()
        {
            return lstAreas.GetSelectedIndices()
                .Select(i => Convert.ToInt32(lstAreas.Items[i].Value))
                .ToArray();
        }

        public void SetSelectedAreaIds(int[] areaIds)
        {
            try
            {
                if (lstAreas.Items.Count == 0)
                {
                    CargarAreas();
                }

                if (areaIds != null && areaIds.Any())
                {
                    foreach (ListItem item in lstAreas.Items)
                    {
                        item.Selected = areaIds.Contains(Convert.ToInt32(item.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al establecer áreas seleccionadas: {ex.Message}");
            }
        }

        public void CargarAreasSeleccionadas(int[] areaIds)
        {
            try
            {
                if (db == null) db = new IsomanagerContext();

                var areas = db.Areas
                    .Where(a => areaIds.Contains(a.AreaId) && a.Activo)
                    .OrderBy(a => a.Nombre)
                    .ToList();

                lstAreas.Items.Clear();
                foreach (var area in areas)
                {
                    var item = new ListItem(area.Nombre, area.AreaId.ToString());
                    item.Selected = true;
                    lstAreas.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar áreas seleccionadas: {ex.Message}");
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        protected void lstAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Este evento se dispara cuando el usuario selecciona o deselecciona un área
            try
            {
                var selectedIds = GetSelectedAreaIds();
                System.Diagnostics.Debug.WriteLine($"Áreas seleccionadas: {string.Join(", ", selectedIds)}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al procesar selección: {ex.Message}");
            }
        }
    }
} 