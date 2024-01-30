using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace User32_Testing
{
    sealed class Message
    {
        public static DialogResult DisplayMessageBox(string title, string message, MessageBoxType type, MessageBoxIcon icon)
        {
            HandleRef href = new HandleRef();
            ulong boxCode = (ulong)type | (ulong)icon;
            return GetResult(MessageBox(href, message, title, (uint)boxCode));
        }

        private static DialogResult GetResult(int val)
        {
            switch (val)
            {
                case 1:
                    return DialogResult.OK;
                case 2:
                    return DialogResult.Cancel;
                case 3:
                    return DialogResult.Abort;
                case 4:
                    return DialogResult.Retry;
                case 5:
                    return DialogResult.Ignore;
                case 6:
                    return DialogResult.Yes;
                case 7:
                    return DialogResult.No;
                default:
                    return DialogResult.No;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int MessageBox(HandleRef window, string text, string caption, uint type);

        private Message() { }
    }

    public enum MessageBoxType : long
    {
        AbortRetryIgnore = 0x00000002L,
        CancelTryContinue = 0x00000006L,
        Help = 0x00004000L,
        Ok = 0x00000000L,
        OkCancel = 0x00000001L,
        RetryCancel = 0x00000005L,
        YesNo = 0x00000004L,
        YesNoCancel = 0x00000003L
    }

    public enum MessageBoxIcon : long
    {
        None = 0x00000000L,
        Exclamation = 0x00000030L,
        Warning = 0x00000030L,
        Information = 0x00000040L,
        Asterisk = 0x00000040L,
        Question = 0x00000020L,
        Error = 0x00000010L
    }

    public enum DialogResult
    {
        OK,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No
    }
}
