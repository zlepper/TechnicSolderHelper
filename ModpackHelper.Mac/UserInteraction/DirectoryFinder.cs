using System;
using ModpackHelper.Shared.UserInteraction;
using MonoMac.AppKit;
using System.IO;
using System.Threading;

namespace ModpackHelper.Mac.UserInteraction
{
	public class DirectoryFinder : IDirectoryFinder
	{
		#region IDirectoryFinder implementation

		/// <summary>
		/// Prompts the user to select a directory
		/// </summary>
		/// <param name="whereTo">The directory the user should locate</param>
		/// <param name="start">This parameter is ignored on OSX</param>
		/// <returns>The directory the user selected, or an empty string if they didn't select anything</returns>
		public string GetDirectory (string whereTo, string start = "")
		{
			// Setup the new dialog to prompt the user for information
			var dialog = new NSOpenPanel ();
			dialog.CanChooseDirectories = true;
			dialog.CanChooseFiles = false;
			dialog.Title = whereTo;

			// Show the dialog to the user
			var result = dialog.RunModal ();

			// The user clicked OK
			if (result == 1) {
				return dialog.Url.Path;
			} else {
				// The user clicked cancel
				return "";
			}

			#endregion
		}
	}
}
