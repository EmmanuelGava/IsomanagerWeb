        protected void btnNuevaNorma_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarModal();
                litTituloModal.Text = "NUEVA NORMA";
                hdnNormaId.Value = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                    "$('#modalNorma').modal('show');", true);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al preparar el formulario para nueva norma: {ex.Message}");
            }
        }

        private void CargarNormaParaEditar(int normaId)
        {
            try
            {
                using (var context = new IsomanagerContext())
                {
                    var norma = context.Norma.Find(normaId);
                    if (norma != null)
                    {
                        litTituloModal.Text = "EDITAR NORMA";
                        hdnNormaId.Value = norma.NormaId.ToString();
                        txtTitulo.Text = norma.Titulo;
                        txtDescripcion.Text = norma.Descripcion;
                        txtVersion.Text = norma.Version;
                        txtFechaInicio.Text = norma.FechaInicio?.ToString("yyyy-MM-dd");
                        txtFechaFin.Text = norma.FechaFin?.ToString("yyyy-MM-dd");
                        ddlEstado.SelectedValue = norma.Estado;

                        ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                            "$('#modalNorma').modal('show');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar la norma para editar: {ex.Message}");
            }
        }

        private void MostrarErrorModal(string mensaje)
        {
            lblErrorModal.Text = mensaje;
            lblErrorModal.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "showModalScript",
                "$('#modalNorma').modal('show');", true);
        } 