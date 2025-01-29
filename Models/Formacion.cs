using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Formacion", Schema = "dbo")]
    public class Formacion
    {
        [Key]
        public int FormacionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoFormacion { get; set; }

        [Required]
        public string Descripcion { get; set; } 

        [Required]
        public DateTime FechaObtencion { get; set; }

        [Required]
        [Range(1, 999)]
        public int Duracion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int NormaId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }
    }
} 