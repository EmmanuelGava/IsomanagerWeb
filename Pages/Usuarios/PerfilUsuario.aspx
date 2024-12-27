<%@ Page Title="Perfil de Usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs" Inherits="IsomanagerWeb.Pages.Usuarios.PerfilUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Content/css/perfil-usuario.css" rel="stylesheet" />
    
    <div class="container-fluid py-4">
        <div class="row">
            <div class="col-12">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center mb-4">
                            <div class="d-flex align-items-center">
                                <div id="avatarCircle" runat="server" class="avatar-circle avatar-lg">
                                    <asp:Literal ID="litInitials" runat="server"></asp:Literal>
                                </div>
                                <div class="ms-3">
                                    <h1 class="h3 mb-1">
                                        <asp:Literal ID="litNombreUsuario" runat="server"></asp:Literal>
                                    </h1>
                                    <p class="text-muted mb-0">
                                        <asp:Literal ID="litEmail" runat="server"></asp:Literal>
                                    </p>
                                    <p class="text-muted mb-0">
                                        Área: <asp:Literal ID="litArea" runat="server"></asp:Literal>
                                    </p>
                                    <p class="text-muted mb-0">
                                        Fecha de registro: <asp:Literal ID="litFechaRegistro" runat="server"></asp:Literal>
                                    </p>
                                    <div class="mt-2">
                                        <span class="badge bg-primary me-2">
                                            <asp:Literal ID="litRol" runat="server"></asp:Literal>
                                        </span>
                                        <span class="badge bg-success">
                                            <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-outline-dark me-2" OnClick="btnEditar_Click">
                                    <i class="bi bi-pencil me-2"></i>Editar
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnNuevoMensajeModal" runat="server" CssClass="btn btn-dark" data-bs-toggle="modal" data-bs-target="#modalNuevoMensaje">
                                    <i class="bi bi-envelope me-2"></i>Enviar Mensaje
                                </asp:LinkButton>
                            </div>
                        </div>

                        <ul class="nav nav-tabs mb-4" id="userTabs" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="resumen-tab" data-bs-toggle="tab" data-bs-target="#resumen" type="button" role="tab" aria-controls="resumen" aria-selected="true">
                                    <i class="bi bi-clipboard-data me-2"></i>Resumen
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="formacion-tab" data-bs-toggle="tab" data-bs-target="#formacion" type="button" role="tab" aria-controls="formacion" aria-selected="false">
                                    <i class="bi bi-mortarboard me-2"></i>Formación
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="procesos-tab" data-bs-toggle="tab" data-bs-target="#procesos" type="button" role="tab" aria-controls="procesos" aria-selected="false">
                                    <i class="bi bi-diagram-3 me-2"></i>Procesos
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="calificaciones-tab" data-bs-toggle="tab" data-bs-target="#calificaciones" type="button" role="tab" aria-controls="calificaciones" aria-selected="false">
                                    <i class="bi bi-star me-2"></i>Calificaciones
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="tareas-tab" data-bs-toggle="tab" data-bs-target="#tareas" type="button" role="tab" aria-controls="tareas" aria-selected="false">
                                    <i class="bi bi-list-task me-2"></i>Tareas
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="mensajes-tab" data-bs-toggle="tab" data-bs-target="#mensajes" type="button" role="tab" aria-controls="mensajes" aria-selected="false">
                                    <i class="bi bi-envelope me-2"></i>Mensajes
                                </button>
                            </li>
                        </ul>

                        <div class="tab-content" id="userTabsContent">
                            <div class="tab-pane fade show active" id="resumen" role="tabpanel" aria-labelledby="resumen-tab">
                                <h4 class="mb-4">Resumen de Rendimiento</h4>
                                <p class="text-muted mb-4">Vista general del desempeño y logros del usuario</p>

                                <div class="row">
                                    <div class="col-12 mb-4">
                                        <div class="progress" style="height: 25px;">
                                            <asp:Panel ID="pnlProgressBar" runat="server" CssClass="progress-bar bg-success" role="progressbar">
                                                <asp:Literal ID="litRendimiento" runat="server"></asp:Literal>%
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-4">
                                    <div class="col-md-3">
                                        <div class="card bg-light">
                                            <div class="card-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="bi bi-star text-warning fs-3 me-3"></i>
                                                    <div>
                                                        <h6 class="mb-0">Valoración Promedio</h6>
                                                        <h4 class="mb-0">
                                                            <asp:Literal ID="litValoracion" runat="server"></asp:Literal>/5
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="card bg-light">
                                            <div class="card-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="bi bi-check-circle text-success fs-3 me-3"></i>
                                                    <div>
                                                        <h6 class="mb-0">Proyectos Completados</h6>
                                                        <h4 class="mb-0">
                                                            <asp:Literal ID="litProyectos" runat="server"></asp:Literal>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="card bg-light">
                                            <div class="card-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="bi bi-award text-primary fs-3 me-3"></i>
                                                    <div>
                                                        <h6 class="mb-0">Certificaciones</h6>
                                                        <h4 class="mb-0">
                                                            <asp:Literal ID="litCertificaciones" runat="server"></asp:Literal>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="card bg-light">
                                            <div class="card-body">
                                                <div class="d-flex align-items-center">
                                                    <i class="bi bi-clock text-info fs-3 me-3"></i>
                                                    <div>
                                                        <h6 class="mb-0">Formación Total</h6>
                                                        <h4 class="mb-0">
                                                            <asp:Literal ID="litFormacion" runat="server"></asp:Literal> horas
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="formacion" role="tabpanel" aria-labelledby="formacion-tab">
                                <h4 class="mb-4">Formación y Certificaciones</h4>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvFormacion" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-hover align-middle" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="TipoFormacion" HeaderText="Tipo" />
                                            <asp:BoundField DataField="Duracion" HeaderText="Duración (horas)" />
                                            <asp:BoundField DataField="FechaObtencion" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <span class='badge <%# GetFormacionBadgeClass(Eval("Estado").ToString()) %>'>
                                                        <%# Eval("Estado") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="procesos" role="tabpanel" aria-labelledby="procesos-tab">
                                <h4 class="mb-4">Procesos Asignados</h4>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvProcesos" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-hover align-middle" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Nombre" HeaderText="Proceso" />
                                            <asp:BoundField DataField="Area" HeaderText="Área" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <span class='badge <%# GetProcesoBadgeClass(Eval("Estado").ToString()) %>'>
                                                        <%# Eval("Estado") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Rol" HeaderText="Rol" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- Pestaña de Calificaciones -->
                            <div class="tab-pane fade" id="calificaciones" role="tabpanel" aria-labelledby="calificaciones-tab">
                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <div>
                                        <h4 class="mb-1">Calificaciones y Evaluaciones</h4>
                                        <p class="text-muted mb-0">Historial de evaluaciones del usuario</p>
                                    </div>
                                    <asp:LinkButton ID="btnNuevaCalificacion" runat="server" CssClass="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalNuevaCalificacion">
                                        <i class="bi bi-plus-lg me-2"></i>Nueva Calificación
                                    </asp:LinkButton>
                                </div>
                                
                                <div class="table-responsive">
                                    <asp:GridView ID="gvCalificaciones" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-hover align-middle" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="Evaluador" HeaderText="Evaluador" />
                                            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                            <asp:TemplateField HeaderText="Calificación">
                                                <ItemTemplate>
                                                    <div class="text-warning">
                                                        <%# GetStarRating(Convert.ToInt32(Eval("Calificacion"))) %>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- Pestaña de Tareas -->
                            <div class="tab-pane fade" id="tareas" role="tabpanel" aria-labelledby="tareas-tab">
                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <div>
                                        <h4 class="mb-1">Tareas Asignadas</h4>
                                        <p class="text-muted mb-0">Lista de tareas y actividades pendientes</p>
                                    </div>
                                    <asp:LinkButton ID="btnNuevaTarea" runat="server" CssClass="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalNuevaTarea">
                                        <i class="bi bi-plus-lg me-2"></i>Asignar Tarea
                                    </asp:LinkButton>
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView ID="gvTareas" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-hover align-middle" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Titulo" HeaderText="Título" />
                                            <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha Creación" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <span class='badge <%# GetTareaStatusBadgeClass(Eval("Estado").ToString()) %>'>
                                                        <%# Eval("Estado") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prioridad">
                                                <ItemTemplate>
                                                    <span class='badge <%# GetTareaPrioridadBadgeClass(Eval("Prioridad").ToString()) %>'>
                                                        <%# Eval("Prioridad") %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AsignadoPor" HeaderText="Asignado Por" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <!-- Pestaña de Mensajes -->
                            <div class="tab-pane fade" id="mensajes" role="tabpanel" aria-labelledby="mensajes-tab">
                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <div>
                                        <h4 class="mb-1">Mensajes</h4>
                                        <p class="text-muted mb-0">Comunicación directa con el usuario</p>
                                    </div>
                                    <asp:LinkButton ID="btnNuevoMensaje" runat="server" CssClass="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalNuevoMensaje">
                                        <i class="bi bi-plus-lg me-2"></i>Nuevo Mensaje
                                    </asp:LinkButton>
                                </div>

                                <div class="messages-container">
                                    <asp:Repeater ID="rptMensajes" runat="server">
                                        <ItemTemplate>
                                            <div class='message-item <%# Convert.ToBoolean(Eval("IsOutgoing")) ? "outgoing" : "incoming" %>'>
                                                <div class="message-content">
                                                    <div class="message-header">
                                                        <strong><%# Eval("FromUser") %></strong>
                                                        <small class="text-muted"><%# Eval("FechaEnvio", "{0:dd/MM/yyyy HH:mm}") %></small>
                                                    </div>
                                                    <div class="message-body">
                                                        <%# Eval("Contenido") %>
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
    </div>

    <!-- Modal Nueva Calificación -->
    <div class="modal fade" id="modalNuevaCalificacion" tabindex="-1" aria-labelledby="modalNuevaCalificacionLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalNuevaCalificacionLabel">Nueva Calificación</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="ddlCategoria" class="form-label">Categoría</label>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Desempeño General" Value="Desempeño" />
                            <asp:ListItem Text="Trabajo en Equipo" Value="Equipo" />
                            <asp:ListItem Text="Cumplimiento de Objetivos" Value="Objetivos" />
                            <asp:ListItem Text="Calidad del Trabajo" Value="Calidad" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="ddlCalificacion" class="form-label">Calificación</label>
                        <asp:DropDownList ID="ddlCalificacion" runat="server" CssClass="form-select">
                            <asp:ListItem Text="5 - Excelente" Value="5" />
                            <asp:ListItem Text="4 - Muy Bueno" Value="4" />
                            <asp:ListItem Text="3 - Bueno" Value="3" />
                            <asp:ListItem Text="2 - Regular" Value="2" />
                            <asp:ListItem Text="1 - Necesita Mejorar" Value="1" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="txtComentarios" class="form-label">Comentarios</label>
                        <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarCalificacion" runat="server" Text="Guardar Calificación" CssClass="btn btn-primary" OnClick="btnGuardarCalificacion_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Nueva Tarea -->
    <div class="modal fade" id="modalNuevaTarea" tabindex="-1" aria-labelledby="modalNuevaTareaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalNuevaTareaLabel">Asignar Nueva Tarea</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtTituloTarea" class="form-label">Título</label>
                        <asp:TextBox ID="txtTituloTarea" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtDescripcionTarea" class="form-label">Descripción</label>
                        <asp:TextBox ID="txtDescripcionTarea" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label for="txtFechaVencimiento" class="form-label">Fecha de Vencimiento</label>
                            <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="ddlPrioridad" class="form-label">Prioridad</label>
                            <asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Alta" Value="Alta" />
                                <asp:ListItem Text="Media" Value="Media" />
                                <asp:ListItem Text="Baja" Value="Baja" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarTarea" runat="server" Text="Asignar Tarea" CssClass="btn btn-primary" OnClick="btnGuardarTarea_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Nuevo Mensaje -->
    <div class="modal fade" id="modalNuevoMensaje" tabindex="-1" aria-labelledby="modalNuevoMensajeLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalNuevoMensajeLabel">Nuevo Mensaje</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="txtAsunto" class="form-label">Asunto</label>
                        <asp:TextBox ID="txtAsunto" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtMensaje" class="form-label">Mensaje</label>
                        <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnEnviarMensaje" runat="server" Text="Enviar Mensaje" CssClass="btn btn-primary" OnClick="btnEnviarMensaje_Click" />
                </div>
            </div>
        </div>
    </div>

    <style>
        .messages-container {
            max-height: 500px;
            overflow-y: auto;
            padding: 1rem;
        }

        .message-item {
            margin-bottom: 1rem;
            display: flex;
        }

        .message-item.outgoing {
            justify-content: flex-end;
        }

        .message-content {
            max-width: 70%;
            padding: 1rem;
            border-radius: 0.5rem;
            background-color: #f8f9fa;
        }

        .message-item.outgoing .message-content {
            background-color: #e3f2fd;
        }

        .message-header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 0.5rem;
        }

        .message-body {
            white-space: pre-wrap;
        }
    </style>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            // Inicializar las pestañas de Bootstrap
            var firstTabEl = document.querySelector('#userTabs button[data-bs-toggle="tab"]');
            var firstTab = new bootstrap.Tab(firstTabEl);
            firstTab.show();

            // Manejar el cambio de pestañas
            document.querySelectorAll('#userTabs button').forEach(function(button) {
                button.addEventListener('click', function (e) {
                    e.preventDefault();
                    var tab = new bootstrap.Tab(this);
                    tab.show();
                });
            });
        });
    </script>
</asp:Content> 