using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class ReservedRows : Stock
    {
        public override int OrderRowId { get; set; }

        [JsonPropertyName("rowId")]
        public override Guid RowId { get; set; }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("rowType")]
        public int RowType { get; set; }

        [JsonPropertyName("prtId")]
        public string PrtId { get; set; }

        [JsonPropertyName("nnt")]
        public int Nnt { get; set; }

        [JsonPropertyName("qnt")]
        public double Qnt { get; set; }

        [JsonPropertyName("prc")]
        public double Prc { get; set; }

        [JsonPropertyName("prcDsc")]
        public double PrcDsc { get; set; }

        [JsonPropertyName("dscUnion")]
        public string DscUnion { get; set; }

        [JsonPropertyName("dnt")]
        public double Dnt { get; set; }

        [JsonPropertyName("prcLoyal")]
        public double PrcLoyal { get; set; }

        [JsonPropertyName("prcOptNds")]
        public double PrcOptNds { get; set; }

        [JsonPropertyName("supInn")]
        public string SupInn { get; set; }

        [JsonPropertyName("dlvDate")]
        public DateTime DlvDate { get; set; }

        [JsonPropertyName("qntUnrsv")]
        public double QntUnrsv { get; set; }

        [JsonPropertyName("prcFix")]
        public double PrcFix { get; set; }

        [JsonPropertyName("ts")]
        public DateTime Ts { get; set; }
    }
}
