using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class FullStockListAndDate
    {
        public DateTime Date { get; set; }
        public List<PostStock> Stocks {get;set;}
        public FullStockListAndDate(DateTime Date, List<PostStock> Stocks)
        {
            this.Date = Date;
            this.Stocks = Stocks;
        }
    }
}
