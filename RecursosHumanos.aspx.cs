protected void btnNuevaCapacitacion_Click(object sender, EventArgs e)
{
    try
    {
        // Limpiar campos del modal
        ucUsuarioSelectorCapacitacion.SelectedUsuarioId = null;
        txtNombreCapacitacion.Text = string.Empty;
        txtDuracion.Text = string.Empty;
        txtFechaObtencion.Text = DateTime.Now.ToString("yyyy-MM-dd");
        hdnFormacionId.Value = string.Empty;

        // Mostrar el modal
        ScriptManager.RegisterStartupScript(this, GetType(), "showModalCapacitacion",
            "handleModal('modalCapacitacion', 'show');", true);
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al preparar nueva capacitación: " + ex.Message, "danger");
    }
}

protected void btnGuardarCapacitacion_Click(object sender, EventArgs e)
{
    try
    {
        if (!Page.IsValid)
        {
            return;
        }

        if (!ucUsuarioSelectorCapacitacion.SelectedUsuarioId.HasValue)
        {
            MostrarMensaje("Debe seleccionar un usuario", "warning");
            return;
        }

        using (var db = new IsomanagerContext())
        {
            var formacionId = string.IsNullOrEmpty(hdnFormacionId.Value) ? 0 : int.Parse(hdnFormacionId.Value);
            Formacion formacion;

            if (formacionId > 0)
            {
                formacion = db.Formaciones.Find(formacionId);
                if (formacion == null)
                {
                    MostrarMensaje("No se encontró la capacitación", "danger");
                    return;
                }
            }
            else
            {
                formacion = new Formacion();
                db.Formaciones.Add(formacion);
            }

            formacion.UsuarioId = ucUsuarioSelectorCapacitacion.SelectedUsuarioId.Value;
            formacion.Nombre = txtNombreCapacitacion.Text.Trim();
            formacion.TipoFormacion = "Capacitación";
            formacion.FechaObtencion = DateTime.Parse(txtFechaObtencion.Text);
            formacion.Duracion = int.Parse(txtDuracion.Text);
            formacion.Estado = "Activo";

            db.SaveChanges();

            CargarCapacitaciones();
            MostrarMensaje("Capacitación guardada exitosamente", "success");

            // Cerrar el modal
            ScriptManager.RegisterStartupScript(this, GetType(), "hideModalCapacitacion",
                "handleModal('modalCapacitacion', 'hide');", true);
        }
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al guardar capacitación: " + ex.Message, "danger");
    }
}

protected void gvCapacitaciones_RowCommand(object sender, GridViewCommandEventArgs e)
{
    try
    {
        int formacionId = Convert.ToInt32(e.CommandArgument);

        using (var db = new IsomanagerContext())
        {
            var formacion = db.Formaciones.Find(formacionId);

            if (formacion == null)
            {
                MostrarMensaje("No se encontró la capacitación", "danger");
                return;
            }

            switch (e.CommandName)
            {
                case "Editar":
                    ucUsuarioSelectorCapacitacion.SelectedUsuarioId = formacion.UsuarioId;
                    txtNombreCapacitacion.Text = formacion.Nombre;
                    txtDuracion.Text = formacion.Duracion.ToString();
                    txtFechaObtencion.Text = formacion.FechaObtencion.ToString("yyyy-MM-dd");
                    hdnFormacionId.Value = formacionId.ToString();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showModalCapacitacion",
                        "handleModal('modalCapacitacion', 'show');", true);
                    break;

                case "Eliminar":
                    formacion.Estado = "Inactivo";
                    db.SaveChanges();
                    CargarCapacitaciones();
                    MostrarMensaje("Capacitación eliminada exitosamente", "success");
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al procesar comando: " + ex.Message, "danger");
    }
}

private void CargarCapacitaciones()
{
    try
    {
        using (var db = new IsomanagerContext())
        {
            var capacitaciones = db.Formaciones
                .Include(f => f.Usuario)
                .Where(f => f.TipoFormacion == "Capacitación" && f.Estado == "Activo")
                .Select(f => new
                {
                    f.FormacionId,
                    f.Nombre,
                    Participante = f.Usuario.Nombre,
                    f.FechaObtencion,
                    f.Duracion
                })
                .ToList();

            gvCapacitaciones.DataSource = capacitaciones;
            gvCapacitaciones.DataBind();
        }
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al cargar capacitaciones: " + ex.Message, "danger");
    }
}

protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        if (!IsPostBack)
        {
            string normaIdStr = Request.QueryString["normaId"];
            if (string.IsNullOrEmpty(normaIdStr) || !int.TryParse(normaIdStr, out normaId))
            {
                Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
                return;
            }

            CargarDatosNorma();
            CargarRoles();
            CargarCompetencias();
            CargarCapacitaciones();
        }
    }
    catch (Exception ex)
    {
        MostrarMensaje("Error al cargar la página: " + ex.Message, "danger");
    }
}

private void MostrarMensaje(string mensaje, string tipo)
{
    pnlMensaje.Visible = true;
    pnlMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show";
    litMensaje.Text = mensaje;
}

private void CargarDatosNorma()
{
    using (var db = new IsomanagerContext())
    {
        var norma = db.Normas.Find(normaId);
        if (norma == null)
        {
            Response.Redirect("~/Pages/Normas/GestionNormas.aspx");
            return;
        }

        litTituloNorma.Text = norma.Titulo;
        litDetallesNorma.Text = $"Versión {norma.Version} - {norma.Estado}";
    }
}

private void CargarRoles()
{
    using (var db = new IsomanagerContext())
    {
        var usuarios = db.Usuarios
            .Where(u => u.Estado == "Activo")
            .Select(u => new
            {
                u.UsuarioId,
                u.Nombre,
                u.Cargo,
                u.Responsabilidades,
                Area = u.Area.Nombre
            })
            .ToList();

        gvRoles.DataSource = usuarios;
        gvRoles.DataBind();
    }
}

private void CargarCompetencias()
{
    using (var db = new IsomanagerContext())
    {
        var competencias = db.CompetenciaNormas
            .Where(c => c.NormaId == normaId)
            .Select(c => new
            {
                c.CompetenciaId,
                c.Nombre,
                Participante = c.Usuario.Nombre,
                c.NivelRequerido
            })
            .ToList();

        gvCompetencias.DataSource = competencias;
        gvCompetencias.DataBind();
    }
} 