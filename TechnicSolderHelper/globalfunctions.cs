using System;

namespace TechnicSolderHelper
{
    public class globalfunctions
    {
        public static char pathSeperator;

        public static Boolean isUnix()
        {
            return Environment.OSVersion.ToString().ToLower().Contains("unix");
        }
    }
}

