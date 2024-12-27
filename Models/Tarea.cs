using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Tarea")]
    public class Tarea
    {
        [Key]
        public int TareaId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaVencimiento { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string Prioridad { get; set; }

        [Required]
        public int UsuarioAsignadoId { get; set; }

        [Required]
        public int UsuarioCreadorId { get; set; }

        [ForeignKey("UsuarioAsignadoId")]
        public virtual Usuario UsuarioAsignado { get; set; }

        [ForeignKey("UsuarioCreadorId")]
        public virtual Usuario UsuarioCreador { get; set; }
    }
} 