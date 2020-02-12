using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Preorder
    {
        [JsonPropertyName("nnt")]
        public int Nnt { get; set; }

        //[JsonPropertyName("sku")]
        //public int Sku { get; set; }

        [JsonPropertyName("supInn")]
        public string SupInn { get; set; }

        [JsonPropertyName("supDate")]
        public DateTime SupDate { get; set; }

        [JsonPropertyName("prcOptNds")]
        public double PrcOptNds { get; set; }

        [JsonPropertyName("prclDate")]
        public DateTime PrclDate { get; set; }

        [JsonPropertyName("plType")]
        public int PlType { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}
