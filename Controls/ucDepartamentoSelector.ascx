<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDepartamentoSelector.ascx.cs" Inherits="IsomanagerWeb.Controls.ucDepartamentoSelector" %>

<asp:UpdatePanel ID="upDepartamento" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="departamento-selector">
            <div class="input-group">
                <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione un área" Value="" />
                    <asp:ListItem Text="[Crear Nueva Área]" Value="nuevo" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvDepartamento" runat="server"
                    ControlToValidate="ddlDepartamento"
                    InitialValue=""
                    Display="Dynamic"
                    CssClass="text-danger"
                    ErrorMessage="El área es requerida."
                    Enabled="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<!-- Modal Nueva Área -->
<div class="modal fade" id="modalNuevaArea" tabindex="-1" aria-labelledby="modalNuevaAreaLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalNuevaAreaLabel">Nueva Área</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <asp:UpdatePanel ID="upModalArea" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="<%=txtNombreArea.ClientID %>" class="form-label">Nombre del Área</label>
                            <asp:TextBox ID="txtNombreArea" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNombreArea" runat="server" 
                                ControlToValidate="txtNombreArea" 
                                ValidationGroup="NuevaArea"
                                CssClass="text-danger" 
                                Display="Dynamic"
                                ErrorMessage="El nombre del área es requerido." />
                        </div>
                        <div class="mb-3">
                            <label for="<%=txtDescripcionArea.ClientID %>" class="form-label">Descripción</label>
                            <asp:TextBox ID="txtDescripcionArea" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            <asp:RequiredFieldValidator ID="rfvDescripcionArea" runat="server" 
                                ControlToValidate="txtDescripcionArea" 
                                ValidationGroup="NuevaArea"
                                CssClass="text-danger" 
                                Display="Dynamic"
                                ErrorMessage="La descripción es requerida." />
                        </div>
                        <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnGuardarArea" runat="server" Text="Guardar" 
                            CssClass="btn btn-dark" 
                            ValidationGroup="NuevaArea"
                            OnClick="btnGuardarArea_Click" 
                            UseSubmitBehavior="false" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarArea" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddlDepartamento" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>

<script type="text/javascript">
    function showModalNuevaArea() {
        var myModal = new bootstrap.Modal(document.getElementById('modalNuevaArea'));
        myModal.show();
    }

    function closeModalNuevaArea() {
        var modal = bootstrap.Modal.getInstance(document.getElementById('modalNuevaArea'));
        if (modal) {
            modal.hide();
        }
        // Limpiar el fondo oscuro
        document.body.classList.remove('modal-open');
        var modalBackdrop = document.querySelector('.modal-backdrop');
        if (modalBackdrop) {
            modalBackdrop.remove();
        }
    }
</script> 