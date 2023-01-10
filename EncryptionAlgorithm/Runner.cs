using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionAlgorithm
{
    internal static class Runner
    {
        private static void Main()
        {
            Console.WriteLine(EncryptionEngine.DecryptImageFileToString("dragonImage.png"));
        }
    }
}
