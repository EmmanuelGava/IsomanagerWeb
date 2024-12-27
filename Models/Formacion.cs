using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsomanagerWeb.Models
{
    public class Formacion
    {
        public int FormacionId { get; set; }
        public string Nombre { get; set; }
        public string TipoFormacion { get; set; }
        public DateTime FechaObtencion { get; set; }
        public int Duracion { get; set; }
        public string Estado { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 