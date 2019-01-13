using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public class Entry
    {
        protected int value = -1;
        protected Object data = null;
        protected bool isDeleted = false;

        public Entry(int value)
        {
            this.value = value;
            this.data = null;
        }

        public Entry(int v, object d)
        {
            this.value = v;
            this.data = d;
        }

        public int getValue()
        {
            return this.value;
        }

        public object getData()
        {
            return this.data;
        }

        public bool Deleted()
        {
            return this.isDeleted;
        }

        public void setDeleted(bool isDeleted)
        {
            this.isDeleted = isDeleted;
        }
    }
}
