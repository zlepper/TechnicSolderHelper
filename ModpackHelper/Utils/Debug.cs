using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Utils
{
    public static class Debug
    {
        public static bool OutputDebug = false;
        private static string outputFile = Path.Combine(Constants.ApplicationDataPath, "debug");

        public static void WriteLine(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            if (!OutputDebug) return;
            while (true)
            {
                try
                {
                    File.AppendAllText(outputFile, text);
                    break;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
