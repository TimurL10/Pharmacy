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

        public List<OrderHeaderToStore> headers  { get; set; }

        public List<OrderRowToStore> rows { get; set; }

        public List<OrderStatusToStore> statuses { get; set; }

        //public PutOrderToSite(List<OrderRowToStore> rows, List<OrderStatusToStore> statuses)
        //{            
        //    rows = new List<OrderRowToStore>();
        //    statuses = new List<OrderStatusToStore>();
        //}

    }

}
