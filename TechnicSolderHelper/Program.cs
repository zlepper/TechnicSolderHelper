using System;
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
            /*try
            {*/
                Application.Run(new SolderHelper());
            /*}
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
                MessageBox.Show("An unknown error occured  Please check the error log on you desktop" + Environment.NewLine + "It should have opened by itself");
            }*/

        }

    }

}
