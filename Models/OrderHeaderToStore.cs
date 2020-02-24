using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace WorkWithFarmacy.Models
{    
    
    public class OrderHeaderToStore
    {
        public int OrderHeaderId { get; set; }

        [JsonPropertyName("orderId")]        
        public Guid OrderId { get; set; }

        [JsonPropertyName("storeId")]
        public Guid StoreId { get; set; }

        [JsonPropertyName("issuerId")]
        public Guid Nnt { get; set; }

        [JsonPropertyName("src")]
        public string Src { get; set; }

        [JsonPropertyName("num")]
        public string Num { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mPhone")]
        public string MPhone { get; set; }

        [JsonPropertyName("payType")]
        public string PayType { get; set; }

        [JsonPropertyName("payTypeId")]
        public int PayTypeId { get; set; }

        [JsonPropertyName("dCard")]
        public string DCard { get; set; }

        [JsonPropertyName("ae")]
        public int Ae { get; set; }

        [JsonPropertyName("unionId")]
        public Guid UnionId { get; set; }

        [JsonPropertyName("ts")]
        public DateTime Ts { get; set; }

        [JsonPropertyName("delivery")]
        public bool Delivery { get; set; }

        [JsonPropertyName("deliveryInfo")]
        public string DeliveryInfo { get; set; }
    }
}
