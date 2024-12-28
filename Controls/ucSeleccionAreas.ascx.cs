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
            }
            catch (Exception ex)
            {
                // Log del error o manejo apropiado
                System.Diagnostics.Debug.WriteLine($"Error al cargar áreas: {ex.Message}");
            }
        }

        private void CargarAreas()
        {
            try
            {
                if (db == null) db = new IsomanagerContext();
                
                var areas = db.Areas
                    .Where(a => a.Activo)
                    .OrderBy(a => a.Nombre)
                    .ToList();

                if (areas != null && areas.Any())
                {
                    lstAreas.Items.Clear();
                    lstAreas.DataSource = areas;
                    lstAreas.DataTextField = "Nombre";
                    lstAreas.DataValueField = "AreaId";
                    lstAreas.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar las áreas: {ex.Message}");
            }
        }

        protected void btnGuardarArea_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var area = new Area
                {
                    Nombre = txtNombreArea.Text.Trim(),
                    Descripcion = txtDescripcionArea.Text.Trim(),
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    UltimaModificacion = DateTime.Now
                };

                db.Areas.Add(area);
                db.SaveChanges();

                // Limpiar formulario
                txtNombreArea.Text = string.Empty;
                txtDescripcionArea.Text = string.Empty;

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
            foreach (ListItem item in lstAreas.Items)
            {
                item.Selected = areaIds.Contains(Convert.ToInt32(item.Value));
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
} 