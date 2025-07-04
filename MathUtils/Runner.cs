using Microsoft.Win32;
using System.Collections;
using System.Runtime.InteropServices;

namespace MathUtils
{
    public static class Runner
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine(RuntimeInformation.FrameworkDescription);
            Console.WriteLine(RuntimeEnvironment.GetSystemVersion());
            bool[] bools = new bool[] { false, true, false };
            BitArray arr = new BitArray(bools);
            foreach (bool i in arr)
            {
                Console.WriteLine(i);
            }*/

            //Matrices.RotationTest();

            double[] inputs = new double[] { 0.1, 0.2 };
            double[] biases = new double[] { 0.3, 0.1, 0.4 };
            double[,] weights = new double[,]
            {
                { 0.2, 0.4, 0.7 },
                { 0.5, 0.6, 0.1 }
            };
            Console.WriteLine(Matrices.LayerTest(inputs, weights, biases));
        }
    }
}