using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    public class DynamicBin : Bin
    {
        protected static double MAX_SCALE = 2.5;
        protected static double MIN_SCALE = 1.5;

        public double scale = 2;
        private bool inc = false;
        private double step = 0.25;

        public DynamicBin(double scale, double step, bool inc)
        {
            this.scale = scale;
            this.step = step;
        }

        public void update()
        {
            if (inc)
            {
                if(this.scale < MAX_SCALE)
                {
                    if ((this.scale + this.step) > MAX_SCALE)
                    {
                        this.scale = MAX_SCALE;
                    }
                    else this.scale += this.step;
                }
            }
            else
            {
                if(this.scale > MIN_SCALE)
                {
                    if ((this.scale - this.step) < MIN_SCALE)
                    {
                        this.scale = MIN_SCALE;
                    }
                    else this.scale -= this.step;
                }
            }
        }
    }
}
