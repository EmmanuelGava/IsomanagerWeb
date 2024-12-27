using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("CambiosProceso", Schema = "dbo")]
    public class CambiosProceso
    {
        [Key]
        public int CambioId { get; set; }

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

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [ForeignKey("Contexto")]
        public int ContextoId { get; set; }

        public virtual Proceso Proceso { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Contexto Contexto { get; set; }
    }
} 