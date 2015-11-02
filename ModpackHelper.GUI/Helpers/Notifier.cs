using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ModpackHelper.Shared;

namespace ModpackHelper.GUI.Helpers
{
    public static class Notifier
    {
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public static void FlashWindow(Form f)
        {
            if (!Constants.IsUnix)
            {
                FlashWindow(f.Handle, true);
            }
        }
    }
}
