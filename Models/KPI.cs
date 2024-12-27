using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("KPIs")]
    public class KPI
    {
        [Key]
        public int KpiId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Valor { get; set; }

        [Required]
        public DateTime FechaMedicion { get; set; }

        public int ProcesoId { get; set; }

        [ForeignKey(nameof(ProcesoId))]
        public virtual Proceso Proceso { get; set; }
    }
} 