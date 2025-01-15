using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("FactoresInternos")]
    public class FactoresInternos
    {
        [Key]
        public int FactorInternoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int TipoFactorId { get; set; }

        [ForeignKey("TipoFactorId")]
        public virtual TipoFactor TipoFactor { get; set; }

        [Required]
        public int Relevancia { get; set; } // 1-5

        public string Comentarios { get; set; }

        public string PosiblesDesafios { get; set; }

        [Required]
        public int NormaId { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }

        public virtual ICollection<Area> Areas { get; set; }

        [Required]
        public int CreadorId { get; set; }

        [ForeignKey("CreadorId")]
        public virtual Usuario Creador { get; set; }

        public int? UltimoEditorId { get; set; }

        [ForeignKey("UltimoEditorId")]
        public virtual Usuario UltimoEditor { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? UltimaModificacion { get; set; }

        public DateTime FechaIdentificacion { get; set; }

        public bool Activo { get; set; }

        public FactoresInternos()
        {
            Areas = new HashSet<Area>();
            FechaCreacion = DateTime.Now;
            FechaIdentificacion = DateTime.Now;
            Activo = true;
        }
    }
} 