using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class PutOrderToSite
    {
        //[Key]
        //public int PutOrderToSiteId { get; set; }
        public List<OrderHeaderToStore> headers = new List<OrderHeaderToStore>();

        public List<OrderRowToStore> rows = new List<OrderRowToStore>();

        public List<OrderStatusToStore> statuses = new List<OrderStatusToStore>();
    }

}
