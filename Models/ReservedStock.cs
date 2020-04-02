using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class ReservedStock
    {
        public int ReservedStockItemId { get; set; }

        public string PrtId { get; set; }

        public int Nnt { get; set; }

        public double Qnt { get; set; }

        public string SupInn { get; set; }

        public int Nds { get; set; }

        public double PrcOptNds { get; set; }

        public double PrcRet { get; set; }

        public Guid RowId { get; set; }

        public Guid OrderId { get; set; }

        public int RowType { get; set; }

        public double Prc { get; set; }

        public double PrcDsc { get; set; }

        public string DscUnion { get; set; }

        public double Dnt { get; set; }

        public double PrcLoyal { get; set; }

        public DateTime DlvDate { get; set; }

        public double QntUnrsv { get; set; }

        public double PrcFix { get; set; }

        public DateTime Ts { get; set; }

        public ReservedStock()
        {

        }
        public ReservedStock(Stock stockrow, OrderRowToStore orderrow)
        {
            try
            {
                PrtId = stockrow.PrtId;
                Nnt = stockrow.Nnt;
                Qnt = stockrow.Qnt;
                SupInn = stockrow.SupInn;
                Nds = stockrow.Nds;
                PrcOptNds = stockrow.PrcOptNds;
                PrcRet = stockrow.PrcRet;
                RowId = orderrow.RowId;
                OrderId = orderrow.OrderId;
                RowType = orderrow.RowType;
                Prc = orderrow.Prc;
                PrcDsc = orderrow.PrcDsc;
                DscUnion = orderrow.DscUnion;
                Dnt = orderrow.Dnt;
                PrcLoyal = orderrow.PrcLoyal;
                DlvDate = orderrow.DlvDate;
                QntUnrsv = orderrow.QntUnrsv;
                PrcFix = orderrow.PrcFix;
                Ts = orderrow.Ts;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
