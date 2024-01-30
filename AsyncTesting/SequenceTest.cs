using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTesting
{
    [MemoryDiagnoser]
    public class SequenceTest
    {
        [Params(100, 1000)]
        public int iterations;

        [Benchmark]
        public double Run1()
        {
            DateTime startTime = DateTime.Now;

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                t += i;
            }

            return (DateTime.Now - startTime).TotalMilliseconds;
        }

        [Benchmark]
        public async Task<double> Run2()
        {
            DateTime startTime = DateTime.Now;

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                await Task.Run(() => t += i);
            }

            return (DateTime.Now - startTime).TotalMilliseconds;
        }

        [Benchmark]
        public double Run3()
        {
            DateTime startTime = DateTime.Now;

            List<Task> tasks = new List<Task>();

            int t = 0;

            for (int i = 0; i < iterations; i++)
            {
                tasks.Add(Task.Run(() => t += i));
            }

            Task.WaitAll(tasks.ToArray());

            return (DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
