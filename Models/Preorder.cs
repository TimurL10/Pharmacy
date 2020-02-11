using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Preorder
    {
        [JsonPropertyName("srcId")]
        public Guid SrcId { get; set; }

        [JsonPropertyName("nnt")]
        public int Nnt { get; set; }

    }
}
