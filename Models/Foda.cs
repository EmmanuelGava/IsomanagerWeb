using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Fodas")]
    public class Foda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FodaId { get; set; }

        [Required]
        [ForeignKey(nameof(Norma))]
        public int NormaId { get; set; }

        [StringLength(4000)]
        public string Fortalezas { get; set; }

        [StringLength(4000)]
        public string Oportunidades { get; set; }

        [StringLength(4000)]
        public string Debilidades { get; set; }

        [StringLength(4000)]
        public string Amenazas { get; set; }

        public virtual Norma Norma { get; set; }
    }
} 