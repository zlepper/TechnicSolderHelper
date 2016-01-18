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
            try
            {
                Application.Run(new SolderHelper());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString(), true);
                Debug.WriteLine(e.Message, true);
                Debug.WriteLine(e.StackTrace, true);
                Debug.WriteLine(e.InnerException, true);
                Debug.Save();
                MessageBox.Show("An error occured, please check the log on your desktop");
            }

        }

    }

}
