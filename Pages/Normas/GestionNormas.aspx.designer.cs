using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IsomanagerWeb.Pages.Normas
{
    public partial class GestionNormas
    {
        /// <summary>
        /// txtBuscar control.
        /// </summary>
        protected global::System.Web.UI.WebControls.TextBox txtBuscar;

        /// <summary>
        /// btnNuevaNorma control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Button btnNuevaNorma;

        /// <summary>
        /// upNormas control.
        /// </summary>
        protected global::System.Web.UI.UpdatePanel upNormas;

        /// <summary>
        /// lblMensaje control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Label lblMensaje;

        /// <summary>
        /// gvNormas control.
        /// </summary>
        protected global::System.Web.UI.WebControls.GridView gvNormas;

        /// <summary>
        /// litTituloModal control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Literal litTituloModal;

        /// <summary>
        /// upModalNuevaNorma control.
        /// </summary>
        protected global::System.Web.UI.UpdatePanel upModalNuevaNorma;

        /// <summary>
        /// hdnNormaId control.
        /// </summary>
        protected global::System.Web.UI.WebControls.HiddenField hdnNormaId;

        /// <summary>
        /// txtTitulo control.
        /// </summary>
        protected global::System.Web.UI.WebControls.TextBox txtTitulo;

        /// <summary>
        /// rfvTitulo control.
        /// </summary>
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvTitulo;

        /// <summary>
        /// txtDescripcion control.
        /// </summary>
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;

        /// <summary>
        /// txtVersion control.
        /// </summary>
        protected global::System.Web.UI.WebControls.TextBox txtVersion;

        /// <summary>
        /// rfvVersion control.
        /// </summary>
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvVersion;

        /// <summary>
        /// ddlEstado control.
        /// </summary>
        protected global::System.Web.UI.WebControls.DropDownList ddlEstado;

        /// <summary>
        /// ucResponsable control.
        /// </summary>
        protected global::IsomanagerWeb.Controls.ucUsuarioSelector ucResponsable;

        /// <summary>
        /// lblErrorModal control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Label lblErrorModal;

        /// <summary>
        /// btnGuardarNorma control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Button btnGuardarNorma;
    }
} 