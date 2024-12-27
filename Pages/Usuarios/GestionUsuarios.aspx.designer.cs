namespace IsomanagerWeb.Pages.Usuarios
{
    public partial class GestionUsuarios
    {
        protected global::System.Web.UI.WebControls.TextBox txtBuscar;
        protected global::System.Web.UI.WebControls.LinkButton btnMostrarModal;
        protected global::System.Web.UI.UpdatePanel upPrincipal;
        protected global::System.Web.UI.WebControls.Label lblMensaje;
        protected global::System.Web.UI.WebControls.GridView gvUsuarios;
        protected global::System.Web.UI.WebControls.HiddenField hdnUsuarioId;
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvNombre;
        protected global::System.Web.UI.WebControls.TextBox txtEmail;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revEmail;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;
        protected global::System.Web.UI.WebControls.DropDownList ddlRol;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvRol;
        protected global::Isomanager.Controls.ucDepartamentoSelector ucDepartamento;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divPassword;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmPassword;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvPassword;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvConfirmPassword;
        protected global::System.Web.UI.WebControls.CompareValidator cvPassword;
        protected global::System.Web.UI.WebControls.Literal litTituloModal;
        protected global::System.Web.UI.WebControls.Label lblErrorModal;
        protected global::System.Web.UI.WebControls.Button btnGuardarUsuario;
        protected global::System.Web.UI.UpdatePanel upModal;
    }
} 