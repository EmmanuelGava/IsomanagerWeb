<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUsuarioSelector.ascx.cs" Inherits="IsomanagerWeb.Controls.ucUsuarioSelector" %>

<div class="usuario-selector">
    <div class="input-group">
        <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-select">
            <asp:ListItem Text="Seleccione un usuario" Value="" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvUsuario" runat="server"
            ControlToValidate="ddlUsuario"
            InitialValue=""
            Display="Dynamic"
            CssClass="text-danger"
            ErrorMessage="El usuario es requerido."
            Enabled="false" />
    </div>
</div> 