
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using MonoMac.Foundation;
using MonoMac.AppKit;
using ModpackHelper.Shared.Mods;
using System.Diagnostics;
using System.IO;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Mac.UserInteraction;
using ModpackHelper.Shared.MinecraftForge;
using ModpackHelper.Shared.IO;
using System.IO.Abstractions;

namespace ModpackHelper.Mac
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		private IMacMessageShower messageShower;

		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			messageShower = new MessageShower ();
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			// TODO Do any stuff that needs to be done, like loading the minecraft versions here!

			// Minecraft versions
			ForgeHandler forgeHandler = new ForgeHandler ();
			if (forgeHandler.GetMinecraftVersions ().Count < 5) {
				forgeHandler.DownloadForgeVersions ();
			}
			MinecraftVersionComboBox.RemoveAllItems ();
			MinecraftVersionComboBox.AddItems (forgeHandler.GetMinecraftVersions ().ToArray ());

		}

		#endregion

		public void packMods (string minecraftVersion, string outputdirectory, string inputDirectory)
		{

			BackgroundWorker bw = new BackgroundWorker ();
			bw.DoWork += delegate {
				ModExtractor modExtrator = new ModExtractor ();
				List<Mcmod> mods = modExtrator.FindAllMods (inputDirectory);
				BeginInvokeOnMainThread (new NSAction (() => OpenModsInfoForm (mods, minecraftVersion, outputdirectory)));
			};
			bw.RunWorkerAsync ();
		}

		private void OpenModsInfoForm (List<Mcmod> mods, string minecraftVersion, string outputDirectory)
		{
			ModsInfoWindowController miwc = new ModsInfoWindowController ();
			miwc.Window.InitializeContent (mods, minecraftVersion);
			miwc.Window.MakeKeyAndOrderFront (this);
			miwc.Window.DoneFillingInInfo += (List<Mcmod> modsList) => {
				var bw = new BackgroundWorker ();
				bw.DoWork += (object sender, DoWorkEventArgs e) => {
					ModPacker packer = new ModPacker ();
					packer.Pack (modsList, new FileSystem ().DirectoryInfo.FromDirectoryName (outputDirectory));
					string html = packer.GetFinishedHTML ();
					File.WriteAllText (Path.Combine (outputDirectory, "mods.html"), html);
				};
				bw.RunWorkerAsync ();
			};
		}

		partial void BrowseForInputDirectoryButtonClicked (NSObject sender)
		{
			var input = new DirectoryFinder ().GetDirectory ("Input directory");
			// The user clicked cancel
			if (string.IsNullOrWhiteSpace (input)) {
				return;
			}
			// Check if the entered directory is valid
			if (input.EndsWith (Path.DirectorySeparatorChar + "mods")) {
				InputDirectoryTextBox.StringValue = input;
			} else {
				IMacMessageShower ms = new MessageShower ();
				ms.ShowMessageAsync ("Input directory has to be a /mods/ directory");
			}
		}

		partial void BrowseForOutputDirectoryButtonClicked (NSObject sender)
		{
			var output = new DirectoryFinder ().GetDirectory ("Output Directory");

			if (!string.IsNullOrWhiteSpace (output)) {
				OutputDirectoryTextBox.StringValue = output;
			}
		}

		partial void StartPackingButtonClicked (NSObject sender)
		{
			string inputDirectory = InputDirectoryTextBox.StringValue;
			string outputDirectory = OutputDirectoryTextBox.StringValue;
			string minecraftVersion = MinecraftVersionComboBox.TitleOfSelectedItem;

			// Validate input directory
			if (!Directory.Exists (inputDirectory)) {
				ShowMessage ("Inputdirectory does not exist");
				return;
			}
			if (!inputDirectory.EndsWith (Path.DirectorySeparatorChar + "mods")) {
				ShowMessage ("Inputdirectory is not a /mods/ directory");
				return;
			}

			// Validate minecraft version
			if (string.IsNullOrWhiteSpace (minecraftVersion)) {
				ShowMessage ("You have to select a minecraft version");
				return;
			}
			ForgeHandler fh = new ForgeHandler ();
			if (!fh.GetMinecraftVersions ().Contains (minecraftVersion)) {
				ShowMessage ("Invalid Minecraft version");
				return;
			}

			packMods (minecraftVersion, outputDirectory, inputDirectory);

		}

		private void ShowMessage (string message)
		{
			messageShower.ShowMessage (message, this);
		}
	}
}

