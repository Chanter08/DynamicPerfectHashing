using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public class HashFunction
    {
        protected int a = -1;
        protected int b = -1;
        protected int m = -1;

        public HashFunction(int a, int b, int m)
        {
            this.a = a;
            this.b = b;
            this.m = m;
        }

        public int hash(int x)
        {

            int result = (int)(((a * (long)x + b) % DPHimp.P) % this.m);

            return result;
        }

        public String toString()
        {
            return "a = " + a + ", b = " + b;
        }
    }
}
