using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    public class Usuario
    {
        public Usuario()
        {
            // Inicializar colecciones
            NormasResponsable = new HashSet<Norma>();
            ProcesosResponsable = new HashSet<Proceso>();
            ProcesosAdministrador = new HashSet<Proceso>();
            MejorasResponsable = new HashSet<MejoraProceso>();
            MejorasCreadas = new HashSet<MejoraProceso>();
            Auditorias = new HashSet<AuditoriasInternaProceso>();
            Cambios = new HashSet<CambiosProceso>();
            Documentos = new HashSet<Documentos>();
            Evaluaciones = new HashSet<EvaluacionProcesos>();
        }

        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Rol { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public int? AreaId { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        public DateTime? UltimaConexion { get; set; }

        public int ContadorIntentos { get; set; }

        // Propiedades de navegación
        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        public virtual ICollection<Norma> NormasResponsable { get; set; }
        public virtual ICollection<Proceso> ProcesosResponsable { get; set; }
        public virtual ICollection<Proceso> ProcesosAdministrador { get; set; }
        public virtual ICollection<MejoraProceso> MejorasResponsable { get; set; }
        public virtual ICollection<MejoraProceso> MejorasCreadas { get; set; }
        public virtual ICollection<AuditoriasInternaProceso> Auditorias { get; set; }
        public virtual ICollection<CambiosProceso> Cambios { get; set; }
        public virtual ICollection<Documentos> Documentos { get; set; }
        public virtual ICollection<EvaluacionProcesos> Evaluaciones { get; set; }
    }
} 