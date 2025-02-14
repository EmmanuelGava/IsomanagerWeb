<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="IsomanagerWeb.Pages.Dashboard" %>
<%@ Register Src="~/Controls/ucCalendario.ascx" TagPrefix="uc" TagName="Calendario" %>
<%@ Register Src="~/Controls/ucProximasAuditorias.ascx" TagPrefix="uc" TagName="ProximasAuditorias" %>
<%@ OutputCache Location="None" NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/Content/css/dashboard.css") %>" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.7.2/font/bootstrap-icons.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.css" rel="stylesheet" />
    <script src="<%: ResolveUrl("~/Scripts/chart.min.js") %>"></script>
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/main.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/fullcalendar@5.11.3/locales/es.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>
    <script>
        // Fallback si Chart.js no se carga desde la CDN
        window.addEventListener('error', function(e) {
            if (e.target.src && e.target.src.indexOf('chart.js') > -1) {
                var fallbackScript = document.createElement('script');
                fallbackScript.src = '<%: ResolveUrl("~/Scripts/chart.umd.min.js") %>';
                document.head.appendChild(fallbackScript);
            }
        }, true);
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashboard-container">
        <!-- Header Section -->
        <div class="dashboard-header">
            <div class="welcome-section">
                <asp:Literal ID="litSaludo" runat="server"></asp:Literal>
                <span class="user-initials"><asp:Literal ID="litUserInitials" runat="server"></asp:Literal></span>
            </div>
        </div>

        <!-- Mensajes -->
        <asp:Panel ID="pnlMensajes" runat="server" Visible="false" CssClass="alert alert-info">
            <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
        </asp:Panel>

        <!-- Main Content -->
        <div class="container-fluid p-0">
            <div class="row">
                <!-- Left Side - Stats and ISO Cards -->
                <div class="col-md-9">
                    <!-- Stats Cards -->
                    <div class="stats-container">
                        <div class="stat-card">
                            <div class="stat-header">
                                <i class="bi bi-check-circle"></i>
                                <span>Cumplimiento General</span>
                            </div>
                            <div class="stat-value">65%</div>
                            <div class="stat-trend">↑ 5% más que el mes anterior</div>
                        </div>
                        <div class="stat-card">
                            <div class="stat-header">
                                <i class="bi bi-exclamation-triangle"></i>
                                <span>Tareas Pendientes</span>
                            </div>
                            <div class="stat-value">8</div>
                            <div class="stat-description">3 con alta prioridad</div>
                        </div>
                        <div class="stat-card">
                            <div class="stat-header">
                                <i class="bi bi-calendar-event"></i>
                                <span>Próximas Auditorías</span>
                            </div>
                            <div class="stat-value">2</div>
                            <div class="stat-description">Próxima en 15 días</div>
                        </div>
                    </div>

                    <!-- ISO Section -->
                    <div class="iso-section">
                        <asp:UpdatePanel ID="upNormas" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- Filters Section -->
                                <div class="filters-section mb-4">
                                    <div class="search-box">
                                        <asp:TextBox ID="txtBuscarNormas" runat="server" 
                                            CssClass="form-control" 
                                            placeholder="Buscar normas ISO o procesos..."
                                            AutoPostBack="true"
                                            OnTextChanged="OnBuscarClick" />
                                    </div>
                                    <div class="filter-options">
                                        <div class="filter-group">
                                            <label for="<%= ddlEstadoFiltro.ClientID %>">Estado:</label>
                                            <asp:DropDownList ID="ddlEstadoFiltro" runat="server" 
                                                CssClass="form-select"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlEstadoFiltro_SelectedIndexChanged">
                                                <asp:ListItem Text="Todos los estados" Value="" />
                                                <asp:ListItem Text="Borrador" Value="Borrador" />
                                                <asp:ListItem Text="En Revisión" Value="En Revisión" />
                                                <asp:ListItem Text="Aprobado" Value="Aprobado" />
                                                <asp:ListItem Text="Obsoleto" Value="Obsoleto" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="filter-group">
                                            <label for="<%= chkSoloActivas.ClientID %>" class="form-check-label">
                                                <asp:CheckBox ID="chkSoloActivas" runat="server" 
                                                    CssClass="form-check-input"
                                                    AutoPostBack="true"
                                                    OnCheckedChanged="chkSoloActivas_CheckedChanged" />
                                                Solo normas activas
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <!-- Panel No Data -->
                                <asp:Panel ID="pnlNoData" runat="server" Visible="false" CssClass="alert alert-info">
                                    No se encontraron normas ISO.
                                </asp:Panel>

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
                                                        <span class="status-badge <%# GetStatusBadgeClass(Eval("Estado")?.ToString() ?? "") %>">
                                                            <%# GetStatusText(Eval("Estado")?.ToString() ?? "") %>
                                                        </span>
                                                    </div>
                                                </div>

                                                <div class="iso-card-body">
                                                    <div class="iso-card-title-wrapper">
                                                        <h3 class="iso-card-title"><%# HttpUtility.HtmlEncode(Eval("Titulo")) %></h3>
                                                    </div>

                                                    <div class="iso-progress-section">
                                                        <div class="iso-progress-label">
                                                            <span>Progreso General</span>
                                                            <span><%# Eval("Progreso") %>%</span>
                                                        </div>
                                                        <div class="iso-progress-bar">
                                                            <div class="iso-progress-fill" style="width: <%# Eval("Progreso") %>%"></div>
                                                        </div>
                                                    </div>

                                                    <div class="iso-stats">
                                                        <asp:LinkButton runat="server" CssClass="iso-stat" CommandName="VerDocumentos" CommandArgument='<%# Eval("NormaId") %>'>
                                                            <i class="bi bi-file-text"></i>
                                                            <div class="stat-count"><%# Eval("TotalDocumentos") %></div>
                                                            <span>Documentos</span>
                                                            <i class="bi bi-chevron-right"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" CssClass="iso-stat" CommandName="VerProcesos" CommandArgument='<%# Eval("NormaId") %>' OnClick="OnIrProcesosClick">
                                                            <i class="bi bi-gear"></i>
                                                            <div class="stat-count"><%# Eval("TotalProcesos") %></div>
                                                            <span>Procesos</span>
                                                            <i class="bi bi-chevron-right"></i>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <!-- Right Side - Chart and Tasks -->
                <div class="col-md-3">
                    <div class="sidebar">
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

                            <!-- Reemplazar el calendario hardcodeado por el User Control -->
                            <uc:Calendario runat="server" ID="ucCalendario" />
                        </div>

                        <!-- Tasks Section -->
                        <div class="tasks-section">
                            <h3 class="section-title">Tareas Pendientes</h3>
                            <div class="tasks-list">
                                <asp:Repeater ID="rptTareas" runat="server">
                                    <ItemTemplate>
                                        <div class="task-item">
                                            <div class="task-icon">
                                                <i class="bi <%# GetTaskIcon(Eval("TipoTarea").ToString()) %>"></i>
                                            </div>
                                            <div class="task-content">
                                                <h4 class="task-title"><%# Eval("Titulo") %></h4>
                                                <div class="task-meta">
                                                    <span class="task-id"><%# Eval("TaskId") %></span>
                                                    <span class="task-priority <%# GetTaskPriorityClass(Eval("Prioridad").ToString()) %>">
                                                        <%# Eval("Prioridad") %>
                                                    </span>
                                                </div>
                                                <div class="task-assignee">
                                                    <i class="bi bi-person"></i>
                                                    <%# Eval("ResponsableNombre") %>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
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