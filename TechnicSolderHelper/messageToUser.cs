using System;
using System.Windows.Forms;

namespace TechnicSolderHelper
{
    public class messageToUser
    {
        public messageToUser()
        {
        }

        public void firstTimeRun()
        {
            MessageBox.Show("This is the first time you are running the problem, so it might take a while to start, since it needs to build some databases.");
        }
    }
}

