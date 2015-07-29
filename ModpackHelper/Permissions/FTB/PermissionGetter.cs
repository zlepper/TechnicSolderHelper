using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Permissions.FTB
{
    public class PermissionGetter
    {
        public static readonly string PermissionsFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SolderHelper",
                "permissions.json");

        private readonly IFileSystem fileSystem;
        private List<Permission> permissions;

        public PermissionGetter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            Load();
        }

        public PermissionGetter()
            : this(new FileSystem())
        {
        }

        public string GetShortName(string modId)
        {
            Permission permission = permissions.SingleOrDefault(p => p.modids.Contains(modId));
            return permission == null ? "" : permission.shortName;
        }

        public Permission GetPermissionFromShortname(string shortname)
        {
            return permissions.SingleOrDefault(p => p.shortName.Equals(shortname));
        }

        public Permission GetPermissionFromModId(string modId)
        {
            return permissions.SingleOrDefault(p => p.modids.Contains(modId));
        }

        public PermissionPolicy FindPermissionPolicy(string toCheck, bool isPublic)
        {
            Permission perm = permissions.FirstOrDefault(p => p.modids.Contains(toCheck) || p.shortName.Equals(toCheck));
            if(perm == null) return PermissionPolicy.Unknown;
            return isPublic ? perm.publicPolicy : perm.privatePolicy;
        }

        private void Load()
        {
            if (fileSystem.File.Exists(PermissionsFile))
                using (Stream s = fileSystem.File.OpenRead(PermissionsFile))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    permissions = serializer.Deserialize<List<Permission>>(reader);
                }
            else
                LoadOnlinePermissions();
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(permissions);
            fileSystem.FileInfo.FromFileName(PermissionsFile).Directory.Create();
            fileSystem.File.WriteAllText(PermissionsFile, json);
        }

        public void LoadOnlinePermissions()
        {
            HttpClient client = new HttpClient();
            using (Stream s = client.GetStreamAsync("http://www.feed-the-beast.com/mods/json").Result)
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                permissions = serializer.Deserialize<List<Permission>>(reader);
            }
            this.permissions = new List<Permission>();
            foreach (Permission p in permissions.Where(p => !String.IsNullOrWhiteSpace(p.privateStringPolicy) &&
                                                            !String.IsNullOrWhiteSpace(p.publicStringPolicy)))
            {
                PermissionPolicy pp;
                bool r = Enum.TryParse(p.privateStringPolicy, out pp);
                if (r)
                {
                    p.privatePolicy = pp;
                }
                r = Enum.TryParse(p.publicStringPolicy, out pp);
                if (r) p.publicPolicy = pp;
                p.privateStringPolicy = null;
                p.publicStringPolicy = null;
            }
            Save();
        }
    }
}
