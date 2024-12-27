using System;
using System.Web.UI;

namespace IsomanagerWeb.Pages.Documentos
{
    public partial class GestionArchivos : Page
    {
        private bool disposed = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Liberar recursos manejados
                }
                disposed = true;
            }
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 