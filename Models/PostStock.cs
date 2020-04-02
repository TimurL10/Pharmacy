using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithFarmacy.Models
{
    public class PostStock
    {   
        public string PrtId { get; }

        public int Nnt { get; }

        public double Qnt { get; }

        public string SupInn { get;}

        public int Nds { get; }

        public double PrcOptNds { get;}

        public double PrcRet { get; }


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

        public PostStock()
        {

        }





    }
}
