using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPH
{
    class Program
    {
        protected static int UNIVERSE_SIZE = 10000;

        protected static int PRIME = 10000007;


        static void Main(string[] args)
        {
            dp();
        }

        public static void dp()
        {
            int[] testSizes = { 2500, 5000, 10000 };
            double[] steps = { 0.05, 0.1, 0.15, 0.2, .25 };
            int numOfTests5 = 3;
            long start, end;
            bool increment = true;

            for (int i = 0; i < steps.Length; i++)
            {
                for (int m = 0; m < numOfTests5; m++)
                {
                    for (int test = 0; test < testSizes.Length; test++)
                    {
                        DynamicImpl hash = new DynamicImpl(UNIVERSE_SIZE, PRIME, steps[test],increment);
                        Console.WriteLine("Dynamic Scale test: Decrement by {0} \n", steps[i]);
                        Console.WriteLine("Run no {0}\n", m + 1);

                        Console.WriteLine("Size of Test: {0}\n", testSizes[test]);

                        Console.WriteLine("Inserting into hash");
                        start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        Parallel.For(0, testSizes[test], j =>
                         {
                             hash.Insert(j, null);
                             hash.Lookup(j);
                         });
                        end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        Console.WriteLine("Inserting done in {0} ms\n", end - start);

                        //Console.WriteLine("Checking if correct elements are present in hash");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int k = 0; k < testSizes[test]; k++)
                        //{
                        //    if (!hash.Lookup(k))
                        //    {
                        //        Console.WriteLine("{0} was not found", k);
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finsihed Querying in {0} ms \n", end - start);

                        //Console.WriteLine("Checking for incorrect elements in hash");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int l = testSizes[test]; l < 2 * testSizes[test]; l++)
                        //{
                        //    if (hash.Lookup(l))
                        //    {
                        //        Console.WriteLine("{0} Should not have been found", l);
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        //Console.WriteLine("Finished in {0} ms \n", end - start);

                        //Console.WriteLine("Removing 1/2 of the elements");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int n = 0; n < testSizes[test] / 2; n++)
                        //{
                        //    hash.Delete(n);
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finished in {0} ms \n", end - start);

                        //Console.WriteLine("Checking previously deleted items have been truely removed");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int o = 0; o < testSizes[test] / 2; o++)
                        //{
                        //    if (hash.Lookup(o))
                        //    {
                        //        Console.WriteLine("{0} should not have been found", o);
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finished in {0} ms \n", end - start);

                        //Console.WriteLine("Checking for elements that should be present");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int p = testSizes[test] / 2; p < testSizes[test]; p++)
                        //{
                        //    if (!hash.Lookup(p))
                        //    {
                        //        Console.WriteLine("{0} was not found", p);
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finished in {0} ms \n", end - start);

                        //Console.WriteLine("Re-inserting deleted elements in hash");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int q = 0; q < testSizes[test] / 2; q++)
                        //{
                        //    hash.Insert(q, null);
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finished Re-inserting in {0} ms \n", end - start);

                        //Console.WriteLine("Querying for all elements");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int r = 0; r < testSizes[test]; r++)
                        //{
                        //    if (!hash.Lookup(r))
                        //    {
                        //        Console.WriteLine("{0} was not found");
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finsihed in {0} ms \n", end - start);

                        //Console.WriteLine("Deleting all elements");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int s = 0; s < testSizes[test]; s++)
                        //{
                        //    hash.Delete(s);
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finished in {0} ms \n", end - start);

                        //Console.WriteLine("Querying for all elements");
                        //start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //for (int t = 0; t < testSizes[test]; t++)
                        //{
                        //    if (hash.Lookup(t))
                        //    {
                        //        Console.WriteLine(" {0} Should not have been found");
                        //    }
                        //}
                        //end = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        //Console.WriteLine("Finish in {0} ms \n", end - start);

                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }
    }
}
