using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public class Bin
    {

        public static int SCALE_FACTOR = 2;

        public HashFunction h = null;

        public int b = 0;

        public int m = 0;

        public int s = 0;

        public Entry[] bin = null;

        public Bin() { }
    }
}
