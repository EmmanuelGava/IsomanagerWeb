<%@ Page Title="Recursos Humanos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecursosHumanos.aspx.cs" Inherits="IsomanagerWeb.Pages.Normas.Secciones.RecursosHumanos" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc" TagName="ucUsuarioSelector" %>
<%@ Register Src="~/Controls/ucDepartamentoSelector.ascx" TagPrefix="uc" TagName="ucDepartamentoSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Panel de mensajes -->
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-success alert-dismissible fade show">
                <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </asp:Panel>

            <!-- Encabezado -->
            <div class="d-flex justify-content-between align-items-center mb-3">
                <div>
                    <h3 class="mb-0">
                        <asp:Literal ID="litTituloNorma" runat="server"></asp:Literal>
                    </h3>
                    <small class="text-muted">
                        <asp:Literal ID="litDetallesNorma" runat="server"></asp:Literal>
                    </small>
                </div>
                <div class="d-flex gap-2">
                    <asp:LinkButton ID="btnDescargarReporte" runat="server" CssClass="btn btn-outline-primary" OnClick="btnDescargarReporte_Click">
                        <i class="bi bi-file-earmark-pdf me-2"></i>Descargar Reporte
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnVolver_Click">
                        <i class="bi bi-arrow-left me-2"></i>Volver
                    </asp:LinkButton>
                </div>
            </div>

            <!-- Pestañas -->
            <ul class="nav nav-tabs mb-3" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#roles" type="button">
                        <i class="bi bi-people me-2"></i>Roles y Responsabilidades
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#competencias" type="button">
                        <i class="bi bi-trophy me-2"></i>Competencias
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" data-bs-toggle="tab" data-bs-target="#capacitaciones" type="button">
                        <i class="bi bi-mortarboard me-2"></i>Capacitaciones
                    </button>
                </li>
            </ul>

            <!-- Contenido de las pestañas -->
            <div class="tab-content">
                <!-- Pestaña de Roles -->
                <div class="tab-pane fade show active" id="roles" role="tabpanel">
                    <div class="d-flex justify-content-end mb-3">
                        <asp:LinkButton ID="btnMostrarModalUsuario" runat="server" CssClass="btn btn-dark">
                            <i class="bi bi-plus-circle me-2"></i>Nuevo Usuario
                        </asp:LinkButton>
                    </div>

                    <asp:UpdatePanel ID="upRoles" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <!-- Formulario de Roles -->
                            <div class="card mb-3">
                                <div class="card-body">
                                    <h5 class="card-title">Asignar Rol</h5>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <label class="form-label">Usuario</label>
                                                <uc:ucUsuarioSelector ID="ucUsuarioSelectorRol" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <label class="form-label">Cargo</label>
                                                <asp:TextBox ID="txtCargo" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="mb-3">
                                                <label class="form-label">Responsabilidades</label>
                                                <asp:TextBox ID="txtResponsabilidadesNuevo" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="d-flex justify-content-end gap-2">
                                        <asp:Button ID="btnLimpiarRol" runat="server" Text="Limpiar" CssClass="btn btn-light" OnClick="btnLimpiarRol_Click" />
                                        <asp:Button ID="btnGuardarRol" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarRol_Click" />
                                    </div>
                                </div>
                            </div>

                            <!-- Lista de Roles -->
                            <asp:GridView ID="gvRoles" runat="server" CssClass="table" AutoGenerateColumns="false"
                                OnRowCommand="gvRoles_RowCommand" OnRowDataBound="gvRoles_RowDataBound">
                                <HeaderStyle CssClass="header-row" />
                                <Columns>
                                    <asp:BoundField HeaderText="NOMBRE" DataField="NombreUsuario" />
                                    <asp:BoundField HeaderText="CARGO" DataField="Cargo" />
                                    <asp:BoundField HeaderText="ÁREA" DataField="Area" />
                                    <asp:BoundField HeaderText="RESPONSABILIDADES" DataField="Responsabilidad" />
                                    <asp:TemplateField HeaderText="ACCIONES">
                                        <ItemTemplate>
                                            <div class="d-flex justify-content-end gap-2">
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-primary"
                                                    CommandName="EditarRol" CommandArgument='<%# Eval("RolId") %>'>
                                                    <i class="bi bi-pencil"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                                    CommandName="EliminarRol" CommandArgument='<%# Eval("RolId") %>'
                                                    OnClientClick="return confirm('¿Está seguro de eliminar este rol?');">
                                                    <i class="bi bi-trash"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarRol" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiarRol" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvRoles" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUsuario" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <!-- Modal Nuevo Usuario -->
                <div class="modal fade" id="modalNuevoUsuario" tabindex="-1">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Nuevo Usuario</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <label class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvNombreUsuario" runat="server"
                                        ControlToValidate="txtNombreUsuario"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="El nombre es requerido." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                        ControlToValidate="txtEmail"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="El email es requerido." />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                        ControlToValidate="txtEmail"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="El email no es válido."
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Contraseña</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                        ControlToValidate="txtPassword"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="La contraseña es requerida." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Confirmar Contraseña</label>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="La confirmación de contraseña es requerida." />
                                    <asp:CompareValidator ID="cvPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtPassword"
                                        ValidationGroup="Usuario"
                                        CssClass="text-danger"
                                        ErrorMessage="Las contraseñas no coinciden." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Rol</label>
                                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Usuario" Value="Usuario" />
                                        <asp:ListItem Text="Administrador" Value="Administrador" />
                                    </asp:DropDownList>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Área</label>
                                    <uc:ucDepartamentoSelector ID="ucDepartamento" runat="server" />
                                </div>
                                <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnGuardarUsuario" runat="server" Text="Guardar" 
                                    CssClass="btn btn-primary" ValidationGroup="Usuario" OnClick="btnGuardarUsuario_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal Capacitación -->
                <div class="modal fade" id="modalCapacitacion" tabindex="-1">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Nueva Capacitación</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <label class="form-label">Usuario</label>
                                    <uc:ucUsuarioSelector ID="ucUsuarioSelectorCapacitacion" runat="server" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Nombre de la Capacitación</label>
                                    <asp:TextBox ID="txtNombreCapacitacion" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvNombreCapacitacion" runat="server"
                                        ControlToValidate="txtNombreCapacitacion"
                                        ValidationGroup="Capacitacion"
                                        CssClass="text-danger"
                                        ErrorMessage="El nombre es requerido." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Duración (horas)</label>
                                    <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvDuracion" runat="server"
                                        ControlToValidate="txtDuracion"
                                        ValidationGroup="Capacitacion"
                                        CssClass="text-danger"
                                        ErrorMessage="La duración es requerida." />
                                    <asp:RangeValidator ID="rvDuracion" runat="server"
                                        ControlToValidate="txtDuracion"
                                        ValidationGroup="Capacitacion"
                                        CssClass="text-danger"
                                        MinimumValue="1"
                                        MaximumValue="999"
                                        Type="Integer"
                                        ErrorMessage="La duración debe ser un número entre 1 y 999." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Fecha de Obtención</label>
                                    <asp:TextBox ID="txtFechaCapacitacion" runat="server" CssClass="form-control" TextMode="Date" />
                                    <asp:RequiredFieldValidator ID="rfvFechaCapacitacion" runat="server"
                                        ControlToValidate="txtFechaCapacitacion"
                                        ValidationGroup="Capacitacion"
                                        CssClass="text-danger"
                                        ErrorMessage="La fecha es requerida." />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnGuardarCapacitacion" runat="server" Text="Guardar" 
                                    CssClass="btn btn-primary" ValidationGroup="Capacitacion" OnClick="btnGuardarCapacitacion_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Pestaña de Competencias -->
                <div class="tab-pane fade" id="competencias" role="tabpanel">
                    <div class="d-flex justify-content-end mb-3">
                        <asp:LinkButton ID="btnMostrarModalCompetencia" runat="server" CssClass="btn btn-dark">
                            <i class="bi bi-plus-circle me-2"></i>Nueva Competencia
                        </asp:LinkButton>
                    </div>

                    <!-- GridView de Competencias -->
                    <asp:GridView ID="gvCompetencias" runat="server" CssClass="table" AutoGenerateColumns="false"
                        OnRowCommand="gvCompetencias_RowCommand" OnRowDataBound="gvCompetencias_RowDataBound">
                        <HeaderStyle CssClass="header-row" />
                        <Columns>
                            <asp:BoundField HeaderText="PERSONAL" DataField="PersonalAsignado" />
                            <asp:BoundField HeaderText="COMPETENCIA" DataField="Nombre" />
                            <asp:BoundField HeaderText="NIVEL REQUERIDO" DataField="NivelRequerido" />
                            <asp:TemplateField HeaderText="ACCIONES">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-end gap-2">
                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-primary"
                                            CommandName="EditarCompetencia" CommandArgument='<%# Eval("CompetenciaId") %>'>
                                            <i class="bi bi-pencil"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="EliminarCompetencia" CommandArgument='<%# Eval("CompetenciaId") %>'
                                            OnClientClick="return confirm('¿Está seguro de eliminar esta competencia?');">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Modal Competencia -->
                <div class="modal fade" id="modalCompetencia" tabindex="-1">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Nueva Competencia</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <label class="form-label">Usuario</label>
                                    <uc:ucUsuarioSelector ID="ucUsuarioSelectorCompetencia" runat="server" />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Nombre de la Competencia</label>
                                    <asp:TextBox ID="txtCompetencia" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvCompetencia" runat="server"
                                        ControlToValidate="txtCompetencia"
                                        ValidationGroup="Competencia"
                                        CssClass="text-danger"
                                        ErrorMessage="El nombre es requerido." />
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Nivel Requerido</label>
                                    <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Básico" Value="1" />
                                        <asp:ListItem Text="Intermedio" Value="2" />
                                        <asp:ListItem Text="Avanzado" Value="3" />
                                        <asp:ListItem Text="Experto" Value="4" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvNivel" runat="server"
                                        ControlToValidate="ddlNivel"
                                        ValidationGroup="Competencia"
                                        CssClass="text-danger"
                                        ErrorMessage="El nivel es requerido." />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnGuardarCompetencia" runat="server" Text="Guardar" 
                                    CssClass="btn btn-primary" ValidationGroup="Competencia" OnClick="btnGuardarCompetencia_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Pestaña de Capacitaciones -->
                <div class="tab-pane fade" id="capacitaciones" role="tabpanel">
                    <div class="d-flex justify-content-end mb-3">
                        <asp:LinkButton ID="btnMostrarModalCapacitacion" runat="server" CssClass="btn btn-dark">
                            <i class="bi bi-plus-circle me-2"></i>Nueva Capacitación
                        </asp:LinkButton>
                    </div>

                    <!-- GridView de Capacitaciones -->
                    <asp:GridView ID="gvCapacitaciones" runat="server" CssClass="table" AutoGenerateColumns="false"
                        OnRowCommand="gvCapacitaciones_RowCommand" OnRowDataBound="gvCapacitaciones_RowDataBound">
                        <HeaderStyle CssClass="header-row" />
                        <Columns>
                            <asp:BoundField HeaderText="USUARIO" DataField="NombreUsuario" />
                            <asp:BoundField HeaderText="CAPACITACIÓN" DataField="Nombre" />
                            <asp:BoundField HeaderText="DURACIÓN (HORAS)" DataField="Duracion" />
                            <asp:BoundField HeaderText="FECHA" DataField="FechaObtencion" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="ACCIONES">
                                <ItemTemplate>
                                    <div class="d-flex justify-content-end gap-2">
                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-primary"
                                            CommandName="EditarCapacitacion" CommandArgument='<%# Eval("CapacitacionId") %>'>
                                            <i class="bi bi-pencil"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                            CommandName="EliminarCapacitacion" CommandArgument='<%# Eval("CapacitacionId") %>'
                                            OnClientClick="return confirm('¿Está seguro de eliminar esta capacitación?');">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function showSuccessMessage(message) {
            toastr.success(message);
        }

        function showErrorMessage(message) {
            toastr.error(message);
        }

        function showModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalNuevoUsuario'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalNuevoUsuario');
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }
            document.body.classList.remove('modal-open');
            var modalBackdrop = document.querySelector('.modal-backdrop');
            if (modalBackdrop) {
                modalBackdrop.remove();
            }
        }

        function showCapacitacionModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalCapacitacion'));
            modal.show();
        }

        function hideCapacitacionModal() {
            var modalElement = document.getElementById('modalCapacitacion');
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }
            document.body.classList.remove('modal-open');
            var modalBackdrop = document.querySelector('.modal-backdrop');
            if (modalBackdrop) {
                modalBackdrop.remove();
            }
        }

        function showCompetenciaModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalCompetencia'));
            modal.show();
        }

        function hideCompetenciaModal() {
            var modalElement = document.getElementById('modalCompetencia');
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }
            document.body.classList.remove('modal-open');
            var modalBackdrop = document.querySelector('.modal-backdrop');
            if (modalBackdrop) {
                modalBackdrop.remove();
            }
        }

        // Función para inicializar los modales después de un postback parcial
        function initializeModals() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                // Reinicializar los botones después del postback
                var btnNuevoUsuario = document.getElementById('<%= btnMostrarModalUsuario.ClientID %>');
                if (btnNuevoUsuario) {
                    btnNuevoUsuario.onclick = function () { showModal(); return false; };
                }

                var btnNuevaCapacitacion = document.getElementById('<%= btnMostrarModalCapacitacion.ClientID %>');
                if (btnNuevaCapacitacion) {
                    btnNuevaCapacitacion.onclick = function () { showCapacitacionModal(); return false; };
                }

                var btnNuevaCompetencia = document.getElementById('<%= btnMostrarModalCompetencia.ClientID %>');
                if (btnNuevaCompetencia) {
                    btnNuevaCompetencia.onclick = function () { showCompetenciaModal(); return false; };
                }
            });
        }

        // Inicializar los modales cuando la página carga
        initializeModals();
    </script>
</asp:Content> 