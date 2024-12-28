using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("EvaluacionProcesos", Schema = "dbo")]
    public class EvaluacionProcesos
    {
        [Key]
        public int EvaluacionId { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaEvaluacion { get; set; }

        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }

        [StringLength(1000)]
        public string Observaciones { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [ForeignKey("Proceso")]
        public int ProcesoId { get; set; }

        [Required]
        [ForeignKey("Creador")]
        public int CreadorId { get; set; }

        [Required]
        [ForeignKey("Asignado")]
        public int AsignadoId { get; set; }

        public virtual Proceso Proceso { get; set; }
        public virtual Usuario Creador { get; set; }
        public virtual Usuario Asignado { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Required]
        [StringLength(500)]
        public string Comentarios { get; set; }

        [Required]
        [StringLength(500)]
        public string Recomendaciones { get; set; }
    }
} 