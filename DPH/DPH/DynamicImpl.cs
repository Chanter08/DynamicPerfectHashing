using DPH;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DPH
{
    public class DynamicImpl : DPHimp
    {
        private ReaderWriterLockSlim reader = new ReaderWriterLockSlim();

        public static double scale = 2;

        public static double step = 0.25;

        public static bool increment = false;

        protected DynamicBin[] dpHash = null;

        public DynamicImpl(int uniSize, int p, double step, bool increment)
        {
            DynamicImpl.MAX_UNI_SIZE = uniSize;
            DynamicImpl.P = p;
            DynamicImpl.step = step;
            DynamicImpl.increment = increment;
            this.dpHash = new DynamicBin[DPHimp.SM];

            this.rehash(-1);
        }

        public override void Insert(int x, object data)
        {
            try {
                reader.EnterWriteLock();
                this.count++;

                if (this.count > DynamicImpl.M)
                {
                    this.rehash(x);
                }
                else
                {
                    int j = this.h.hash(x);
                    int location = -1;
                    //
                    if (this.dpHash[j].m > 0)
                    {
                        location = this.dpHash[j].h.hash(x); //
                    }

                    if ((location != -1) && (this.dpHash[j].bin[location] != null) && (this.dpHash[j].bin[location].getValue() == x))
                    {
                        if (this.dpHash[j].bin[location].Deleted())
                        {
                            this.dpHash[j].bin[location].setDeleted(false);
                        }
                    }
                    else
                    {
                        this.dpHash[j].b++;

                        if (this.dpHash[j].b <= this.dpHash[j].m)
                        {
                            if (this.dpHash[j].bin[location] == null)
                            {
                                this.dpHash[j].bin[location] = new Entry(x, data);
                            }
                            else
                            {
                                List<Entry> l = new List<Entry>();

                                for (int i = 0; i < this.dpHash[j].s; i++)
                                {
                                    // should the 2nd null check be here?
                                    if (this.dpHash[j].bin[i] != null && !this.dpHash[j].bin[i].Deleted())
                                    {
                                        l.Add(this.dpHash[j].bin[i]);
                                    }

                                    this.dpHash[j].bin[i] = null;
                                }

                                l.Add(new Entry(x, data));

                                bool inj = false;

                                while (!inj)
                                {
                                    inj = true;
                                    this.dpHash[j].bin = null;
                                    this.dpHash[j].bin = new Entry[this.dpHash[j].s];
                                    this.dpHash[j].h = UniversalHF.GenerateHF(this.dpHash[j].s);

                                    for (int i = 0; i < l.Count; i++)
                                    {
                                        Entry e = (Entry)l[i];
                                        int y = this.dpHash[j].h.hash(e.getValue());

                                        if (this.dpHash[j].bin[y] != null)
                                        {
                                            inj = false;
                                            break;
                                        }
                                        this.dpHash[j].bin[y] = e;
                                    }
                                }

                                l.Clear();
                            }
                        }
                        else
                        {

                            if ((this.dpHash[j].m != 0))
                            {
                                this.dpHash[j].update();
                            }

                            this.dpHash[j].m = (int)(this.dpHash[j].scale * Math.Max(this.dpHash[j].m, 1));
                            this.dpHash[j].s = 2 * (this.dpHash[j].m) * (this.dpHash[j].m - 1);

                            if (this.verify())
                            {
                                ArrayList l = new ArrayList();

                                if (this.dpHash[j].bin != null)
                                {
                                    for (int i = 0; i < this.dpHash[j].bin.Length; i++)
                                    {
                                        if ((this.dpHash[j].bin[i] != null) && (!this.dpHash[j].bin[i].Deleted()))
                                        {
                                            l.Add(this.dpHash[j].bin[i]);
                                        }

                                        this.dpHash[j].bin[i] = null;
                                    }
                                }

                                l.Add(new Entry(x, data));

                                bool inj = false;

                                while (!inj)
                                {
                                    inj = true;
                                    this.dpHash[j].bin = null;
                                    this.dpHash[j].bin = new Entry[this.dpHash[j].s];
                                    this.dpHash[j].h = UniversalHF.GenerateHF(this.dpHash[j].s);

                                    for (int i = 0; i < l.Count; i++)
                                    {
                                        Entry e = (Entry)l[i];

                                        int y = this.dpHash[j].h.hash(e.getValue());

                                        if (this.dpHash[j].bin[y] != null)
                                        {
                                            inj = false;
                                            break;
                                        }
                                        this.dpHash[j].bin[y] = e;
                                    }
                                }
                                l.Clear();
                            }
                            else
                            {
                                this.rehash(x);
                            }
                        }
                    }
                }
            }
            finally
            {
                reader.ExitWriteLock();
            }
        }

        public override void Delete(int x)
        {
            try
            {
                reader.EnterWriteLock();

                this.count++;

                int j = this.h.hash(x);

                if (this.dpHash[j] != null)
                {
                    int loc = this.dpHash[j].h.hash(x);

                    if (this.dpHash[j].bin[loc] != null)
                    {
                        this.dpHash[j].bin[loc].setDeleted(true);
                    }
                }

                if (this.count >= DPHimp.M)
                {
                    this.rehash(-1);
                }
            }
            finally { reader.ExitWriteLock(); }

        }

        public override bool Lookup(int x)
        {
            try
            {
                reader.EnterReadLock();

                int j = this.h.hash(x);

                if ((this.dpHash[j] != null) && (this.dpHash[j].b > 0))
                {
                    int loc = this.dpHash[j].h.hash(x);

                    if ((this.dpHash[j].bin[loc] != null) && (this.dpHash[j].bin[loc].getValue() == x) && (!this.dpHash[j].bin[loc].Deleted()))
                    {
                        return true;
                    }
                }
                return false;
            }finally
            {
                reader.ExitReadLock();
            }
        }

        protected void rehash(int x)
        {
            List<Entry> l = new List<Entry>();

            for (int i = 0; i < DPHimp.SM; ++i)
            {
                if ((this.dpHash[i] != null) && (this.dpHash[i].bin != null))
                {
                    //

                    for (int j = 0; j < this.dpHash[i].bin.Length; ++j)
                    {
                        //
                        if ((this.dpHash[i].bin != null) && (this.dpHash[i].bin[j] != null))
                        {
                            if (!this.dpHash[i].bin[j].Deleted())
                            {
                                l.Add(this.dpHash[i].bin[j]);
                            }
                            //
                            this.dpHash[i].bin[j] = null;
                        }
                    }

                    this.dpHash[i].h = null;
                    this.dpHash[i].bin = null;
                    this.dpHash[i] = null;
                }
            }

            this.dpHash = null;

            if (x != -1)
            {
                l.Add(new Entry(x));
            }

            this.count = l.Count;

            DPHimp.M = (1 + DPHimp.C) * Math.Max(this.count, 4);
            DPHimp.SM = DPHimp.M * 2;
            this.dpHash = new DynamicBin[DPHimp.SM];

            ArrayList[] sublists = new ArrayList[DPHimp.SM];

            do
            {
                this.h = UniversalHF.GenerateHF(DPHimp.SM);
                for (int k = 0; k < DPHimp.SM; ++k)
                {
                    if (sublists[k] != null)
                    {
                        sublists[k].Clear();
                    }
                    else
                    {
                        sublists[k] = new ArrayList();
                    }
                }

                for (int i = 0; i < l.Count; ++i)
                {
                    Entry e = (Entry)l[i];
                    int bucket = this.h.hash(e.getValue());

                    sublists[bucket].Add(e);
                }

                for (int j = 0; j < DPHimp.SM; ++j)
                {
                    if (sublists[j] == null)
                    {
                        sublists[j] = new ArrayList();
                    }

                    this.dpHash[j] = null;
                    this.dpHash[j] = new DynamicBin(DynamicImpl.scale, DynamicImpl.step, DynamicImpl.increment);
                    this.dpHash[j].b = sublists[j].Count;
                    this.dpHash[j].m = (int)(this.dpHash[j].scale * this.dpHash[j].b);
                    this.dpHash[j].s = 2 * (this.dpHash[j].m) * (this.dpHash[j].m - 1);
                    this.dpHash[j].h = UniversalHF.GenerateHF(this.dpHash[j].s);
                }
            } while (!this.verify());

            l.Clear();

            for (int j = 0; j < DPHimp.SM; ++j)
            {
                if ((sublists[j] != null) && (sublists[j].Count != 0))
                {
                    bool inj = false;

                    while (!inj)
                    {
                        this.dpHash[j].bin = null;
                        this.dpHash[j].bin = new Entry[this.dpHash[j].s];
                        this.dpHash[j].h = UniversalHF.GenerateHF(this.dpHash[j].s);

                        inj = true;

                        for (int i = 0; i < sublists[j].Count; ++i)
                        {
                            Entry e = (Entry)sublists[j][i];
                            int loc = this.dpHash[j].h.hash(e.getValue());

                            if (this.dpHash[j].bin[loc] != null)
                            {
                                inj = false;
                                break;
                            }

                            dpHash[j].bin[loc] = e;
                        }
                    }
                }
                sublists[j].Clear();
            }
        }

        protected bool verify()
        {
            int total = 0;
            int condition = (32 * (DPHimp.M ^ 2)) / DPHimp.SM + 4 * DPHimp.M;

            for (int i = 0; i < this.dpHash.Length; i++)
            {
                if (this.dpHash[i] != null)
                {
                    total += this.dpHash[i].s;
                }

                if (total > condition)
                {
                    return false;
                }
            }

            return (total <= condition);
        }
    }
}

