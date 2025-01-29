using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("CompetenciaNorma")]
    public class CompetenciaNorma
    {
        [Key]
        public int CompetenciaId { get; set; }

        [Required]
        public int NormaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [Range(1, 5)]
        public int NivelRequerido { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaActualizacion { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 