using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public class UniversalHF
    {

        private UniversalHF() { }

        public static HashFunction GenerateHF(int m)
        {
            Random r = new Random();
            int a = 1 + r.Next(0, DPHimp.P - 2);  // a is a value between 1 and DPHIMP.H - 1
            int b = r.Next(0, DPHimp.P - 1);

           return new HashFunction(a, b, m);
        }
    }
}
  