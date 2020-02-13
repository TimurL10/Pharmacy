using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Stock
    {
        [JsonPropertyName("prtId")]
        public string PrtId { get; set; }

        [JsonPropertyName("nnt")]
        public int Nnt { get; set; }

        [JsonPropertyName("qnt")]
        public double Qnt { get; set; }

        [JsonPropertyName("supInn")]
        public string SupInn { get; set; }

        [JsonPropertyName("nds")]
        public int Nds { get; set; }

        [JsonPropertyName("prcOptNds")]
        public double PrcOptNds { get; set; }

        [JsonPropertyName("prcRet")]
        public double PrcRet { get; set; }
    }
}
