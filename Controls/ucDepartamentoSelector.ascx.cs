using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Controls
{
    public partial class ucDepartamentoSelector : System.Web.UI.UserControl
    {
        public bool Required { get; set; }
        public string ValidationGroup { get; set; }

        public bool Enabled
        {
            get { return ddlDepartamento.Enabled; }
            set { ddlDepartamento.Enabled = value; }
        }

        public int? SelectedDepartamentoId
        {
            get
            {
                if (int.TryParse(ddlDepartamento.SelectedValue, out int areaId) && areaId > 0)
                    return areaId;
                return null;
            }
            set
            {
                try
                {
                    ddlDepartamento.ClearSelection();
                    if (value.HasValue)
                    {
                        var item = ddlDepartamento.Items.FindByValue(value.ToString());
                        if (item != null)
                            ddlDepartamento.SelectedValue = value.ToString();
                        else
                            ddlDepartamento.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlDepartamento.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error en SelectedDepartamentoId setter: {ex.Message}");
                    ddlDepartamento.SelectedIndex = 0;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarAreas();

                // Configurar el validador si es requerido
                if (Required)
                {
                    rfvDepartamento.Enabled = true;
                    rfvDepartamento.ValidationGroup = ValidationGroup;
                }
            }
        }

        private void CargarAreas()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    ddlDepartamento.Items.Clear();
                    ddlDepartamento.Items.Add(new ListItem("Seleccione un área", ""));

                    // Cargar áreas desde la base de datos
                    var areas = context.Areas
                        .OrderBy(a => a.Nombre)
                        .Select(a => new { a.AreaId, a.Nombre })
                        .ToList();

                    // Agregar áreas al dropdown
                    foreach (var area in areas)
                    {
                        ddlDepartamento.Items.Add(new ListItem(area.Nombre, area.AreaId.ToString()));
                    }

                    // Agregar opción de nueva área
                    ddlDepartamento.Items.Add(new ListItem("[Crear Nueva Área]", "nuevo"));
                    
                    // Asegurar que solo el primer elemento esté seleccionado
                    ddlDepartamento.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                lblErrorModal.Text = "Error al cargar las áreas: " + ex.Message;
                lblErrorModal.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error en CargarAreas: {ex.Message}");
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepartamento.SelectedValue == "nuevo")
            {
                // Limpiar campos del modal
                txtNombreArea.Text = string.Empty;
                txtDescripcionArea.Text = string.Empty;
                lblErrorModal.Visible = false;

                // Mostrar el modal de nueva área
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalNuevaArea",
                    "showModalNuevaArea();", true);
                
                // Restaurar la selección anterior
                if (ddlDepartamento.Items.Count > 2)
                {
                    ddlDepartamento.SelectedIndex = 0;
                }
            }
        }

        protected void btnGuardarArea_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var context = new IsomanagerContext())
                    {
                        // Verificar si ya existe un área con el mismo nombre
                        if (context.Areas.Any(a => a.Nombre.ToLower() == txtNombreArea.Text.Trim().ToLower()))
                        {
                            lblErrorModal.Text = "Ya existe un área con este nombre.";
                            lblErrorModal.Visible = true;
                            return;
                        }

                        // Obtener la ubicación principal
                        var ubicacionPrincipal = context.UbicacionesGeograficas.FirstOrDefault();
                        if (ubicacionPrincipal == null)
                        {
                            ubicacionPrincipal = new UbicacionGeografica
                            {
                                Nombre = "Sede Principal",
                                Direccion = "Dirección Principal",
                                Ciudad = "Ciudad Principal",
                                Estado = "Estado Principal",
                                Pais = "País Principal",
                                FechaCreacion = DateTime.Now,
                                UltimaModificacion = DateTime.Now,
                                Activo = true
                            };
                            context.UbicacionesGeograficas.Add(ubicacionPrincipal);
                            context.SaveChanges();
                        }

                        var nuevaArea = new Area
                        {
                            Nombre = txtNombreArea.Text.Trim(),
                            Descripcion = txtDescripcionArea.Text.Trim(),
                            UbicacionId = ubicacionPrincipal.UbicacionId
                        };

                        context.Areas.Add(nuevaArea);
                        context.SaveChanges();

                        // Recargar las áreas
                        CargarAreas();

                        // Seleccionar la nueva área
                        ddlDepartamento.SelectedValue = nuevaArea.AreaId.ToString();

                        // Cerrar el modal y mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal",
                            "closeModalNuevaArea(); toastr.success('Área creada exitosamente.');", true);

                        // Actualizar los paneles
                        upDepartamento.Update();
                        upModalArea.Update();
                    }
                }
                catch (Exception ex)
                {
                    lblErrorModal.Text = "Error al crear el área: " + ex.Message;
                    lblErrorModal.Visible = true;
                }
            }
        }
    }
} 