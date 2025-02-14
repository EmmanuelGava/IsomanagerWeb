<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IsomanagerWeb.Pages.Dashboard" %>
<%@ OutputCache Location="None" NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/Content/css/dashboard.css") %>" rel="stylesheet" type="text/css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <!-- Header Section -->
        <div class="dashboard-header">
            <h2>Buenos días, admin</h2>
            <div class="search-container">
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar..."></asp:TextBox>
            </div>
        </div>

        <!-- Widgets Section -->
        <div class="widgets-container">
            <div class="widget-card cumplimiento">
                <div class="widget-header">
                    <h3 class="widget-title">
                        <i class="bi bi-check-circle"></i>
                        Cumplimiento General
                    </h3>
                    <i class="bi bi-info-circle"></i>
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
                    <i class="bi bi-info-circle"></i>
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
                    <i class="bi bi-info-circle"></i>
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

        <!-- Main Content Area -->
        <div class="dashboard-content">
            <div class="row">
                <!-- Left Side - ISO Cards -->
                <div class="col-md-9">
                    <!-- Filters Section -->
                    <div class="filters-section mb-4">
                        <div class="search-box">
                            <input type="text" placeholder="Buscar normas ISO o procesos..." />
                        </div>
                        <div class="filter-options">
                            <select>
                                <option>Todos los estados</option>
                            </select>
                            <div class="toggle-group">
                                <label>Solo normas activas</label>
                                <div class="toggle-switch"></div>
                            </div>
                            <div class="toggle-group">
                                <label>Mostrar progreso</label>
                                <div class="toggle-switch"></div>
                            </div>
                        </div>
                    </div>

                    <!-- ISO Cards Container -->
                    <div class="iso-cards-container">
                        <asp:Repeater ID="rptNormas1" runat="server" 
                            OnItemDataBound="OnNormasItemDataBound"
                            OnItemCommand="rptNormas1_ItemCommand">
                            <ItemTemplate>
                                <div class="iso-card <%# GetEstadoClass(Eval("Estado")?.ToString() ?? "") %>">
                                    <div class="iso-card-header">
                                        <div class="iso-card-badges">
                                            <span class="iso-card-version">V<%# Eval("Version") %></span>
                                        </div>
                                        <span class="iso-card-status <%# GetEstadoClass(Eval("Estado")?.ToString() ?? "") %>">
                                            <%# GetStatusText(Eval("Estado")?.ToString() ?? "") %>
                                        </span>
                                    </div>

                                    <div class="iso-card-body">
                                        <div class="iso-card-title-wrapper">
                                            <h3 class="iso-card-title"><%# Eval("TipoNorma") %></h3>
                                            <p class="iso-card-subtitle"><%# HttpUtility.HtmlEncode(Eval("Nombre")) %></p>
                                        </div>

                                        <div class="iso-progress-section">
                                            <div class="iso-progress-label">
                                                <span>Progreso General</span>
                                                <span><%# Eval("Progreso") %>%</span>
                                            </div>
                                            <div class="iso-progress-bar">
                                                <div class="iso-progress-fill progress-<%# Convert.ToInt32(Eval("Progreso")) %>"></div>
                                            </div>
                                        </div>

                                        <div class="iso-stats">
                                            <asp:LinkButton runat="server" CssClass="iso-stat" CommandName="VerDocumentos" CommandArgument='<%# Eval("NormaId") %>'>
                                                <i class="bi bi-file-text"></i>
                                                <span><%# Eval("TotalDocumentos") %> Documentos</span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" CssClass="iso-stat" CommandName="VerProcesos" CommandArgument='<%# Eval("NormaId") %>'>
                                                <i class="bi bi-gear"></i>
                                                <span><%# Eval("TotalProcesos") %> Procesos</span>
                                            </asp:LinkButton>
                                        </div>

                                        <div class="iso-meta">
                                            <div class="iso-meta-item">
                                                <i class="bi bi-person"></i>
                                                <span>Responsable</span>
                                                <strong><%# Eval("ResponsableNombre") %></strong>
                                            </div>
                                            <div class="iso-meta-item">
                                                <i class="bi bi-calendar"></i>
                                                <span>Última modificación</span>
                                                <strong><%# Convert.ToDateTime(Eval("UltimaModificacion")).ToString("dd/MM/yyyy") %></strong>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="iso-card-footer">
                                        <asp:LinkButton runat="server" 
                                            CssClass="iso-primary-action" 
                                            OnClick="OnIrComponentesClick"
                                            CommandArgument='<%# Eval("NormaId") %>'>
                                            Ver estructura completa
                                            <i class="bi bi-arrow-right"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

                <!-- Right Side - Chart and Audits -->
                <div class="col-md-3">
                    <!-- Monthly Progress Chart -->
                    <div class="chart-section">
                        <h3 class="chart-title">
                            <i class="bi bi-graph-up"></i>
                            Progreso Mensual
                        </h3>
                        <div class="chart-container">
                            <canvas id="monthlyProgressChart"></canvas>
                        </div>
                    </div>

                    <!-- Auditorías Section -->
                    <div class="audit-section">
                        <h3>Auditorías Programadas</h3>
                        
                        <div class="audit-item">
                            <div class="audit-icon">
                                <i class="bi bi-calendar-event"></i>
                            </div>
                            <div class="audit-content">
                                <h4 class="audit-title">Auditoría Interna</h4>
                                <div class="audit-meta">
                                    <i class="bi bi-clock"></i>
                                    <span>13/02/2025 - 17:44</span>
                                    <span class="audit-status">En Proceso</span>
                                </div>
                            </div>
                        </div>

                        <!-- Weekly Calendar -->
                        <div class="weekly-calendar">
                            <div class="calendar-header">
                                <h4>Calendario Semanal</h4>
                                <button type="button" class="btn btn-link">
                                    <i class="bi bi-calendar3"></i>
                                    Ver Mes Completo
                                </button>
                            </div>
                            <div class="calendar-days">
                                <div class="calendar-row">
                                    <div class="day-label">Lun</div>
                                    <div class="day-label">Mar</div>
                                    <div class="day-label">Mié</div>
                                    <div class="day-label">Jue</div>
                                    <div class="day-label">Vie</div>
                                    <div class="day-label">Sáb</div>
                                    <div class="day-label">Dom</div>
                                </div>
                                <div class="calendar-row">
                                    <div class="day-number">10/2</div>
                                    <div class="day-number">11/2</div>
                                    <div class="day-number">12/2</div>
                                    <div class="day-number active">13/2</div>
                                    <div class="day-number">14/2</div>
                                    <div class="day-number">15/2</div>
                                    <div class="day-number">16/2</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function() {
            const ctx = document.getElementById('monthlyProgressChart').getContext('2d');
            const gradient = ctx.createLinearGradient(0, 0, 0, 200);
            gradient.addColorStop(0, 'rgba(13, 110, 253, 0.2)');
            gradient.addColorStop(1, 'rgba(13, 110, 253, 0)');

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun'],
                    datasets: [{
                        data: [68, 70, 72, 70, 69, 68],
                        borderColor: '#0d6efd',
                        borderWidth: 2,
                        backgroundColor: gradient,
                        fill: true,
                        tension: 0.4,
                        pointRadius: 4,
                        pointBackgroundColor: '#fff',
                        pointBorderColor: '#0d6efd',
                        pointBorderWidth: 2
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            max: 100,
                            ticks: {
                                stepSize: 20,
                                callback: function(value) {
                                    return value + '%';
                                }
                            },
                            grid: {
                                color: '#f0f0f0'
                            }
                        },
                        x: {
                            grid: {
                                display: false
                            }
                        }
                    }
                }
            });
        });
    </script>
</asp:Content> 