using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModpackHelper.Shared.IO;
using ModpackHelper.Shared.Utils;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web;

// ReSharper disable InconsistentNaming

namespace ModpackHelper.Shared.Mods
{
    public delegate void DoneUpdatingModsEventHandler();
    public class Solder
    {
        private Modpack modpack;
        private string username;
        private string password;
        private List<Mcmod> mods;

        public class ApiMod
        {
            public string name { get; set; }
            public string pretty_name { get; set; }
            public string author { get; set; }
            public string description { get; set; }
            public string link { get; set; }
            public List<string> versions { get; set; }
        }

        public class ApiModVersion
        {
            public string md5 { get; set; }
            public string url { get; set; }
        }

        public class ModpackWithoutSlug
        {
            public Dictionary<string, string> modpacks { get; set; }
            public string mirror_url { get; set; }
        }

        public class ModpackWithoutBuild
        {
            public string id { get; set; }
            public string name { get; set; }
            public string display_name { get; set; }
            public string url { get; set; }
            public string icon { get; set; }
            public string icon_md5 { get; set; }
            public string logo { get; set; }
            public string logo_md5 { get; set; }
            public string background { get; set; }
            public string background_md5 { get; set; }
            public string recommended { get; set; }
            public string latest { get; set; }
            public List<string> builds { get; set; }
        }

        public class ModpackWithBuild
        {
            public string id;

            public class Mod
            {
                public string name { get; set; }
                public string version { get; set; }
                public string md5 { get; set; }
                public string url { get; set; }
                public string id { get; set; }
            }

            public string minecraft { get; set; }
            public string minecraft_md5 { get; set; }
            public object forge { get; set; }
            public List<Mod> mods { get; set; }
        }

        public class Error
        {
            public string error { get; set; }
        }

        private IFileSystem fileSystem;

        public Solder() : this(new FileSystem()) { }

        public Solder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Initialize(string username, string password, List<Mcmod> mods, Modpack modpack)
        {
            this.username = username;
            this.password = password;
            this.mods = mods;
            this.modpack = modpack;
        }

        public event DoneUpdatingModsEventHandler DoneUpdatingMods;

        /// <summary>
        /// Called when everything is filled in, and the form closes
        /// </summary>
        protected virtual void OnDoneUpdatingMods()
        {
            DoneUpdatingMods?.Invoke();
        }

        public void Update(string url)
        {
            ISolderWebClient wc = new SolderWebClient(url);
            bool loginSuccessful = wc.Login(username, password);
            if (!loginSuccessful)
            {
                throw new Exception("Attempted to update solder, but could not log in");
            }
            string modpackId = null;
            if (wc.IsPackOnline(modpack))
            {
                modpackId = wc.GetModpackId(modpack.GetSlug());
            }
            else
            {
                modpackId = wc.CreatePack(modpack.Name, modpack.GetSlug());
            }
            string buildId = null;
            if (wc.IsBuildOnline(modpack))
            {
                buildId = wc.GetBuildId(modpack);
            }
            else
            {

                buildId = wc.CreateBuild(modpack, modpackId);
            }

            var backgroundWorkers = new List<BackgroundWorker>(mods.Count);
            foreach (Mcmod mod in mods)
            {
                if (mod.IsSkipping) continue;
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += BwOnDoWork;
                var parameters = new Dictionary<string, object>
                {
                    {"mod", mod},
                    {"build", buildId},
                    {"wc", wc }
                };
                bw.RunWorkerAsync(parameters);
                backgroundWorkers.Add(bw);
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
            OnDoneUpdatingMods();
        }

        private void BwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // Parse all the arguments
            var parameters = doWorkEventArgs.Argument as Dictionary<string, object>;
            if (parameters == null) throw new NullReferenceException();
            ISolderWebClient wc = parameters["wc"] as ISolderWebClient;
            Mcmod mod = parameters["mod"] as Mcmod;
            string buildid = parameters["build"] as string;
            if (wc == null) throw new NullReferenceException();
            if (mod == null) throw new NullReferenceException();
            if (buildid == null) throw new NullReferenceException();

            if (!mod.IsSkipping)
            {
                // If the mod is already in solder, then we only need to add a build and add it to the modpack build
                string modid;
                try
                {
                    modid = wc.GetModId(mod);
                    if (string.IsNullOrWhiteSpace(modid))
                    {
                        throw new NullReferenceException();
                    }
                }
                catch (Exception)
                {
                    modid = wc.AddMod(mod);
                }
                if (string.IsNullOrWhiteSpace(modid))
                    throw new Exception("Something went wrong when adding a mod to solder.");


                if (!wc.IsModversionOnline(mod))
                {
                    IOHandler io = new IOHandler(fileSystem);
                    string md5 = io.CalculateMd5(fileSystem.FileInfo.FromFileName(mod.OutputFile));
                    wc.AddModVersion(modid, md5, mod.GetOnlineVersion());
                }
                if (wc.IsModversionActiveInBuild(mod, buildid))
                {
                    
                }
                else
                {
                    if (wc.IsModInBuild(mod, buildid))
                    {
                        wc.SetModversionInBuild(mod, buildid);
                    }
                    else
                    {
                        wc.AddModversionToBuild(mod, buildid);
                    }
                }
                Debug.WriteLine("Done");
            }
            else
            {
                Debug.WriteLine("Skipped");
            }
        }
    }
}
