using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IsomanagerWeb.Models
{
    [Table("AlcanceSistemaGestion", Schema = "dbo")]
    public class AlcanceSistemaGestion
    {
        public AlcanceSistemaGestion()
        {
            FechaCreacion = DateTime.Now;
            UltimaModificacion = DateTime.Now;
            Activo = true;
        }

        [Key]
        public int AlcanceId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public int NormaId { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }
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
            HasRequired(a => a.Norma)
                .WithMany()
                .HasForeignKey(a => a.NormaId)
                .WillCascadeOnDelete(false);

            // Propiedades requeridas
            Property(a => a.Descripcion).IsRequired().HasMaxLength(1000);
            Property(a => a.FechaCreacion).IsRequired();
            Property(a => a.UltimaModificacion).IsRequired();
            Property(a => a.Activo).IsRequired();
        }
    }
} 