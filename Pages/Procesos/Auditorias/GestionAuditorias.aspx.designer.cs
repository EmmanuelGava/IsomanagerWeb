namespace IsomanagerWeb.Pages.Procesos.Auditorias {
    
    public partial class GestionAuditorias {
        
        protected global::System.Web.UI.WebControls.Literal litTituloPagina;
        
        protected global::System.Web.UI.WebControls.Literal litNormaTitulo;
        
        protected global::System.Web.UI.WebControls.LinkButton btnVolver;
        
        protected global::System.Web.UI.WebControls.Label lblMensaje;
        
        protected global::System.Web.UI.WebControls.Literal litAprobadas;
        
        protected global::System.Web.UI.WebControls.Literal litPendientes;
        
        protected global::System.Web.UI.WebControls.Literal litNoConformes;
        
        protected global::System.Web.UI.WebControls.Literal litEnProceso;
        
        protected global::System.Web.UI.WebControls.TextBox txtBuscar;
        
        protected global::System.Web.UI.WebControls.Button btnNuevaAuditoria;
        
        protected global::System.Web.UI.WebControls.GridView gvAuditorias;
        
        protected global::System.Web.UI.WebControls.Repeater rptDetalles;
        
        protected global::System.Web.UI.WebControls.Literal litTituloModal;
        
        protected global::System.Web.UI.WebControls.Label lblErrorModal;
        
        protected global::System.Web.UI.WebControls.HiddenField hdnAuditoriaId;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlProceso;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvProceso;
        
        protected global::IsomanagerWeb.Controls.ucUsuarioSelector ucUsuarioAsignado;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvUsuarioAsignado;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlTipoAuditoria;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvTipoAuditoria;
        
        protected global::System.Web.UI.WebControls.TextBox txtFechaAuditoria;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvFechaAuditoria;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEstado;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvEstado;
        
        protected global::System.Web.UI.WebControls.TextBox txtAlcance;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvAlcance;
        
        protected global::System.Web.UI.WebControls.TextBox txtHallazgos;
        
        protected global::System.Web.UI.WebControls.TextBox txtRecomendaciones;
        
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;
        
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvDescripcion;
        
        protected global::System.Web.UI.WebControls.Button btnGuardarAuditoria;
    }
} 