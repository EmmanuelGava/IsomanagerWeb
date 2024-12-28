using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Proceso", Schema = "dbo")]
    public class Proceso
    {
        [Key]
        public int ProcesoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Objetivo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaActualizacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public int NormaId { get; set; }

        [Required]
        public int AreaId { get; set; }

        [Required]
        public int ResponsableId { get; set; }

        [Required]
        public int AdministradorId { get; set; }

        public int Progreso { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        // Propiedades de navegación
        [ForeignKey(nameof(NormaId))]
        public virtual Norma Norma { get; set; }

        [ForeignKey(nameof(AreaId))]
        public virtual Area Area { get; set; }

        [ForeignKey(nameof(ResponsableId))]
        public virtual Usuario Responsable { get; set; }

        [ForeignKey(nameof(AdministradorId))]
        public virtual Usuario Administrador { get; set; }
    }
} 