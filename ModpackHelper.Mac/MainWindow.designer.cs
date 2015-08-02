// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace ModpackHelper.Mac
{
	[Register ("MainWindow")]
	partial class MainWindow
	{
		[Outlet]
		MonoMac.AppKit.NSTextField InputDirectoryTextBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton MinecraftVersionComboBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModpackNameTextBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModpackVersionTextBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField OutputDirectoryTextBox { get; set; }

		[Action ("BrowseForInputDirectoryButtonClicked:")]
		partial void BrowseForInputDirectoryButtonClicked (MonoMac.Foundation.NSObject sender);

		[Action ("BrowseForOutputDirectoryButtonClicked:")]
		partial void BrowseForOutputDirectoryButtonClicked (MonoMac.Foundation.NSObject sender);

		[Action ("StartPackingButtonClicked:")]
		partial void StartPackingButtonClicked (MonoMac.Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (InputDirectoryTextBox != null) {
				InputDirectoryTextBox.Dispose ();
				InputDirectoryTextBox = null;
			}

			if (MinecraftVersionComboBox != null) {
				MinecraftVersionComboBox.Dispose ();
				MinecraftVersionComboBox = null;
			}

			if (ModpackNameTextBox != null) {
				ModpackNameTextBox.Dispose ();
				ModpackNameTextBox = null;
			}

			if (ModpackVersionTextBox != null) {
				ModpackVersionTextBox.Dispose ();
				ModpackVersionTextBox = null;
			}

			if (OutputDirectoryTextBox != null) {
				OutputDirectoryTextBox.Dispose ();
				OutputDirectoryTextBox = null;
			}
		}
	}

	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
