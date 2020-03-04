using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class SendOrderStatuses
    {
        [JsonPropertyName("statusId")]
        public Guid RowId { get; set; }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("storeId")]
        public Guid StoreId { get; set; }        

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

         [JsonPropertyName("status")]
        public int Status { get; set; }

        public SendOrderStatuses(Guid RowIdn, Guid OrderIdn, Guid StoreIdn, DateTime Daten, int Statusn)
        {
            RowId = RowIdn;
            OrderId = OrderIdn;
            StoreId = StoreIdn;
            Date = Daten;
            Status = Statusn;
        }

    }
}
