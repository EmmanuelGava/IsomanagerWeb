using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using IsomanagerWeb.Models;

namespace IsomanagerWeb.Models
{
    public class IsomanagerContext : DbContext
    {
        private static string _connectionString;

        public IsomanagerContext()
            : base("name=IsomanagerContext")
        {
            Database.SetInitializer(new DatabaseInitializer());

            // Habilitar el log de las consultas SQL para depuración
            Database.Log = sql => Debug.WriteLine(sql);

            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public static void SetConnectionString(string databaseType)
        {
            _connectionString = ConnectionManager.GetConnectionString(databaseType);
        }

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión no ha sido configurada. Llame a SetConnectionString antes de usar el contexto.");
            }
            return _connectionString;
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Norma> Normas { get; set; }
        public virtual DbSet<Proceso> Procesos { get; set; }
        public virtual DbSet<Contexto> Contexto { get; set; }
        public virtual DbSet<AuditoriasInternaProceso> AuditoriasInternaProceso { get; set; }
        public virtual DbSet<CambiosProceso> CambiosProceso { get; set; }
        public virtual DbSet<DefinicionObjetivoAlcance> DefinicionObjetivoAlcance { get; set; }
        public virtual DbSet<Desempeno> Desempenos { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<EvaluacionProcesos> EvaluacionProcesos { get; set; }
        public virtual DbSet<FactoresExternos> FactoresExternos { get; set; }
        public virtual DbSet<Foda> Fodas { get; set; }
        public virtual DbSet<Formacion> Formaciones { get; set; }
        public virtual DbSet<KPI> KPIs { get; set; }
        public virtual DbSet<MejoraProceso> MejoraProceso { get; set; }
        public virtual DbSet<TipoFactor> TiposFactores { get; set; }
        public virtual DbSet<UbicacionGeografica> UbicacionesGeograficas { get; set; }
        public virtual DbSet<Calificacion> Calificaciones { get; set; }
        public virtual DbSet<Tarea> Tareas { get; set; }
        public virtual DbSet<Mensaje> Mensajes { get; set; }
        public virtual DbSet<Archivo> Archivos { get; set; }
        public virtual DbSet<AlcanceSistemaGestion> AlcanceSistemaGestion { get; set; }
        public virtual DbSet<PartesInteresadas> PartesInteresadas { get; set; }
        public virtual DbSet<FactoresInternos> FactoresInternos { get; set; }
        public virtual DbSet<CompetenciaNorma> CompetenciasNorma { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Eliminar convenciones innecesarias
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Configurar las entidades para evitar ciclos en cascada

            // Usuario
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario", "dbo")
                .HasKey(u => u.UsuarioId);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Cargo)
                .HasColumnName("Cargo")
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Responsabilidades)
                .HasColumnName("Responsabilidades")
                .HasMaxLength(1000)
                .IsOptional();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Area
            modelBuilder.Entity<Area>()
                .ToTable("Area", "dbo")
                .HasKey(a => a.AreaId);

            modelBuilder.Entity<Area>()
                .HasMany(a => a.Usuarios)
                .WithOptional(u => u.Area)
                .HasForeignKey(u => u.AreaId);

            // Norma
            modelBuilder.Entity<Norma>()
                .ToTable("Norma", "dbo")
                .HasKey(n => n.NormaId);

            modelBuilder.Entity<Norma>()
                .HasRequired(n => n.Responsable)
                .WithMany(u => u.NormasResponsable)
                .HasForeignKey(n => n.ResponsableId)
                .WillCascadeOnDelete(false);

            // Proceso
            modelBuilder.Entity<Proceso>()
                .ToTable("Proceso", "dbo")
                .HasKey(p => p.ProcesoId);

