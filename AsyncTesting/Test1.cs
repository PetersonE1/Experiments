using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTesting
{
    internal static class Test1
    {
        public static double Run(int iterations)
        {
            DateTime startTime = DateTime.Now;

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                t += i;
            }

            Console.WriteLine(t);
            return (DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
