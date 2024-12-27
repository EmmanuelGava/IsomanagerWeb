using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Contexto")]
    public class Contexto
    {
        public Contexto()
        {
            AuditoriasInternaProceso = new HashSet<AuditoriasInternaProceso>();
            CambiosProceso = new HashSet<CambiosProceso>();
            EvaluacionProcesos = new HashSet<EvaluacionProcesos>();
            FactoresExternos = new HashSet<FactoresExternos>();
            Proceso = new HashSet<Proceso>();
            DefinicionesObjetivoAlcance = new HashSet<DefinicionObjetivoAlcance>();
        }

        [Key]
        public int ContextoId { get; set; }

        [Required]
        public int NormaId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime UltimaActualizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [ForeignKey("NormaId")]
        public virtual Norma Norma { get; set; }

        public virtual ICollection<AuditoriasInternaProceso> AuditoriasInternaProceso { get; set; }
        public virtual ICollection<CambiosProceso> CambiosProceso { get; set; }
        public virtual ICollection<EvaluacionProcesos> EvaluacionProcesos { get; set; }
        public virtual ICollection<FactoresExternos> FactoresExternos { get; set; }
        public virtual Foda Foda { get; set; }
        public virtual ICollection<Proceso> Proceso { get; set; }
        public virtual ICollection<DefinicionObjetivoAlcance> DefinicionesObjetivoAlcance { get; set; }
    }
} 