            modelBuilder.Entity<Proceso>()
                .HasRequired(p => p.Norma)
                .WithMany()
                .HasForeignKey(p => p.NormaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Proceso>()
                .HasRequired(p => p.Responsable)
                .WithMany(u => u.ProcesosResponsable)
                .HasForeignKey(p => p.ResponsableId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Proceso>()
                .HasRequired(p => p.Administrador)
                .WithMany(u => u.ProcesosAdministrador)
                .HasForeignKey(p => p.AdministradorId)
                .WillCascadeOnDelete(false);

            // Contexto
            modelBuilder.Entity<Contexto>()
                .ToTable("Contexto", "dbo")
                .HasKey(c => c.ContextoId);

            modelBuilder.Entity<Contexto>()
                .HasRequired(c => c.Norma)
                .WithMany(n => n.Contextos)
                .HasForeignKey(c => c.NormaId)
                .WillCascadeOnDelete(false);

            // MejoraProceso
            modelBuilder.Entity<MejoraProceso>()
                .ToTable("MejoraProceso", "dbo")
                .HasKey(m => m.MejoraId);

            modelBuilder.Entity<MejoraProceso>()
                .HasRequired(m => m.Proceso)
                .WithMany()
                .HasForeignKey(m => m.ProcesoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MejoraProceso>()
                .HasRequired(m => m.Creador)
                .WithMany()
                .HasForeignKey(m => m.CreadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MejoraProceso>()
                .HasRequired(m => m.Asignado)
                .WithMany()
                .HasForeignKey(m => m.AsignadoId)
                .WillCascadeOnDelete(false);

            // AuditoriasInternaProceso
            modelBuilder.Entity<AuditoriasInternaProceso>()
                .ToTable("AuditoriasInternaProceso", "dbo")
                .HasKey(a => a.AuditoriaInternaProcesoId);

            modelBuilder.Entity<AuditoriasInternaProceso>()
                .HasRequired(a => a.Proceso)
                .WithMany()
                .HasForeignKey(a => a.ProcesoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AuditoriasInternaProceso>()
                .HasRequired(a => a.Creador)
                .WithMany()
                .HasForeignKey(a => a.CreadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AuditoriasInternaProceso>()
                .HasRequired(a => a.Asignado)
                .WithMany()
                .HasForeignKey(a => a.AsignadoId)
                .WillCascadeOnDelete(false);

            // CambiosProceso
            modelBuilder.Entity<CambiosProceso>()
                .ToTable("CambiosProceso", "dbo")
                .HasKey(c => c.CambioId);

            modelBuilder.Entity<CambiosProceso>()
                .HasRequired(c => c.Proceso)
                .WithMany()
                .HasForeignKey(c => c.ProcesoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CambiosProceso>()
                .HasRequired(c => c.Creador)
                .WithMany()
                .HasForeignKey(c => c.CreadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CambiosProceso>()
                .HasRequired(c => c.Asignado)
                .WithMany()
                .HasForeignKey(c => c.AsignadoId)
                .WillCascadeOnDelete(false);

            // Documentos
            modelBuilder.Entity<Documentos>()
                .ToTable("Documentos", "dbo")
                .HasKey(d => d.DocumentoId);

            modelBuilder.Entity<Documentos>()
                .HasRequired(d => d.Usuario)
                .WithMany(u => u.Documentos)
                .HasForeignKey(d => d.UsuarioId)
                .WillCascadeOnDelete(false);

            // EvaluacionProcesos
            modelBuilder.Entity<EvaluacionProcesos>()
                .ToTable("EvaluacionProcesos", "dbo")
                .HasKey(e => e.EvaluacionId);

            modelBuilder.Entity<EvaluacionProcesos>()
                .HasRequired(e => e.Proceso)
                .WithMany()
                .HasForeignKey(e => e.ProcesoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluacionProcesos>()
                .HasRequired(e => e.Creador)
                .WithMany()
                .HasForeignKey(e => e.CreadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluacionProcesos>()
                .HasRequired(e => e.Asignado)
                .WithMany()
                .HasForeignKey(e => e.AsignadoId)
                .WillCascadeOnDelete(false);

            // Calificacion
            modelBuilder.Entity<Calificacion>()
                .ToTable("Calificacion", "dbo")
                .HasKey(c => c.CalificacionId);

            modelBuilder.Entity<Calificacion>()
                .HasRequired(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Calificacion>()
                .HasRequired(c => c.Evaluador)
                .WithMany()
                .HasForeignKey(c => c.EvaluadorId)
                .WillCascadeOnDelete(false);

            // Tarea
            modelBuilder.Entity<Tarea>()
                .ToTable("Tarea", "dbo")
                .HasKey(t => t.TareaId);

            modelBuilder.Entity<Tarea>()
                .HasRequired(t => t.UsuarioAsignado)
                .WithMany()
                .HasForeignKey(t => t.UsuarioAsignadoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tarea>()
                .HasRequired(t => t.UsuarioCreador)
                .WithMany()
                .HasForeignKey(t => t.UsuarioCreadorId)
                .WillCascadeOnDelete(false);

            // Mensaje
            modelBuilder.Entity<Mensaje>()
                .ToTable("Mensaje", "dbo")
                .HasKey(m => m.MensajeId);

            modelBuilder.Entity<Mensaje>()
                .HasRequired(m => m.Remitente)
                .WithMany()
                .HasForeignKey(m => m.RemitenteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mensaje>()
                .HasRequired(m => m.Destinatario)
                .WithMany()
                .HasForeignKey(m => m.DestinatarioId)
                .WillCascadeOnDelete(false);

            // Archivo
            modelBuilder.Entity<Archivo>()
                .ToTable("Archivo", "dbo")
                .HasKey(a => a.ArchivoId);

            modelBuilder.Entity<Archivo>()
                .HasRequired(a => a.Creador)
                .WithMany(u => u.Archivos)
                .HasForeignKey(a => a.CreadorId)
                .WillCascadeOnDelete(false);

            // Configurar Foda
            modelBuilder.Entity<Foda>()
                .ToTable("Fodas", "dbo")
                .HasKey(f => f.FodaId);

            modelBuilder.Entity<Foda>()
                .HasRequired(f => f.Norma)
                .WithMany()
                .HasForeignKey(f => f.NormaId)
                .WillCascadeOnDelete(false);

            // AlcanceSistemaGestion
            modelBuilder.Entity<AlcanceSistemaGestion>()
                .ToTable("AlcanceSistemaGestion", "dbo")
                .HasKey(a => a.AlcanceId);

            modelBuilder.Entity<AlcanceSistemaGestion>()
                .HasRequired(a => a.Norma)
                .WithMany()
                .HasForeignKey(a => a.NormaId)
                .WillCascadeOnDelete(false);

            // TipoFactor
            modelBuilder.Entity<TipoFactor>()
                .ToTable("TipoFactor")
                .HasKey(t => t.TipoFactorId);

            modelBuilder.Entity<TipoFactor>()
                .Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<TipoFactor>()
                .Property(t => t.Descripcion)
                .HasMaxLength(200);

            // FactoresExternos
            modelBuilder.Entity<FactoresExternos>()
                .ToTable("FactoresExternos")
                .HasKey(f => f.FactorId);

            modelBuilder.Entity<FactoresExternos>()
                .HasRequired(f => f.TipoFactor)
                .WithMany()
                .HasForeignKey(f => f.TipoFactorId)
                .WillCascadeOnDelete(false);

            // Configurar PartesInteresadas
            modelBuilder.Entity<PartesInteresadas>()
                .ToTable("PartesInteresadas", "dbo")
                .HasKey(p => p.ParteID);

            modelBuilder.Entity<PartesInteresadas>()
                .HasRequired(p => p.Norma)
                .WithMany()
                .HasForeignKey(p => p.NormaId)
                .WillCascadeOnDelete(false);

            // Configurar FactoresInternos
            modelBuilder.Entity<FactoresInternos>()
                .ToTable("FactoresInternos", "dbo")
                .HasKey(f => f.FactorInternoId);

            modelBuilder.Entity<FactoresInternos>()
                .HasRequired(f => f.TipoFactor)
                .WithMany()
                .HasForeignKey(f => f.TipoFactorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoresInternos>()
                .HasRequired(f => f.Norma)
                .WithMany()
                .HasForeignKey(f => f.NormaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoresInternos>()
                .HasRequired(f => f.Creador)
                .WithMany()
                .HasForeignKey(f => f.CreadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FactoresInternos>()
                .HasOptional(f => f.UltimoEditor)
                .WithMany()
                .HasForeignKey(f => f.UltimoEditorId)
                .WillCascadeOnDelete(false);

            // CompetenciaNorma
            modelBuilder.Entity<CompetenciaNorma>()
                .ToTable("CompetenciaNorma", "dbo")
                .HasKey(c => c.CompetenciaId);

            modelBuilder.Entity<CompetenciaNorma>()
                .HasRequired(c => c.Norma)
                .WithMany()
                .HasForeignKey(c => c.NormaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompetenciaNorma>()
                .HasRequired(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .WillCascadeOnDelete(false);

            // Configurar Formacion
            modelBuilder.Entity<Formacion>()
                .ToTable("Formacion", "dbo")
                .HasKey(f => f.FormacionId);

            modelBuilder.Entity<Formacion>()
                .HasRequired(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Formacion>()
                .HasRequired(f => f.Norma)
                .WithMany()
                .HasForeignKey(f => f.NormaId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}