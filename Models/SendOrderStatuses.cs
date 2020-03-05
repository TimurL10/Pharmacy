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
        public Guid StatusId { get; set; }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("storeId")]
        public Guid StoreId { get; set; }        

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

         [JsonPropertyName("status")]
        public int Status { get; set; }

        public SendOrderStatuses(Guid StatusIdn, Guid OrderIdn, Guid StoreIdn, DateTime Daten, int Statusn)
        {
            StatusId = StatusIdn;
            OrderId = OrderIdn;
            StoreId = StoreIdn;
            Date = Daten;
            Status = Statusn;
        }

    }
}
