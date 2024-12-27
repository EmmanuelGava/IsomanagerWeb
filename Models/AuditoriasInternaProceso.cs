using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("AuditoriasInternasProcesos")]
    public class AuditoriasInternaProceso
    {
        [Key]
        public int AuditoriaInternaProcesoId { get; set; }

        [Required]
        public int ProcesoId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaActualizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual Proceso Proceso { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 