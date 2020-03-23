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

        //public PostStock(string PrtIdn, int Nntn, double Qntn, string SupInnn, int Ndsn, double PrcOptNdsn, double PrcRetn)
        //{
        //    PrtId = PrtIdn;
        //    Nnt = Nntn;
        //    Qnt = Qntn;
        //    SupInn = SupInnn;
        //    Nds = Ndsn;
        //    PrcOptNds = PrcOptNdsn;
        //    PrcRet = PrcRetn;
        //}
    }
}
