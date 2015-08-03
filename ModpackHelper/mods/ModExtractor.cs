using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading;
using ModpackHelper.Shared.IO;

namespace ModpackHelper.Shared.Mods
{
	public delegate void WorkModChangedEventHandler (string filename);

	public delegate void SerializationExceptionOccuredEventHandler (string filename);

	/// <summary>
	/// Used to extract mods
	/// </summary>
	public class ModExtractor
	{
		private readonly IFileSystem fileSystem;

		/// <summary>
		/// Happens everything works starts on a new modfile
		/// </summary>
		public event WorkModChangedEventHandler WorkingModChanged;
		/// <summary>
		/// Happens when a mod fails to serialize
		/// </summary>
		public event SerializationExceptionOccuredEventHandler SerializationExceptionOccured;

		/// <summary>
		/// Emited when the working file changes
		/// </summary>
		/// <param name="filename"></param>
		protected virtual void OnWorkingModChanged (string filename)
		{
			WorkingModChanged?.Invoke (filename);
		}

		/// <summary>
		/// Emited when there is a mod that couldn't be parsed
		/// </summary>
		/// <param name="filename"></param>
		protected virtual void OnSerializationExceptionOccured (string filename)
		{
			SerializationExceptionOccured?.Invoke (filename);
		}

		public ModExtractor (IFileSystem fileSystem)
		{
			this.fileSystem = fileSystem;
		}

		public ModExtractor () : this (fileSystem: new FileSystem ())
		{
            
		}

		/// <summary>
		/// Reads a file and returns the mcmod data
		/// </summary>
		/// <param name="pathToJson">The file to read</param>
		/// <returns>A mod created from the files json</returns>
		public Mcmod GetMcmodDataFromFile (string pathToJson)
		{
			string json = new IOHandler (fileSystem).ReadText (pathToJson);

			return GetMcmodDataFromJson (json);
		}

		/// <summary>
		/// Reads a file and returns the mcmod data
		/// </summary>
		/// <param name="pathToJson">The file to read</param>
		/// <returns>A mod created from the files json</returns>
		public Mcmod GetMcmodDataFromFile (FileInfoBase pathToJson)
		{
			return GetMcmodDataFromFile (pathToJson.FullName);
		}

		public static Mcmod GetMcmodDataFromJson (string json)
		{
			// Try to convert the json into an mcmod
			Mcmod mod = Mcmod.GetMcmod (json);

			// Try other formats
			if (mod == null) {
				Mcmod2 mcmod2 = Mcmod2.GetMcmod2 (json);
				if (mcmod2 == null) {
					Litemod litemod = Litemod.GetLitemod (json);
					if (litemod != null) {
						mod = litemod.ToMcmod ();
					}
				} else {
					mod = mcmod2.ToMcmod ();
				}
			}

			// No matching format was found for the mod.
			if (mod == null) {
				throw new SerializationException ("I can't turn that into anything useful");
			}



			return mod;
		}

		/// <summary>
		/// Starts the process of packing all the mods
		/// All data should be validated before this method is called since it contains no validation by itself.
		/// Also: You really shouldn't call this method on the main thread, use a background worker or a seperate thread. 
		/// </summary>
		/// <param name="inputDirectory"></param>
		/// <returns></returns>
		public List<Mcmod> FindAllMods (string inputDirectory)
		{
			return FindAllMods (fileSystem.DirectoryInfo.FromDirectoryName (inputDirectory));
		}

