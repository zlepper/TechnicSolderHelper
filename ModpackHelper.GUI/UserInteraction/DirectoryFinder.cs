using System.Windows.Forms;

namespace ModpackHelper.GUI.UserInteraction
{
    public class DirectoryFinder 
    {
        public string GetDirectory(string whereTo)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog {Description = whereTo};
            DialogResult result = dialog.ShowDialog();
            return result == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }
    }
}
