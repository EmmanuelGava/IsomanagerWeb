using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("EvaluacionProcesos", Schema = "dbo")]
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

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Proceso")]
        public int ProcesoId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Evaluador")]
        public int EvaluadorId { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual Proceso Proceso { get; set; }

        [ForeignKey("EvaluadorId")]
        public virtual Usuario Evaluador { get; set; }
    }
} 