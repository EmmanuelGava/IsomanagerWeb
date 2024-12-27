using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Fodas")]
    public class Foda
    {
        [Key]
        [ForeignKey(nameof(Contexto))]
        public int ContextoId { get; set; }

        [StringLength(4000)]
        public string Fortalezas { get; set; }

        [StringLength(4000)]
        public string Oportunidades { get; set; }

        [StringLength(4000)]
        public string Debilidades { get; set; }

        [StringLength(4000)]
        public string Amenazas { get; set; }

        public virtual Contexto Contexto { get; set; }
    }
} 