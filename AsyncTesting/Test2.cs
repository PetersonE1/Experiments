using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTesting
{
    internal static class Test2
    {
        public static async Task<double> Run(int iterations)
        {
            DateTime startTime = DateTime.Now;

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                await Task.Run(() => t += i);
            }

            Console.WriteLine(t);
            return (DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
