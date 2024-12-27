<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IsomanagerWeb.Login" %>

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

                            <div class="d-grid gap-2">
                                <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" 
                                    CssClass="btn btn-dark" OnClick="btnLogin_Click" />
                                <asp:LinkButton ID="btnRecuperarPassword" runat="server" 
                                    CssClass="btn btn-link text-dark text-decoration-none"
                                    CausesValidation="false"
                                    OnClick="btnRecuperarPassword_Click">
                                    <i class="bi bi-key me-2"></i>¿Olvidaste tu contraseña?
                                </asp:LinkButton>
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