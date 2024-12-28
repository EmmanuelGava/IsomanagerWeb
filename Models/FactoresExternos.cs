using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("FactoresExternos")]
    public class FactoresExternos
    {
        public FactoresExternos()
        {
            Areas = new HashSet<Area>();
        }

        [Key]
        public int FactorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; }

        [Required]
        public int TipoFactorId { get; set; }

        [Required]
        [Range(0, 2)] // 0: Bajo, 1: Medio, 2: Alto
        public int Impacto { get; set; }

        [Required]
        [Range(0, 2)] // 0: Bajo, 1: Medio, 2: Alto
        public int Probabilidad { get; set; }

        [Required]
        public DateTime FechaIdentificacion { get; set; }

        [StringLength(1000)]
        public string AccionesRecomendadas { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public int NormaId { get; set; }

        [ForeignKey("TipoFactorId")]
        public virtual TipoFactor TipoFactor { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
    }
} 