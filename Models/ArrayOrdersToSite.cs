using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class ArrayOrdersToSite
    {
        public List<SendOrderRows> rows { get; set; }

        public List<SendOrderStatuses> statuses { get; set; }

        public ArrayOrdersToSite(List<SendOrderRows> rowsn, List<SendOrderStatuses> statusesn)
        {
            rows = rowsn;
            statuses = statusesn;
        }

    }
}
