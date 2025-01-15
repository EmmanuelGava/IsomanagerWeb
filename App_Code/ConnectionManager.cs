using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;

public static class ConnectionManager
{
    public static string GetConnectionString(string dbType = null)
    {
        // Si no se especifica tipo, usar el valor por defecto del Web.config
        if (string.IsNullOrEmpty(dbType))
        {
            dbType = ConfigurationManager.AppSettings["DefaultDatabaseType"] ?? "local";
        }

        // Si estamos en desarrollo o se especifica local, usar la conexión local
        if (dbType == "local")
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Para Azure, usar la conexión de Azure
        return ConfigurationManager.ConnectionStrings["IsomanagerContext"].ConnectionString;
    }

    public static async Task<(bool success, string message)> TestConnection(string dbType = null)
    {
        try
        {
            string connectionString = GetConnectionString(dbType);
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                return (true, "Conexión exitosa");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error de conexión: {ex.Message}");
        }
    }
} 