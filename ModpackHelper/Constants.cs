using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared
{
    /// <summary>
    /// A list of constants used throughout the application
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The url of the remote api
        /// </summary>
        public const string ApiUrl = "http://localhost:58013/";

        /// <summary>
        /// The path to where all Modpack Helpers own files are stored
        /// TODO Refractor everything to use this instead of it's own files
        /// </summary>
        public static string ApplicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper");
    }
}
