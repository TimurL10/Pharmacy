using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class Store
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("stocksExist")]
        public bool StocksExist { get; set; }

        [JsonPropertyName("preordersExist")]
        public bool PreordersExist { get; set; }

    }
}
