using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IsomanagerWeb.Models
{
    public class TipoFactor
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<FactoresExternos> FactoresExternos { get; set; }
    }
} 