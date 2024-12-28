<%@ Page Title="Datos de Partida" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DatosPartida.aspx.cs" Inherits="IsomanagerWeb.Pages.Normas.Secciones.DatosPartida" %>
<%@ Register Src="~/Controls/ucSeleccionAreas.ascx" TagPrefix="uc" TagName="SeleccionAreas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="~/Default.aspx" runat="server">Inicio</a></li>
                <li class="breadcrumb-item"><a href="~/Pages/Normas/GestionNormas.aspx" runat="server">Normas</a></li>
                <li class="breadcrumb-item active" aria-current="page">Datos de Partida</li>
            </ol>
        </nav>

        <div class="card shadow-sm">
            <div class="card-header">
                <h2 class="h4 mb-0">
                    <i class="bi bi-diagram-2 me-2"></i>
                    <asp:Literal ID="litTituloNorma" runat="server" />
                </h2>
                <small class="text-muted">
                    <asp:Literal ID="litDetallesNorma" runat="server" />
                </small>
            </div>

            <div class="card-body">
                <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-info">
                    <asp:Literal ID="litMensaje" runat="server" />
                </asp:Panel>

                <!-- Pestañas de navegación -->
                <ul class="nav nav-tabs mb-3" id="contextTabs" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="foda-tab" data-bs-toggle="tab" href="#foda" role="tab">
                            <i class="bi bi-pie-chart me-2"></i>Análisis FODA
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="factores-externos-tab" data-bs-toggle="tab" href="#factores-externos" role="tab">
                            <i class="bi bi-globe me-2"></i>Factores Externos
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="partes-interesadas-tab" data-bs-toggle="tab" href="#partes-interesadas" role="tab">
                            <i class="bi bi-people me-2"></i>Partes Interesadas
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="factores-internos-tab" data-bs-toggle="tab" href="#factores-internos" role="tab">
                            <i class="bi bi-building me-2"></i>Factores Internos
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="alcance-tab" data-bs-toggle="tab" href="#alcance" role="tab">
                            <i class="bi bi-bullseye me-2"></i>Alcance del Sistema
                        </a>
                    </li>
                </ul>

                <!-- Contenido de las pestañas -->
                <div class="tab-content" id="contextTabContent">
                    <!-- Pestaña FODA -->
                    <div class="tab-pane fade show active" id="foda" role="tabpanel">
                        <div class="d-flex justify-content-end mb-3">
                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-light me-2" OnClick="btnEditar_Click">
                                <i class="bi bi-pencil me-2"></i>Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnBorrar" runat="server" CssClass="btn btn-light" OnClick="btnBorrar_Click" 
                                      OnClientClick="return confirm('¿Está seguro que desea eliminar este FODA?');">
                                <i class="bi bi-trash me-2"></i>Borrar
                            </asp:LinkButton>
                        </div>

                        <div class="row g-4">
                            <div class="col-md-6">
                                <div class="card h-100">
                                    <div class="card-header bg-success bg-opacity-10">
                                        <span class="text-success">
                                            <i class="bi bi-star me-2"></i>Fortalezas
                                        </span>
                                    </div>
                                    <div class="card-body">
                                        <asp:TextBox ID="txtFortalezas" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card h-100">
                                    <div class="card-header bg-info bg-opacity-10">
                                        <span class="text-info">
                                            <i class="bi bi-lightbulb me-2"></i>Oportunidades
                                        </span>
                                    </div>
                                    <div class="card-body">
                                        <asp:TextBox ID="txtOportunidades" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card h-100">
                                    <div class="card-header bg-warning bg-opacity-10">
                                        <span class="text-warning">
                                            <i class="bi bi-exclamation-triangle me-2"></i>Debilidades
                                        </span>
                                    </div>
                                    <div class="card-body">
                                        <asp:TextBox ID="txtDebilidades" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card h-100">
                                    <div class="card-header bg-danger bg-opacity-10">
                                        <span class="text-danger">
                                            <i class="bi bi-shield me-2"></i>Amenazas
                                        </span>
                                    </div>
                                    <div class="card-body">
                                        <asp:TextBox ID="txtAmenazas" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex justify-content-between mt-4">
                            <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-light" OnClick="btnVolver_Click">
                                <i class="bi bi-arrow-left me-2"></i>Volver
                            </asp:LinkButton>
                            <div>
                                <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-light me-2" OnClick="btnCancelar_Click">
                                    <i class="bi bi-x me-2"></i>Cancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-dark" OnClick="btnGuardar_Click">
                                    <i class="bi bi-save me-2"></i>Guardar
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <!-- Factores Externos -->
                    <div class="tab-pane fade" id="factores-externos" role="tabpanel">
                        <!-- Grid de Factores -->
                        <div class="card mb-4">
                            <div class="card-header bg-white d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Factores Externos Relevantes</h5>
                                <div>
                                    <asp:LinkButton ID="btnNuevoFactor" runat="server" CssClass="btn btn-success" OnClick="btnNuevoFactor_Click">
                                        <i class="bi bi-plus-circle me-2"></i>Nuevo Factor
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="card-body">
                                <asp:UpdatePanel ID="upFactores" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvFactores" runat="server" CssClass="table table-hover" AutoGenerateColumns="false"
                                            OnRowCommand="gvFactores_RowCommand" DataKeyNames="FactorId">
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" HeaderText="Factor" />
                                                <asp:TemplateField HeaderText="Categoría">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoria" runat="server" Text='<%# GetCategoriaNombre(Eval("TipoFactor.Categoria")) %>'
                                                            CssClass='<%# GetCategoriaBadgeClass(Eval("TipoFactor.Categoria")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                <asp:TemplateField HeaderText="Impacto">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImpacto" runat="server" Text='<%# GetImpactoNombre(Eval("Impacto")) %>'
                                                            CssClass='<%# GetImpactoBadgeClass(Eval("Impacto")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acciones">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-primary me-2"
                                                            CommandName="EditarFactor" CommandArgument='<%# Eval("FactorId") %>'>
                                                            <i class="bi bi-pencil"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                                                            CommandName="EliminarFactor" CommandArgument='<%# Eval("FactorId") %>'
                                                            OnClientClick="return confirm('¿Está seguro de eliminar este factor?');">
                                                            <i class="bi bi-trash"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardarFactor" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnNuevoFactor" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <!-- Formulario Colapsable de Factor -->
                        <div class="collapse mb-4" id="formFactor">
                            <div class="card">
                                <div class="card-header bg-light d-flex justify-content-between align-items-center">
                                    <h5 class="mb-0">
                                        <asp:Literal ID="litTituloFactor" runat="server">Nuevo Factor</asp:Literal>
                                    </h5>
                                    <button type="button" class="btn-close" onclick="hideForm()"></button>
                                </div>
                                <div class="card-body">
                                    <asp:UpdatePanel ID="upFormFactor" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="mb-3">
                                                <label class="form-label">Título del Factor</label>
                                                <asp:TextBox ID="txtNombreFactor" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombreFactor" runat="server" ControlToValidate="txtNombreFactor"
                                                    ValidationGroup="Factor" CssClass="text-danger" Display="Dynamic"
                                                    ErrorMessage="El título es obligatorio"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Descripción Detallada</label>
                                                <asp:TextBox ID="txtDescripcionFactor" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDescripcionFactor" runat="server" ControlToValidate="txtDescripcionFactor"
                                                    ValidationGroup="Factor" CssClass="text-danger" Display="Dynamic"
                                                    ErrorMessage="La descripción es obligatoria"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Tipo de Factor</label>
                                                <asp:DropDownList ID="ddlTipoFactor" runat="server" CssClass="form-select">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvTipoFactor" runat="server" 
                                                    ControlToValidate="ddlTipoFactor"
                                                    InitialValue=""
                                                    ValidationGroup="Factor" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="Debe seleccionar un tipo de factor"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Impacto</label>
                                                <asp:DropDownList ID="ddlImpacto" runat="server" CssClass="form-select">
                                                    <asp:ListItem Value="0" Text="Bajo"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Medio"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Alto"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Probabilidad</label>
                                                <asp:DropDownList ID="ddlProbabilidad" runat="server" CssClass="form-select">
                                                    <asp:ListItem Value="0" Text="Baja"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Media"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Alta"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Fecha de Identificación</label>
                                                <asp:TextBox ID="txtFechaIdentificacion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvFechaIdentificacion" runat="server" ControlToValidate="txtFechaIdentificacion"
                                                    ValidationGroup="Factor" CssClass="text-danger" Display="Dynamic"
                                                    ErrorMessage="La fecha es obligatoria"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Áreas Afectadas</label>
                                                <uc:SeleccionAreas runat="server" ID="ucAreas" />
                                                <asp:CustomValidator ID="cvAreas" runat="server" 
                                                    ValidationGroup="Factor"
                                                    CssClass="text-danger"
                                                    Display="Dynamic"
                                                    ErrorMessage="Debe seleccionar al menos un área"
                                                    ClientValidationFunction="ValidateAreas"
                                                    OnServerValidate="cvAreas_ServerValidate"></asp:CustomValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Acciones Recomendadas</label>
                                                <asp:TextBox ID="txtAccionesRecomendadas" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </div>

                                            <div class="d-flex justify-content-end gap-2">
                                                <button type="button" class="btn btn-light" onclick="hideForm()">Cancelar</button>
                                                <asp:Button ID="btnGuardarFactor" runat="server" Text="Guardar" CssClass="btn btn-primary"
                                                    ValidationGroup="Factor" OnClick="btnGuardarFactor_Click" OnClientClick="if(Page_ClientValidate('Factor')) { hideForm(); return true; } return false;" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardarFactor" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvFactores" EventName="RowCommand" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex justify-content-between mt-4">
                            <asp:LinkButton ID="btnVolverFactores" runat="server" CssClass="btn btn-light" OnClick="btnVolverFactores_Click">
                                <i class="bi bi-arrow-left me-2"></i>Volver
                            </asp:LinkButton>
                            <div>
                                <asp:LinkButton ID="btnCancelarFactores" runat="server" CssClass="btn btn-light me-2" OnClick="btnCancelarFactores_Click">
                                    <i class="bi bi-x me-2"></i>Cancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnGuardarFactores" runat="server" CssClass="btn btn-dark" OnClick="btnGuardarFactores_Click">
                                    <i class="bi bi-save me-2"></i>Guardar
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="partes-interesadas" role="tabpanel">
                        <div class="alert alert-info">
                            <i class="bi bi-info-circle me-2"></i>
                            Sección en desarrollo
                        </div>
                    </div>
                    <div class="tab-pane fade" id="factores-internos" role="tabpanel">
                        <div class="alert alert-info">
                            <i class="bi bi-info-circle me-2"></i>
                            Sección en desarrollo
                        </div>
                    </div>
                    <!-- Alcance del Sistema -->
                    <div class="tab-pane fade" id="alcance" role="tabpanel">
                        <div class="d-flex justify-content-end mb-3">
                            <asp:LinkButton ID="btnEditarAlcance" runat="server" CssClass="btn btn-light me-2" OnClick="btnEditarAlcance_Click">
                                <i class="bi bi-pencil me-2"></i>Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnBorrarAlcance" runat="server" CssClass="btn btn-light" OnClick="btnBorrarAlcance_Click"
                                          OnClientClick="return confirm('¿Está seguro que desea eliminar el alcance del sistema?');">
                                <i class="bi bi-trash me-2"></i>Borrar
                            </asp:LinkButton>
                        </div>

                        <div class="card">
                            <div class="card-header bg-dark bg-opacity-10">
                                <span class="text-dark">
                                    <i class="bi bi-bullseye me-2"></i>Definición del Alcance
                                </span>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="txtAlcance" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" />
                            </div>
                        </div>

                        <div class="d-flex justify-content-between mt-4">
                            <asp:LinkButton ID="btnVolverAlcance" runat="server" CssClass="btn btn-light" OnClick="btnVolverAlcance_Click">
                                <i class="bi bi-arrow-left me-2"></i>Volver
                            </asp:LinkButton>
                            <div>
                                <asp:LinkButton ID="btnCancelarAlcance" runat="server" CssClass="btn btn-light me-2" OnClick="btnCancelarAlcance_Click">
                                    <i class="bi bi-x me-2"></i>Cancelar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnGuardarAlcance" runat="server" CssClass="btn btn-dark" OnClick="btnGuardarAlcance_Click">
                                    <i class="bi bi-save me-2"></i>Guardar
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .nav-tabs {
            border-bottom: 1px solid #dee2e6;
            margin-bottom: 1rem;
        }

        .nav-tabs .nav-link {
            color: #6c757d;
            border: 1px solid transparent;
            border-bottom: none;
            padding: 0.5rem 1rem;
            font-weight: normal;
            transition: all 0.2s ease;
            font-size: 0.875rem;
            border-radius: 0.375rem 0.375rem 0 0;
            margin-right: 0.25rem;
        }

        .nav-tabs .nav-link:hover {
            color: #212529;
            border-color: #e9ecef #e9ecef #dee2e6;
            background-color: #f8f9fa;
        }

        .nav-tabs .nav-link.active {
            color: #212529;
            background-color: #fff;
            border-color: #dee2e6 #dee2e6 #fff;
            font-weight: 500;
        }

        .card {
            transition: transform 0.2s, box-shadow 0.2s;
            border: none;
            background-color: #fff;
        }

        .card:hover {
            transform: translateY(-3px);
            box-shadow: 0 0.5rem 1rem rgba(0,0,0,0.1);
        }

        .card-header {
            background-color: #fff;
            border-bottom: 1px solid #dee2e6;
            padding: 1rem;
        }

        .form-control {
            resize: none;
            border-color: #dee2e6;
            padding: 0.5rem 0.75rem;
            font-size: 0.875rem;
        }

        .form-control:focus {
            border-color: #dee2e6;
            box-shadow: none;
        }

        .btn {
            font-weight: 500;
            font-size: 0.875rem;
            padding: 0.5rem 1rem;
            transition: all 0.2s ease;
        }

        .btn-dark {
            background-color: #212529 !important;
            border-color: #212529 !important;
        }

        .btn-dark:hover {
            background-color: #000 !important;
            border-color: #000 !important;
            transform: translateY(-1px);
        }

        .btn-light {
            background-color: #f8f9fa;
            border-color: #dee2e6;
            color: #212529;
        }

        .btn-light:hover {
            background-color: #e9ecef;
            border-color: #dee2e6;
            color: #000;
        }

        .alert {
            border-radius: 0.5rem;
            font-size: 0.875rem;
        }

        /* Estilos para el modal */
        .modal {
            padding-right: 0 !important;
        }
        
        .modal-open {
            overflow: hidden;
            padding-right: 0 !important;
        }

        .modal-backdrop {
            z-index: 1040;
        }

        .modal {
            z-index: 1050;
        }

        /* Prevenir scroll horizontal */
        body {
            overflow-x: hidden;
        }
    </style>

    <script type="text/javascript">
        function showSuccessMessage(message) {
            toastr.success(message);
        }

        function showErrorMessage(message) {
            toastr.error(message);
        }

        function showForm() {
            var formElement = document.getElementById('formFactor');
            if (formElement) {
                var bsCollapse = new bootstrap.Collapse(formElement, {
                    toggle: false
                });
                bsCollapse.show();
                // Scroll al formulario
                formElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }

        function hideForm() {
            var formElement = document.getElementById('formFactor');
            if (formElement) {
                var bsCollapse = new bootstrap.Collapse(formElement, {
                    toggle: false
                });
                bsCollapse.hide();
            }
        }

        // Inicializar eventos después de cada postback parcial
        if (typeof (Sys) !== 'undefined' && Sys.WebForms) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                // Aquí puedes agregar cualquier inicialización necesaria después de postbacks
            });
        }

        // Vincular el click del botón ASP con la función de mostrar formulario
        Sys.Application.add_load(function () {
            var btnNuevoFactor = document.getElementById('<%= btnNuevoFactor.ClientID %>');
            if (btnNuevoFactor) {
                btnNuevoFactor.addEventListener('click', function (e) {
                    e.preventDefault();
                    showForm();
                });
            }
        });

    </script>
</asp:Content> 