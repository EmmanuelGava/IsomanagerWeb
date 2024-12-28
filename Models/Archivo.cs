using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Archivo", Schema = "dbo")]
    public class Archivo
    {
        [Key]
        public int ArchivoId { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public string RutaArchivo { get; set; }

        [Required]
        public int ProcesoId { get; set; }

        [Required]
        public int CreadorId { get; set; }

        [Required]
        public bool Activo { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual Proceso Proceso { get; set; }

        [ForeignKey("CreadorId")]
        public virtual Usuario Creador { get; set; }
    }
} 