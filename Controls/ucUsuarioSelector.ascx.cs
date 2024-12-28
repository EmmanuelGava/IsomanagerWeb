using System;
using System.Linq;
using System.Web.UI;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Controls
{
    public partial class ucUsuarioSelector : System.Web.UI.UserControl
    {
        public bool Required
        {
            get { return rfvUsuarios.Enabled; }
            set { rfvUsuarios.Enabled = value; }
        }

        public string ValidationGroup
        {
            get { return rfvUsuarios.ValidationGroup; }
            set 
            { 
                rfvUsuarios.ValidationGroup = value;
                ddlUsuarios.ValidationGroup = value;
            }
        }

        public int? SelectedUsuarioId
        {
            get
            {
                if (string.IsNullOrEmpty(ddlUsuarios.SelectedValue))
                    return null;
                return Convert.ToInt32(ddlUsuarios.SelectedValue);
            }
            set
            {
                try
                {
                    ddlUsuarios.ClearSelection();
                    if (value.HasValue)
                    {
                        var item = ddlUsuarios.Items.FindByValue(value.ToString());
                        if (item != null)
                            ddlUsuarios.SelectedValue = value.ToString();
                        else
                            ddlUsuarios.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlUsuarios.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error en SelectedUsuarioId setter: {ex.Message}");
                    ddlUsuarios.SelectedIndex = 0;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                if (ddlUsuarios == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: ddlUsuarios es null");
                    return;
                }

                ddlUsuarios.Items.Clear();
                ddlUsuarios.Items.Add(new System.Web.UI.WebControls.ListItem("-- Seleccione un usuario --", ""));

                using (var context = new IsomanagerContext())
                {
                    var usuarios = context.Usuarios
                        .Where(u => u.Estado == "Activo")
                        .OrderBy(u => u.Nombre)
                        .Select(u => new
                        {
                            u.UsuarioId,
                            u.Nombre
                        })
                        .ToList();

                    foreach (var usuario in usuarios)
                    {
                        if (usuario != null && !string.IsNullOrEmpty(usuario.Nombre))
                        {
                            ddlUsuarios.Items.Add(new System.Web.UI.WebControls.ListItem(
                                usuario.Nombre,
                                usuario.UsuarioId.ToString()
                            ));
                        }
                    }
                }

                // Asegurar que solo el primer elemento esté seleccionado
                ddlUsuarios.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en CargarUsuarios: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                if (!IsPostBack)
                {
                    CargarUsuarios();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR en OnInit: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
} 