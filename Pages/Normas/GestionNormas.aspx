<%@ Page Title="Gestión de Normas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionNormas.aspx.cs" Inherits="IsomanagerWeb.Pages.Normas.GestionNormas" %>
<%@ Register Src="~/Controls/ucUsuarioSelector.ascx" TagPrefix="uc1" TagName="ucUsuarioSelector" %>
<%@ Register Src="~/Controls/ucDepartamentoSelector.ascx" TagPrefix="uc1" TagName="ucDepartamentoSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="search-container">
                <div class="input-group">
                    <span class="input-group-text bg-transparent border-end-0">
                        <i class="bi bi-search"></i>
                    </span>
                    <asp:TextBox ID="txtBuscar" runat="server" 
                        CssClass="form-control border-start-0" 
                        placeholder="Buscar normas..."
                        AutoPostBack="true"
                        OnTextChanged="txtBuscar_TextChanged">
                    </asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnNuevaNorma" runat="server" Text="+ Agregar Norma" 
                CssClass="btn btn-dark" OnClick="btnNuevaNorma_Click" />
        </div>

        <asp:UpdatePanel ID="upNormas" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mb-3"></asp:Label>

                <div class="table-responsive">
                    <asp:GridView ID="gvNormas" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-hover align-middle" 
                        OnRowCommand="gvNormas_RowCommand" DataKeyNames="NormaId"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Titulo" HeaderText="Nombre" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:BoundField DataField="Version" HeaderText="Versión" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge rounded-pill <%# GetEstadoBadgeClass(Eval("Estado").ToString()) %>'>
                                        <%# Eval("Estado") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FechaImplementacion" HeaderText="Fecha Implementación" 
                                DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="ProximaAuditoria" HeaderText="Próxima Auditoría" 
                                DataFormatString="{0:yyyy-MM-dd}" />
                            <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="action-column">
                                <ItemTemplate>
                                    <div class="dropdown">
                                        <button class="btn btn-icon" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <i class="bi bi-three-dots-vertical"></i>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" CommandName="Editar" CommandArgument='<%# Eval("NormaId") %>'>
                                                    <i class="bi bi-pencil"></i>Editar
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnVerDetalles" runat="server" CssClass="dropdown-item" CommandName="VerDetalles" CommandArgument='<%# Eval("NormaId") %>'>
                                                    <i class="bi bi-eye"></i>Ver Detalles
                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnDescargar" runat="server" CssClass="dropdown-item" CommandName="Descargar" CommandArgument='<%# Eval("NormaId") %>'>
                                                    <i class="bi bi-download"></i>Descargar
                                                </asp:LinkButton>
                                            </li>
                                            <li><hr class="dropdown-divider"></li>
                                            <li>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger" CommandName="Eliminar" CommandArgument='<%# Eval("NormaId") %>'
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar esta norma?');">
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
        </asp:UpdatePanel>
    </div>

    <!-- Modal Nueva Norma -->
    <div class="modal fade" id="modalNuevaNorma" tabindex="-1" aria-labelledby="modalNuevaNormaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalNuevaNormaLabel">
                        <asp:Literal ID="litTituloModal" runat="server" Text="Nueva Norma"></asp:Literal>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="upModalNuevaNorma" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnNormaId" runat="server" Value="" />
                            <div class="mb-3">
                                <label for="<%= txtTitulo.ClientID %>" class="form-label">Título</label>
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" MaxLength="200" />
                                <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" 
                                    ControlToValidate="txtTitulo"
                                    ValidationGroup="NuevaNorma"
                                    CssClass="text-danger"
                                    ErrorMessage="El título es requerido." />
                            </div>
                            <div class="mb-3">
                                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" 
                                    TextMode="MultiLine" Rows="3" MaxLength="500" />
                            </div>
                            <div class="mb-3">
                                <label for="<%= txtVersion.ClientID %>" class="form-label">Versión</label>
                                <asp:TextBox ID="txtVersion" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="rfvVersion" runat="server" 
                                    ControlToValidate="txtVersion"
                                    ValidationGroup="NuevaNorma"
                                    CssClass="text-danger"
                                    ErrorMessage="La versión es requerida." />
                            </div>
                            <div class="mb-3">
                                <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Borrador" Value="Borrador" />
                                    <asp:ListItem Text="En Revisión" Value="En Revisión" />
                                    <asp:ListItem Text="Aprobado" Value="Aprobado" />
                                    <asp:ListItem Text="Obsoleto" Value="Obsoleto" />
                                </asp:DropDownList>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Responsable</label>
                                <uc1:ucUsuarioSelector runat="server" ID="ucResponsable" 
                                    Required="true" 
                                    ValidationGroup="NuevaNorma"
                                    ErrorMessage="El responsable es requerido." />
                            </div>
                            <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarNorma" runat="server" Text="Guardar" 
                        CssClass="btn btn-primary"
                        ValidationGroup="NuevaNorma"
                        OnClick="btnGuardarNorma_Click" />
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
            var modal = new bootstrap.Modal(document.getElementById('modalNuevaNorma'));
            modal.show();
        }

        function hideModal() {
            var modalElement = document.getElementById('modalNuevaNorma');
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
        var modalNuevaNorma = document.getElementById('modalNuevaNorma');
        if (modalNuevaNorma) {
            modalNuevaNorma.addEventListener('hidden.bs.modal', function () {
                hideModal();
            });
        }
    </script>
</asp:Content> 