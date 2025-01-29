<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IsomanagerWeb.Login" Async="true" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesión - ISOManager</title>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" rel="stylesheet">
    <!-- Toastr CSS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" rel="stylesheet">
    <!-- Site CSS -->
    <link href="Content/css/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Path="https://code.jquery.com/jquery-3.6.0.min.js" />
                <asp:ScriptReference Path="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" />
                <asp:ScriptReference Path="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js" />
            </Scripts>
        </asp:ScriptManager>

        <div class="login-container">
            <div class="login-box">
                <div class="text-center mb-4">
                    <i class="bi bi-shield-lock text-primary display-4"></i>
                    <h4 class="mt-2">Iniciar Sesión</h4>
                </div>

                <asp:UpdatePanel ID="upLogin" runat="server">
                    <ContentTemplate>
                        <!-- Panel de error -->
                        <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
                            <asp:Label ID="lblError" runat="server"></asp:Label>
                        </asp:Panel>

                        <div class="mb-3">
                            <label class="form-label">Base de Datos</label>
                            <asp:DropDownList ID="ddlDbType" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Local" Value="local" />
                                <asp:ListItem Text="Remota" Value="remote" />
                                <asp:ListItem Text="Azure" Value="azure" />
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationGroup="Login"
                                CssClass="text-danger"
                                ErrorMessage="El email es requerido." />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                ControlToValidate="txtPassword"
                                ValidationGroup="Login"
                                CssClass="text-danger"
                                ErrorMessage="La contraseña es requerida." />
                        </div>

                        <div class="mb-3 form-check">
                            <asp:CheckBox ID="chkRecordarme" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="<%= chkRecordarme.ClientID %>">Recordarme</label>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" 
                                CssClass="btn btn-primary" ValidationGroup="Login" OnClick="btnLogin_Click" />
                            <button type="button" class="btn btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#modalRegistro">
                                Registrarse
                            </button>
                            <asp:Button ID="btnRecuperarPassword" runat="server" Text="¿Olvidaste tu contraseña?" 
                                CssClass="btn btn-link" OnClick="btnRecuperarPassword_Click" CausesValidation="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Modal de Registro -->
        <div class="modal fade" id="modalRegistro" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Registro de Usuario</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upRegistro" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlErrorRegistro" runat="server" CssClass="alert alert-danger" Visible="false">
                                    <asp:Label ID="lblErrorRegistro" runat="server"></asp:Label>
                                </asp:Panel>

                                <div class="mb-3">
                                    <label class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombreRegistro" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvNombreRegistro" runat="server"
                                        ControlToValidate="txtNombreRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="El nombre es requerido." />
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Email</label>
                                    <asp:TextBox ID="txtEmailRegistro" runat="server" CssClass="form-control" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="rfvEmailRegistro" runat="server"
                                        ControlToValidate="txtEmailRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="El email es requerido." />
                                    <asp:RegularExpressionValidator ID="revEmailRegistro" runat="server"
                                        ControlToValidate="txtEmailRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="El email no es válido."
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Contraseña</label>
                                    <asp:TextBox ID="txtPasswordRegistro" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvPasswordRegistro" runat="server"
                                        ControlToValidate="txtPasswordRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="La contraseña es requerida." />
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Confirmar Contraseña</label>
                                    <asp:TextBox ID="txtConfirmPasswordRegistro" runat="server" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmPasswordRegistro" runat="server"
                                        ControlToValidate="txtConfirmPasswordRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="La confirmación de contraseña es requerida." />
                                    <asp:CompareValidator ID="cvPasswordRegistro" runat="server"
                                        ControlToValidate="txtConfirmPasswordRegistro"
                                        ControlToCompare="txtPasswordRegistro"
                                        ValidationGroup="Registro"
                                        CssClass="text-danger"
                                        ErrorMessage="Las contraseñas no coinciden." />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" 
                            CssClass="btn btn-primary" ValidationGroup="Registro" OnClick="btnRegistrar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal de Configuración de BD -->
        <div class="modal fade" id="modalDbConfig" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Configuración de Base de Datos</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upDbConfig" runat="server">
                            <ContentTemplate>
                                <div class="alert alert-info">
                                    <i class="bi bi-info-circle me-2"></i>
                                    <span id="connectionHelp" runat="server"></span>
                                </div>

                                <div class="mb-3">
                                    <label class="form-label">Tipo de Base de Datos</label>
                                    <asp:DropDownList ID="ddlDbTypeConfig" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDbType_SelectedIndexChanged">
                                        <asp:ListItem Text="Local" Value="local" />
                                        <asp:ListItem Text="Remota" Value="remote" />
                                        <asp:ListItem Text="Azure" Value="azure" />
                                    </asp:DropDownList>
                                </div>

                                <!-- Configuración Local -->
                                <div id="localConfig" runat="server">
                                    <div class="mb-3">
                                        <label class="form-label">Instancia</label>
                                        <asp:TextBox ID="txtLocalInstance" runat="server" CssClass="form-control" placeholder=".\SQLEXPRESS" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Base de Datos</label>
                                        <asp:TextBox ID="txtLocalDatabase" runat="server" CssClass="form-control" placeholder="IsomanagerDB" />
                                    </div>
                                    <div class="mb-3 form-check">
                                        <asp:CheckBox ID="chkLocalIntegratedSecurity" runat="server" CssClass="form-check-input" AutoPostBack="true" OnCheckedChanged="chkIntegratedSecurity_CheckedChanged" />
                                        <label class="form-check-label" for="<%= chkLocalIntegratedSecurity.ClientID %>">Usar Seguridad Integrada</label>
                                    </div>
                                    <div id="localCredentials" runat="server">
                                        <div class="mb-3">
                                            <label class="form-label">Usuario</label>
                                            <asp:TextBox ID="txtLocalUser" runat="server" CssClass="form-control" />
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Contraseña</label>
                                            <asp:TextBox ID="txtLocalPassword" runat="server" CssClass="form-control" TextMode="Password" />
                                        </div>
                                    </div>
                                </div>

                                <!-- Configuración Remota -->
                                <div id="remoteConfig" runat="server">
                                    <div class="mb-3">
                                        <label class="form-label">Servidor</label>
                                        <asp:TextBox ID="txtRemoteServer" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Puerto</label>
                                        <asp:TextBox ID="txtRemotePort" runat="server" CssClass="form-control" placeholder="1433" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Base de Datos</label>
                                        <asp:TextBox ID="txtRemoteDatabase" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Usuario</label>
                                        <asp:TextBox ID="txtRemoteUser" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Contraseña</label>
                                        <asp:TextBox ID="txtRemotePassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    </div>
                                </div>

                                <!-- Configuración Azure -->
                                <div id="azureConfig" runat="server">
                                    <div class="mb-3">
                                        <label class="form-label">Servidor</label>
                                        <asp:TextBox ID="txtAzureServer" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Base de Datos</label>
                                        <asp:TextBox ID="txtAzureDatabase" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Usuario</label>
                                        <asp:TextBox ID="txtAzureUser" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Contraseña</label>
                                        <asp:TextBox ID="txtAzurePassword" runat="server" CssClass="form-control" TextMode="Password" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnTestConnection" runat="server" Text="Probar Conexión" 
                            CssClass="btn btn-outline-primary" OnClick="btnTestConnection_Click" />
                        <asp:Button ID="btnSaveDbConfig" runat="server" Text="Guardar" 
                            CssClass="btn btn-primary" OnClick="btnSaveDbConfig_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Scripts de respaldo -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>

    <script type="text/javascript">
        function showSuccessAndHideModal() {
            var modal = bootstrap.Modal.getInstance(document.getElementById('modalRegistro'));
            if (modal) {
                modal.hide();
            }
            toastr.success('Usuario registrado exitosamente. Ya puede iniciar sesión.');
        }
    </script>
</body>
</html> 