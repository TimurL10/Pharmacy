using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class OrderStatusToStore
    {
        public int OrderStatusId { get; set; }

        [JsonPropertyName("statusId")]
        public Guid StatusId { get; set; }

        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("rowId")]
        public Guid RowId { get; set; }

        [JsonPropertyName("storeId")]
        public Guid StoreId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("rcDate")]
        public DateTime RcDate { get; set; }

        [JsonPropertyName("cmnt")]
        public string Cmnt { get; set; }

        [JsonPropertyName("ts")]
        public DateTime Ts { get; set; }

    }
}
