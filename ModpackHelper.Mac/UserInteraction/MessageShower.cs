using System;
using ModpackHelper.Shared.UserInteraction;
using MonoMac.AppKit;
using System.ComponentModel;

namespace ModpackHelper.Mac.UserInteraction
{
	public class MessageShower : IMacMessageShower
	{
		#region IMessageShower implementation

		public void ShowMessage (string message)
		{
			// Setup the messagebox
			var alert = new NSAlert {
				MessageText = message,
				AlertStyle = NSAlertStyle.Informational
			};

			//alert.AddButton ("OK");

			// Show the message to the user
			alert.RunModal ();
		}

		public void ShowMessageAsync (string message)
		{
			ShowMessage (message);
		}


		public void ShowMessage (string message, NSWindow window)
		{
			var alert = new NSAlert {
				MessageText = message,
				AlertStyle = NSAlertStyle.Informational
			};

			alert.BeginSheet (window);
		}

		#endregion
	}

	public interface IMacMessageShower : IMessageShower
	{
		void ShowMessage (string message, NSWindow window);
	}
}

