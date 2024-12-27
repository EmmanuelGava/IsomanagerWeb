using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("DefinicionObjetivoAlcance")]
    public class DefinicionObjetivoAlcance
    {
        [Key]
        public int DefinicionObjetivoAlcanceId { get; set; }

        [Required]
        public int ContextoId { get; set; }

        [Required]
        [StringLength(500)]
        public string Objetivo { get; set; }

        [Required]
        [StringLength(500)]
        public string Alcance { get; set; }

        [ForeignKey("ContextoId")]
        public virtual Contexto Contexto { get; set; }
    }
} 