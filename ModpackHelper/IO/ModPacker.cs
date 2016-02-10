using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using ModpackHelper.IO;
using ModpackHelper.Shared.MinecraftForge;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web;

namespace ModpackHelper.Shared.IO
{
    public class ModPacker
    {
        private readonly IFileSystem fileSystem;
        private readonly StringBuilder sb;

        private static readonly List<string> BannedFileNames = new List<string>() { "YAMPST.nbt" };

        public ModPacker() : this(new FileSystem())
        {

        }

        public ModPacker(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            sb = new StringBuilder();
            const string html = "<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><script src=\"https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js\"></script><script src=\"http://zlepper.dk/modpackhelper.js\"></script><link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.0/css/materialize.min.css\"></head><body class=\"container\"><table class=\"\"><thead><tr><th>Modname</th><th>Modslug</th><th>Version</th></tr></thead><tbody>";
            sb.AppendLine(html);
        }

        public void Pack(List<Mcmod> mods, Modpack modpack)
        {
            // Create a SignalR connection to the api
            using (HubConnection hubConnection = new HubConnection(Constants.ApiUrl))
            {
                IHubProxy apiHubProxy = hubConnection.CreateHubProxy("ApiHub");
                Task con = hubConnection.Start();

                DirectoryInfoBase outputDirectory = fileSystem.DirectoryInfo.FromDirectoryName(modpack.OutputDirectory);


                ISolderWebClient solderWebClient = null;
                if (modpack.UseSolder)
                {
                    using (var config = new ConfigHandler(fileSystem))
                    {
                        var sli = config.Configs.SolderLoginInfo;
                        solderWebClient = new SolderWebClient(sli.Address);
                    }
                }

                // Create the output directory where we should put all the new files
                outputDirectory.Create();
                List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>(mods.Count);
                ZipUtils zipUtils = new ZipUtils(fileSystem);
                using (ModsDBContext db = new ModsDBContext(fileSystem))
                {
                    Mcmod forge = null;
                    // Check if we should pack forge, and then do it!
                    if (modpack.CreateForgeZip)
                    {
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (sender, args) =>
                        {
                            bool skip = false;
                            var forgedownloadUrl =
                                new ForgeHandler(fileSystem).GetDownloadUrl((int.Parse(modpack.ForgeVersion)));
                            if (modpack.UseSolder)
                            {

                                //SolderMySQLHelper ssh = new SolderMySQLHelper(modpack.Name, modpack.Version);
                                if (
                                    solderWebClient != null && solderWebClient.IsModversionOnline(Mcmod.GetForgeMod(modpack.MinecraftVersion, modpack.ForgeVersion)))
                                {
                                    skip = true;
                                }
                            }
                            if (!skip)
                            {
                                ZipUtils zu = new ZipUtils(fileSystem);
                                var forgeZip =
                                    fileSystem.FileInfo.FromFileName(Path.Combine(modpack.OutputDirectory, "mods",
                                        "forge", $"forge-{modpack.MinecraftVersion}-{modpack.ForgeVersion}.zip"));
                                WebClient wb = new WebClient();
                                using (Stream s = wb.OpenRead(forgedownloadUrl))
                                {
                                    zu.SpecialPackForgeZip(s, forgeZip);
                                }
                                forge = Mcmod.GetForgeMod(modpack.MinecraftVersion, modpack.ForgeVersion);
                                forge.OutputFile = forgeZip.FullName;
                                AddDataToOutput(forge.Name, forge.Modid, forge.GetOnlineVersion());
                            }
                        };
                        backgroundWorkers.Add(bw);
                        bw.RunWorkerAsync();
                    }

                    // Queue additional folder for packing
                    foreach (AdditionalFolder additionalFolder in modpack.AdditionalFolders.Where(f => f.Pack))
                    {
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (sender, args) =>
                        {
                            var outputFile =
                                fileSystem.FileInfo.FromFileName(Path.Combine(modpack.OutputDirectory, "mods",
                                    $"{modpack.GetSlug()}-{additionalFolder.Name}",
                                    $"{modpack.GetSlug()}-{additionalFolder.Name}-{modpack.MinecraftVersion}-{modpack.Version}.zip"));

                            ZipUtils zu = new ZipUtils(fileSystem);
                            zu.ZipDirectory(additionalFolder.Fullname, outputFile.FullName);
                            AddDataToOutput($"{modpack.Name} {additionalFolder.Name}", $"{modpack.GetSlug()}-{additionalFolder.Name}", $"{modpack.MinecraftVersion}-{modpack.Version}.zip");
                        };
                        backgroundWorkers.Add(bw);
                        bw.RunWorkerAsync();
                    }

                    // Wait for the SignalR connection to be established
                    con.Wait();

                    // Iterate over all the mods and create a thread for each mod
                    foreach (Mcmod mod in mods)
                    {
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += (sender, args) =>
                        {
                            string modID = mod.GetSafeModId();
                            string modversion = mod.GetOnlineVersion().ToLower();

                            // Check if mod is online
                            Mcmod mo = db.Mods.FirstOrDefault(m => m.JarMd5 != null && m.JarMd5.Equals(mod.JarMd5));
                            if (mo != null && (mo.IsOnSolder && !modpack.ForceSolder || (solderWebClient != null && solderWebClient.IsModversionOnline(mod))))
                            {
                                mod.IsSkipping = true;
                                return;
                            }
                            // Create the output directory 
                            string zipFileDirectory = fileSystem.Path.Combine(outputDirectory.FullName, "mods", modID);
                            fileSystem.Directory.CreateDirectory(zipFileDirectory);

                            mod.OutputFile = fileSystem.Path.Combine(outputDirectory.FullName, "mods", modID,
                                modID + "-" + modversion + ".zip");
                            FileInfoBase zipFile = fileSystem.FileInfo.FromFileName(mod.OutputFile);

                            Debug.WriteLine("Packing " + modID);
                            zipUtils.SpecialPackSolderMod(mod.GetPath(), zipFile);
                            Debug.WriteLine("Done packing " + modID);
                            mod.IsOnSolder = true;
                            AddDataToOutput(mod.Name, modID, modversion);

                            // Check if this mods data was entered by the user
                            // And if it is, upload it to the webapi
                            if (!mod.IsSkipping && mod.FromUser && !mod.FromSuggestion)
                            {
                                mod.UploadToApi(apiHubProxy);
                            }
                        };
                        backgroundWorkers.Add(bw);
                        bw.RunWorkerAsync();
                    }


                    // Make sure all backgroundworkers are finished running before returning to the caller
                    int count = -1;
                    while (backgroundWorkers.Any())
                    {
                        // Remove background workers that are done
                        foreach (BackgroundWorker bw in backgroundWorkers.Where(b => !b.IsBusy))
                        {
                            bw.Dispose();
                        }
                        backgroundWorkers.RemoveAll(b => !b.IsBusy);
                        int c = backgroundWorkers.Count;
                        if (c != count)
                        {
                            count = c;
                            Debug.WriteLine(count + " backgroundworkers remaining.");
                        }
                    }

                    // Make sure to include forge in any future actions taken
                    if (forge != null)
                    {
                        mods.Add(forge);
                    }

                    // Save updated mod data to the database
                    foreach (Mcmod mod in mods)
                    {
                        db.Mods.RemoveAll(m => m.JarMd5 != null && m.JarMd5.Equals(mod.JarMd5));
                        db.Mods.Add(mod);
                    }
                }
            }
        }

        private void AddDataToOutput(string name, string id, string version)
        {
            string addedMod = $"<tr><td><input readonly type=\"text\" class=\"containsInfo\" value=\"{name}\"></td>" +
                              $"<td><input readonly type=\"text\" class=\"containsInfo\" value=\"{id}\"></td>" +
                              $"<td><input readonly type=\"text\" class=\"containsInfo\" value=\"{version}\"></td>" +
                              "<td><button class=\"waves-effect waves-light btn\" type=\"button\">Hide</button></td></tr>";
            sb.AppendLine(addedMod);
        }

        public string GetFinishedHTML()
        {
            sb.AppendLine(
                @"</table><button id=""Reshow"" type=""button"">Unhide Everything</button><p>List autogenerated by Modpack Helper &copy; 2016 - Zlepper</p></body></html>");
            return sb.ToString();
        }
    }
}