		/// <summary>
		/// Starts the process of packing all the mods
		/// All data should be validated before this method is called since it contains no validation by itself.
		/// Also: You really shouldn't call this method on the main thread, use a background worker or a seperate thread. 
		/// </summary> 
		public List<Mcmod> FindAllMods (DirectoryInfoBase inputDirectory)
		{
			// Find the mod files in the input directory
			Finder finder = new Finder (fileSystem);
			List<FileInfoBase> modFiles = finder.GetModFiles (inputDirectory);

			// Used to take care of all the io we will be doing
			IOHandler ioHandler = new IOHandler (fileSystem);
			ZipUtils zipUtils = new ZipUtils (fileSystem);

			// Create the list we will return
			List<Mcmod> mods = new List<Mcmod> (modFiles.Count);

			List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker> ();

			// Context for the local mods database
			using (ModsDBContext modsDB = new ModsDBContext (fileSystem)) {
				// Walk through all the found modfiles
				foreach (FileInfoBase modFile in modFiles) {
					// Create a background worker, so we can work in multiple threads and speedup what we are doing
					BackgroundWorker bw = new BackgroundWorker ();
					bw.DoWork += delegate {
						string filename = modFile.Name;

						// Update any event listernes, such as the interface
						OnWorkingModChanged (filename);

						// Check is the mod should be handled in a special way, 
						//fx liteloader requires a special way of finding the mod details
						int modAction = Mcmod.IsSpecialHandledMod (filename);
						if (modAction == 0) {
							return;
						}

						// Check if we already have this mod stored in the Database
						string md5Value = ioHandler.CalculateMd5 (modFile);
						// ReSharper disable once AccessToDisposedClosure
						Mcmod mod = modsDB.Mods.SingleOrDefault (m => m.JarMd5.Equals (md5Value));
						if (mod == null) {
							// We don't know about that specific mod file

							// Create a temporary folder where we can do stuff without messing up other things 
							// In theory anyway
							DirectoryInfoBase tempOutputFolder =
								fileSystem.DirectoryInfo.FromDirectoryName (
									fileSystem.Path.Combine (fileSystem.Path.GetTempPath (), "ModpackHelper",
										Guid.NewGuid ().ToString ()));

							// Extract all the .info files
							List<FileInfoBase> infoFiles = zipUtils.GetInfoFilesFromArchive (modFile, tempOutputFolder);

							// Check if we actually extracted any .info files
							if (infoFiles.Count == 0) {
								// Since there wasn't extracted any files, then we don't know 
								// anything about the file, and should just return and emptry file
								mod = new Mcmod ();
							}

							// Remove dependencies.info files, since we can't use them for anything at all. 
							foreach (FileInfoBase infoFile in infoFiles) {
								// Attempt to turn the .info files into info Mcmod objects
								try {
									mod = GetMcmodDataFromFile (infoFile);
								} catch (SerializationException) {
									// Inform the caller that we couldn't parse this file info an Mcmod for some reason
									OnSerializationExceptionOccured (modFile.FullName);
									mod = new Mcmod ();
								}
							}
							// Clear the temporary setup
							tempOutputFolder.Delete (true);
						}
						if (mod == null)
							return;
						mod.SetPath (modFile); // Done to ensure we can actually find the file again

						mod.JarMd5 = CalculateMd5 (mod.GetPath ()); // Make sure we have the file md5 so we can compare files

						mods.Add (mod); // Save back to our return values
					};
					// Save the background worker so we can find it later
					backgroundWorkers.Add (bw);
					// Start the background worker
					bw.RunWorkerAsync ();
				}
				// Make sure all backgroundworkers are finished running before returning to the caller
				int count = -1;
				while (backgroundWorkers.Any ()) { 
					// Remove background workers that are done
					foreach (BackgroundWorker bw in backgroundWorkers.Where(b => !b.IsBusy)) {
						bw.Dispose ();
					}
					backgroundWorkers.RemoveAll (b => !b.IsBusy);
					int c = backgroundWorkers.Count;
					if (c != count) {
						count = c;
						Debug.WriteLine (count + " backgroundworkers remaining.");
					}
				}

			}
			return mods;
		}

		public string CalculateMd5 (FileInfoBase f)
		{
			using (MD5 md5 = MD5.Create ()) {
				while (true) {
					try {
						using (Stream stream = f.OpenRead ()) {
							return BitConverter.ToString (md5.ComputeHash (stream)).Replace ("-", string.Empty);
						}
					} catch {
						Thread.Sleep (10);
					}
				}
			}
		}
	}
}
