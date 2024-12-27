using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("UbicacionGeografica", Schema = "dbo")]
    public class UbicacionGeografica
    {
        public UbicacionGeografica()
        {
            Areas = new HashSet<Area>();
        }

        [Key]
        public int UbicacionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; }

        [Required]
        [StringLength(100)]
        public string Estado { get; set; }

        [Required]
        [StringLength(100)]
        public string Pais { get; set; }

        [StringLength(500)]
        public string Direccion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
    }
} 