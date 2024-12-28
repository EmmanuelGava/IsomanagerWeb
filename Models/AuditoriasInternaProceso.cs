using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("AuditoriasInternaProceso", Schema = "dbo")]
    public class AuditoriasInternaProceso
    {
        [Key]
        public int AuditoriaInternaProcesoId { get; set; }

        [Required]
        public int ProcesoId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(500)]
        public string Alcance { get; set; }

        [Required]
        [StringLength(500)]
        public string Hallazgos { get; set; }

        [Required]
        [StringLength(500)]
        public string Recomendaciones { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaActualizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [ForeignKey("Creador")]
        public int CreadorId { get; set; }

        [Required]
        [ForeignKey("Asignado")]
        public int AsignadoId { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual Proceso Proceso { get; set; }

        public virtual Usuario Creador { get; set; }
        public virtual Usuario Asignado { get; set; }
    }
} 