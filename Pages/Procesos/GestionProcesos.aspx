<%@ Page Title="Gestión de Procesos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="GestionProcesos.aspx.cs" 
    Inherits="IsomanagerWeb.Pages.Procesos.GestionProcesos" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc1" TagName="ucUsuarioSelector" %>
<%@ Register Src="~/Controls/ucDepartamentoSelector.ascx" TagPrefix="uc1" TagName="ucDepartamentoSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Referencia al archivo CSS de procesos -->
    <link href="../../Content/css/procesos.css" rel="stylesheet" type="text/css" />

    <div class="container-fluid">
        <!-- Título de la página -->
        <h2 class="mb-4">
            <asp:Literal ID="litTituloPagina" runat="server"></asp:Literal>
        </h2>

        <!-- Cards de Contadores -->
        <div class="row mb-4">
            <div class="col-md-3">
                <div class="card shadow-sm process-card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Evaluaciones</h6>
                                <h3 class="mb-0"><asp:Literal ID="litEvaluaciones" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-clipboard-check fs-4 text-success"></i>
                            </div>
                        </div>
                        <asp:LinkButton ID="btnIrEvaluaciones" runat="server" CssClass="btn btn-dark w-100 mt-3"
                            OnClick="btnIrEvaluaciones_Click">
                            Ver Evaluaciones <i class="bi bi-arrow-right"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm process-card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Cambios</h6>
                                <h3 class="mb-0"><asp:Literal ID="litCambios" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-arrow-repeat fs-4 text-warning"></i>
                            </div>
                        </div>
                        <asp:LinkButton ID="btnIrCambios" runat="server" CssClass="btn btn-dark w-100 mt-3"
                            OnClick="btnIrCambios_Click">
                            Ver Cambios <i class="bi bi-arrow-right"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm process-card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Mejoras</h6>
                                <h3 class="mb-0"><asp:Literal ID="litMejoras" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-graph-up-arrow fs-4 text-primary"></i>
                            </div>
                        </div>
                        <asp:LinkButton ID="btnIrMejoras" runat="server" CssClass="btn btn-dark w-100 mt-3"
                            OnClick="btnIrMejoras_Click">
                            Ver Mejoras <i class="bi bi-arrow-right"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm process-card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Auditorías</h6>
                                <h3 class="mb-0"><asp:Literal ID="litAuditorias" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-shield-check fs-4 text-info"></i>
                            </div>
                        </div>
                        <asp:LinkButton ID="btnIrAuditorias" runat="server" CssClass="btn btn-dark w-100 mt-3"
                            OnClick="btnIrAuditorias_Click">
                            Ver Auditorías <i class="bi bi-arrow-right"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

        <!-- Barra de búsqueda y botón nuevo -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="search-container">
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtBuscar" runat="server" 
                        CssClass="form-control border-start-0" 
                        placeholder="Buscar procesos..."
                        AutoPostBack="true"
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevoProceso" runat="server" Text="+ Agregar Proceso" 
                CssClass="btn btn-dark" OnClick="btnNuevoProceso_Click" />
        </div>

        <!-- Lista de Procesos -->
        <asp:UpdatePanel ID="upProcesos" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mb-3"></asp:Label>

                <div class="custom-table-container">
                    <asp:GridView ID="gvProcesos" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-hover align-middle" 
                        OnRowCommand="gvProcesos_RowCommand" DataKeyNames="ProcesoId"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:BoundField DataField="Area.Nombre" HeaderText="Área" />
                            <asp:BoundField DataField="Responsable.Nombre" HeaderText="Responsable" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Progreso">
                                <ItemTemplate>
                                    <div class="progress" style="height: 8px;">
                                        <div class="progress-bar bg-success" 
                                            role="progressbar" 
                                            style="width: 0"
                                            data-progress='<%# Eval("Progreso") %>'
                                            aria-valuenow="<%# Eval("Progreso") %>" 
                                            aria-valuemin="0" 
                                            aria-valuemax="100">
                                        </div>
                                    </div>
                                    <small class="text-muted"><%# Eval("Progreso") %>%</small>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="action-column">
                                <ItemTemplate>
                                    <div class="dropdown">
                                        <button class="btn btn-icon" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="bi bi-three-dots-vertical"></i>
                                        </button>
                                        <ul class="dropdown-menu dropdown-menu-end">
                                            <li>
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                    CommandName="Editar" CommandArgument='<%# Eval("ProcesoId") %>'>
                                                    <i class="bi bi-pencil"></i> Editar
                                                </asp:LinkButton>
                                            </li>
                                            <li><hr class="dropdown-divider"></li>
                                            <li>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger" 
                                                    CommandName="Eliminar" CommandArgument='<%# Eval("ProcesoId") %>'
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este proceso?');">
                                                    <i class="bi bi-trash"></i> Eliminar
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Proceso -->
    <div class="modal fade" id="modalProceso" tabindex="-1" aria-labelledby="modalProcesoLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalProcesoLabel">
                        <asp:Literal ID="litTituloModal" runat="server" Text="Nuevo Proceso"></asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upModal" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnProcesoId" runat="server" Value="" />
                            
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                            ControlToValidate="txtNombre"
                                            ValidationGroup="NuevoProceso"
                                            CssClass="text-danger"
                                            ErrorMessage="El nombre es requerido." />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="En Progreso" Value="En Progreso" />
                                            <asp:ListItem Text="Completado" Value="Completado" />
                                            <asp:ListItem Text="Pendiente" Value="Pendiente" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtObjetivo.ClientID %>" class="form-label">Objetivo</label>
                                <asp:TextBox ID="txtObjetivo" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="2" />
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3" MaxLength="500" />
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label">Área</label>
                                        <uc1:ucDepartamentoSelector runat="server" ID="ucArea" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label class="form-label">Responsable</label>
                                        <uc1:ucUsuarioSelector runat="server" ID="ucResponsable" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= txtFechaInicio.ClientID %>" class="form-label">Fecha de Inicio</label>
                                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" 
                                            TextMode="Date" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= txtFechaFin.ClientID %>" class="form-label">Fecha de Fin</label>
                                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" 
                                            TextMode="Date" />
                                    </div>
                                </div>
                            </div>

                            <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger d-block mt-2" 
                                Visible="false">
                            </asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-light" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarProceso" runat="server" Text="Guardar" 
                        CssClass="btn btn-dark" 
                        ValidationGroup="NuevoProceso"
                        OnClick="btnGuardarProceso_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script type="text/javascript">
        function showSuccessMessage(message) {
            toastr.success(message);
        }

        function showErrorMessage(message) {
            toastr.error(message);
        }

        function showModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalProceso'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalProceso');
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

        function updateProgressBars() {
            document.querySelectorAll('.progress-bar').forEach(function(bar) {
                var progress = bar.getAttribute('data-progress');
                if (progress) {
                    bar.style.width = progress + '%';
                }
            });
        }

        // Actualizar barras de progreso cuando el documento esté listo
        document.addEventListener('DOMContentLoaded', updateProgressBars);

        // Actualizar barras de progreso después de cada actualización parcial
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(updateProgressBars);
        }

        // Manejar el cierre del modal
        var modalProceso = document.getElementById('modalProceso');
        if (modalProceso) {
            modalProceso.addEventListener('hidden.bs.modal', function () {
                hideModal();
            });
        }
    </script>
</asp:Content> 