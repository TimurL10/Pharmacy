using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class FullStock
    {
        public DateTime Date { get; set; }
        public List<Stock> Stock { get; set; }

        public FullStock(DateTime Daten, List<Stock> Stockn)
        {
            Date = Daten;
            Stock = Stockn;
        }
    }
}
