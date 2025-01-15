<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IsomanagerWeb.Pages.Dashboard" %>
<%@ OutputCache Location="None" NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Estilos críticos inline para evitar FOUC */
        .widgets-container {
            display: flex !important;
            flex-direction: row !important;
            gap: 1.75rem;
            margin-bottom: 2.5rem;
            width: 100%;
        }
        .widget-card {
            flex: 1 1 0;
            min-width: 250px;
        }
    </style>
    <link href="<%: ResolveUrl("~/Content/css/dashboard.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <div class="widgets-container">
            <div class="widget-card cumplimiento">
                <div class="widget-header">
                    <div class="widget-title">
                        <i class="bi bi-check-circle"></i>
                        <span>Cumplimiento General</span>
                    </div>
                    <i class="bi bi-info-circle" data-tooltip="Promedio de cumplimiento de todas las normas"></i>
                </div>
                <div class="widget-value">65%</div>
                <div class="widget-footer">
                    <i class="bi bi-arrow-up-short text-success"></i>
                    <span>5% más que el mes anterior</span>
                </div>
            </div>

            <div class="widget-card tareas">
                <div class="widget-header">
                    <div class="widget-title">
                        <i class="bi bi-list-task"></i>
                        <span>Tareas Pendientes</span>
                    </div>
                    <i class="bi bi-info-circle" data-tooltip="Tareas que requieren atención inmediata"></i>
                </div>
                <div class="widget-value">8</div>
                <div class="widget-footer">
                    <span class="text-warning">3 con alta prioridad</span>
                </div>
            </div>

            <div class="widget-card auditorias">
                <div class="widget-header">
                    <div class="widget-title">
                        <i class="bi bi-calendar-check"></i>
                        <span>Próximas Auditorías</span>
                    </div>
                    <i class="bi bi-info-circle" data-tooltip="Auditorías programadas para los próximos 30 días"></i>
                </div>
                <div class="widget-value">2</div>
                <div class="widget-footer">
                    <span>Próxima en 15 días</span>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="upDashboard" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="filters-section mb-4">
                    <div class="row g-3 align-items-center">
                        <div class="col-md-6">
                            <div class="input-group input-group-sm">
                                <span class="input-group-text bg-transparent border-end-0">
                                    <i class="bi bi-search"></i>
                                </span>
                                <asp:TextBox ID="txtBuscar" runat="server" 
                                    CssClass="form-control form-control-sm border-start-0" 
                                    placeholder="Buscar normas ISO o procesos..."
                                    AutoPostBack="true"
                                    OnTextChanged="OnBuscarClick">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="d-flex align-items-center justify-content-md-end gap-3">
                                <div class="form-check form-check-inline mb-0">
                                    <input type="checkbox" id="showActiveOnly" class="form-check-input" />
                                    <label class="form-check-label" for="showActiveOnly">Solo normas activas</label>
                                </div>
                                <div class="form-check form-check-inline mb-0">
                                    <input type="checkbox" id="showProgress" class="form-check-input" checked />
                                    <label class="form-check-label" for="showProgress">Mostrar progreso</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlMensajes" runat="server" Visible="false" CssClass="alert alert-danger">
                    <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
                </asp:Panel>

                <div class="iso-cards-container">
                    <asp:Repeater ID="rptNormas1" runat="server" 
                        OnItemDataBound="OnNormasItemDataBound"
                        OnItemCommand="rptNormas1_ItemCommand">
                        <ItemTemplate>
                            <div class="iso-card">
                                <div class="card-header">
                                    <h3 class="card-title"><%# HttpUtility.HtmlEncode(Eval("Nombre")) %></h3>
                                    <span class="status-badge <%# GetStatusClass(Eval("Estado")?.ToString() ?? "") %>">
                                        <i class="bi bi-circle-fill"></i>
                                        <%# GetStatusText(Eval("Estado")?.ToString() ?? "") %>
                                    </span>
                                </div>

                                <div class="card-content">
                                    <div class="progress-section">
                                        <div class="progress-label">Progreso general</div>
                                        <div class="progress" data-tooltip="<%# Eval("Progreso") %>% completado">
                                            <div class="progress-bar" 
                                                role="progressbar"
                                                data-progress="<%# Eval("Progreso") %>"
                                                aria-valuemin="0" 
                                                aria-valuemax="100"
                                                aria-valuenow="<%# Eval("Progreso") %>">
                                            </div>
                                        </div>
                                    </div>

                                    <div class="stats-grid">
                                        <div class="stat-item">
                                            <i class="bi bi-file-text"></i>
                                            <div class="stat-info">
                                                <span class="stat-label">Documentos</span>
                                                <span class="stat-value"><%# Eval("TotalDocumentos") %></span>
                                            </div>
                                        </div>

                                        <div class="stat-item">
                                            <div class="d-flex align-items-center justify-content-between w-100">
                                                <div class="d-flex align-items-center">
                                                    <i class="bi bi-gear me-2"></i>
                                                    <div class="stat-info">
                                                        <span class="stat-label">Procesos</span>
                                                        <span class="stat-value"><%# Eval("TotalProcesos") %></span>
                                                    </div>
                                                </div>
                                                <asp:LinkButton runat="server" 
                                                    CssClass="btn btn-dark" 
                                                    OnClick="OnIrProcesosClick"
                                                    CommandArgument='<%# Eval("NormaId") %>'
                                                    ToolTip="Ver página de procesos">
                                                    <i class="bi bi-arrow-right"></i> Ver procesos
                                                </asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="stat-item">
                                            <i class="bi bi-calendar-check"></i>
                                            <div class="stat-info">
                                                <span class="stat-label">Última actualización</span>
                                                <span class="stat-value"><%# Eval("UltimaActualizacion") %></span>
                                            </div>
                                        </div>

                                        <div class="stat-item">
                                            <i class="bi bi-person"></i>
                                            <div class="stat-info">
                                                <span class="stat-label">Responsable</span>
                                                <span class="stat-value"><%# HttpUtility.HtmlEncode(Eval("Responsable")) %></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-footer bg-transparent border-0 text-end mt-3">
                                        <asp:LinkButton runat="server" 
                                            CssClass="btn btn-dark w-100" 
                                            OnClick="OnIrComponentesClick"
                                            CommandArgument='<%# Eval("NormaId") %>'
                                            ToolTip="Ver estructura completa de la norma">
                                            <i class="bi bi-diagram-3"></i> Ver estructura completa
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
                        <div class="alert alert-info w-100">
                            <i class="bi bi-info-circle me-2"></i>
                            No se encontraron normas para mostrar.
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function updateProgressBars() {
            document.querySelectorAll('.progress-bar').forEach(function(bar) {
                var progress = bar.getAttribute('data-progress');
                bar.style.width = progress + '%';
            });
        }

        // Ejecutar cuando el documento esté listo
        document.addEventListener('DOMContentLoaded', updateProgressBars);

        // Ejecutar después de actualizaciones parciales de ASP.NET
        if (typeof Sys !== 'undefined' && Sys.WebForms) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(updateProgressBars);
        }
    </script>
</asp:Content> 