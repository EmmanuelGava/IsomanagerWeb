using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Areas")]
    public class Area
    {
        public Area()
        {
            Usuarios = new HashSet<Usuario>();
            Procesos = new HashSet<Proceso>();
            Activo = true;
            FechaCreacion = DateTime.Now;
            UltimaModificacion = DateTime.Now;
        }

        [Key]
        public int AreaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public int UbicacionId { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaModificacion { get; set; }

        [ForeignKey("UbicacionId")]
        public virtual UbicacionGeografica UbicacionGeografica { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Proceso> Procesos { get; set; }
    }
} 