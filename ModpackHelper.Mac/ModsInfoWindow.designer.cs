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
	[Register ("ModsInfoWindow")]
	partial class ModsInfoWindow
	{
		[Outlet]
		MonoMac.AppKit.NSTextField AuthorTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField FileNameTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModIDTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModsNameTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton ModsSelectionPopUp { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModVersionTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton ShowDoneCheckBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton SkipModCheckBox { get; set; }

		[Action ("CancelButtonClicked:")]
		partial void CancelButtonClicked (MonoMac.Foundation.NSObject sender);

		[Action ("DoneButtonClicked:")]
		partial void DoneButtonClicked (MonoMac.Foundation.NSObject sender);

		[Action ("ModAuthorTextChanged:")]
		partial void ModAuthorTextChanged (MonoMac.Foundation.NSObject sender);

		[Action ("ModIdTextChanged:")]
		partial void ModIdTextChanged (MonoMac.Foundation.NSObject sender);

		[Action ("ModNameTextChanged:")]
		partial void ModNameTextChanged (MonoMac.Foundation.NSObject sender);

		[Action ("ModVersionTextChanged:")]
		partial void ModVersionTextChanged (MonoMac.Foundation.NSObject sender);

		[Action ("SelectedIndexChanged:")]
		partial void SelectedIndexChanged (MonoMac.Foundation.NSObject sender);

		[Action ("ShowDoneCheckboxChanged:")]
		partial void ShowDoneCheckboxChanged (MonoMac.Foundation.NSObject sender);

		[Action ("SkipAllButtonClicked:")]
		partial void SkipAllButtonClicked (MonoMac.Foundation.NSObject sender);

		[Action ("SkipModCheckboxChanged:")]
		partial void SkipModCheckboxChanged (MonoMac.Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (AuthorTextField != null) {
				AuthorTextField.Dispose ();
				AuthorTextField = null;
			}

			if (FileNameTextField != null) {
				FileNameTextField.Dispose ();
				FileNameTextField = null;
			}

			if (ModIDTextField != null) {
				ModIDTextField.Dispose ();
				ModIDTextField = null;
			}

			if (ModsNameTextField != null) {
				ModsNameTextField.Dispose ();
				ModsNameTextField = null;
			}

			if (ModsSelectionPopUp != null) {
				ModsSelectionPopUp.Dispose ();
				ModsSelectionPopUp = null;
			}

			if (ModVersionTextField != null) {
				ModVersionTextField.Dispose ();
				ModVersionTextField = null;
			}

			if (ShowDoneCheckBox != null) {
				ShowDoneCheckBox.Dispose ();
				ShowDoneCheckBox = null;
			}

			if (SkipModCheckBox != null) {
				SkipModCheckBox.Dispose ();
				SkipModCheckBox = null;
			}
		}
	}

	[Register ("ModsInfoWindowController")]
	partial class ModsInfoWindowController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
