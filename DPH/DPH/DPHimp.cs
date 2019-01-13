using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public abstract class DPHimp : DPH
    {
        public static int MAX_UNI_SIZE = 0;

        public static int P = 0;
        public static int C = 2;
        public static int M = 0;

        public static int SM = 0;

        protected HashFunction h = null;
        protected int count = 0;

        public abstract void Delete(int x);

        public abstract void Insert(int x, object data);

        public abstract bool Lookup(int x);
    }
}
