using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IsomanagerWeb.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("AlcanceSistemaGestion", Schema = "dbo")]
    public class AlcanceSistemaGestion
    {
        [Key]
        public int AlcanceId { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Contexto")]
        public int ContextoId { get; set; }

        public virtual Contexto Contexto { get; set; }
    }

    public class AlcanceSistemaGestionConfiguration : EntityTypeConfiguration<AlcanceSistemaGestion>
    {
        public AlcanceSistemaGestionConfiguration()
        {
            // Configuración de la tabla
            ToTable("AlcanceSistemaGestion", "dbo");

            // Clave primaria
            HasKey(a => a.AlcanceId);

            // Relaciones
            HasRequired(a => a.Contexto)
                .WithMany()
                .HasForeignKey(a => a.ContextoId)
                .WillCascadeOnDelete(false);

            // Propiedades requeridas
            Property(a => a.Descripcion).IsRequired().HasMaxLength(500);
            Property(a => a.FechaCreacion).IsRequired();
            Property(a => a.UltimaModificacion).IsRequired();
            Property(a => a.Activo).IsRequired();
        }
    }
} 