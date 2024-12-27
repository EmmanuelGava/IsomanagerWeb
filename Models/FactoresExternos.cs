using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("FactoresExternos")]
    public class FactoresExternos
    {
        [Key]
        public int FactorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [Range(0, 2)] // 0: Bajo, 1: Medio, 2: Alto
        public int Impacto { get; set; }

        [Required]
        public bool Activo { get; set; }

        public int TipoFactorId { get; set; }

        public int ContextoId { get; set; }

        [ForeignKey(nameof(TipoFactorId))]
        public virtual TipoFactor TipoFactor { get; set; }

        [ForeignKey(nameof(ContextoId))]
        public virtual Contexto Contexto { get; set; }
    }
} 