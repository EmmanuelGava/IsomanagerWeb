using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Norma", Schema = "dbo")]
    public class Norma
    {
        public Norma()
        {
            Contextos = new HashSet<Contexto>();
            Documentos = new HashSet<Documentos>();
        }

        [Key]
        public int NormaId { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoNorma { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public int ResponsableId { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [ForeignKey(nameof(ResponsableId))]
        public virtual Usuario Responsable { get; set; }

        public virtual ICollection<Contexto> Contextos { get; set; }
        public virtual ICollection<Documentos> Documentos { get; set; }
    }
} 