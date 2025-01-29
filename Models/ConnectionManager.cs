using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IsomanagerWeb.Models
{
    public static class ConnectionManager
    {
        public static string GetConnectionString(string databaseType)
        {
            switch (databaseType.ToLower())
            {
                case "local":
                    return BuildLocalConnectionString();
                case "remote":
                    return BuildRemoteConnectionString();
                case "azure":
                    return BuildAzureConnectionString();
                default:
                    throw new ArgumentException($"Tipo de base de datos no válido: {databaseType}");
            }
        }

        private static string BuildLocalConnectionString()
        {
            // Usar directamente la cadena de conexión DefaultConnection del Web.config
            return ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=LEGION5;Initial Catalog=IsomanagerDB;Integrated Security=True;MultipleActiveResultSets=True";
        }

        private static string BuildRemoteConnectionString()
        {
            var database = ConfigurationManager.AppSettings["LocalDatabase"] ?? "IsomanagerDB";
            
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "181.117.162.6,1",
                InitialCatalog = database,
                IntegratedSecurity = false,
                UserID = "Emmanuel",
                Password = "1234",
                MultipleActiveResultSets = true,
                Encrypt = false,
                TrustServerCertificate = true,
                ConnectTimeout = 30
            };

            return builder.ConnectionString;
        }

        private static string BuildAzureConnectionString()
        {
            var server = ConfigurationManager.AppSettings["AzureDBServer"] ?? "isomanagerserver.database.windows.net";
            var database = ConfigurationManager.AppSettings["AzureDBName"] ?? "IsomanagerDB";
            var user = ConfigurationManager.AppSettings["AzureDBUser"] ?? "CloudSAcfe3eae3";
            var password = ConfigurationManager.AppSettings["AzureDBPassword"];
            
            Debug.WriteLine($"Construyendo conexión Azure - Servidor: {server}, DB: {database}, Usuario: {user}");
            
            return $"Server=tcp:{server};Database={database};" +
                   $"User Id={user};Password={password};" +
                   $"MultipleActiveResultSets=True;" +
                   $"Encrypt=True;TrustServerCertificate=False;" +
                   $"Connection Timeout=30;";
        }

        public static async Task<(bool success, string message)> TestConnection(string databaseType)
        {
            try
            {
                string connectionString = GetConnectionString(databaseType);
                Debug.WriteLine($"Intentando conectar con: {connectionString}");

                // Obtener la IP real del cliente
                string clientIp = System.Web.HttpContext.Current?.Request?.UserHostAddress ?? "No disponible";
                string forwardedFor = System.Web.HttpContext.Current?.Request?.Headers["X-Forwarded-For"];
                string realIp = System.Web.HttpContext.Current?.Request?.Headers["X-Real-IP"];
                
                Debug.WriteLine($"IP Cliente (directa): {clientIp}");
                Debug.WriteLine($"IP Cliente (X-Forwarded-For): {forwardedFor}");
                Debug.WriteLine($"IP Cliente (X-Real-IP): {realIp}");

                // Si estamos en localhost, intentar obtener la IP pública
                if (clientIp == "::1" || clientIp == "127.0.0.1")
                {
                    try
                    {
                        using (var client = new System.Net.WebClient())
                        {
                            string publicIp = await client.DownloadStringTaskAsync("https://api.ipify.org");
                            Debug.WriteLine($"IP Pública detectada: {publicIp}");
                            clientIp = publicIp;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error al obtener IP pública: {ex.Message}");
                    }
                }

                Debug.WriteLine($"IP Final que necesita ser agregada al firewall: {clientIp}");

                // Primero intentamos conectar al servidor usando la base de datos master
                var masterBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master",
                    ConnectTimeout = 60
                };
                
                Debug.WriteLine("Intentando conectar a la base de datos master...");
                using (var masterConnection = new SqlConnection(masterBuilder.ConnectionString))
                {
                    try
                    {
                        await masterConnection.OpenAsync();
                        Debug.WriteLine("Conexión a master exitosa.");
                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine($"Error al conectar a master: {ex.Message}");
                        if (ex.Number == -2) // Timeout
                        {
                            return (false, $"Error de conexión: No se pudo conectar al servidor. " +
                                   $"Necesita agregar la IP {clientIp} al firewall de Azure SQL.");
                        }
                        throw;
                    }
                    
                    // Verificar si la base de datos existe y crearla si no existe
                    string dbName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
                    using (var command = new SqlCommand($@"
                        IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}')
                        BEGIN
                            CREATE DATABASE [{dbName}]
                        END", masterConnection))
                    {
                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Base de datos {dbName} verificada/creada exitosamente.");
                    }
                }

                // Ahora intentamos conectar a la base de datos específica
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return (true, "Conexión exitosa a la base de datos.");
                }
            }
            catch (SqlException ex)
            {
                string errorMessage = "Error de conexión: ";
                string clientIp = "No disponible";
                try
                {
                    using (var client = new System.Net.WebClient())
                    {
                        clientIp = await client.DownloadStringTaskAsync("https://api.ipify.org");
                    }
                }
                catch { }

                switch (ex.Number)
                {
                    case 4060: // Invalid Database
                        errorMessage += $"Error al crear la base de datos. Verifique que tenga permisos suficientes.";
                        break;
                    case 18456: // Login Failed
                        errorMessage += $"Error de autenticación. Verifique sus credenciales.";
                        break;
                    case 40615: // Azure authentication error
                        errorMessage += "Error de autenticación con Azure. Verifique sus credenciales de Azure.";
                        break;
                    case -2: // Timeout
                        errorMessage += $"No se pudo conectar al servidor. Necesita agregar la IP {clientIp} al firewall de Azure SQL.";
                        break;
                    default:
                        errorMessage += $"{ex.Message} (Error {ex.Number})";
                        break;
                }
                Debug.WriteLine($"SQL Error {ex.Number}: {ex.Message}");
                Debug.WriteLine($"Connection String: {GetConnectionString(databaseType)}");
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inesperado: {ex.Message}");
                Debug.WriteLine($"Connection String: {GetConnectionString(databaseType)}");
                return (false, $"Error al conectar con la base de datos: {ex.Message}");
            }
        }
    }
} 