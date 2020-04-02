using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Stock
    {
        public int StockItemId { get; set; }

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

        public Stock()
        {

        }
        public Stock(ReservedStock reservedStock)
        {
            PrtId = reservedStock.PrtId;
            Nnt = reservedStock.Nnt;
            Qnt = reservedStock.Qnt;
            SupInn = reservedStock.SupInn;
            Nds = reservedStock.Nds;
            PrcOptNds = reservedStock.PrcOptNds;
            PrcRet = reservedStock.PrcRet;
        }
    }
}
