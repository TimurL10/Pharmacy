using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class PostStock
    {   
        public string PrtId { get; set; }

        public int Nnt { get; set; }

        public double Qnt { get; set; }

        public string SupInn { get; set; }

        public int Nds { get; set; }

        public double PrcOptNds { get; set; }

        public double PrcRet { get; set; }

        public PostStock(string prtid, int nnt, double qnt, string supinn, int nds, double prcoptnds, double prcret)
        {
            this.PrtId = prtid;
            this.Nnt = nnt;
            this.Qnt = qnt;
            this.SupInn = supinn;
            this.Nds = nds;
            this.PrcOptNds = prcoptnds;
            this.PrcRet = prcret;
        }


    }
}
