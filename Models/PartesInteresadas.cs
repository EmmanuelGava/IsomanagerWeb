using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("PartesInteresadas")]
    public class PartesInteresadas
    {
        [Key]
        public int ParteID { get; set; }

        [Required]
        public int NormaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(1)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(1)]
        public string Categoria { get; set; }

        [Required]
        public string Intereses { get; set; }

        [Required]
        public int Relevancia { get; set; }

        [Required]
        public string Impacto { get; set; }

        [Required]
        public string Acciones { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }
    }
} 