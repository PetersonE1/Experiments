using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Wintellect.Interop.Sound;

namespace User32_Testing
{
    public static class Caller
    {
        public static void Run()
        {
            DialogResult result = Message.DisplayMessageBox("Title", "Message", MessageBoxType.Ok, MessageBoxIcon.Error);
            Console.WriteLine(result);
        }
    }
}
