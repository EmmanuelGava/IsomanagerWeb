<%@ Page Title="Gestión de Cambios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionCambios.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Cambios.GestionCambios" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc" TagName="UsuarioSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Referencia al archivo CSS de procesos -->
    <link href="../../../Content/css/procesos.css" rel="stylesheet" type="text/css" />
    <div class="container-fluid">
        <!-- Encabezado y botón volver -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="mb-1">
                    <asp:Literal ID="litTituloPagina" runat="server"></asp:Literal>
                </h2>
                <div class="text-muted">
                    <asp:Literal ID="litNormaTitulo" runat="server"></asp:Literal>
                </div>
            </div>
            <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-dark" OnClick="btnVolver_Click">
                <i class="bi bi-arrow-left"></i> Volver a Procesos
            </asp:LinkButton>
        </div>

        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-danger"></asp:Label>

        <!-- Cards de contadores -->
        <div class="row mb-4">
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Aprobados</h6>
                                <h3 class="mb-0"><asp:Literal ID="litAprobados" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-check-circle fs-4 text-dark"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Pendientes</h6>
                                <h3 class="mb-0"><asp:Literal ID="litPendientes" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-clock fs-4 text-dark"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Rechazados</h6>
                                <h3 class="mb-0"><asp:Literal ID="litRechazados" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-x-circle fs-4 text-dark"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">En Revisión</h6>
                                <h3 class="mb-0"><asp:Literal ID="litEnRevision" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-search fs-4 text-dark"></i>
                            </div>
                        </div>
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
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control border-start-0" 
                        placeholder="Buscar cambios..." AutoPostBack="true" 
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevoCambio" runat="server" Text="Nuevo Cambio" 
                CssClass="btn btn-dark" OnClick="btnNuevoCambio_Click" />
        </div>

        <!-- Grid de Cambios -->
        <asp:UpdatePanel ID="upCambios" runat="server">
            <ContentTemplate>
                <div class="custom-table-container">
                    <asp:GridView ID="gvCambios" runat="server" CssClass="table table-hover align-middle" 
                        AutoGenerateColumns="False" DataKeyNames="CambioId"
                        OnRowCommand="gvCambios_RowCommand" OnRowDataBound="gvCambios_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <tr>
                                        <th>Proceso</th>
                                        <th>Título</th>
                                        <th>Fecha</th>
                                        <th>Responsable</th>
                                        <th>Estado</th>
                                        <th>Acciones</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Proceso") %></td>
                                        <td><%# Eval("Titulo") %></td>
                                        <td><%# Eval("FechaCreacion", "{0:dd/MM/yyyy}") %></td>
                                        <td><%# Eval("Responsable") %></td>
                                        <td>
                                            <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                                <%# Eval("Estado") %>
                                            </span>
                                        </td>
                                        <td>
                                            <div class="d-flex align-items-center">
                                                <button type="button" class="btn btn-icon me-2" 
                                                    data-bs-toggle="collapse" 
                                                    data-bs-target="#collapse_<%# Eval("CambioId") %>" 
                                                    aria-expanded="false">
                                                    <i class="bi bi-eye"></i>
                                                </button>
                                                <div class="dropdown">
                                                    <button class="dots-button" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                                        <i class="bi bi-three-dots-vertical"></i>
                                                    </button>
                                                    <ul class="dropdown-menu">
                                                        <li>
                                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                                CommandName="Editar" CommandArgument='<%# Eval("CambioId") %>'>
                                                                <i class="bi bi-pencil me-2"></i> Editar
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                CommandName="Eliminar" CommandArgument='<%# Eval("CambioId") %>'
                                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este cambio?');">
                                                                <i class="bi bi-trash me-2"></i> Eliminar
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" class="p-0">
                                            <div class="collapse" id="collapse_<%# Eval("CambioId") %>">
                                                <div class="detalles-row">
                                                    <!-- Tabs de navegación -->
                                                    <ul class="nav nav-tabs mb-3" role="tablist">
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link active" 
                                                                    id="descripcion-tab-<%# Eval("CambioId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#descripcion-<%# Eval("CambioId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="descripcion" 
                                                                    aria-selected="true">Descripción</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="justificacion-tab-<%# Eval("CambioId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#justificacion-<%# Eval("CambioId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="justificacion">Justificación</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="impacto-tab-<%# Eval("CambioId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#impacto-<%# Eval("CambioId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="impacto">Impacto Estimado</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="riesgos-tab-<%# Eval("CambioId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#riesgos-<%# Eval("CambioId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="riesgos">Riesgos Asociados</button>
                                                        </li>
                                                    </ul>

                                                    <!-- Contenido de los tabs -->
                                                    <div class="tab-content">
                                                        <div class="tab-pane fade show active" 
                                                             id="descripcion-<%# Eval("CambioId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="descripcion-tab-<%# Eval("CambioId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Descripcion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="justificacion-<%# Eval("CambioId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="justificacion-tab-<%# Eval("CambioId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Justificacion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="impacto-<%# Eval("CambioId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="impacto-tab-<%# Eval("CambioId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("ImpactoEstimado") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="riesgos-<%# Eval("CambioId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="riesgos-tab-<%# Eval("CambioId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("RiesgosAsociados") %></p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Cambio -->
    <div class="modal fade" id="modalCambio" tabindex="-1" aria-labelledby="modalCambioLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal ID="litTituloModal" runat="server">Nuevo Cambio</asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upModalCambio" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorModal" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
                            <asp:HiddenField ID="hdnCambioId" runat="server" />
                            
                            <div class="mb-3">
                                <label for="<%= ddlProceso.ClientID %>" class="form-label">Proceso</label>
                                <asp:DropDownList ID="ddlProceso" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvProceso" runat="server"
                                    ControlToValidate="ddlProceso"
                                    InitialValue=""
                                    ValidationGroup="Cambio"
                                    CssClass="text-danger"
                                    ErrorMessage="Debe seleccionar un proceso." />
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtTitulo.ClientID %>" class="form-label">Título</label>
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server"
                                    ControlToValidate="txtTitulo"
                                    ValidationGroup="Cambio"
                                    CssClass="text-danger"
                                    ErrorMessage="El título es requerido." />
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Seleccione..." Value="" />
                                        <asp:ListItem Text="Pendiente" Value="Pendiente" />
                                        <asp:ListItem Text="En Revisión" Value="En Revisión" />
                                        <asp:ListItem Text="Aprobado" Value="Aprobado" />
                                        <asp:ListItem Text="Rechazado" Value="Rechazado" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
                                        ControlToValidate="ddlEstado"
                                        InitialValue=""
                                        ValidationGroup="Cambio"
                                        CssClass="text-danger"
                                        ErrorMessage="Debe seleccionar un estado." />
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label for="<%= ddlImpacto.ClientID %>" class="form-label">Impacto Estimado</label>
                                    <asp:DropDownList ID="ddlImpacto" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Seleccione..." Value="" />
                                        <asp:ListItem Text="Alto" Value="Alto" />
                                        <asp:ListItem Text="Medio" Value="Medio" />
                                        <asp:ListItem Text="Bajo" Value="Bajo" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvImpacto" runat="server"
                                        ControlToValidate="ddlImpacto"
                                        InitialValue=""
                                        ValidationGroup="Cambio"
                                        CssClass="text-danger"
                                        ErrorMessage="Debe seleccionar un impacto." />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server"
                                    ControlToValidate="txtDescripcion"
                                    ValidationGroup="Cambio"
                                    CssClass="text-danger"
                                    ErrorMessage="La descripción es requerida." />
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtJustificacion.ClientID %>" class="form-label">Justificación</label>
                                <asp:TextBox ID="txtJustificacion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                                <asp:RequiredFieldValidator ID="rfvJustificacion" runat="server"
                                    ControlToValidate="txtJustificacion"
                                    ValidationGroup="Cambio"
                                    CssClass="text-danger"
                                    ErrorMessage="La justificación es requerida." />
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtRiesgos.ClientID %>" class="form-label">Riesgos Asociados</label>
                                <asp:TextBox ID="txtRiesgos" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                                <asp:RequiredFieldValidator ID="rfvRiesgos" runat="server"
                                    ControlToValidate="txtRiesgos"
                                    ValidationGroup="Cambio"
                                    CssClass="text-danger"
                                    ErrorMessage="Los riesgos asociados son requeridos." />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Responsable</label>
                                <uc:UsuarioSelector runat="server" ID="ucUsuarioAsignado" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarCambio" runat="server" Text="Guardar" 
                        CssClass="btn btn-primary"
                        ValidationGroup="Cambio"
                        OnClick="btnGuardarCambio_Click" />
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
            var modal = new bootstrap.Modal(document.getElementById('modalCambio'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalCambio');
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

        // Manejar el cierre del modal
        var modalCambio = document.getElementById('modalCambio');
        if (modalCambio) {
            modalCambio.addEventListener('hidden.bs.modal', function () {
                hideModal();
            });
        }
    </script>
</asp:Content> 