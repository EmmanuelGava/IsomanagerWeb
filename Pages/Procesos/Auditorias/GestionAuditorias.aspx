<%@ Page Title="Gestión de Auditorías" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionAuditorias.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Auditorias.GestionAuditorias" %>
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
                                <h6 class="text-muted mb-2">No Conformes</h6>
                                <h3 class="mb-0"><asp:Literal ID="litNoConformes" runat="server">0</asp:Literal></h3>
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
                                <h6 class="text-muted mb-2">En Proceso</h6>
                                <h3 class="mb-0"><asp:Literal ID="litEnProceso" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-gear fs-4 text-dark"></i>
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
                        placeholder="Buscar auditorías..." AutoPostBack="true" 
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevaAuditoria" runat="server" Text="Nueva Auditoría" 
                CssClass="btn btn-dark" OnClick="btnNuevaAuditoria_Click" />
        </div>

        <!-- GridView -->
        <asp:UpdatePanel ID="upAuditorias" runat="server">
            <ContentTemplate>
                <div class="custom-table-container">
                    <asp:GridView ID="gvAuditorias" runat="server" CssClass="table table-hover align-middle" 
                        AutoGenerateColumns="False" DataKeyNames="AuditoriaInternaProcesoId"
                        OnRowCommand="gvAuditorias_RowCommand" OnRowDataBound="gvAuditorias_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <tr>
                                        <th>Proceso</th>
                                        <th>Título</th>
                                        <th>Fecha Auditoría</th>
                                        <th>Auditor</th>
                                        <th>Tipo</th>
                                        <th>Estado</th>
                                        <th>Acciones</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Proceso") %></td>
                                        <td><%# Eval("Titulo") %></td>
                                        <td><%# Eval("FechaAuditoria", "{0:dd/MM/yyyy}") %></td>
                                        <td><%# Eval("Auditor") %></td>
                                        <td><%# Eval("Titulo") %></td>
                                        <td>
                                            <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                                <%# Eval("Estado") %>
                                            </span>
                                        </td>
                                        <td>
                                            <div class="d-flex align-items-center">
                                                <button type="button" class="btn btn-icon me-2" 
                                                    data-bs-toggle="collapse" 
                                                    data-bs-target="#collapse_<%# Eval("AuditoriaInternaProcesoId") %>" 
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
                                                                CommandName="Editar" CommandArgument='<%# Eval("AuditoriaInternaProcesoId") %>'>
                                                                <i class="bi bi-pencil me-2"></i> Editar
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <hr class="dropdown-divider" />
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                CommandName="Eliminar" CommandArgument='<%# Eval("AuditoriaInternaProcesoId") %>'
                                                                OnClientClick="return confirm('¿Está seguro que desea eliminar esta auditoría?');">
                                                                <i class="bi bi-trash me-2"></i> Eliminar
                                                            </asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" class="p-0">
                                            <div class="collapse" id="collapse_<%# Eval("AuditoriaInternaProcesoId") %>">
                                                <div class="detalles-row">
                                                    <!-- Tabs de navegación -->
                                                    <ul class="nav nav-tabs mb-3" role="tablist">
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link active" 
                                                                    id="descripcion-tab-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#descripcion-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="descripcion" 
                                                                    aria-selected="true">Descripción</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="alcance-tab-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#alcance-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="alcance">Alcance</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="hallazgos-tab-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#hallazgos-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="hallazgos">Hallazgos</button>
                                                        </li>
                                                        <li class="nav-item" role="presentation">
                                                            <button class="nav-link" 
                                                                    id="recomendaciones-tab-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    data-bs-toggle="tab" 
                                                                    data-bs-target="#recomendaciones-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                                    type="button" 
                                                                    role="tab" 
                                                                    aria-controls="recomendaciones">Recomendaciones</button>
                                                        </li>
                                                    </ul>

                                                    <!-- Contenido de los tabs -->
                                                    <div class="tab-content">
                                                        <div class="tab-pane fade show active" 
                                                             id="descripcion-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="descripcion-tab-<%# Eval("AuditoriaInternaProcesoId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Descripcion") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="alcance-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="alcance-tab-<%# Eval("AuditoriaInternaProcesoId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Alcance") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="hallazgos-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="hallazgos-tab-<%# Eval("AuditoriaInternaProcesoId") %>">
                                                            <p class="text-muted mb-0"><%# Eval("Hallazgos") %></p>
                                                        </div>
                                                        <div class="tab-pane fade" 
                                                             id="recomendaciones-<%# Eval("AuditoriaInternaProcesoId") %>" 
                                                             role="tabpanel" 
                                                             aria-labelledby="recomendaciones-tab-<%# Eval("AuditoriaInternaProcesoId") %>">
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

                    <!-- Scripts -->
                    <script type="text/javascript">
                        function toggleDetalles(id) {
                            var detallesId = 'detalles_' + id;
                            var detallesRow = document.getElementById(detallesId);
                            var row = document.querySelector('[data-auditoria-id="' + id + '"]');
                            
                            if (!detallesRow) {
                                // Si no existe, crear el contenedor de detalles
                                var template = document.getElementById('detallesTemplate').innerHTML;
                                var tr = document.createElement('tr');
                                tr.id = detallesId;
                                tr.className = 'detalles-container';
                                var td = document.createElement('td');
                                td.colSpan = '6';
                                
                                // Reemplazar los placeholders en el template
                                var content = template
                                    .replace(/{id}/g, id)
                                    .replace('{alcance}', row.querySelector('[id$="pAlcance"]').innerText)
                                    .replace('{hallazgos}', row.querySelector('[id$="pHallazgos"]').innerText)
                                    .replace('{recomendaciones}', row.querySelector('[id$="pRecomendaciones"]').innerText);
                                
                                td.innerHTML = content;
                                tr.appendChild(td);
                                row.parentNode.insertBefore(tr, row.nextSibling);
                                detallesRow = tr;
                            }
                            
                            if (detallesRow.style.display === 'none' || !detallesRow.style.display) {
                                detallesRow.style.display = 'table-row';
                                row.classList.add('selected-row');
                            } else {
                                detallesRow.style.display = 'none';
                                row.classList.remove('selected-row');
                            }
                        }
                    </script>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Modal de Auditoría -->
        <div class="modal fade" id="modalAuditoria" tabindex="-1" aria-labelledby="modalAuditoriaLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <asp:Literal ID="litTituloModal" runat="server">Nueva Auditoría</asp:Literal>
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarAuditoria" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvAuditorias" EventName="RowCommand" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblErrorModal" runat="server" CssClass="alert alert-danger d-block" Visible="false"></asp:Label>
                                <asp:HiddenField ID="hdnAuditoriaId" runat="server" />

                                <div class="mb-3">
                                    <label for="<%= ddlProceso.ClientID %>" class="form-label">Proceso</label>
                                    <asp:DropDownList ID="ddlProceso" runat="server" CssClass="form-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProceso" runat="server" 
                                        ControlToValidate="ddlProceso"
                                        ErrorMessage="Debe seleccionar un proceso" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= ddlTipoAuditoria.ClientID %>" class="form-label">Tipo de Auditoría</label>
                                    <asp:DropDownList ID="ddlTipoAuditoria" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                        <asp:ListItem Text="Auditoría Interna" Value="Auditoría Interna"></asp:ListItem>
                                        <asp:ListItem Text="Auditoría Externa" Value="Externa"></asp:ListItem>
                                        <asp:ListItem Text="Revisión por la Dirección" Value="Revisión por la Dirección"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTipoAuditoria" runat="server" 
                                        ControlToValidate="ddlTipoAuditoria"
                                        ErrorMessage="Debe seleccionar un tipo de auditoría" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= txtFechaAuditoria.ClientID %>" class="form-label">Fecha de Auditoría</label>
                                    <asp:TextBox ID="txtFechaAuditoria" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFechaAuditoria" runat="server" 
                                        ControlToValidate="txtFechaAuditoria"
                                        ErrorMessage="La fecha de auditoría es requerida" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                        <asp:ListItem Text="Pendiente" Value="Pendiente"></asp:ListItem>
                                        <asp:ListItem Text="En Proceso" Value="En Proceso"></asp:ListItem>
                                        <asp:ListItem Text="Aprobada" Value="Aprobada"></asp:ListItem>
                                        <asp:ListItem Text="No Conforme" Value="No Conforme"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                                        ControlToValidate="ddlEstado"
                                        ErrorMessage="Debe seleccionar un estado" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Usuario Asignado</label>
                                    <uc:UsuarioSelector runat="server" ID="ucUsuarioAsignado" />
                                </div>

                                <div class="mb-3">
                                    <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                        ControlToValidate="txtDescripcion"
                                        ErrorMessage="La descripción es requerida" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= txtAlcance.ClientID %>" class="form-label">Alcance</label>
                                    <asp:TextBox ID="txtAlcance" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAlcance" runat="server" 
                                        ControlToValidate="txtAlcance"
                                        ErrorMessage="El alcance es requerido" 
                                        CssClass="text-danger"
                                        ValidationGroup="Auditoria" 
                                        Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= txtHallazgos.ClientID %>" class="form-label">Hallazgos</label>
                                    <asp:TextBox ID="txtHallazgos" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>

                                <div class="mb-3">
                                    <label for="<%= txtRecomendaciones.ClientID %>" class="form-label">Recomendaciones</label>
                                    <asp:TextBox ID="txtRecomendaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        <asp:Button ID="btnGuardarAuditoria" runat="server" Text="Guardar" CssClass="btn btn-primary"
                            OnClick="btnGuardarAuditoria_Click" ValidationGroup="Auditoria" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script type="text/javascript">
        function showModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalAuditoria'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalAuditoria');
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

        function showSuccessMessage(message) {
            toastr.success(message);
        }

        function showErrorMessage(message) {
            toastr.error(message);
        }

        // Manejar el cierre del modal
        var modalAuditoria = document.getElementById('modalAuditoria');
        if (modalAuditoria) {
            modalAuditoria.addEventListener('hidden.bs.modal', function () {
                hideModal();
            });
        }
    </script>
</asp:Content> 