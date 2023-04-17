using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickEx.Server.Models.QuickPrueba
{
    [Table("alumnos")]
    public partial class Alumno
    {
        [Key]
        [Required]
        public string dni { get; set; }

        public string nombre { get; set; }

        public double? Primer_parcial { get; set; }

        public double? Segundo_parcial { get; set; }

    }
}