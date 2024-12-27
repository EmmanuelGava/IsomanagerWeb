using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("MejoraProceso", Schema = "dbo")]
    public class MejoraProceso
    {
        [Key]
        public int MejoraId { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [ForeignKey("Proceso")]
        public int ProcesoId { get; set; }

        [ForeignKey("Responsable")]
        public int ResponsableId { get; set; }

        [ForeignKey("CreadoPor")]
        public int CreadoPorId { get; set; }

        public virtual Proceso Proceso { get; set; }
        public virtual Usuario Responsable { get; set; }
        public virtual Usuario CreadoPor { get; set; }
    }
} 