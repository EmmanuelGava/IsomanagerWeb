<%@ Page Title="Gestión de Evaluaciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionEvaluaciones.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Evaluaciones.GestionEvaluaciones" %>
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
                                <h6 class="text-muted mb-2">Aprobadas</h6>
                                <h3 class="mb-0"><asp:Literal ID="litAprobadas" runat="server">0</asp:Literal></h3>
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
                                <h6 class="text-muted mb-2">Rechazadas</h6>
                                <h3 class="mb-0"><asp:Literal ID="litRechazadas" runat="server">0</asp:Literal></h3>
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
                        placeholder="Buscar evaluaciones..." AutoPostBack="true" 
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevaEvaluacion" runat="server" Text="Nueva Evaluación" 
                CssClass="btn btn-dark" OnClick="btnNuevaEvaluacion_Click" />
        </div>

        <!-- Grid de Evaluaciones -->
        <asp:UpdatePanel ID="upEvaluaciones" runat="server">
            <ContentTemplate>
                <div class="table-responsive">
                    <asp:GridView ID="gvEvaluaciones" runat="server" CssClass="table table-hover" 
                        AutoGenerateColumns="False" DataKeyNames="EvaluacionId"
                        OnRowCommand="gvEvaluaciones_RowCommand" OnRowDataBound="gvEvaluaciones_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <tr>
                                        <th>Proceso</th>
                                        <th>Fecha Evaluación</th>
                                        <th>Evaluador</th>
                                        <th>Calificación</th>
                                        <th>Estado</th>
                                        <th>Acciones</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Proceso") %></td>
                                        <td><%# Eval("FechaEvaluacion", "{0:dd/MM/yyyy}") %></td>
                                        <td><%# Eval("Evaluador") %></td>
                                        <td><%# Eval("Calificacion") %></td>
                                        <td>
                                            <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                                <%# Eval("Estado") %>
                                            </span>
                                        </td>
                                        <td>
                                            <div class="d-flex align-items-center">
                                                <button type="button" class="btn btn-icon me-2" 
                                                    data-bs-toggle="collapse" 
                                                    data-bs-target="#collapse_<%# Eval("EvaluacionId") %>" 
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
                                                                CommandName="Editar" CommandArgument='<%# Eval("EvaluacionId") %>'>
                                                                <i class="bi bi-pencil me-2"></i> Editar
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                CommandName="Eliminar" CommandArgument='<%# Eval("EvaluacionId") %>'
                                                                OnClientClick="return confirm('¿Está seguro que desea eliminar esta evaluación?');">
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
                                            <div class="collapse" id="collapse_<%# Eval("EvaluacionId") %>">
                                                <div class="detalles-row">
                                                    <!-- Tabs de navegación -->
                                                    <ul class="nav nav-tabs mb-3" role="tablist">
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link active" 
                                                                    id="descripcion-tab-<%# Eval("EvaluacionId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#descripcion-<%# Eval("EvaluacionId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="descripcion" 
                                                                    aria-selected="true">Descripción</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="observaciones-tab-<%# Eval("EvaluacionId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#observaciones-<%# Eval("EvaluacionId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="observaciones">Observaciones</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="comentarios-tab-<%# Eval("EvaluacionId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#comentarios-<%# Eval("EvaluacionId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="comentarios">Comentarios</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="recomendaciones-tab-<%# Eval("EvaluacionId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#recomendaciones-<%# Eval("EvaluacionId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="recomendaciones">Recomendaciones</button>
                                                        </li>
                                                    </ul>

                                                    <!-- Contenido de los tabs -->
                                                    <div class="tab-content">
                                                        <div class="tab-pane fade show active" 
                                                             id="descripcion-<%# Eval("EvaluacionId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="descripcion-tab-<%# Eval("EvaluacionId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Descripcion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="observaciones-<%# Eval("EvaluacionId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="observaciones-tab-<%# Eval("EvaluacionId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Observaciones") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="comentarios-<%# Eval("EvaluacionId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="comentarios-tab-<%# Eval("EvaluacionId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Comentarios") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="recomendaciones-<%# Eval("EvaluacionId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="recomendaciones-tab-<%# Eval("EvaluacionId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Recomendaciones") %></p>
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

    <!-- Modal para nueva/editar evaluación -->
    <div class="modal fade" id="modalEvaluacion" tabindex="-1" aria-labelledby="modalEvaluacionLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Literal ID="litTituloModal" runat="server">Nueva Evaluación</asp:Literal>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblErrorModal" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
                    <asp:HiddenField ID="hdnEvaluacionId" runat="server" />

                    <div class="mb-3">
                        <label for="<%= ddlProceso.ClientID %>" class="form-label">Proceso</label>
                        <asp:DropDownList ID="ddlProceso" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvProceso" runat="server" ControlToValidate="ddlProceso"
                            ErrorMessage="Debe seleccionar un proceso" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="ucUsuarioAsignado" class="form-label">Usuario Asignado</label>
                        <uc:UsuarioSelector runat="server" ID="ucUsuarioAsignado" />
                        <asp:RequiredFieldValidator ID="rfvUsuarioAsignado" runat="server" 
                            ControlToValidate="ucUsuarioAsignado$ddlUsuarios"
                            InitialValue=""
                            ErrorMessage="Debe seleccionar un usuario asignado" 
                            CssClass="text-danger"
                            ValidationGroup="Evaluacion"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion"
                            ErrorMessage="La descripción es requerida" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtObservaciones.ClientID %>" class="form-label">Observaciones</label>
                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                            <asp:ListItem Text="Pendiente" Value="Pendiente"></asp:ListItem>
                            <asp:ListItem Text="En Revisión" Value="En Revisión"></asp:ListItem>
                            <asp:ListItem Text="Aprobada" Value="Aprobada"></asp:ListItem>
                            <asp:ListItem Text="Rechazada" Value="Rechazada"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEstado" runat="server" ControlToValidate="ddlEstado"
                            ErrorMessage="Debe seleccionar un estado" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtFechaEvaluacion.ClientID %>" class="form-label">Fecha de Evaluación</label>
                        <asp:TextBox ID="txtFechaEvaluacion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFechaEvaluacion" runat="server" ControlToValidate="txtFechaEvaluacion"
                            ErrorMessage="La fecha es requerida" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= ddlCalificacion.ClientID %>" class="form-label">Calificación</label>
                        <asp:DropDownList ID="ddlCalificacion" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                            <asp:ListItem Text="Excelente" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Bueno" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Regular" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Deficiente" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Muy Deficiente" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCalificacion" runat="server" ControlToValidate="ddlCalificacion"
                            ErrorMessage="Debe seleccionar una calificación" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtComentarios.ClientID %>" class="form-label">Comentarios</label>
                        <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvComentarios" runat="server" ControlToValidate="txtComentarios"
                            ErrorMessage="Los comentarios son requeridos" CssClass="text-danger"
                            ValidationGroup="Evaluacion" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtRecomendaciones.ClientID %>" class="form-label">Recomendaciones</label>
                        <asp:TextBox ID="txtRecomendaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarEvaluacion" runat="server" Text="Guardar" CssClass="btn btn-primary"
                        OnClick="btnGuardarEvaluacion_Click" ValidationGroup="Evaluacion" />
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script type="text/javascript">
        var modalEvaluacion;

        function initModal() {
            modalEvaluacion = new bootstrap.Modal(document.getElementById('modalEvaluacion'), {
                backdrop: 'static',
                keyboard: false
            });
        }

        function showModal() {
            if (!modalEvaluacion) {
                initModal();
            }
            modalEvaluacion.show();
        }

        function hideModal() {
            if (modalEvaluacion) {
                modalEvaluacion.hide();
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