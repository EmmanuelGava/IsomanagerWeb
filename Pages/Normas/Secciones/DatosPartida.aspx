<%@ Page Title="Datos de Partida" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DatosPartida.aspx.cs" Inherits="IsomanagerWeb.Pages.Normas.Secciones.DatosPartida" %>
<%@ Register Src="~/Controls/ucSeleccionAreas.ascx" TagPrefix="uc" TagName="SeleccionAreas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/css/datospartida.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="~/Content/css/procesos.css" rel="stylesheet" type="text/css" runat="server" />
    
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
                    <li class="nav-item">
                        <a class="nav-link" id="areas-tab" data-bs-toggle="tab" href="#areas" role="tab">
                            <i class="bi bi-grid me-2"></i>Áreas Afectadas
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
                                        <asp:GridView ID="gvFactores" runat="server" CssClass="table" AutoGenerateColumns="false"
                                            OnRowCommand="gvFactores_RowCommand" DataKeyNames="FactorId">
                                            <HeaderStyle CssClass="header-row" />
                                            <Columns>
                                                <asp:BoundField HeaderText="FACTOR" DataField="Nombre" ItemStyle-Width="20%" />
                                                <asp:TemplateField HeaderText="TIPO" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetCategoriaBadgeClass(Eval("TipoFactor.Categoria")) %>'>
                                                            <%# GetCategoriaNombre(Eval("TipoFactor.Categoria")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IMPACTO" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetImpactoBadgeClass(Eval("Impacto")) %>'>
                                                            <%# GetImpactoNombre(Eval("Impacto")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PROBABILIDAD" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class="badge bg-secondary">
                                                            <%# GetProbabilidadNombre(Eval("Probabilidad")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="FECHA" DataField="FechaIdentificacion" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="15%" />
                                                <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <div class="d-flex align-items-center justify-content-end gap-2">
                                                            <button type="button" class="btn btn-icon" 
                                                                    onclick='toggleDetails("collapse_factor_<%# Eval("FactorId") %>")'>
                                                                <i class="bi bi-eye"></i>
                                                            </button>
                                                            <div class="dropdown">
                                                                <button class="btn btn-icon" type="button" data-bs-toggle="dropdown">
                                                                    <i class="bi bi-three-dots-vertical"></i>
                                                                </button>
                                                                <ul class="dropdown-menu dropdown-menu-end">
                                                                    <li>
                                                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                                            CommandName="EditarFactor" CommandArgument='<%# Eval("FactorId") %>'>
                                                                            <i class="bi bi-pencil me-2"></i>Editar
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                    <li><hr class="dropdown-divider"></li>
                                                                    <li>
                                                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                            CommandName="EliminarFactor" CommandArgument='<%# Eval("FactorId") %>'
                                                                            OnClientClick="return confirm('¿Está seguro de eliminar este factor?');">
                                                                            <i class="bi bi-trash me-2"></i>Eliminar
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="align-middle" />
                                        </asp:GridView>
                                        
                                        <!-- Detalles colapsables -->
                                        <asp:Repeater ID="rptDetalles" runat="server">
                                            <ItemTemplate>
                                                <div class="collapse" id='<%# "collapse_factor_" + Eval("FactorId") %>'>
                                                    <div class="detalles-row">
                                                        <!-- Tabs de navegación -->
                                                        <ul class="nav nav-tabs mb-3" role="tablist">
                                                            <li class="nav-item">
                                                                <a class="nav-link active" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#descripcion_" + Eval("FactorId") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-file-text me-2"></i>Descripción
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#areas_" + Eval("FactorId") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-diagram-2 me-2"></i>Áreas Afectadas
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#acciones_" + Eval("FactorId") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-list-check me-2"></i>Acciones
                                                                </a>
                                                            </li>
                                                        </ul>

                                                        <!-- Contenido de los tabs -->
                                                        <div class="tab-content p-3">
                                                            <div class="tab-pane fade show active" id='<%# "descripcion_" + Eval("FactorId") %>' role="tabpanel">
                                                                <p class="text-muted mb-0"><%# Eval("Descripcion") %></p>
                                                            </div>
                                                            <div class="tab-pane fade" id='<%# "areas_" + Eval("FactorId") %>' role="tabpanel">
                                                                <div class="d-flex flex-wrap gap-1">
                                                                    <%# GetAreasAfectadas(Eval("Areas")) %>
                                                                </div>
                                                            </div>
                                                            <div class="tab-pane fade" id='<%# "acciones_" + Eval("FactorId") %>' role="tabpanel">
                                                                <p class="text-muted mb-0"><%# Eval("AccionesRecomendadas") %></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
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

                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label class="form-label">Impacto</label>
                                                        <asp:DropDownList ID="ddlImpacto" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="0" Text="Bajo"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Medio"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Alto"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label class="form-label">Probabilidad</label>
                                                        <asp:DropDownList ID="ddlProbabilidad" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="0" Text="Baja"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Media"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="Alta"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
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
                                                    ValidationGroup="Factor" OnClick="btnGuardarFactor_Click" />
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
                    <!-- Partes Interesadas -->
                    <div class="tab-pane fade" id="partes-interesadas" role="tabpanel">
                        <!-- Grid de Partes Interesadas -->
                        <div class="card mb-4">
                            <div class="card-header bg-white d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Partes Interesadas</h5>
                                <div>
                                    <asp:LinkButton ID="btnNuevaParte" runat="server" CssClass="btn btn-success" OnClick="btnNuevaParte_Click">
                                        <i class="bi bi-plus-circle me-2"></i>Nueva Parte Interesada
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="card-body">
                                <asp:UpdatePanel ID="upPartesInteresadas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvPartesInteresadas" runat="server" CssClass="table" AutoGenerateColumns="false"
                                            OnRowCommand="gvPartesInteresadas_RowCommand" DataKeyNames="ParteID">
                                            <HeaderStyle CssClass="header-row" />
                                            <Columns>
                                                <asp:BoundField HeaderText="NOMBRE/GRUPO" DataField="Nombre" ItemStyle-Width="20%" />
                                                <asp:TemplateField HeaderText="TIPO" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetTipoParteBadgeClass(Eval("Tipo")) %>'>
                                                            <%# GetTipoParteNombre(Eval("Tipo")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CATEGORÍA" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetCategoriaParteBadgeClass(Eval("Categoria")) %>'>
                                                            <%# GetCategoriaParteNombre(Eval("Categoria")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RELEVANCIA" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <span class='badge <%# GetRelevanciaBadgeClass(Eval("Relevancia")) %>'>
                                                            <%# GetRelevanciaNombre(Eval("Relevancia")) %>
                                                        </span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <div class="d-flex align-items-center justify-content-end gap-2">
                                                            <button type="button" class="btn btn-icon" 
                                                                    onclick='toggleDetails("collapse_parte_<%# Eval("ParteID") %>")'>
                                                                <i class="bi bi-eye"></i>
                                                            </button>
                                                            <div class="dropdown">
                                                                <button class="btn btn-icon" type="button" data-bs-toggle="dropdown">
                                                                    <i class="bi bi-three-dots-vertical"></i>
                                                                </button>
                                                                <ul class="dropdown-menu dropdown-menu-end">
                                                                    <li>
                                                                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                                            CommandName="EditarParte" CommandArgument='<%# Eval("ParteID") %>'>
                                                                            <i class="bi bi-pencil me-2"></i>Editar
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                    <li><hr class="dropdown-divider"></li>
                                                                    <li>
                                                                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                                            CommandName="EliminarParte" CommandArgument='<%# Eval("ParteID") %>'
                                                                            OnClientClick="return confirm('¿Está seguro de eliminar esta parte interesada?');">
                                                                            <i class="bi bi-trash me-2"></i>Eliminar
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="align-middle" />
                                        </asp:GridView>

                                        <!-- Detalles colapsables -->
                                        <asp:Repeater ID="rptDetallesPartes" runat="server">
                                            <ItemTemplate>
                                                <div class="collapse" id='<%# "collapse_parte_" + Eval("ParteID") %>'>
                                                    <div class="detalles-row">
                                                        <!-- Tabs de navegación -->
                                                        <ul class="nav nav-tabs mb-3" role="tablist">
                                                            <li class="nav-item">
                                                                <a class="nav-link active" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#intereses_" + Eval("ParteID") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-list-task me-2"></i>Intereses/Expectativas
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#impacto_" + Eval("ParteID") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-graph-up me-2"></i>Impacto
                                                                </a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" 
                                                                   data-bs-toggle="tab" 
                                                                   href='<%# "#acciones_" + Eval("ParteID") %>' 
                                                                   role="tab">
                                                                    <i class="bi bi-check2-square me-2"></i>Acciones
                                                                </a>
                                                            </li>
                                                        </ul>

                                                        <!-- Contenido de los tabs -->
                                                        <div class="tab-content p-3">
                                                            <div class="tab-pane fade show active" id='<%# "intereses_" + Eval("ParteID") %>' role="tabpanel">
                                                                <p class="text-muted mb-0"><%# Eval("Intereses") %></p>
                                                            </div>
                                                            <div class="tab-pane fade" id='<%# "impacto_" + Eval("ParteID") %>' role="tabpanel">
                                                                <p class="text-muted mb-0"><%# Eval("Impacto") %></p>
                                                            </div>
                                                            <div class="tab-pane fade" id='<%# "acciones_" + Eval("ParteID") %>' role="tabpanel">
                                                                <p class="text-muted mb-0"><%# Eval("Acciones") %></p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <!-- Formulario Colapsable de Parte Interesada -->
                        <div class="collapse mb-4" id="formParte">
                            <div class="card">
                                <div class="card-header bg-light d-flex justify-content-between align-items-center">
                                    <h5 class="mb-0">
                                        <asp:Literal ID="litTituloParte" runat="server">Nueva Parte Interesada</asp:Literal>
                                    </h5>
                                    <button type="button" class="btn-close" onclick="hideFormParte()"></button>
                                </div>
                                <div class="card-body">
                                    <asp:UpdatePanel ID="upFormParte" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="mb-3">
                                                <label class="form-label">Nombre/Grupo</label>
                                                <asp:TextBox ID="txtNombreParte" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombreParte" runat="server" 
                                                    ControlToValidate="txtNombreParte"
                                                    ValidationGroup="Parte" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="El nombre es obligatorio"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label class="form-label">Tipo</label>
                                                        <asp:DropDownList ID="ddlTipoParte" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="" Text="Seleccione..."></asp:ListItem>
                                                            <asp:ListItem Value="I" Text="Interna"></asp:ListItem>
                                                            <asp:ListItem Value="E" Text="Externa"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvTipoParte" runat="server" 
                                                            ControlToValidate="ddlTipoParte"
                                                            InitialValue=""
                                                            ValidationGroup="Parte" 
                                                            CssClass="text-danger" 
                                                            Display="Dynamic"
                                                            ErrorMessage="Debe seleccionar un tipo"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label class="form-label">Categoría</label>
                                                        <asp:DropDownList ID="ddlCategoriaParte" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="" Text="Seleccione..."></asp:ListItem>
                                                            <asp:ListItem Value="L" Text="Legal"></asp:ListItem>
                                                            <asp:ListItem Value="E" Text="Económica"></asp:ListItem>
                                                            <asp:ListItem Value="S" Text="Social"></asp:ListItem>
                                                            <asp:ListItem Value="T" Text="Tecnológica"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCategoriaParte" runat="server" 
                                                            ControlToValidate="ddlCategoriaParte"
                                                            InitialValue=""
                                                            ValidationGroup="Parte" 
                                                            CssClass="text-danger" 
                                                            Display="Dynamic"
                                                            ErrorMessage="Debe seleccionar una categoría"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Intereses/Expectativas</label>
                                                <asp:TextBox ID="txtIntereses" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvIntereses" runat="server" 
                                                    ControlToValidate="txtIntereses"
                                                    ValidationGroup="Parte" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="Los intereses son obligatorios"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Relevancia</label>
                                                <asp:DropDownList ID="ddlRelevancia" runat="server" CssClass="form-select">
                                                    <asp:ListItem Value="" Text="Seleccione..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Baja"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Media"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Alta"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvRelevancia" runat="server" 
                                                    ControlToValidate="ddlRelevancia"
                                                    InitialValue=""
                                                    ValidationGroup="Parte" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="Debe seleccionar la relevancia"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Impacto en la Norma</label>
                                                <asp:TextBox ID="txtImpacto" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvImpacto" runat="server" 
                                                    ControlToValidate="txtImpacto"
                                                    ValidationGroup="Parte" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="El impacto es obligatorio"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Acciones Relacionadas</label>
                                                <asp:TextBox ID="txtAccionesParte" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAccionesParte" runat="server" 
                                                    ControlToValidate="txtAccionesParte"
                                                    ValidationGroup="Parte" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="Las acciones son obligatorias"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="d-flex justify-content-end gap-2">
                                                <button type="button" class="btn btn-light" onclick="hideFormParte()">Cancelar</button>
                                                <asp:Button ID="btnGuardarParte" runat="server" Text="Guardar" CssClass="btn btn-primary"
                                                    ValidationGroup="Parte" OnClick="btnGuardarParte_Click" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
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
                    <!-- Pestaña de Áreas -->
                    <div class="tab-pane fade" id="areas" role="tabpanel">
                        <div class="card mb-4">
                            <div class="card-header bg-white d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Áreas Afectadas</h5>
                                <div>
                                    <asp:LinkButton ID="btnNuevaArea" runat="server" CssClass="btn btn-success" OnClick="btnNuevaArea_Click">
                                        <i class="bi bi-plus-circle me-2"></i>Nueva Área
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="card-body">
                                <asp:UpdatePanel ID="upAreas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvAreas" runat="server" CssClass="table table-hover" AutoGenerateColumns="false"
                                            OnRowCommand="gvAreas_RowCommand" DataKeyNames="AreaId">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="row-content">
                                                            <div class="main-row d-flex justify-content-between align-items-center p-2">
                                                                <div class="d-flex align-items-center">
                                                                    <button type="button" class="btn btn-link text-dark p-0 me-3" 
                                                                            data-bs-toggle="collapse" 
                                                                            data-bs-target='<%# "#collapse_" + Eval("AreaId") %>'>
                                                                        <i class="bi bi-chevron-down"></i>
                                                                    </button>
                                                                    <div>
                                                                        <h6 class="mb-0"><%# Eval("Nombre") %></h6>
                                                                        <small class="text-muted"><%# Eval("UbicacionGeografica.Nombre") %></small>
                                                                    </div>
                                                                </div>
                                                                <div class="d-flex align-items-center">
                                                                    <span class='badge me-3 <%# (bool)Eval("Activo") ? "bg-success" : "bg-danger" %>'>
                                                                        <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
                                                                    </span>
                                                                    <div class="dropdown">
                                                                        <button class="btn btn-icon" type="button" data-bs-toggle="dropdown">
                                                                            <i class="bi bi-three-dots-vertical"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu dropdown-menu-end">
                                                                            <li>
                                                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                                                    CommandName="EditarArea" CommandArgument='<%# Eval("AreaId") %>'>
                                                                                    <i class="bi bi-pencil me-2"></i>Editar
                                                                                </asp:LinkButton>
                                                                            </li>
                                                                            <li><hr class="dropdown-divider"></li>
                                                                            <li>
                                                                                <asp:LinkButton ID="btnCambiarEstado" runat="server" 
                                                                                    CssClass='<%# (bool)Eval("Activo") ? "dropdown-item text-danger" : "dropdown-item text-success" %>'
                                                                                    CommandName="CambiarEstado" CommandArgument='<%# Eval("AreaId") %>'>
                                                                                    <i class='<%# (bool)Eval("Activo") ? "bi bi-x-circle me-2" : "bi bi-check-circle me-2" %>'></i>
                                                                                    <%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>
                                                                                </asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="collapse border-top" id='<%# "collapse_" + Eval("AreaId") %>'>
                                                                <div class="p-3">
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                            <h6 class="mb-2">Descripción</h6>
                                                                            <p class="text-muted mb-3"><%# Eval("Descripcion") %></p>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="mb-3">
                                                                                <h6 class="mb-2">Fecha de Creación</h6>
                                                                                <p class="text-muted"><%# Eval("FechaCreacion", "{0:dd/MM/yyyy}") %></p>
                                                                            </div>
                                                                            <div>
                                                                                <h6 class="mb-2">Última Modificación</h6>
                                                                                <p class="text-muted"><%# Eval("UltimaModificacion", "{0:dd/MM/yyyy}") %></p>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <!-- Formulario Colapsable de Área -->
                        <div class="collapse mb-4" id="formArea">
                            <div class="card">
                                <div class="card-header bg-light d-flex justify-content-between align-items-center">
                                    <h5 class="mb-0">
                                        <asp:Literal ID="litTituloArea" runat="server">Nueva Área</asp:Literal>
                                    </h5>
                                    <button type="button" class="btn-close" onclick="hideFormArea()"></button>
                                </div>
                                <div class="card-body">
                                    <asp:UpdatePanel ID="upFormArea" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="mb-3">
                                                <label class="form-label">Nombre del Área</label>
                                                <asp:TextBox ID="txtNombreArea" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNombreArea" runat="server" 
                                                    ControlToValidate="txtNombreArea"
                                                    ValidationGroup="Area" 
                                                    CssClass="text-danger" 
                                                    Display="Dynamic"
                                                    ErrorMessage="El nombre del área es obligatorio"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Descripción</label>
                                                <asp:TextBox ID="txtDescripcionArea" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </div>

                                            <div class="mb-3">
                                                <label class="form-label">Ubicación</label>
                                                <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="form-select">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvUbicacion" runat="server"
                                                    ControlToValidate="ddlUbicacion"
                                                    ValidationGroup="Area"
                                                    CssClass="text-danger"
                                                    Display="Dynamic"
                                                    InitialValue=""
                                                    ErrorMessage="Debe seleccionar una ubicación"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="d-flex justify-content-end gap-2">
                                                <asp:Button ID="btnCancelarArea" runat="server" Text="Cancelar" CssClass="btn btn-light"
                                                    OnClick="btnCancelarArea_Click" CausesValidation="false" />
                                                <asp:Button ID="btnGuardarArea" runat="server" Text="Guardar" CssClass="btn btn-primary"
                                                    OnClick="btnGuardarArea_Click" ValidationGroup="Area" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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

        // Función para manejar los collapses de detalles
        function toggleDetails(targetId) {
            // Cerrar todos los collapses abiertos excepto el objetivo
            document.querySelectorAll('.collapse.show').forEach(function(collapse) {
                if (collapse.id !== targetId) {
                    bootstrap.Collapse.getInstance(collapse)?.hide();
                }
            });

            // Abrir o cerrar el collapse objetivo
            var targetCollapse = document.getElementById(targetId);
            if (targetCollapse) {
                var bsCollapse = bootstrap.Collapse.getInstance(targetCollapse) || 
                                new bootstrap.Collapse(targetCollapse, { toggle: false });
                
                if (targetCollapse.classList.contains('show')) {
                    bsCollapse.hide();
                } else {
                    bsCollapse.show();
                    targetCollapse.scrollIntoView({ behavior: 'smooth', block: 'start' });
                }
            }
        }
    </script>
</asp:Content> 