using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickEx.Server.Models.QuickPrueba
{
    [Table("usuarios")]
    public partial class Usuario
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
        public long IdUsuario { get; set; }

        [ConcurrencyCheck]
        public string nombre { get; set; }

        [ConcurrencyCheck]
        public string cargo { get; set; }

        [ConcurrencyCheck]
        public string email { get; set; }

        [ConcurrencyCheck]
        public long? telefonoMovil { get; set; }

        [ConcurrencyCheck]
        public long? telefonoEmpresa { get; set; }

    }
}