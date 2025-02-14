using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace IsomanagerWeb.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<IsomanagerContext>
    {
        protected override void Seed(IsomanagerContext context)
        {
            try
            {
                // Verificar si ya existe el usuario administrador
                if (!context.Usuarios.Any(u => u.Email == "admin@isomanager.com"))
                {
                    // Crear ubicación principal si no existe
                    var ubicacionPrincipal = context.UbicacionesGeograficas.FirstOrDefault(u => u.Nombre == "Sede Principal");
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

                    // Crear área por defecto si no existe
                    var areaDefault = context.Areas.FirstOrDefault(a => a.Nombre == "Administración");
                    if (areaDefault == null)
                    {
                        areaDefault = new Area
                        {
                            Nombre = "Administración",
                            Descripcion = "Área administrativa principal",
                            UbicacionId = ubicacionPrincipal.UbicacionId,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            UltimaModificacion = DateTime.Now
                        };
                        context.Areas.Add(areaDefault);
                        context.SaveChanges();
                    }

                    // Crear usuario administrador
                    var adminUser = new Usuario
                    {
                        Nombre = "Administrador",
                        Email = "admin@isomanager.com",
                        Password = GeneratePasswordHash("admin123"),
                        Rol = "Administrador",
                        Estado = "Activo",
                        AreaId = areaDefault.AreaId,
                        FechaRegistro = DateTime.Now,
                        ContadorIntentos = 0,
                        Cargo = "Administrador del Sistema",
                        Responsabilidades = "Administración general del sistema"
                    };
                    context.Usuarios.Add(adminUser);
                    context.SaveChanges();
                }

                // Crear tipos de factores predeterminados
                if (!context.TiposFactores.Any())
                {
                    // Factores Externos
                    var factoresExternos = new[]
                    {
                        new TipoFactor { Nombre = "Político", Descripcion = "Factores relacionados con políticas y gobierno", Categoria = "P", Activo = true },
                        new TipoFactor { Nombre = "Económico", Descripcion = "Factores relacionados con la economía y finanzas", Categoria = "E", Activo = true },
                        new TipoFactor { Nombre = "Social", Descripcion = "Factores relacionados con la sociedad y cultura", Categoria = "S", Activo = true },
                        new TipoFactor { Nombre = "Tecnológico", Descripcion = "Factores relacionados con la tecnología e innovación", Categoria = "T", Activo = true },
                        new TipoFactor { Nombre = "Ambiental", Descripcion = "Factores relacionados con el medio ambiente", Categoria = "A", Activo = true },
                        new TipoFactor { Nombre = "Legal", Descripcion = "Factores relacionados con leyes y regulaciones", Categoria = "L", Activo = true }
                    };
                    context.TiposFactores.AddRange(factoresExternos);

                    // Factores Internos
                    var factoresInternos = new[]
                    {
                        new TipoFactor { Nombre = "Recursos Humanos", Descripcion = "Factores relacionados con el personal y la gestión del talento", Categoria = "R", Activo = true },
                        new TipoFactor { Nombre = "Infraestructura", Descripcion = "Factores relacionados con instalaciones y equipamiento", Categoria = "I", Activo = true },
                        new TipoFactor { Nombre = "Procesos", Descripcion = "Factores relacionados con los procesos internos", Categoria = "P", Activo = true },
                        new TipoFactor { Nombre = "Finanzas", Descripcion = "Factores relacionados con recursos financieros", Categoria = "F", Activo = true },
                        new TipoFactor { Nombre = "Tecnología", Descripcion = "Factores relacionados con sistemas y tecnología", Categoria = "T", Activo = true }
                    };
                    context.TiposFactores.AddRange(factoresInternos);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Seed: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        private string GeneratePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 