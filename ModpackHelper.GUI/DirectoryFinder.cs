using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModpackHelper;
using ModpackHelper.UserInteraction;

namespace ModpackHelper.GUI
{
    public class DirectoryFinder : IDirectoryFinder
    {
        public string GetDirectory(string whereTo)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog {Description = whereTo};
            DialogResult result = dialog.ShowDialog();
            return result == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }
    }
}
