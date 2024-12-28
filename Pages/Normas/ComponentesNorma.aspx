<%@ Page Title="Componentes de la Norma" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComponentesNorma.aspx.cs" Inherits="IsomanagerWeb.Pages.Procesos.Normas.ComponentesNorma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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

        <!-- Mensajes de Error -->
        <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>
        <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

        <!-- Fila 1 - Azul (Contexto) -->
        <div class="row mb-4">
            <div class="col-md-6">
                <asp:LinkButton ID="lnkDatosPartida" runat="server" CssClass="component-link" OnClick="lnkDatosPartida_Click">
                    <div class="card component-card bg-context text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-house-door fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Datos de Partida</h5>
                                    <p class="card-text small mb-0">Contexto y análisis inicial</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-6">
                <asp:LinkButton ID="lnkRecursosHumanos" runat="server" CssClass="component-link" OnClick="lnkRecursosHumanos_Click">
                    <div class="card component-card bg-context text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-people fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Recursos Humanos</h5>
                                    <p class="card-text small mb-0">Gestión del personal</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>

        <!-- Fila 2 - Verde (Procesos) -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:LinkButton ID="lnkInfraestructura" runat="server" CssClass="component-link" OnClick="lnkInfraestructura_Click">
                    <div class="card component-card bg-process text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-tools fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Infraestructura</h5>
                                    <p class="card-text small mb-0">Gestión de recursos físicos</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="lnkRealizacion" runat="server" CssClass="component-link" OnClick="lnkRealizacion_Click">
                    <div class="card component-card bg-process text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-gear fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Realización</h5>
                                    <p class="card-text small mb-0">Procesos productivos</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="lnkControlOperacional" runat="server" CssClass="component-link" OnClick="lnkControlOperacional_Click">
                    <div class="card component-card bg-process text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-shield-check fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Control Operacional</h5>
                                    <p class="card-text small mb-0">Supervisión y control</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>

        <!-- Fila 3 - Amarillo (Documentación) -->
        <div class="row mb-4">
            <div class="col-md-6">
                <asp:LinkButton ID="lnkDocumentacion" runat="server" CssClass="component-link" OnClick="lnkDocumentacion_Click">
                    <div class="card component-card bg-docs text-dark">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-file-text fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Documentación</h5>
                                    <p class="card-text small mb-0">Gestión documental</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-6">
                <asp:LinkButton ID="lnkCompras" runat="server" CssClass="component-link" OnClick="lnkCompras_Click">
                    <div class="card component-card bg-docs text-dark">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-cart fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Compras</h5>
                                    <p class="card-text small mb-0">Gestión de proveedores</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>

        <!-- Fila 4 - Morado (Estrategia) -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:LinkButton ID="lnkIDI" runat="server" CssClass="component-link" OnClick="lnkIDI_Click">
                    <div class="card component-card bg-strategy text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-lightbulb fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">I + D + i</h5>
                                    <p class="card-text small mb-0">Investigación y desarrollo</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-8">
                <asp:LinkButton ID="lnkPlanificacion" runat="server" CssClass="component-link" OnClick="lnkPlanificacion_Click">
                    <div class="card component-card bg-strategy text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-graph-up fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Planificación y Revisión</h5>
                                    <p class="card-text small mb-0">Gestión estratégica</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>

        <!-- Fila 5 - Rojo (Riesgos) -->
        <div class="row mb-4">
            <div class="col-md-4">
                <asp:LinkButton ID="lnkIncidentes" runat="server" CssClass="component-link" OnClick="lnkIncidentes_Click">
                    <div class="card component-card bg-risk text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-plus-circle fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Incidentes</h5>
                                    <p class="card-text small mb-0">Gestión de incidencias</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="lnkPlanesEmergencia" runat="server" CssClass="component-link" OnClick="lnkPlanesEmergencia_Click">
                    <div class="card component-card bg-risk text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-exclamation-triangle fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Planes de Emergencia</h5>
                                    <p class="card-text small mb-0">Gestión de riesgos</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="lnkNoConformidades" runat="server" CssClass="component-link" OnClick="lnkNoConformidades_Click">
                    <div class="card component-card bg-risk text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-x-circle fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">No Conformidades</h5>
                                    <p class="card-text small mb-0">Control de calidad</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>

        <!-- Fila 6 - Verde (Comunicación y Control) -->
        <div class="row">
            <div class="col-md-6">
                <asp:LinkButton ID="lnkComunicacion" runat="server" CssClass="component-link" OnClick="lnkComunicacion_Click">
                    <div class="card component-card bg-process text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-telephone fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Comunicación</h5>
                                    <p class="card-text small mb-0">Gestión comunicativa</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
            <div class="col-md-6">
                <asp:LinkButton ID="lnkAuditoria" runat="server" CssClass="component-link" OnClick="lnkAuditoria_Click">
                    <div class="card component-card bg-process text-white">
                        <div class="card-body">
                            <div class="d-flex align-items-center">
                                <div class="icon-container">
                                    <i class="bi bi-clipboard-check fs-1"></i>
                                </div>
                                <div class="ms-3">
                                    <h5 class="card-title mb-1">Auditoría</h5>
                                    <p class="card-text small mb-0">Seguimiento y control</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <style>
        /* Variables de color */
        :root {
            --color-context: #4a90e2;      /* Azul para contexto */
            --color-process: #2ecc71;      /* Verde para procesos */
            --color-docs: #f1c40f;         /* Amarillo para documentación */
            --color-risk: #e74c3c;         /* Rojo para riesgos */
            --color-strategy: #9b59b6;     /* Morado para estrategia */
            --hover-opacity: 0.9;
        }

        /* Estilos base de las tarjetas */
        .component-card {
            height: 100%;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            cursor: pointer;
            border: none;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .component-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 16px rgba(0,0,0,0.2);
        }

        .component-card:hover .icon-container i {
            transform: scale(1.1) rotate(5deg);
        }

        /* Contenedor de iconos */
        .icon-container {
            width: 60px;
            height: 60px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            background: rgba(255,255,255,0.2);
            margin-right: 1rem;
        }

        .icon-container i {
            transition: transform 0.3s ease;
        }

        /* Colores de fondo personalizados */
        .bg-context { background-color: var(--color-context) !important; }
        .bg-process { background-color: var(--color-process) !important; }
        .bg-docs { background-color: var(--color-docs) !important; }
        .bg-risk { background-color: var(--color-risk) !important; }
        .bg-strategy { background-color: var(--color-strategy) !important; }

        /* Estilos de texto */
        .card-body {
            padding: 1.5rem;
        }

        .card-title {
            font-weight: 600;
            font-size: 1.1rem;
            margin-bottom: 0.25rem;
            color: white;
        }

        .card-text {
            opacity: 0.9;
            font-size: 0.9rem;
            color: rgba(255, 255, 255, 0.9);
        }

        /* Enlaces */
        .component-link {
            text-decoration: none;
            color: inherit;
            display: block;
            height: 100%;
        }

        .component-link:hover {
            color: inherit;
            text-decoration: none;
        }

        /* Espaciado y organización */
        .row {
            margin-bottom: 1.5rem;
        }

        .col-md-3, .col-md-4, .col-md-6, .col-md-8 {
            margin-bottom: 1.5rem;
        }

        /* Breadcrumbs */
        .breadcrumb {
            background: transparent;
            padding: 0.5rem 0;
            margin-bottom: 1rem;
        }

        .breadcrumb-item + .breadcrumb-item::before {
            content: "›";
        }

        /* Botón de ayuda flotante */
        .help-button {
            position: fixed;
            bottom: 2rem;
            right: 2rem;
            width: 50px;
            height: 50px;
            border-radius: 50%;
            background: white;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all 0.3s ease;
            z-index: 1000;
        }

        .help-button:hover {
            transform: scale(1.1);
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
        }

        /* Indicador de actividad */
        .activity-indicator {
            position: absolute;
            top: 10px;
            right: 10px;
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background-color: #ff4444;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0% { transform: scale(1); opacity: 1; }
            50% { transform: scale(1.2); opacity: 0.8; }
            100% { transform: scale(1); opacity: 1; }
        }

        /* Mejoras de accesibilidad */
        @media (prefers-reduced-motion: reduce) {
            .component-card,
            .icon-container i,
            .help-button {
                transition: none;
            }
        }

        /* Estilos del encabezado */
        .page-header {
            background: linear-gradient(to right, #f8f9fa, #ffffff);
            padding: 1.5rem;
            border-radius: 12px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            margin-bottom: 2rem;
        }

        .page-title {
            font-size: 2rem;
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 0.5rem;
            display: flex;
            align-items: center;
            gap: 0.75rem;
        }

        .page-title i {
            color: var(--color-context);
            font-size: 1.75rem;
        }

        .page-subtitle {
            color: #6c757d;
            font-size: 0.95rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .version-badge {
            background-color: var(--color-context);
            color: white;
            padding: 0.25rem 0.75rem;
            border-radius: 50px;
            font-size: 0.85rem;
            font-weight: 500;
        }

        .date-info {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            color: #6c757d;
        }

        .date-info i {
            font-size: 0.9rem;
        }
    </style>

    <!-- Breadcrumbs -->
    <nav aria-label="breadcrumb" class="breadcrumb mb-4">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="~/Default.aspx">Inicio</a></li>
            <li class="breadcrumb-item"><a href="~/Pages/Dashboard.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active" aria-current="page">Componentes de la Norma</li>
        </ol>
    </nav>

    <!-- Botón de ayuda flotante -->
    <div class="help-button" role="button" aria-label="Ayuda" onclick="mostrarAyuda()">
        <i class="bi bi-question-circle fs-4"></i>
    </div>

    <script>
        function mostrarAyuda() {
            // Aquí puedes implementar la lógica para mostrar la ayuda
            alert('Página de ayuda en construcción');
        }
    </script>
</asp:Content> 