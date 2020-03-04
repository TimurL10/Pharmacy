using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class SendOrderRows
    {
        [JsonPropertyName("rowId")]
        public Guid RowId { get; set; }

        [JsonPropertyName("qntUnrsv")]
        public double QntUnrsv { get; set; }

        public SendOrderRows(Guid RowIdn, double QntUnrsvn)
        {
            RowId = RowIdn;
            QntUnrsv = QntUnrsvn;
        }
    }
}
