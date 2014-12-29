using System;

namespace TechnicSolderHelper
{
    public static class Globalfunctions
    {
        public static char PathSeperator;

        public static Boolean IsUnix()
        {
            return Environment.OSVersion.ToString().ToLower().Contains("unix");
        }
    }
}

