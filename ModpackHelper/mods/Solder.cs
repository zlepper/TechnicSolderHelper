using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.IO;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web;

// ReSharper disable InconsistentNaming

namespace ModpackHelper.Shared.Mods
{
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
            public string id;
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

        public void Update(string url)
        {
            ISolderWebClient wc = new SolderWebClient(url);
            wc.Login(username, password);
            wc.CreatePack(modpack.Name, modpack.GetSlug());
            string buildId = wc.CreateBuild(modpack);

            var backgroundWorkers = new List<BackgroundWorker>(mods.Count);
            for (int i = 0; i < mods.Count; i++)
            {
                Mcmod mod = mods[i];
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
            while (backgroundWorkers.Count > 0)
            {
                var toRemove = backgroundWorkers.Where(backgroundWorker => !backgroundWorker.IsBusy).ToList();
                foreach (var backgroundWorker in toRemove)
                {
                    backgroundWorker.Dispose();
                    backgroundWorkers.Remove(backgroundWorker);
                }
            }
        }

        private void BwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // Parse all the arguments
            var parameters = doWorkEventArgs.Argument as Dictionary<string, object>;
            if(parameters == null) throw new NullReferenceException();
            ISolderWebClient wc = parameters["wc"] as ISolderWebClient;
            Mcmod mod = parameters["mod"] as Mcmod;
            string buildid = parameters["build"] as string;
            if(wc == null) throw new NullReferenceException();
            if(mod == null) throw new NullReferenceException();
            if(buildid == null) throw new NullReferenceException();

            // If the mod is already in solder, then we only need to add a build and add it to the modpack build
            string modid;
            try
            {
                modid = wc.GetModId(mod);
            }
            catch (Exception)
            {
                modid = wc.AddMod(mod);
            }
            if(string.IsNullOrWhiteSpace(modid)) throw new Exception("Something went wrong when adding a mod to solder.");

            IOHandler io = new IOHandler(fileSystem);
            string md5 = io.CalculateMd5(fileSystem.FileInfo.FromFileName(mod.OutputFile));
            wc.AddModVersion(modid, md5, mod.Version);

            wc.AddBuildToModpack(mod, buildid);
        }
    }
}
