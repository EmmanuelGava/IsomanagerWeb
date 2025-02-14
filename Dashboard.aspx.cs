using System;
using System.Web.UI;

namespace IsomanagerWeb.Pages
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            // Aquí cargarías los datos de tu base de datos
        }

        protected string GetAuditIconClass(string estado)
        {
            return estado?.ToLower() switch
            {
                "pendiente" => "pending",
                "en_proceso" => "in-progress",
                "completada" => "completed",
                _ => "pending"
            };
        }

        protected string GetAuditStatusClass(string estado)
        {
            return estado?.ToLower() switch
            {
                "pendiente" => "pending",
                "en_proceso" => "in-progress",
                "completada" => "completed",
                _ => "pending"
            };
        }

        protected string GetAuditStatusText(string estado)
        {
            return estado?.ToLower() switch
            {
                "pendiente" => "Pendiente",
                "en_proceso" => "En Proceso",
                "completada" => "Completada",
                _ => "Pendiente"
            };
        }

        // Métodos existentes para las normas ISO
        protected string GetEstadoClass(string estado)
        {
            return $"iso-card-{estado.ToLower()}";
        }

        protected string GetStatusText(string estado)
        {
            return estado switch
            {
                "borrador" => "Borrador",
                "revision" => "En Revisión",
                "aprobado" => "Aprobado",
                "obsoleto" => "Obsoleto",
                _ => estado
            };
        }
    }
} 