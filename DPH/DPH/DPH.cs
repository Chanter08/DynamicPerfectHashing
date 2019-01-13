using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public interface DPH
    {
        void Insert(int x, Object data);

        void Delete(int x);

        bool Lookup(int x);
    }
}
