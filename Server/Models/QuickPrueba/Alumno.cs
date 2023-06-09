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

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [Required]
        public long dni { get; set; }

        [ConcurrencyCheck]
        public string nombre { get; set; }

        [ConcurrencyCheck]
        public double? Primer_parcial { get; set; }

        [ConcurrencyCheck]
        public double? Segundo_parcial { get; set; }

    }
}