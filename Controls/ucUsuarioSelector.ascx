<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUsuarioSelector.ascx.cs" Inherits="IsomanagerWeb.Controls.ucUsuarioSelector" %>

<div class="form-group">
    <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="form-control">
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvUsuarios" runat="server" 
        ControlToValidate="ddlUsuarios"
        InitialValue=""
        ErrorMessage="El usuario es requerido"
        CssClass="text-danger"
        Display="Dynamic"
        Enabled="false">
    </asp:RequiredFieldValidator>
</div> 