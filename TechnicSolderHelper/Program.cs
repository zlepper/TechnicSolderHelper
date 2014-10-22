using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using System.Net;
using System.Windows.Forms;

namespace TechnicSolderHelper
{

    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new SolderHelper());
            }
            catch (Exception e)
            {
                String errorLocation = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\errorFromTechnicSolderHelper.txt";
                if (globalfunctions.isUnix())
                {
                    errorLocation.Replace("\\", "/");
                }
                if (File.Exists(errorLocation))
                {
                    File.Delete(errorLocation);
                }
                File.AppendAllText(errorLocation, e.Message);
                File.AppendAllText(errorLocation, e.StackTrace);
                if (e.InnerException != null)
                {
                    File.AppendAllText(errorLocation, Environment.NewLine + e.InnerException.ToString());
                }
                Process.Start(errorLocation);
                MessageBox.Show("An unknown error occured. Please check the error log on you desktop." + Environment.NewLine + "It should have opened by itself.");
                return;
            }

        }

    }

}
