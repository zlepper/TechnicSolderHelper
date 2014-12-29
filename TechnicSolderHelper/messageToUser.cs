using System;
using System.Windows.Forms;
using TechnicSolderHelper.Properties;

namespace TechnicSolderHelper
{
    public class MessageToUser
    {
        public void FirstTimeRun()
        {
            MessageBox.Show(Resources.MessageToUser_FirstTimeRun_This_is_the_first_time_you_are_running_the_problem__so_it_might_take_a_while_to_start__since_it_needs_to_build_some_databases_);
        }

        public void UploadingToFtp()
        {
            MessageBox.Show(Resources.MessageToUser_UploadingToFtp_Uploading_stuff_to_FTP);
            Console.WriteLine(Resources.MessageToUser_UploadingToFtp_Yep__i_m_uploading_all_the_things___);
        }
    }
}

