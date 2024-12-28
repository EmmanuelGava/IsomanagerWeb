using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IsomanagerWeb.Pages.Documentos
{
    public partial class GestionArchivos
    {
        /// <summary>
        /// Control UpdatePanel1.
        /// </summary>
        protected global::System.Web.UI.UpdatePanel UpdatePanel1;

        /// <summary>
        /// Control gvDocumentos.
        /// </summary>
        protected global::System.Web.UI.WebControls.GridView gvDocumentos;

        /// <summary>
        /// Control btnNuevoDocumento.
        /// </summary>
        protected global::System.Web.UI.WebControls.LinkButton btnNuevoDocumento;

        /// <summary>
        /// Control lblMensaje.
        /// </summary>
        protected global::System.Web.UI.WebControls.Label lblMensaje;

        protected global::System.Web.UI.WebControls.Literal litTituloPagina;
        
        protected global::System.Web.UI.WebControls.Literal litNormaTitulo;
        
        protected global::System.Web.UI.WebControls.LinkButton btnVolver;
        
        protected global::System.Web.UI.WebControls.Literal litActivos;
        
        protected global::System.Web.UI.WebControls.Literal litEnRevision;
        
        protected global::System.Web.UI.WebControls.Literal litObsoletos;
        
        protected global::System.Web.UI.WebControls.Literal litPendientes;
        
        protected global::System.Web.UI.WebControls.TextBox txtBuscar;
        
        protected global::System.Web.UI.WebControls.Button btnNuevoArchivo;
        
        protected global::System.Web.UI.UpdatePanel upArchivos;
        
        protected global::System.Web.UI.WebControls.GridView gvArchivos;
        
        protected global::System.Web.UI.WebControls.Literal litTituloModal;
        
        protected global::System.Web.UI.WebControls.Label lblErrorModal;
        
        protected global::System.Web.UI.WebControls.HiddenField hdnArchivoId;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlProceso;
        
        protected global::IsomanagerWeb.Controls.ucUsuarioSelector ucUsuarioAsignado;
        
        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        
        protected global::System.Web.UI.WebControls.TextBox txtVersion;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEstado;
        
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;
        
        protected global::System.Web.UI.WebControls.FileUpload fuArchivo;
        
        protected global::System.Web.UI.WebControls.Button btnGuardarArchivo;
    }
} 