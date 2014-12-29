using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TechnicSolderHelper.Properties;

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
                String errorLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "errorFromTechnicSolderHelper.txt");
                if (File.Exists(errorLocation))
                {
                    File.Delete(errorLocation);
                }
                File.AppendAllText(errorLocation, e.Message + Environment.NewLine);
                File.AppendAllText(errorLocation, e.StackTrace);
                if (e.InnerException != null)
                {
                    File.AppendAllText(errorLocation, Environment.NewLine + e.InnerException);
                }
                Process.Start(errorLocation);
                MessageBox.Show(Resources.Program_Main_An_unknown_error_occured__Please_check_the_error_log_on_you_desktop_ + Environment.NewLine + Resources.Program_Main_It_should_have_opened_by_itself_);
            }

        }

    }

}
