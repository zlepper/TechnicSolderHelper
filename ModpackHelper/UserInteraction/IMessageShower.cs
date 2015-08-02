namespace ModpackHelper.Shared.UserInteraction
{
	public interface IMessageShower
	{
		void ShowMessage (string message);

		void ShowMessageAsync (string message);
	}
}
