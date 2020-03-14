using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class FullStockListAndDate
    {
        public DateTime Date { get; set; }
        public List<FullStockFiltered> Stocks {get;set;}
        public FullStockListAndDate(DateTime Daten, List<FullStockFiltered> Stocksn)
        {
            Date = Daten;
            Stocks = Stocksn;
        }
    }
}
