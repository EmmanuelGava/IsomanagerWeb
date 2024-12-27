using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Documentos")]
    public class Documentos
    {
        [Key]
        public int DocumentoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string RutaArchivo { get; set; }

        [Required]
        [StringLength(10)]
        public string TipoArchivo { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        [Required]
        public DateTime FechaCarga { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        public string Seccion { get; set; }

        [Required]
        public int SeccionId { get; set; }

        [Required]
        public bool Activo { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 