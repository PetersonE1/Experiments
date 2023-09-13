using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTesting
{
    internal static class Test3
    {
        public static double Run(int iterations)
        {
            DateTime startTime = DateTime.Now;

            List<Task> tasks = new List<Task>();

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                tasks.Add(Task.Run(() => t += i));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(t);

            return (DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
