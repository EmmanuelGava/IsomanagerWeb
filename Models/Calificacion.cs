using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Calificacion")]
    public class Calificacion
    {
        [Key]
        public int CalificacionId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EvaluadorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Categoria { get; set; }

        [Required]
        [Range(1, 5)]
        public int Valor { get; set; }

        [StringLength(500)]
        public string Comentarios { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("EvaluadorId")]
        public virtual Usuario Evaluador { get; set; }
    }
} 