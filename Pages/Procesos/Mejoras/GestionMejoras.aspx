<%@ Page Title="Gestión de Mejoras" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionMejoras.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Mejoras.GestionMejoras" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc" TagName="UsuarioSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Referencia al archivo CSS de procesos -->
    <link href="~/Content/css/procesos.css" rel="stylesheet" type="text/css" runat="server" />
    

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
                                <h6 class="text-muted mb-2">Implementadas</h6>
                                <h3 class="mb-0"><asp:Literal ID="litImplementadas" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-check-circle fs-4 text-success"></i>
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
                                <i class="bi bi-clock fs-4 text-warning"></i>
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
                                <h6 class="text-muted mb-2">Rechazadas</h6>
                                <h3 class="mb-0"><asp:Literal ID="litRechazadas" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-x-circle fs-4 text-danger"></i>
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
                                <h6 class="text-muted mb-2">En Análisis</h6>
                                <h3 class="mb-0"><asp:Literal ID="litEnAnalisis" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-search fs-4 text-info"></i>
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
                        placeholder="Buscar mejoras..." AutoPostBack="true" 
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevaMejora" runat="server" Text="Nueva Mejora" 
                CssClass="btn btn-dark" OnClick="btnNuevaMejora_Click" />
        </div>

        <!-- Grid de Mejoras -->
        <asp:UpdatePanel ID="upMejoras" runat="server">
            <ContentTemplate>
                <div class="table-responsive">
                    <asp:GridView ID="gvMejoras" runat="server" CssClass="table table-hover" 
                        AutoGenerateColumns="False" DataKeyNames="MejoraId"
                        OnRowCommand="gvMejoras_RowCommand" OnRowDataBound="gvMejoras_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <tr>
                                        <th>Proceso</th>
                                        <th>Título</th>
                                        <th>Fecha Solicitud</th>
                                        <th>Solicitante</th>
                                        <th>Asignado</th>
                                        <th>Estado</th>
                                        <th>Prioridad</th>
                                        <th>Acciones</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Proceso") %></td>
                                        <td><%# Eval("Titulo") %></td>
                                        <td><%# Eval("FechaSolicitud", "{0:dd/MM/yyyy}") %></td>
                                        <td><%# Eval("Solicitante") %></td>
                                        <td><%# Eval("Asignado") %></td>
                                        <td>
                                            <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                                <%# Eval("Estado") %>
                                            </span>
                                        </td>
                                        <td><%# Eval("Prioridad") %></td>
                                        <td>
                                            <div class="d-flex align-items-center">
                                                <button type="button" class="btn btn-icon me-2" 
                                                    data-bs-toggle="collapse" 
                                                    data-bs-target="#collapse_<%# Eval("MejoraId") %>" 
                                                    aria-expanded="false">
                                                    <i class="bi bi-eye"></i>
                                                </button>
                                                <div class="dropdown">
                                                    <button class="btn btn-icon" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                                        <i class="bi bi-three-dots-vertical"></i>
                                                    </button>
                                                    <ul class="dropdown-menu">
                                                        <li>
                                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                                CommandName="Editar" CommandArgument='<%# Eval("MejoraId") %>'>
                                                                <i class="bi bi-pencil me-2"></i> Editar
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                CommandName="Eliminar" CommandArgument='<%# Eval("MejoraId") %>'
                                                                OnClientClick="return confirm('¿Está seguro que desea eliminar esta mejora?');">
                                                                <i class="bi bi-trash me-2"></i> Eliminar
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" class="p-0">
                                            <div class="collapse" id="collapse_<%# Eval("MejoraId") %>">
                                                <div class="detalles-row">
                                                    <!-- Tabs de navegación -->
                                                    <ul class="nav nav-tabs mb-3" role="tablist">
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link active" 
                                                                    id="descripcion-tab-<%# Eval("MejoraId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#descripcion-<%# Eval("MejoraId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="descripcion" 
                                                                    aria-selected="true">Descripción</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="justificacion-tab-<%# Eval("MejoraId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#justificacion-<%# Eval("MejoraId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="justificacion">Justificación</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="beneficios-tab-<%# Eval("MejoraId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#beneficios-<%# Eval("MejoraId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="beneficios">Beneficios Esperados</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="recursos-tab-<%# Eval("MejoraId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#recursos-<%# Eval("MejoraId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="recursos">Recursos Necesarios</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="resultados-tab-<%# Eval("MejoraId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#resultados-<%# Eval("MejoraId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="resultados">Resultados Esperados</button>
                                                        </li>
                                                    </ul>

                                                    <!-- Contenido de los tabs -->
                                                    <div class="tab-content">
                                                        <div class="tab-pane fade show active" 
                                                             id="descripcion-<%# Eval("MejoraId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="descripcion-tab-<%# Eval("MejoraId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Descripcion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="justificacion-<%# Eval("MejoraId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="justificacion-tab-<%# Eval("MejoraId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Justificacion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="beneficios-<%# Eval("MejoraId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="beneficios-tab-<%# Eval("MejoraId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("BeneficiosEsperados") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="recursos-<%# Eval("MejoraId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="recursos-tab-<%# Eval("MejoraId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("RecursosNecesarios") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="resultados-<%# Eval("MejoraId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="resultados-tab-<%# Eval("MejoraId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("ResultadosEsperados") %></p>
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
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardarMejora" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="gvMejoras" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- Modal para nueva/editar mejora -->
    <div class="modal fade" id="modalMejora" tabindex="-1" aria-labelledby="modalMejoraLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal ID="litTituloModal" runat="server">Nueva Mejora</asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarMejora" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvMejoras" EventName="RowCommand" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-body">
                            <asp:Label ID="lblErrorModal" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
                            <asp:HiddenField ID="hdnMejoraId" runat="server" />

                            <div class="mb-3">
                                <label for="<%= ddlProceso.ClientID %>" class="form-label">Proceso</label>
                                <asp:DropDownList ID="ddlProceso" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvProceso" runat="server" 
                                    ControlToValidate="ddlProceso"
                                    ErrorMessage="Debe seleccionar un proceso" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtTitulo.ClientID %>" class="form-label">Título</label>
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" 
                                    ControlToValidate="txtTitulo"
                                    ErrorMessage="El título es requerido" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                            <asp:ListItem Text="Pendiente" Value="Pendiente"></asp:ListItem>
                                            <asp:ListItem Text="En Análisis" Value="En Análisis"></asp:ListItem>
                                            <asp:ListItem Text="Implementada" Value="Implementada"></asp:ListItem>
                                            <asp:ListItem Text="Rechazada" Value="Rechazada"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                                            ControlToValidate="ddlEstado"
                                            ErrorMessage="Debe seleccionar un estado" 
                                            CssClass="text-danger"
                                            ValidationGroup="Mejora" 
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="<%= ddlPrioridad.ClientID %>" class="form-label">Prioridad</label>
                                        <asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                            <asp:ListItem Text="Alta" Value="Alta"></asp:ListItem>
                                            <asp:ListItem Text="Media" Value="Media"></asp:ListItem>
                                            <asp:ListItem Text="Baja" Value="Baja"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPrioridad" runat="server" 
                                            ControlToValidate="ddlPrioridad"
                                            ErrorMessage="Debe seleccionar una prioridad" 
                                            CssClass="text-danger"
                                            ValidationGroup="Mejora" 
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                    ControlToValidate="txtDescripcion"
                                    ErrorMessage="La descripción es requerida" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtJustificacion.ClientID %>" class="form-label">Justificación</label>
                                <asp:TextBox ID="txtJustificacion" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvJustificacion" runat="server" 
                                    ControlToValidate="txtJustificacion"
                                    ErrorMessage="La justificación es requerida" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtBeneficiosEsperados.ClientID %>" class="form-label">Beneficios Esperados</label>
                                <asp:TextBox ID="txtBeneficiosEsperados" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvBeneficios" runat="server" 
                                    ControlToValidate="txtBeneficiosEsperados"
                                    ErrorMessage="Los beneficios esperados son requeridos" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtRecursosNecesarios.ClientID %>" class="form-label">Recursos Necesarios</label>
                                <asp:TextBox ID="txtRecursosNecesarios" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRecursos" runat="server" 
                                    ControlToValidate="txtRecursosNecesarios"
                                    ErrorMessage="Los recursos necesarios son requeridos" 
                                    CssClass="text-danger"
                                    ValidationGroup="Mejora" 
                                    Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtResultadosEsperados.ClientID %>" class="form-label">Resultados Esperados</label>
                                <asp:TextBox ID="txtResultadosEsperados" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3">
                                </asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Usuario Asignado</label>
                                <uc:UsuarioSelector runat="server" ID="ucUsuarioAsignado" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                            <asp:Button ID="btnGuardarMejora" runat="server" Text="Guardar" CssClass="btn btn-primary"
                                OnClick="btnGuardarMejora_Click" ValidationGroup="Mejora" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script type="text/javascript">
        var modalMejora;

        function initModal() {
            modalMejora = new bootstrap.Modal(document.getElementById('modalMejora'), {
                backdrop: 'static',
                keyboard: false
            });
        }

        function showModal() {
            if (!modalMejora) {
                initModal();
            }
            modalMejora.show();
        }

        function hideModal() {
            if (modalMejora) {
                modalMejora.hide();
                document.body.classList.remove('modal-open');
                var modalBackdrop = document.querySelector('.modal-backdrop');
                if (modalBackdrop) {
                    modalBackdrop.remove();
                }
            }
        }

        function showSuccessMessage(message) {
            toastr.success(message);
        }

        function showErrorMessage(message) {
            toastr.error(message);
        }

        // Inicializar el modal cuando el documento esté listo
        document.addEventListener('DOMContentLoaded', function() {
            initModal();
        });
    </script>

</asp:Content> 