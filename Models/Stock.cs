using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Stock
    {
        public virtual int StockItemId { get; set; }

        [JsonPropertyName("prtId")]
        public virtual string PrtId { get; set; }

        [JsonPropertyName("nnt")]
        public virtual int Nnt { get; set; }

        [JsonPropertyName("qnt")]
        public virtual double Qnt { get; set; }

        [JsonPropertyName("supInn")]
        public virtual string SupInn { get; set; }

        [JsonPropertyName("nds")]
        public virtual int Nds { get; set; }

        [JsonPropertyName("prcOptNds")]
        public virtual double PrcOptNds { get; set; }

        [JsonPropertyName("prcRet")]
        public virtual double PrcRet { get; set; }
    }
}
