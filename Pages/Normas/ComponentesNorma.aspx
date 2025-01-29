<%@ Page Title="Componentes de la Norma" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComponentesNorma.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Normas.ComponentesNorma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Panel de Favoritos -->
    <div id="favoritesPanel" class="favorites-panel">
        <div class="favorites-header">
            <h4><i class="bi bi-star-fill"></i> Accesos Rápidos</h4>
        </div>
        <div id="favoritesList" class="favorites-list">
            <!-- Los favoritos se agregarán aquí dinámicamente -->
        </div>
    </div>

    <div class="container-fluid">
        <!-- Encabezado -->
        <div class="d-flex justify-content-between align-items-start mb-4">
            <div class="page-header">
                <h2 class="page-title">
                    <i class="bi bi-journal-check"></i>
                    <asp:Literal ID="litTituloNorma" runat="server"></asp:Literal>
                </h2>
                <div class="page-subtitle">
                    <span class="version-badge">
                        <i class="bi bi-code-square"></i>
                        Versión <asp:Literal ID="litVersion" runat="server"></asp:Literal>
                    </span>
                    <span class="date-info">
                        <i class="bi bi-calendar3"></i>
                        Última actualización: <asp:Literal ID="litFecha" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
            <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-dark rounded-pill" OnClick="btnVolver_Click">
                <i class="bi bi-arrow-left"></i> Volver
            </asp:LinkButton>
        </div>

        <!-- Barra de búsqueda mejorada -->
        <div class="search-bar mb-4">
            <div class="input-group">
                <span class="input-group-text bg-transparent border-end-0">
                    <i class="bi bi-search"></i>
                </span>
                <input type="text" class="form-control border-start-0" id="searchComponents" 
                       placeholder="Buscar componentes..." onkeyup="filterComponents()">
            </div>
            <div class="search-options mt-2">
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="checkbox" id="searchInDescription" checked>
                    <label class="form-check-label" for="searchInDescription">Buscar en descripción</label>
                </div>
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="checkbox" id="showOnlyFavorites">
                    <label class="form-check-label" for="showOnlyFavorites">Mostrar solo favoritos</label>
                </div>
            </div>
        </div>

        <!-- Secciones de Componentes -->
        <div class="components-sections">
            <!-- 1. Contexto y Análisis Inicial -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('contextSection')">
                    <h3><i class="bi bi-diagram-2"></i> 1. Contexto y Análisis Inicial</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="contextSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkDatosPartida" runat="server" CssClass="component-link" OnClick="lnkDatosPartida_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="1.1" 
                                        data-component-title="Datos de Partida"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/DatosPartida.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon context">
                                        <i class="bi bi-house-door"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>1.1 Datos de Partida</h4>
                                        <p>Análisis inicial y punto de partida</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkContextoOrg" runat="server" CssClass="component-link" OnClick="lnkContextoOrg_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="1.2" 
                                        data-component-title="Contexto de la Organización"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/ContextoOrganizacion.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon context">
                                        <i class="bi bi-building"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>1.2 Contexto de la Organización</h4>
                                        <p>Análisis del entorno organizacional</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 2. Gestión de Recursos -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('resourcesSection')">
                    <h3><i class="bi bi-people"></i> 2. Gestión de Recursos</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="resourcesSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkRecursosHumanos" runat="server" CssClass="component-link" OnClick="lnkRecursosHumanos_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="2.1" 
                                        data-component-title="Recurso Humano"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/RecursosHumanos.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon process">
                                        <i class="bi bi-people"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>2.1 Recurso Humano</h4>
                                        <p>Gestión del personal</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkInfraestructura" runat="server" CssClass="component-link" OnClick="lnkInfraestructura_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="2.2" 
                                        data-component-title="Infraestructura"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Infraestructura.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon process">
                                        <i class="bi bi-tools"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>2.2 Infraestructura</h4>
                                        <p>Gestión de recursos físicos</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 3. Realización de Productos y Servicios -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('productionSection')">
                    <h3><i class="bi bi-gear"></i> 3. Realización de Productos y Servicios</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="productionSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkProcesosProductivos" runat="server" CssClass="component-link" OnClick="lnkProcesosProductivos_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="3.1" 
                                        data-component-title="Procesos Productivos"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/ProcesosProductivos.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon process">
                                        <i class="bi bi-gear"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>3.1 Procesos Productivos</h4>
                                        <p>Gestión de la producción</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkControlOperacional" runat="server" CssClass="component-link" OnClick="lnkControlOperacional_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="3.2" 
                                        data-component-title="Control Operacional"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/ControlOperacional.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon process">
                                        <i class="bi bi-shield-check"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>3.2 Control Operacional</h4>
                                        <p>Supervisión y control</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 4. Soporte -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('supportSection')">
                    <h3><i class="bi bi-tools"></i> 4. Soporte</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="supportSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-4">
                            <asp:LinkButton ID="lnkDocumentacion" runat="server" CssClass="component-link" OnClick="lnkDocumentacion_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="4.1" 
                                        data-component-title="Documentación"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Documentacion.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon docs">
                                        <i class="bi bi-file-text"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>4.1 Documentación</h4>
                                        <p>Gestión documental</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                            <asp:LinkButton ID="lnkCompras" runat="server" CssClass="component-link" OnClick="lnkCompras_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="4.2" 
                                        data-component-title="Compras"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Compras.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon docs">
                                        <i class="bi bi-cart"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>4.2 Compras</h4>
                                        <p>Gestión de proveedores</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                            <asp:LinkButton ID="lnkIDI" runat="server" CssClass="component-link" OnClick="lnkIDI_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="4.3" 
                                        data-component-title="I + D + i"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/IDI.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon docs">
                                        <i class="bi bi-lightbulb"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>4.3 I + D + i</h4>
                                        <p>Investigación y desarrollo</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 5. Evaluación del Desempeño -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('performanceSection')">
                    <h3><i class="bi bi-graph-up"></i> 5. Evaluación del Desempeño</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="performanceSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-6 col-lg-3">
                            <asp:LinkButton ID="lnkPlanificacion" runat="server" CssClass="component-link" OnClick="lnkPlanificacion_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="5.1" 
                                        data-component-title="Planificación y Revisión"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Planificacion.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon strategy">
                                        <i class="bi bi-calendar-check"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>5.1 Planificación y Revisión</h4>
                                        <p>Gestión estratégica</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6 col-lg-3">
                            <asp:LinkButton ID="lnkIncidentes" runat="server" CssClass="component-link" OnClick="lnkIncidentes_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="5.2" 
                                        data-component-title="Incidentes"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Incidentes.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon strategy">
                                        <i class="bi bi-exclamation-circle"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>5.2 Incidentes</h4>
                                        <p>Gestión de incidencias</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6 col-lg-3">
                            <asp:LinkButton ID="lnkPlanesEmergencia" runat="server" CssClass="component-link" OnClick="lnkPlanesEmergencia_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="5.3" 
                                        data-component-title="Planes de Emergencia"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/PlanesEmergencia.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon strategy">
                                        <i class="bi bi-shield-exclamation"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>5.3 Planes de Emergencia</h4>
                                        <p>Gestión de riesgos</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6 col-lg-3">
                            <asp:LinkButton ID="lnkNoConformidades" runat="server" CssClass="component-link" OnClick="lnkNoConformidades_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="5.4" 
                                        data-component-title="No Conformidades"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/NoConformidades.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon strategy">
                                        <i class="bi bi-x-circle"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>5.4 No Conformidades</h4>
                                        <p>Control de calidad</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 6. Mejora Continua -->
            <div class="section-group mb-4">
                <div class="section-header" onclick="toggleSection('improvementSection')">
                    <h3><i class="bi bi-arrow-repeat"></i> 6. Mejora Continua</h3>
                    <i class="bi bi-chevron-down"></i>
                </div>
                <div id="improvementSection" class="section-content">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkComunicacion" runat="server" CssClass="component-link" OnClick="lnkComunicacion_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="6.1" 
                                        data-component-title="Comunicación"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Comunicacion.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon control">
                                        <i class="bi bi-chat-dots"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>6.1 Comunicación</h4>
                                        <p>Gestión comunicativa</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6">
                            <asp:LinkButton ID="lnkAuditoria" runat="server" CssClass="component-link" OnClick="lnkAuditoria_Click">
                                <div class="component-card">
                                    <button class="favorite-btn" onclick="toggleFavorite(this, event)" 
                                        data-component-id="6.2" 
                                        data-component-title="Auditoría"
                                        data-url='<%# ResolveUrl($"~/Pages/Normas/Secciones/Auditoria.aspx?NormaId={NormaId}") %>'>
                                        <i class="bi bi-star"></i>
                                    </button>
                                    <div class="card-icon control">
                                        <i class="bi bi-clipboard-check"></i>
                                    </div>
                                    <div class="card-content">
                                        <h4>6.2 Auditoría</h4>
                                        <p>Seguimiento y control</p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        /* Variables */
        :root {
            --color-context: #4a90e2;
            --color-process: #2ecc71;
            --color-docs: #f1c40f;
            --color-strategy: #9b59b6;
            --color-risk: #e74c3c;
            --color-control: #3498db;
        }

        /* Estilos de sección */
        .section-group {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            overflow: hidden;
        }

        .section-header {
            padding: 1.25rem;
            background: #f8f9fa;
            cursor: pointer;
            display: flex;
            justify-content: space-between;
            align-items: center;
            transition: background-color 0.3s ease;
        }

        .section-header:hover {
            background: #e9ecef;
        }

        .section-header h3 {
            margin: 0;
            font-size: 1.25rem;
            color: #2c3e50;
            display: flex;
            align-items: center;
            gap: 0.75rem;
        }

        .section-content {
            padding: 1.5rem;
            display: none;
        }

        .section-content.active {
            display: block;
        }

        /* Estilos de componentes */
        .component-card {
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            padding: 1.25rem;
            gap: 1rem;
            border: 1px solid #e9ecef;
            position: relative;
        }

        .component-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }

        .card-icon {
            width: 48px;
            height: 48px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.5rem;
            color: white;
            flex-shrink: 0;
        }

        .card-icon.context { background: var(--color-context); }
        .card-icon.process { background: var(--color-process); }
        .card-icon.docs { background: var(--color-docs); }
        .card-icon.strategy { background: var(--color-strategy); }
        .card-icon.risk { background: var(--color-risk); }
        .card-icon.control { background: var(--color-control); }

        .card-content {
            flex-grow: 1;
        }

        .card-content h4 {
            margin: 0 0 0.25rem 0;
            font-size: 1rem;
            color: #2c3e50;
        }

        .card-content p {
            margin: 0;
            font-size: 0.875rem;
            color: #6c757d;
        }

        /* Enlaces */
        .component-link {
            text-decoration: none;
            color: inherit;
            display: block;
        }

        .component-link:hover {
            text-decoration: none;
            color: inherit;
        }

        /* Barra de búsqueda */
        .search-bar {
            max-width: 600px;
            margin: 0 auto;
        }

        .search-bar .form-control:focus {
            box-shadow: none;
            border-color: #ced4da;
        }

        /* Animaciones */
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .section-content {
            animation: fadeIn 0.3s ease-out;
        }

        /* Estilos para el panel de favoritos */
        .favorites-panel {
            position: fixed;
            top: 80px;
            right: 20px;
            width: 280px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            z-index: 1000;
        }

        .favorites-header {
            padding: 1rem;
            border-bottom: 1px solid #e9ecef;
        }

        .favorites-header h4 {
            margin: 0;
            font-size: 1rem;
            color: #2c3e50;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .favorites-list {
            padding: 1rem;
            max-height: calc(100vh - 200px);
            overflow-y: auto;
        }

        .favorite-item {
            display: flex;
            align-items: center;
            padding: 0.5rem;
            margin-bottom: 0.5rem;
            border-radius: 8px;
            background: #f8f9fa;
            transition: all 0.2s ease;
            cursor: pointer;
            text-decoration: none;
            color: inherit;
        }

        .favorite-item:hover {
            background: #e9ecef;
            text-decoration: none;
            color: inherit;
        }

        .favorite-item i {
            margin-right: 0.5rem;
            font-size: 1.1rem;
        }

        /* Estilos para el botón de favorito en las cards */
        .favorite-btn {
            position: absolute;
            top: 0.75rem;
            right: 0.75rem;
            background: transparent;
            border: none;
            font-size: 1.2rem;
            color: #adb5bd;
            cursor: pointer;
            transition: all 0.2s ease;
            padding: 0.25rem;
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 2;
        }

        .favorite-btn:hover {
            color: #ffc107;
        }

        .favorite-btn.active {
            color: #ffc107;
        }

        .favorite-btn.active i::before {
            content: "\f586";
        }

        /* Mejoras en la barra de búsqueda */
        .search-options {
            display: flex;
            gap: 1rem;
            font-size: 0.875rem;
        }

        .form-check-input:checked {
            background-color: var(--color-context);
            border-color: var(--color-context);
        }
    </style>

    <script>
        function toggleSection(sectionId) {
            const content = document.getElementById(sectionId);
            const header = content.previousElementSibling;
            const icon = header.querySelector('.bi-chevron-down');
            
            content.classList.toggle('active');
            icon.style.transform = content.classList.contains('active') ? 'rotate(180deg)' : 'rotate(0)';
        }

        function filterComponents() {
            const searchText = document.getElementById('searchComponents').value.toLowerCase();
            const searchInDescription = document.getElementById('searchInDescription').checked;
            const showOnlyFavorites = document.getElementById('showOnlyFavorites').checked;
            const sections = document.querySelectorAll('.section-group');
            let hasVisibleComponents = false;

            sections.forEach(section => {
                const components = section.querySelectorAll('.component-card');
                let hasVisibleInSection = false;

                components.forEach(component => {
                    const title = component.querySelector('h4').textContent.toLowerCase();
                    const description = component.querySelector('p').textContent.toLowerCase();
                    const isFavorite = component.querySelector('.favorite-btn').classList.contains('active');
                    
                    let shouldShow = title.includes(searchText) || 
                        (searchInDescription && description.includes(searchText));

                    if (showOnlyFavorites) {
                        shouldShow = shouldShow && isFavorite;
                    }

                    component.closest('.col-md-4, .col-md-6').style.display = shouldShow ? '' : 'none';
                    if (shouldShow) {
                        hasVisibleInSection = true;
                        hasVisibleComponents = true;
                    }
                });

                // Mostrar/ocultar la sección completa
                section.style.display = hasVisibleInSection ? '' : 'none';
            });

            // Mostrar mensaje si no hay resultados
            const noResultsMessage = document.getElementById('noResultsMessage');
            if (noResultsMessage) {
                noResultsMessage.style.display = hasVisibleComponents ? 'none' : 'block';
            }
        }

        // Gestión de favoritos
        function toggleFavorite(btn, event) {
            // Prevenir que el clic se propague al LinkButton
            if (event) {
                event.preventDefault();
                event.stopPropagation();
            }

            const componentId = btn.dataset.componentId;
            const componentTitle = btn.dataset.componentTitle;
            const componentUrl = btn.dataset.url;
            const card = btn.closest('.component-card');
            
            // Obtener favoritos actuales
            const favorites = JSON.parse(localStorage.getItem('componentFavorites') || '[]');
            const index = favorites.findIndex(f => f.id === componentId);
            
            if (index === -1) {
                // No está en favoritos, agregarlo
                favorites.push({
                    id: componentId,
                    title: componentTitle,
                    icon: card.querySelector('.card-icon i').className,
                    url: componentUrl
                });
                btn.classList.add('active');
            } else {
                // Ya está en favoritos, quitarlo
                favorites.splice(index, 1);
                btn.classList.remove('active');
            }
            
            // Guardar en localStorage y actualizar la lista
            localStorage.setItem('componentFavorites', JSON.stringify(favorites));
            updateFavoritesList();
        }

        function updateFavoritesList() {
            const favoritesList = document.getElementById('favoritesList');
            const favorites = JSON.parse(localStorage.getItem('componentFavorites') || '[]');
            
            favoritesList.innerHTML = favorites.length ? '' : '<div class="text-muted text-center">No hay favoritos</div>';
            
            // Ordenar favoritos por ID para mantener el orden consistente
            favorites.sort((a, b) => a.id.localeCompare(b.id));
            
            favorites.forEach(favorite => {
                favoritesList.innerHTML += `
                    <a href="${favorite.url}" class="favorite-item">
                        <i class="${favorite.icon}"></i>
                        <span>${favorite.title}</span>
                    </a>
                `;
            });
        }

        // Inicialización
        document.addEventListener('DOMContentLoaded', function() {
            // Expandir primera sección
            toggleSection('contextSection');
            
            // Sincronizar estado de favoritos con localStorage
            const favorites = JSON.parse(localStorage.getItem('componentFavorites') || '[]');
            const allFavoriteButtons = document.querySelectorAll('.favorite-btn');
            
            // Limpiar estado visual de todos los botones
            allFavoriteButtons.forEach(btn => btn.classList.remove('active'));
            
            // Aplicar estado activo solo a los que están en favoritos
            favorites.forEach(favorite => {
                const btn = document.querySelector(`[data-component-id="${favorite.id}"]`);
                if (btn) {
                    btn.classList.add('active');
                }
            });
            
            updateFavoritesList();
            
            // Agregar listeners para los checkboxes de búsqueda
            document.getElementById('searchInDescription').addEventListener('change', filterComponents);
            document.getElementById('showOnlyFavorites').addEventListener('change', filterComponents);
        });
    </script>
</asp:Content> 