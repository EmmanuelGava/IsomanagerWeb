<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSeleccionAreas.ascx.cs" Inherits="IsomanagerWeb.Controls.ucSeleccionAreas" %>

<div class="card">
    <div class="card-body">
        <div class="mb-3">
            <asp:UpdatePanel ID="upAreas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ListBox ID="lstAreas" runat="server" 
                        CssClass="form-select" 
                        SelectionMode="Multiple" 
                        Rows="5"
                        OnSelectedIndexChanged="lstAreas_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:ListBox>
                    <small class="form-text text-muted">
                        <ul class="mb-0 mt-2 ps-3">
                            <li>Mantenga presionada la tecla Ctrl para seleccionar múltiples áreas</li>
                            <li>Haga clic en un área seleccionada para deseleccionarla</li>
                            <li>Mantenga presionada la tecla Shift para seleccionar un rango de áreas</li>
                        </ul>
                    </small>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <div class="d-flex align-items-center gap-2 mb-3">
            <button type="button" class="btn btn-light" onclick="mostrarNuevaArea()">
                <i class="bi bi-plus-circle me-2"></i>Nueva Área
            </button>
        </div>

        <div id="formNuevaArea" class="collapse">
            <div class="card">
                <div class="card-body">
                    <asp:UpdatePanel ID="upNuevaArea" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label class="form-label">Nombre del Área</label>
                                <asp:TextBox ID="txtNombreArea" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreArea" runat="server" 
                                    ControlToValidate="txtNombreArea"
                                    ValidationGroup="NuevaArea" 
                                    CssClass="text-danger" 
                                    Display="Dynamic"
                                    ErrorMessage="El nombre del área es obligatorio"></asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Descripción</label>
                                <asp:TextBox ID="txtDescripcionArea" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Ubicación</label>
                                <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvUbicacion" runat="server"
                                    ControlToValidate="ddlUbicacion"
                                    ValidationGroup="NuevaArea"
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    InitialValue=""
                                    ErrorMessage="Debe seleccionar una ubicación"></asp:RequiredFieldValidator>
                            </div>

                            <div class="d-flex justify-content-end gap-2">
                                <button type="button" class="btn btn-light" onclick="ocultarNuevaArea()">Cancelar</button>
                                <asp:Button ID="btnGuardarArea" runat="server" Text="Guardar" CssClass="btn btn-primary"
                                    ValidationGroup="NuevaArea" OnClick="btnGuardarArea_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function mostrarNuevaArea() {
        var formElement = document.getElementById('formNuevaArea');
        if (formElement) {
            var bsCollapse = new bootstrap.Collapse(formElement, {
                toggle: false
            });
            bsCollapse.show();
        }
    }

    function ocultarNuevaArea() {
        var formElement = document.getElementById('formNuevaArea');
        if (formElement) {
            var bsCollapse = new bootstrap.Collapse(formElement, {
                toggle: false
            });
            bsCollapse.hide();
        }
    }
</script> 