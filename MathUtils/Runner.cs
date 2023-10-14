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

            Matrices.Temp();
        }
    }
}