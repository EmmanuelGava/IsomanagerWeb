<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IsomanagerWeb.Pages.Dashboard" %>
<%@ OutputCache Location="None" NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widgets-container mb-4">
        <div class="widget-card">
            <div class="widget-header">
                <h3 class="widget-title">Cumplimiento General</h3>
                <i class="bi bi-info-circle" data-tooltip="Promedio de cumplimiento de todas las normas"></i>
            </div>
            <div class="widget-value">65%</div>
            <div class="widget-footer">
                <i class="bi bi-arrow-up-short text-success"></i>
                <span>5% más que el mes anterior</span>
            </div>
        </div>

        <div class="widget-card">
            <div class="widget-header">
                <h3 class="widget-title">Tareas Pendientes</h3>
                <i class="bi bi-info-circle" data-tooltip="Tareas que requieren atención inmediata"></i>
            </div>
            <div class="widget-value">8</div>
            <div class="widget-footer">
                <span class="text-warning">3 con alta prioridad</span>
            </div>
        </div>

        <div class="widget-card">
            <div class="widget-header">
                <h3 class="widget-title">Próximas Auditorías</h3>
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
                                            style="width: <%# Eval("Progreso") %>%"
                                            aria-valuemin="0" 
                                            aria-valuemax="100"
                                            aria-valuenow="<%# Eval("Progreso") %>">
                                        </div>
                                    </div>
                                    <div class="progress-value"><%# Eval("Progreso") %>%</div>
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
                                                CssClass="btn btn-link text-dark p-0" 
                                                OnClick="OnIrProcesosClick"
                                                CommandArgument='<%# Eval("NormaId") %>'>
                                                <i class="bi bi-arrow-right"></i>
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
</asp:Content> 