<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="IsomanagerWeb.ResetPassword" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Restablecer Contraseña - ISOManager</title>

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
                                <i class="bi bi-shield-lock-fill text-dark display-4"></i>
                                <h4 class="text-dark mt-3">Restablecer Contraseña</h4>
                                <p class="text-muted">Ingrese su nueva contraseña</p>
                            </div>

                            <asp:Panel ID="pnlResetPassword" runat="server" Visible="true">
                                <div class="mb-3">
                                    <label for="txtNewPassword" class="form-label">Nueva contraseña</label>
                                    <div class="input-group">
                                        <span class="input-group-text bg-white border-end-0">
                                            <i class="bi bi-lock text-muted"></i>
                                        </span>
                                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
                                            CssClass="form-control border-start-0" placeholder="********"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                                        ControlToValidate="txtNewPassword"
                                        ErrorMessage="La contraseña es requerida"
                                        Display="Dynamic"
                                        CssClass="text-danger small">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="mb-4">
                                    <label for="txtConfirmPassword" class="form-label">Confirmar contraseña</label>
                                    <div class="input-group">
                                        <span class="input-group-text bg-white border-end-0">
                                            <i class="bi bi-lock-fill text-muted"></i>
                                        </span>
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" 
                                            CssClass="form-control border-start-0" placeholder="********"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ErrorMessage="La confirmación de contraseña es requerida"
                                        Display="Dynamic"
                                        CssClass="text-danger small">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvPasswords" runat="server"
                                        ControlToValidate="txtConfirmPassword"
                                        ControlToCompare="txtNewPassword"
                                        ErrorMessage="Las contraseñas no coinciden"
                                        Display="Dynamic"
                                        CssClass="text-danger small">
                                    </asp:CompareValidator>
                                </div>

                                <div class="d-grid">
                                    <asp:Button ID="btnReset" runat="server" Text="Restablecer Contraseña" 
                                        CssClass="btn btn-dark" OnClick="btnReset_Click" />
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="text-center">
                                <div class="alert alert-danger mb-4" role="alert">
                                    <i class="bi bi-exclamation-triangle-fill me-2"></i>
                                    <asp:Literal ID="litError" runat="server"></asp:Literal>
                                </div>
                                <asp:LinkButton ID="btnVolverLogin" runat="server" 
                                    CssClass="btn btn-light"
                                    OnClick="btnVolverLogin_Click">
                                    <i class="bi bi-arrow-left me-2"></i>Volver al inicio de sesión
                                </asp:LinkButton>
                            </asp:Panel>
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