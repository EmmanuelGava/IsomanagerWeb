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
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaActualizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public int CreadorId { get; set; }

        [Required]
        public int AsignadoId { get; set; }

        [StringLength(500)]
        public string Alcance { get; set; }

        [StringLength(500)]
        public string Hallazgos { get; set; }

        [StringLength(500)]
        public string Recomendaciones { get; set; }

        [Required]
        public DateTime FechaAuditoria { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual Proceso Proceso { get; set; }

        [ForeignKey("CreadorId")]
        public virtual Usuario Creador { get; set; }

        [ForeignKey("AsignadoId")]
        public virtual Usuario Asignado { get; set; }
    }
} 