using System;
using System.Web.UI;

namespace IsomanagerWeb.Pages.Documentos
{
    public partial class GestionNormas : Page
    {
        private bool disposed = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización
            }
        }

        // Implementación correcta sin override
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

        // Implementación de IDisposable
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 