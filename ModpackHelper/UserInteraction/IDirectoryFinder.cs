using System;

namespace ModpackHelper.Shared.UserInteraction
{
	public interface IDirectoryFinder
	{
		/// <summary>
		/// Prompts the user to select a directory
		/// </summary>
		/// <param name="whereTo">The directory the user should locate</param>
		/// <param name="start">Optional parameter. Where the search should start</param>
		/// <returns>The directory the user selected, or an empty string if they didn't select anything</returns>
		string GetDirectory (string whereTo, string start = "");
	}
}

