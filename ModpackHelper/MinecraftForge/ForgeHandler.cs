using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using ModpackHelper.MinecraftForge;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.MinecraftForge
{
    public class ForgeHandler : IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly string forgeVersionFilePath;
        private List<ForgeVersion> forgeVersions; 

        public ForgeHandler(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;

            forgeVersionFilePath =
                this.fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SolderHelper", "forgeversions.json");

            forgeVersions = LoadForgeVersions();
        }

        public ForgeHandler() : this(new FileSystem()) {}

        public string SaveForgeVersions(List<ForgeVersion> forgeVersions)
        {
            this.forgeVersions = forgeVersions;
            return SaveForgeVersions();
        }

        public string SaveForgeVersions()
        {
            string json = JsonConvert.SerializeObject(forgeVersions);
            fileSystem.File.WriteAllText(forgeVersionFilePath, json);
            return json;
        }

        public List<ForgeVersion> LoadForgeVersions()
        {
            if (!fileSystem.File.Exists(forgeVersionFilePath))
            {
                return new List<ForgeVersion>();
            }
            try
            {
                using (Stream s = fileSystem.File.OpenRead(forgeVersionFilePath))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    
                    List<ForgeVersion> fv = serializer.Deserialize<List<ForgeVersion>>(reader);
                    return fv ?? new List<ForgeVersion>();
                }
            }
            catch (Exception)
            {
                return new List<ForgeVersion>();
            }
        }

        public List<string> GetMinecraftVersions()
        {
            Regex minecraftPattern = new Regex(@"[0-9]\.[0-9](\.[0-9]{1,2})?", RegexOptions.IgnoreCase);
            return forgeVersions.Select(f => f.MinecraftVersion).Distinct().Where(f => minecraftPattern.IsMatch(f)).ToList();
        }

        public List<int> GetForgeBuilds(string minecraftVersion)
        {
            return forgeVersions.Where(f => f.MinecraftVersion.Equals(minecraftVersion)).Select(f => f.Build).ToList();
        }

        public string GetDownloadUrl(int build)
        {
            return forgeVersions.Single(f => f.Build == build).DownloadUrl;
        } 

        public List<ForgeVersion> DownloadForgeVersions()
        {
            Forgemaven mavenunjsonend;
            HttpClient client = new HttpClient();
            using (Stream s = client.GetStreamAsync("http://files.minecraftforge.net/maven/net/minecraftforge/forge/json").Result)
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                mavenunjsonend = serializer.Deserialize<Forgemaven>(reader);
            }
            int concurrentgone = 0;
            int i = 1;
            while (concurrentgone <= 100)
            {
                if (mavenunjsonend.Number.ContainsKey(i))
                {
                    string mcversion = mavenunjsonend.Number[i].Mcversion;
                    string version = mavenunjsonend.Number[i].Version;
                    string branch = mavenunjsonend.Number[i].Branch;
                    string downloadUrl = null;
                    downloadUrl = string.Format("{0}/{1}-{2}{3}/forge-{1}-{2}{3}-", mavenunjsonend.Webpath, mcversion, version, string.IsNullOrWhiteSpace(branch) ? "" : "-" + branch);
                    if (i < 183)
                    {
                        downloadUrl += "client.";
                    }
                    else
                    {
                        downloadUrl += "universal.";
                    }
                    if (i < 752)
                    {
                        downloadUrl += "zip";
                    }
                    else
                    {
                        downloadUrl += "jar";
                    }

                    forgeVersions.Add(new ForgeVersion
                    {
                        Build = mavenunjsonend.Number[i].Build,
                        DownloadUrl = downloadUrl,
                        MinecraftVersion = mcversion
                    });

                    concurrentgone = 0;
                }
                else
                {
                    concurrentgone += 1;
                }
                i++;

            }

            SaveForgeVersions(forgeVersions);

            return this.forgeVersions;
        }

        public void Dispose()
        {
            SaveForgeVersions();
        }
    }
}
