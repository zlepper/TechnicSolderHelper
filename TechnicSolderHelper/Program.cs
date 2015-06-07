using System;
using System.Windows.Forms;
using TechnicSolderHelper.OLD;

namespace TechnicSolderHelper
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SolderHelper());

        }

    }

}
