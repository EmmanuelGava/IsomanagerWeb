using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("TipoFactor")]
    public class TipoFactor
    {
        [Key]
        public int TipoFactorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        public string Categoria { get; set; } // P: Político, E: Económico, S: Social, T: Tecnológico, E: Ambiental, L: Legal

        [Required]
        public bool Activo { get; set; }
    }
} 