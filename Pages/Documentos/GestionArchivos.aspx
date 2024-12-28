<%@ Page Title="Gestión de Archivos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionArchivos.aspx.cs" Inherits="IsomanagerWeb.Pages.Documentos.GestionArchivos" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc" TagName="UsuarioSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <!-- Encabezado y botón volver -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="mb-1">
                    <asp:Literal ID="litTituloPagina" runat="server"></asp:Literal>
                </h2>
                <div class="text-muted">
                    <asp:Literal ID="litNormaTitulo" runat="server"></asp:Literal>
                </div>
            </div>
            <asp:LinkButton ID="btnVolver" runat="server" CssClass="btn btn-outline-primary" OnClick="btnVolver_Click">
                <i class="bi bi-arrow-left"></i> Volver a Procesos
            </asp:LinkButton>
        </div>

        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert alert-danger"></asp:Label>

        <!-- Cards de contadores -->
        <div class="row mb-4">
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Activos</h6>
                                <h3 class="mb-0"><asp:Literal ID="litActivos" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-check-circle fs-4 text-success"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">En Revisión</h6>
                                <h3 class="mb-0"><asp:Literal ID="litEnRevision" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-clock fs-4 text-warning"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Obsoletos</h6>
                                <h3 class="mb-0"><asp:Literal ID="litObsoletos" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-x-circle fs-4 text-danger"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="text-muted mb-2">Pendientes</h6>
                                <h3 class="mb-0"><asp:Literal ID="litPendientes" runat="server">0</asp:Literal></h3>
                            </div>
                            <div class="icon-shape bg-light rounded-circle p-3">
                                <i class="bi bi-hourglass fs-4 text-info"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Barra de búsqueda y botón nuevo -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="search-container">
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control border-start-0" 
                        placeholder="Buscar archivos..." AutoPostBack="true" 
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevoArchivo" runat="server" Text="Nuevo Archivo" 
                CssClass="btn btn-primary" OnClick="btnNuevoArchivo_Click" />
        </div>

        <!-- Grid de Archivos -->
        <asp:UpdatePanel ID="upArchivos" runat="server">
            <ContentTemplate>
                <div class="table-responsive">
                    <asp:GridView ID="gvArchivos" runat="server" CssClass="table table-hover" 
                        AutoGenerateColumns="False" DataKeyNames="ArchivoId"
                        OnRowCommand="gvArchivos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Version" HeaderText="Versión" />
                            <asp:BoundField DataField="FechaCreacion" HeaderText="Fecha Creación" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Creador" HeaderText="Creador" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="dropdown">
                                        <button class="btn btn-link dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="bi bi-three-dots-vertical"></i>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                    CommandName="Editar" CommandArgument='<%# Eval("ArchivoId") %>'>
                                                    <i class="bi bi-pencil me-2"></i> Editar
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnDescargar" runat="server" CssClass="dropdown-item"
                                                    CommandName="Descargar" CommandArgument='<%# Eval("ArchivoId") %>'>
                                                    <i class="bi bi-download me-2"></i> Descargar
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnVer" runat="server" CssClass="dropdown-item"
                                                    CommandName="Ver" CommandArgument='<%# Eval("ArchivoId") %>'>
                                                    <i class="bi bi-eye me-2"></i> Ver Detalles
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <hr class="dropdown-divider">
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                    CommandName="Eliminar" CommandArgument='<%# Eval("ArchivoId") %>'
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este archivo?');">
                                                    <i class="bi bi-trash me-2"></i> Eliminar
                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- El resto del código del modal se mantiene igual -->
</asp:Content>
