using System.Windows.Forms;

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

    public interface IDirectoryFinder
    {
        /// <summary>
        /// Prompts the user to select a directory
        /// </summary>
        /// <param name="whereTo">The directory the user should locate</param>
        /// <param name="start">Optional parameter. Where the search should start</param>
        /// <returns>The directory the user selected, or an empty string if they didn't select anything</returns>
        string GetDirectory(string whereTo, string start = "");
    }
}
