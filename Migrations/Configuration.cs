using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<IsomanagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "IsomanagerWeb.Models.IsomanagerContext";
        }

        protected override void Seed(IsomanagerContext context)
        {
            Debug.WriteLine("Iniciando método Seed...");

            try
            {
                // Verificar si el usuario admin ya existe
                var adminExists = context.Usuarios.Any(u => u.Email == "admin@isomanager.com");
                Debug.WriteLine($"¿Usuario admin existe? {adminExists}");

                if (!adminExists)
                {
                    Debug.WriteLine("Creando usuario administrador...");
                    
                    var adminUser = new Usuario
                    {
                        Nombre = "Administrador",
                        Email = "admin@isomanager.com",
                        Password = HashPassword("admin123"),
                        Rol = "Administrador",
                        Estado = "Activo",
                        FechaRegistro = DateTime.Now,
                        ContadorIntentos = 0
                    };

                    context.Usuarios.AddOrUpdate(
                        u => u.Email,
                        adminUser
                    );

                    context.SaveChanges();
                    Debug.WriteLine("Usuario administrador creado exitosamente.");
                }

                // Crear ubicación principal si no existe
                var ubicacionPrincipal = context.UbicacionesGeograficas.FirstOrDefault();
                if (ubicacionPrincipal == null)
                {
                    ubicacionPrincipal = new UbicacionGeografica
                    {
                        Nombre = "Sede Principal",
                        Direccion = "Dirección Principal",
                        Ciudad = "Ciudad Principal",
                        Estado = "Estado Principal",
                        Pais = "País Principal",
                        FechaCreacion = DateTime.Now,
                        UltimaModificacion = DateTime.Now,
                        Activo = true
                    };
                    context.UbicacionesGeograficas.Add(ubicacionPrincipal);
                    context.SaveChanges();
                }

                // Crear áreas predefinidas si no existen
                if (!context.Areas.Any())
                {
                    var areasPreestablecidas = new[]
                    {
                        new Area { Nombre = "Recursos Humanos", Descripcion = "Gestión del personal y desarrollo organizacional", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Calidad", Descripcion = "Gestión y aseguramiento de la calidad", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Producción", Descripcion = "Procesos productivos y operaciones", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Mantenimiento", Descripcion = "Mantenimiento de equipos e instalaciones", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Seguridad y Salud", Descripcion = "Seguridad laboral y salud ocupacional", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Administración", Descripcion = "Gestión administrativa y financiera", UbicacionId = ubicacionPrincipal.UbicacionId },
                        new Area { Nombre = "Sistemas", Descripcion = "Tecnología de la información y sistemas", UbicacionId = ubicacionPrincipal.UbicacionId }
                    };

                    context.Areas.AddRange(areasPreestablecidas);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en Seed: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw; // Re-lanzar la excepción para que Entity Framework la maneje
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
