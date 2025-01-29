<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IsomanagerWeb.Pages.Dashboard" %>
<%@ OutputCache Location="None" NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/Content/css/dashboard.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <div class="widgets-container">
            <div class="widget-card cumplimiento">
                <div class="widget-header">
                    <h3 class="widget-title">
                        <i class="bi bi-check-circle"></i>
                        Cumplimiento General
                    </h3>
                    <i class="bi bi-info-circle" data-bs-toggle="tooltip" title="Promedio de cumplimiento de todas las normas"></i>
                </div>
                <div class="widget-value">65%</div>
                <div class="widget-footer">
                    <div class="widget-trend positive">
                        <i class="bi bi-arrow-up-short"></i>
                        <span>5% más que el mes anterior</span>
                    </div>
                </div>
            </div>

            <div class="widget-card tareas">
                <div class="widget-header">
                    <h3 class="widget-title">
                        <i class="bi bi-list-task"></i>
                        Tareas Pendientes
                    </h3>
                    <i class="bi bi-info-circle" data-bs-toggle="tooltip" title="Tareas que requieren atención inmediata"></i>
                </div>
                <div class="widget-value">8</div>
                <div class="widget-footer">
                    <div class="widget-trend">
                        <i class="bi bi-exclamation-triangle"></i>
                        <span>3 con alta prioridad</span>
                    </div>
                </div>
            </div>

            <div class="widget-card auditorias">
                <div class="widget-header">
                    <h3 class="widget-title">
                        <i class="bi bi-calendar-check"></i>
                        Próximas Auditorías
                    </h3>
                    <i class="bi bi-info-circle" data-bs-toggle="tooltip" title="Auditorías programadas para los próximos 30 días"></i>
                </div>
                <div class="widget-value">2</div>
                <div class="widget-footer">
                    <div class="widget-trend">
                        <i class="bi bi-clock"></i>
                        <span>Próxima en 15 días</span>
                    </div>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="upDashboard" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="filters-section mb-4">
                    <div class="row g-3 align-items-center">
                        <div class="col-md-6">
                            <div class="input-group">
                                <span class="input-group-text bg-transparent border-end-0">
                                    <i class="bi bi-search"></i>
                                </span>
                                <asp:TextBox ID="txtBuscar" runat="server" 
                                    CssClass="form-control border-start-0" 
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
                            <div class="iso-card <%# GetEstadoClass(Eval("Estado")?.ToString() ?? "") %>">
                                <div class="iso-card-header">
                                    <div class="iso-card-badges">
                                        <span class="iso-card-version"><%# Eval("Version") %></span>
                                        <span class="iso-card-status <%# GetEstadoClass(Eval("Estado")?.ToString() ?? "") %>">
                                            <%# GetStatusText(Eval("Estado")?.ToString() ?? "") %>
                                        </span>
                                    </div>
                                    <div class="iso-card-title-wrapper">
                                        <h3 class="iso-card-title">
                                            <i class="bi bi-shield-check"></i>
                                            <span class="norma-codigo"><%# Eval("TipoNorma") %></span>
                                            <span class="norma-descripcion"><%# HttpUtility.HtmlEncode(Eval("Nombre")) %></span>
                                        </h3>
                                    </div>
                                </div>

                                <div class="iso-card-body">
                                    <div class="iso-progress-section">
                                        <div class="iso-progress-header">
                                            <div class="iso-progress-icon">
                                                <i class="bi bi-graph-up"></i>
                                            </div>
                                            <div class="iso-progress-info">
                                                <span class="iso-progress-title">Progreso General</span>
                                                <span class="iso-progress-percentage"><%# Eval("Progreso") %>%</span>
                                            </div>
                                        </div>
                                        <div class="iso-progress-bar">
                                            <div class="iso-progress-fill" style="width: <%# Eval("Progreso") %>%"></div>
                                        </div>
                                    </div>

                                    <div class="iso-section-divider"></div>

                                    <div class="iso-stats-grid">
                                        <div class="iso-stat-item">
                                            <div class="iso-stat-info">
                                                <div class="iso-stat-icon">
                                                    <i class="bi bi-file-text"></i>
                                                </div>
                                                <div class="iso-stat-details">
                                                    <span class="iso-stat-label">Documentos</span>
                                                    <span class="iso-stat-value"><%# Eval("TotalDocumentos") %></span>
                                                </div>
                                            </div>
                                            <asp:LinkButton runat="server" 
                                                CssClass="iso-stat-action" 
                                                OnClick="OnIrDocumentosClick"
                                                CommandArgument='<%# Eval("NormaId") %>'
                                                ToolTip="Ver documentos">
                                                <i class="bi bi-arrow-right"></i>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="iso-stat-item">
                                            <div class="iso-stat-info">
                                                <div class="iso-stat-icon">
                                                    <i class="bi bi-gear"></i>
                                                </div>
                                                <div class="iso-stat-details">
                                                    <span class="iso-stat-label">Procesos</span>
                                                    <span class="iso-stat-value"><%# Eval("TotalProcesos") %></span>
                                                </div>
                                            </div>
                                            <asp:LinkButton runat="server" 
                                                CssClass="iso-stat-action" 
                                                OnClick="OnIrProcesosClick"
                                                CommandArgument='<%# Eval("NormaId") %>'
                                                ToolTip="Ver procesos">
                                                <i class="bi bi-arrow-right"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="iso-section-divider"></div>

                                    <div class="iso-meta-info">
                                        <div class="iso-meta-item">
                                            <div class="iso-meta-icon">
                                                <i class="bi bi-person"></i>
                                            </div>
                                            <div class="iso-meta-data">
                                                <span class="iso-meta-label">Responsable</span>
                                                <span class="iso-meta-value"><%# HttpUtility.HtmlEncode(Eval("Responsable")) %></span>
                                            </div>
                                        </div>
                                        <div class="iso-meta-item">
                                            <div class="iso-meta-icon">
                                                <i class="bi bi-clock" aria-hidden="true"></i>
                                            </div>
                                            <div class="iso-meta-data">
                                                <span class="iso-meta-label">Última modificación</span>
                                                <span class="iso-meta-value"><%# Eval("UltimaModificacion") %></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="iso-card-footer">
                                    <asp:LinkButton runat="server" 
                                        CssClass="iso-primary-action" 
                                        OnClick="OnIrComponentesClick"
                                        CommandArgument='<%# Eval("NormaId") %>'
                                        ToolTip="Ver estructura completa">
                                        Ver estructura completa
                                        <i class="bi bi-arrow-right"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
                        <div class="alert alert-info">
                            <i class="bi bi-info-circle me-2"></i>
                            No se encontraron normas para mostrar.
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        // Los tooltips se inicializan automáticamente desde Site.Master

        // Reinicializar estilos después de cada actualización parcial
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            // Reinicializar tooltips
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            tooltipTriggerList.forEach(function(tooltipTriggerEl) {
                new bootstrap.Tooltip(tooltipTriggerEl);
            });

            // Aplicar animaciones a las cards
            document.querySelectorAll('.iso-card').forEach(function(card) {
                card.style.animation = 'none';
                card.offsetHeight; // Trigger reflow
                card.style.animation = null;
            });
        });
    </script>
</asp:Content> 