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
    <link href="~/Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="vh-100 d-flex align-items-center">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-6 col-lg-4">
                    <div class="card shadow-sm border-0">
                        <div class="card-body p-4">
                            <div class="text-center mb-4">
                                <i class="bi bi-shield-lock text-primary display-4"></i>
                                <h4 class="text-dark mt-3">ISOManager</h4>
                                <p class="text-muted">Inicie sesión para continuar</p>
                            </div>

                            <div class="mb-3">
                                <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            </div>

                            <div class="mb-3">
                                <label for="txtEmail" class="form-label">Correo electrónico</label>
                                <div class="input-group">
                                    <span class="input-group-text bg-white border-end-0">
                                        <i class="bi bi-envelope text-muted"></i>
                                    </span>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control border-start-0" placeholder="nombre@ejemplo.com"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                    ControlToValidate="txtEmail"
                                    ErrorMessage="El correo electrónico es requerido"
                                    Display="Dynamic"
                                    CssClass="text-danger small">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-4">
                                <label for="txtPassword" class="form-label">Contraseña</label>
                                <div class="input-group">
                                    <span class="input-group-text bg-white border-end-0">
                                        <i class="bi bi-key text-muted"></i>
                                    </span>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control border-start-0" placeholder="Ingrese su contraseña"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                    ControlToValidate="txtPassword"
                                    ErrorMessage="La contraseña es requerida"
                                    Display="Dynamic"
                                    CssClass="text-danger small">
                                </asp:RequiredFieldValidator>
                            </div>

                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkRecordarme" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="<%= chkRecordarme.ClientID %>">
                                        Recordarme en este dispositivo
                                    </label>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label for="ddlDbType" class="form-label">Tipo de Base de Datos</label>
                                <asp:DropDownList ID="ddlDbType" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Local" Value="local" />
                                    <asp:ListItem Text="Azure" Value="azure" />
                                </asp:DropDownList>
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" 
                                    CssClass="btn btn-primary" OnClick="btnLogin_Click" UseSubmitBehavior="false" />
                                <asp:LinkButton ID="btnRecuperarPassword" runat="server" 
                                    CssClass="btn btn-link text-dark text-decoration-none"
                                    CausesValidation="false"
                                    OnClick="btnRecuperarPassword_Click">
                                    <i class="bi bi-key me-2"></i>¿Olvidaste tu contraseña?
                                </asp:LinkButton>
                            </div>
                            <div class="text-center mt-3">
                                <button type="button" class="btn btn-link" data-bs-toggle="modal" data-bs-target="#dbConfigModal">
                                    <i class="bi bi-gear"></i> Configurar Base de Datos
                                </button>
                            </div>
                        </div>
                    </div>

                    <div class="text-center mt-4">
                        <p class="text-muted small mb-0">
                            &copy; <%: DateTime.Now.Year %> ISOManager. Todos los derechos reservados.
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Configuración BD -->
        <div class="modal fade" id="dbConfigModal" tabindex="-1" aria-labelledby="dbConfigModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="dbConfigModalLabel">Configuración de Base de Datos</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="ddlDbTypeConfig" class="form-label">Tipo de Conexión</label>
                            <asp:DropDownList ID="ddlDbTypeConfig" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDbType_SelectedIndexChanged">
                                <asp:ListItem Text="SQL Server Local" Value="local" />
                                <asp:ListItem Text="SQL Server Remoto" Value="remote" />
                                <asp:ListItem Text="Azure SQL Database" Value="azure" />
                            </asp:DropDownList>
                        </div>

                        <!-- Configuración SQL Server Local -->
                        <div id="localConfig" runat="server">
                            <div class="mb-3">
                                <label for="txtLocalInstance" class="form-label">Instancia Local</label>
                                <asp:TextBox ID="txtLocalInstance" runat="server" CssClass="form-control" placeholder=".\SQLEXPRESS"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtLocalDatabase" class="form-label">Base de Datos</label>
                                <asp:TextBox ID="txtLocalDatabase" runat="server" CssClass="form-control" placeholder="IsomanagerDB"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkLocalIntegratedSecurity" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="chkIntegratedSecurity_CheckedChanged" />
                                    <label class="form-check-label" for="chkLocalIntegratedSecurity">
                                        Usar Seguridad Integrada de Windows
                                    </label>
                                </div>
                            </div>
                            <div id="localCredentials" runat="server" visible="false">
                                <div class="mb-3">
                                    <label for="txtLocalUser" class="form-label">Usuario</label>
                                    <asp:TextBox ID="txtLocalUser" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label for="txtLocalPassword" class="form-label">Contraseña</label>
                                    <asp:TextBox ID="txtLocalPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Configuración SQL Server Remoto -->
                        <div id="remoteConfig" runat="server" visible="false">
                            <div class="mb-3">
                                <label for="txtRemoteServer" class="form-label">Servidor Remoto</label>
                                <asp:TextBox ID="txtRemoteServer" runat="server" CssClass="form-control" placeholder="IP o nombre del servidor"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtRemotePort" class="form-label">Puerto</label>
                                <asp:TextBox ID="txtRemotePort" runat="server" CssClass="form-control" placeholder="1433" Text="1433"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtRemoteDatabase" class="form-label">Base de Datos</label>
                                <asp:TextBox ID="txtRemoteDatabase" runat="server" CssClass="form-control" placeholder="IsomanagerDB"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtRemoteUser" class="form-label">Usuario</label>
                                <asp:TextBox ID="txtRemoteUser" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtRemotePassword" class="form-label">Contraseña</label>
                                <asp:TextBox ID="txtRemotePassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Configuración Azure SQL -->
                        <div id="azureConfig" runat="server" visible="false">
                            <div class="mb-3">
                                <label for="txtAzureServer" class="form-label">Servidor Azure</label>
                                <asp:TextBox ID="txtAzureServer" runat="server" CssClass="form-control" placeholder="nombre.database.windows.net"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtAzureDatabase" class="form-label">Base de Datos</label>
                                <asp:TextBox ID="txtAzureDatabase" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtAzureUser" class="form-label">Usuario</label>
                                <asp:TextBox ID="txtAzureUser" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtAzurePassword" class="form-label">Contraseña</label>
                                <asp:TextBox ID="txtAzurePassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="alert alert-info mt-3">
                            <i class="bi bi-info-circle me-2"></i>
                            <span id="connectionHelp" runat="server">
                                Seleccione el tipo de conexión y complete los datos necesarios.
                            </span>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnTestConnection" runat="server" Text="Probar Conexión" 
                            CssClass="btn btn-secondary" OnClick="btnTestConnection_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSaveDbConfig" runat="server" Text="Guardar" 
                            CssClass="btn btn-primary" OnClick="btnSaveDbConfig_Click" />
                        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Scripts fundamentales -->
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>

        <script type="text/javascript">
            // Configuración global de toastr
            toastr.options = {
                "closeButton": true,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "timeOut": "5000"
            };
        </script>
    </form>
</body>
</html> 