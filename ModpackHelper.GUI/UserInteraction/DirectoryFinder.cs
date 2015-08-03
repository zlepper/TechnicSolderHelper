using System.Windows.Forms;
using ModpackHelper.Shared.UserInteraction;

namespace ModpackHelper.GUI.UserInteraction
{
    public class DirectoryFinder : IDirectoryFinder
    {
        public string GetDirectory(string whereTo, string start = "")
        {
            // Create a new dialog
            FolderBrowserDialog dialog = new FolderBrowserDialog {Description = whereTo, SelectedPath = start};
            // Show the dialog and get the result
            DialogResult result = dialog.ShowDialog();
            // If the user clicked OK return whatever they selected, otherwise return an empty string
            return result == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }
    }

    
}
