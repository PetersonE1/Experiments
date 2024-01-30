namespace Wintellect.Interop.Sound 
{ 
    using System; 
    using System.Runtime.InteropServices; 
    using System.ComponentModel; 
    
    sealed class Sound 
    {
        public static void MessageBeep(BeepTypes type) 
        { 
            if (!MessageBeep((uint)type)) 
            { 
                int err = Marshal.GetLastWin32Error(); 
                throw new Win32Exception(err); 
            } 
        } 
        
        [DllImport("User32.dll", SetLastError = true)] static extern bool MessageBeep(uint beepType); 

        private Sound() { } 
    } 
    
    enum BeepTypes 
    { 
        Simple = -1,
        Ok = 0x00000000,
        IconHand = 0x00000010,
        IconQuestion = 0x00000020,
        IconExclamation = 0x00000030,
        IconAsterisk = 0x00000040
    }
}