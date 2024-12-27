using System;
using System.ComponentModel.DataAnnotations;

namespace IsomanagerWeb.Models
{
    public class Document
    {
        [Key]
        [StringLength(255)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(255)]
        public string ResponsiblePerson { get; set; }

        [Required]
        public DateTime LastModified { get; set; }
    }
} 