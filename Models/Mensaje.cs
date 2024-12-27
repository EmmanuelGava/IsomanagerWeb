using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    [Table("Mensaje")]
    public class Mensaje
    {
        [Key]
        public int MensajeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Asunto { get; set; }

        [Required]
        public string Contenido { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; }

        [Required]
        public int RemitenteId { get; set; }

        [Required]
        public int DestinatarioId { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [ForeignKey("RemitenteId")]
        public virtual Usuario Remitente { get; set; }

        [ForeignKey("DestinatarioId")]
        public virtual Usuario Destinatario { get; set; }
    }
} 