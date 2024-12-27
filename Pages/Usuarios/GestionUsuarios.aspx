<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="IsomanagerWeb.Pages.Usuarios.GestionUsuarios" %>
<%@ Register Src="~/Controls/ucDepartamentoSelector.ascx" TagPrefix="uc1" TagName="ucDepartamentoSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Content/css/normas.css" rel="stylesheet" />

    <div class="container-fluid">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="search-container">
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtBuscar" runat="server" 
                        CssClass="form-control border-start-0" 
                        placeholder="Buscar usuarios..."
                        AutoPostBack="true"
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevoUsuario" runat="server" Text="+ Agregar Usuario" 
                CssClass="btn btn-dark" OnClick="btnNuevoUsuario_Click" />
        </div>

        <asp:UpdatePanel ID="upUsuarios" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mb-3"></asp:Label>

                <div class="table-responsive">
                    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-hover align-middle" 
                        OnRowCommand="gvUsuarios_RowCommand" DataKeyNames="UsuarioId"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Area" HeaderText="Área" />
                            <asp:BoundField DataField="Rol" HeaderText="Rol" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge rounded-pill <%# GetEstadoBadgeClass(Eval("Estado").ToString() == "Activo") %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="action-column">
                                <ItemTemplate>
                                    <div class="dropdown">
                                        <button class="btn btn-icon" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="bi bi-three-dots-vertical"></i>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" CommandName="Editar" CommandArgument='<%# Eval("UsuarioId") %>'>
                                                    <i class="bi bi-pencil"></i>Editar
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnVerPerfil" runat="server" CssClass="dropdown-item" CommandName="VerPerfil" CommandArgument='<%# Eval("UsuarioId") %>'>
                                                    <i class="bi bi-person"></i>Ver Perfil
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnCambiarPassword" runat="server" CssClass="dropdown-item" CommandName="CambiarPassword" CommandArgument='<%# Eval("UsuarioId") %>'>
                                                    <i class="bi bi-key"></i>Cambiar Contraseña
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnDesactivar" runat="server" CssClass="dropdown-item" CommandName="Desactivar" CommandArgument='<%# Eval("UsuarioId") %>'
                                                    Visible='<%# Eval("Estado").ToString() == "Activo" %>'>
                                                    <i class="bi bi-person-x"></i>Desactivar
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnActivar" runat="server" CssClass="dropdown-item" CommandName="Activar" CommandArgument='<%# Eval("UsuarioId") %>'
                                                    Visible='<%# Eval("Estado").ToString() == "Inactivo" %>'>
                                                    <i class="bi bi-person-check"></i>Activar
                                                </asp:LinkButton>
                                            </li>
                                            <li><hr class="dropdown-divider"></li>
                                            <li>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger" CommandName="Eliminar" CommandArgument='<%# Eval("UsuarioId") %>'
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este usuario?');">
                                                    <i class="bi bi-trash"></i>Eliminar
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
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtBuscar" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="gvUsuarios" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Nuevo Usuario -->
    <div class="modal fade" id="modalNuevoUsuario" tabindex="-1" aria-labelledby="modalNuevoUsuarioLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalNuevoUsuarioLabel">
                        <asp:Literal ID="litTituloModal" runat="server" Text="Nuevo Usuario"></asp:Literal>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upModalNuevoUsuario" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnUsuarioId" runat="server" Value="" />
                            <div class="mb-3">
                                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                    ControlToValidate="txtNombre"
                                    ValidationGroup="NuevoUsuario"
                                    CssClass="text-danger"
                                    ErrorMessage="El nombre es requerido." />
                            </div>
                            <div class="mb-3">
                                <label for="<%= txtEmail.ClientID %>" class="form-label">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" TextMode="Email" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail"
                                    ValidationGroup="NuevoUsuario"
                                    CssClass="text-danger"
                                    ErrorMessage="El email es requerido." />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                    ControlToValidate="txtEmail"
                                    ValidationGroup="NuevoUsuario"
                                    CssClass="text-danger"
                                    ErrorMessage="El email no es válido."
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            </div>
                            <div id="divPassword" runat="server">
                                <div class="mb-3">
                                    <label for="<%= txtPassword.ClientID %>" class="form-label">Contraseña</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                        ControlToValidate="txtPassword"
                                        ValidationGroup="NuevoUsuario"
                                        CssClass="text-danger"
                                        ErrorMessage="La contraseña es requerida." />
                                </div>
                                <div class="mb-3">
                                    <label for="<%= txtConfirmPassword.ClientID %>" class="form-label">Confirmar Contraseña</label>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                                        ControlToValidate="txtConfirmPassword"
                                        ValidationGroup="NuevoUsuario"
                                        CssClass="text-danger"
                                        ErrorMessage="La confirmación de contraseña es requerida." />
                                    <asp:CompareValidator ID="cvPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtPassword"
                                        ValidationGroup="NuevoUsuario"
                                        CssClass="text-danger"
                                        ErrorMessage="Las contraseñas no coinciden." />
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="<%= ddlRol.ClientID %>" class="form-label">Rol</label>
                                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Usuario" Value="Usuario" />
                                    <asp:ListItem Text="Administrador" Value="Administrador" />
                                </asp:DropDownList>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Área</label>
                                <uc1:ucDepartamentoSelector runat="server" ID="ucDepartamento" />
                            </div>
                            <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarUsuario" runat="server" Text="Guardar" 
                        CssClass="btn btn-primary"
                        ValidationGroup="NuevoUsuario"
                        OnClick="btnGuardarUsuario_Click" />
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

        function showModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalNuevoUsuario'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalNuevoUsuario');
            var modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) {
                modal.hide();
            }
            document.body.classList.remove('modal-open');
            var modalBackdrop = document.querySelector('.modal-backdrop');
            if (modalBackdrop) {
                modalBackdrop.remove();
            }
        }

        // Manejar el cierre del modal
        var modalNuevoUsuario = document.getElementById('modalNuevoUsuario');
        if (modalNuevoUsuario) {
            modalNuevoUsuario.addEventListener('hidden.bs.modal', function () {
                hideModal();
            });
        }
    </script>
</asp:Content> 