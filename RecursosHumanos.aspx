<!-- Modal Capacitación -->
<div class="modal fade" id="modalCapacitacion" tabindex="-1" aria-labelledby="modalCapacitacionLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalCapacitacionLabel">Nueva Capacitación</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="upFormularioCapacitacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="mb-3">
                            <label class="form-label">Usuario</label>
                            <uc:ucUsuarioSelector runat="server" ID="ucUsuarioSelectorCapacitacion" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Nombre de la Capacitación</label>
                            <asp:TextBox ID="txtNombreCapacitacion" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNombreCapacitacion" runat="server"
                                ControlToValidate="txtNombreCapacitacion"
                                ValidationGroup="Capacitacion"
                                CssClass="text-danger"
                                ErrorMessage="El nombre de la capacitación es requerido." />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Duración (horas)</label>
                            <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" TextMode="Number" />
                            <asp:RequiredFieldValidator ID="rfvDuracion" runat="server"
                                ControlToValidate="txtDuracion"
                                ValidationGroup="Capacitacion"
                                CssClass="text-danger"
                                ErrorMessage="La duración es requerida." />
                            <asp:RangeValidator ID="rvDuracion" runat="server"
                                ControlToValidate="txtDuracion"
                                ValidationGroup="Capacitacion"
                                CssClass="text-danger"
                                MinimumValue="1"
                                MaximumValue="999"
                                Type="Integer"
                                ErrorMessage="La duración debe ser un número entre 1 y 999." />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Fecha de Obtención</label>
                            <asp:TextBox ID="txtFechaObtencion" runat="server" CssClass="form-control" TextMode="Date" />
                            <asp:RequiredFieldValidator ID="rfvFechaObtencion" runat="server"
                                ControlToValidate="txtFechaObtencion"
                                ValidationGroup="Capacitacion"
                                CssClass="text-danger"
                                ErrorMessage="La fecha de obtención es requerida." />
                        </div>
                        <asp:HiddenField ID="hdnFormacionId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button ID="btnGuardarCapacitacion" runat="server" Text="Guardar" 
                    CssClass="btn btn-primary" 
                    OnClick="btnGuardarCapacitacion_Click"
                    ValidationGroup="Capacitacion" />
            </div>
        </div>
    </div>
</div> 

<div class="tab-pane fade" id="capacitaciones" role="tabpanel">
    <div class="d-flex justify-content-end mb-3">
        <asp:LinkButton ID="btnNuevaCapacitacion" runat="server" CssClass="btn btn-dark" OnClick="btnNuevaCapacitacion_Click">
            <i class="bi bi-plus-circle me-2"></i>Registrar Capacitación
        </asp:LinkButton>
    </div>

    <asp:UpdatePanel ID="upCapacitaciones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvCapacitaciones" runat="server" CssClass="table" AutoGenerateColumns="false"
                OnRowCommand="gvCapacitaciones_RowCommand" DataKeyNames="FormacionId">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" />
                    <asp:BoundField DataField="Participante" HeaderText="PARTICIPANTE" />
                    <asp:BoundField DataField="FechaObtencion" HeaderText="FECHA" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Duracion" HeaderText="DURACIÓN (HORAS)" />
                    <asp:TemplateField HeaderText="ACCIONES" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <div class="d-flex justify-content-end gap-2">
                                <div class="dropdown">
                                    <button class="dots-button" type="button" data-bs-toggle="dropdown">
                                        <i class="bi bi-three-dots-vertical"></i>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-end">
                                        <li>
                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="dropdown-item" 
                                                CommandName="Editar" CommandArgument='<%# Eval("FormacionId") %>'>
                                                <i class="bi bi-pencil me-2"></i>Editar
                                            </asp:LinkButton>
                                        </li>
                                        <li><hr class="dropdown-divider"></li>
                                        <li>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="dropdown-item text-danger"
                                                CommandName="Eliminar" CommandArgument='<%# Eval("FormacionId") %>'
                                                OnClientClick="return confirm('¿Está seguro de eliminar esta capacitación?');">
                                                <i class="bi bi-trash me-2"></i>Eliminar
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="align-middle" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div> 