
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using ModpackHelper.Shared.Mods;
using System.Diagnostics;
using ModpackHelper.Mac.UserInteraction;
using System.Xml;

namespace ModpackHelper.Mac
{
	public delegate void DoneFillingInInfoEventHandler (List<Mcmod> mods);

	public partial class ModsInfoWindow : MonoMac.AppKit.NSWindow
	{
		public event DoneFillingInInfoEventHandler DoneFillingInInfo;

		public virtual void OnDoneFillingInInfo ()
		{
			if (DoneFillingInInfo != null) {
				DoneFillingInInfo.Invoke (mods.Where (m => !m.IsSkipping).ToList ());
			}
		}

		private List<Mcmod> mods;
		private List<Mcmod> nonFinishedMods;
		private string currentMcVersion;
		private Mcmod selectedMod;
		private readonly IMacMessageShower messageShower = new MessageShower ();

		#region Constructors

		// Called when created from unmanaged code
		public ModsInfoWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public ModsInfoWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}

		public void InitializeContent (List<Mcmod> modsList, string minecraftVersion)
		{
			// This run second
			this.currentMcVersion = minecraftVersion;

			mods = modsList;
			// Find all the mods that still needs info
			nonFinishedMods = mods.Where (m => !m.IsValid ()).ToList ();

			// No need to run over everything again if every mod is valid already
			if (nonFinishedMods.Any ()) {
				foreach (Mcmod mod in nonFinishedMods) {
					// Set the minecraft version if it's missing
					if (string.IsNullOrWhiteSpace (mod.Mcversion))
						mod.Mcversion = currentMcVersion;
					// Set the author list if it's missing
					if (mod.AuthorList == null || mod.AuthorList.Count == 0) {
						mod.GetAuthors ();
					}
				}

				// Find all the mods that stil misses info
				nonFinishedMods = nonFinishedMods.Where (m => !m.IsValid ()).ToList ();
			}

			// If all the mods have info then just close the window and continue
			if (!nonFinishedMods.Any ()) {
				OnDoneFillingInInfo ();
				Close ();
			} else {
				// Add all the missing info mods to the modlist
				foreach (var mod in nonFinishedMods) {
					ModsSelectionPopUp.AddItem (string.IsNullOrWhiteSpace (mod.Name) ? mod.GetPath ().Name : mod.Name);
				}
				// Select the first mod on the list
				ModsSelectionPopUp.SelectItem (0);
				SelectedIndexChanged (this);
			}
		}



		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			// This runs first
		}

		#endregion

		private Mcmod GetSelectedMod ()
		{
			int index = ModsSelectionPopUp.IndexOfSelectedItem;
			return ShowDoneCheckBox.State == NSCellStateValue.On ? mods [index] : nonFinishedMods [index];
		}

		private void SetSelectedModsTextInList (string text)
		{
			ModsSelectionPopUp.SelectedItem.Title = text;
		}

		partial void SelectedIndexChanged (NSObject sender)
		{
			selectedMod = GetSelectedMod ();

			//Load data back into the mod info boxes
			SkipModCheckBox.State = selectedMod.IsSkipping ? NSCellStateValue.On : NSCellStateValue.Off;
			ModVersionTextField.StringValue = selectedMod.Version ?? string.Empty;
			ModsNameTextField.StringValue = selectedMod.Name ?? string.Empty;
			FileNameTextField.StringValue = selectedMod.GetPath ().Name;

			// We can't display lists, so we'll display a nicely formatted string
			AuthorTextField.StringValue = selectedMod.AuthorList != null ? string.Join (", ", selectedMod.AuthorList) : string.Empty;
			ModIDTextField.StringValue = selectedMod.Modid ?? string.Empty;
		}

		partial void ModAuthorTextChanged (MonoMac.Foundation.NSObject sender)
		{
			// Convert the string to a list
			selectedMod.AuthorList = AuthorTextField.StringValue.Replace (", ", ",").Split (',').ToList ();
		}

		partial void ModIdTextChanged (NSObject sender)
		{
			selectedMod.Modid = ModIDTextField.StringValue;

			if (string.IsNullOrWhiteSpace (AuthorTextField.StringValue)) {
				var a = selectedMod.GetAuthors ();
				if (a.Any ()) {
					AuthorTextField.StringValue = string.Join (", ", a);
				}
			}
		}

		partial void ModNameTextChanged (MonoMac.Foundation.NSObject sender)
		{
			if (string.IsNullOrWhiteSpace (ModsNameTextField.StringValue)) {
				selectedMod.Name = string.Empty;
				SetSelectedModsTextInList (selectedMod.GetPath ().Name);
			} else {
				selectedMod.Name = ModsNameTextField.StringValue;
				SetSelectedModsTextInList (selectedMod.Name);
			}
		}

		partial void ModVersionTextChanged (MonoMac.Foundation.NSObject sender)
		{
			selectedMod.Version = ModVersionTextField.StringValue;
		}

		partial void ShowDoneCheckboxChanged (MonoMac.Foundation.NSObject sender)
		{
			ModsSelectionPopUp.RemoveAllItems ();
			if (ShowDoneCheckBox.State == NSCellStateValue.On) {
				foreach (var mod in mods) {
					ModsSelectionPopUp.AddItem (string.IsNullOrWhiteSpace (mod.Name) ? mod.GetPath ().Name : mod.Name);
				}
			} else {
				foreach (var mod in nonFinishedMods) {
					ModsSelectionPopUp.AddItem (string.IsNullOrWhiteSpace (mod.Name) ? mod.GetPath ().Name : mod.Name);
				}
			}
			ModsSelectionPopUp.SelectItem (0);
			SelectedIndexChanged (this);
		}

		partial void SkipModCheckboxChanged (MonoMac.Foundation.NSObject sender)
		{
			selectedMod.IsSkipping = SkipModCheckBox.State == NSCellStateValue.On;
		}

		partial void CancelButtonClicked (MonoMac.Foundation.NSObject sender)
		{
			Close ();
		}

		partial void DoneButtonClicked (MonoMac.Foundation.NSObject sender)
		{
			if (mods.Any (m => !m.IsSkipping && !m.IsValid ())) {
				messageShower.ShowMessage ("You are not done entering info. Please finish!", this);
			} else {
				OnDoneFillingInInfo ();
				Close ();
			}
		}

		partial void SkipAllButtonClicked (MonoMac.Foundation.NSObject sender)
		{
			foreach (var mod in nonFinishedMods) {
				mod.IsSkipping = true;
			}
		}
	}
}

