using Microsoft.Win32;
using System.Collections;
using System.Runtime.InteropServices;

namespace MidiUtils
{
    public static class Runner
    {
        static void Main(string[] args)
        {
            MidiTools.StringToMorseMidi("Test message, please ignore", 75);
        }
    }
}