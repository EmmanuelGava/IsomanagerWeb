using System;
using System.ComponentModel.DataAnnotations;

namespace IsomanagerWeb.Models
{
    public class Mejora
    {
        [Key]
        [StringLength(255)]
        public string Proceso { get; set; }

        [StringLength(255)]
        public string AreaMejora { get; set; }

        [StringLength(500)]
        public string AccionRecomendada { get; set; }

        [StringLength(255)]
        public string Responsable { get; set; }

        [Required]
        public DateTime FechaImplementacion { get; set; }
    }
} 