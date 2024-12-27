using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using IsomanagerWeb.Models;
using System.Linq;
using System.Diagnostics;

namespace IsomanagerWeb.Controls
{
    public partial class ucUsuarioSelector : System.Web.UI.UserControl
    {
        public bool Required { get; set; }
        public string ValidationGroup { get; set; }

        public int? SelectedUsuarioId
        {
            get
            {
                if (int.TryParse(ddlUsuario.SelectedValue, out int usuarioId) && usuarioId > 0)
                    return usuarioId;
                return null;
            }
            set
            {
                if (value.HasValue)
                    ddlUsuario.SelectedValue = value.ToString();
                else
                    ddlUsuario.SelectedIndex = 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();

                // Configurar el validador si es requerido
                if (Required)
                {
                    rfvUsuario.Enabled = true;
                    rfvUsuario.ValidationGroup = ValidationGroup;
                }
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var usuarios = context.Usuarios
                        .Where(u => u.Estado == "Activo")
                        .Select(u => new
                        {
                            u.UsuarioId,
                            u.Nombre,
                            u.Email
                        })
                        .AsEnumerable()
                        .Select(u => new
                        {
                            u.UsuarioId,
                            NombreCompleto = $"{u.Nombre} ({u.Email})"
                        })
                        .ToList();

                    ddlUsuario.DataSource = usuarios;
                    ddlUsuario.DataTextField = "NombreCompleto";
                    ddlUsuario.DataValueField = "UsuarioId";
                    ddlUsuario.DataBind();

                    // Agregar item por defecto
                    ddlUsuario.Items.Insert(0, new ListItem("-- Seleccione un usuario --", ""));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar los usuarios: {ex.Message}");
                throw new Exception("Error al cargar los usuarios: " + ex.Message);
            }
        }
    }
} 