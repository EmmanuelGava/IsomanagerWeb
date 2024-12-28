using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("MejoraProceso", Schema = "dbo")]
    public class MejoraProceso
    {
        [Key]
        public int MejoraId { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Required]
        [StringLength(20)]
        public string Prioridad { get; set; }

        [Required]
        [StringLength(500)]
        public string Justificacion { get; set; }

        [StringLength(500)]
        public string ResultadosEsperados { get; set; }

        [Required]
        [StringLength(500)]
        public string BeneficiosEsperados { get; set; }

        [Required]
        [StringLength(500)]
        public string RecursosNecesarios { get; set; }

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
    }
} 