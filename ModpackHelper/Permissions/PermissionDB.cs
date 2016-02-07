using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Permissions.FTB;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Permissions
{
    public class PermissionDB : IDisposable
    {
        IFileSystem fileSystem;
        private PermissionGetter pg;
        private string dbFilePath;
        public List<UserPermission> UserPermissions; 

        public PermissionDB() : this(new FileSystem())
        {
            
        }

        public PermissionDB(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            pg = new PermissionGetter(fileSystem);
            pg.LoadOnlinePermissions();
            dbFilePath = fileSystem.Path.Combine(Constants.ApplicationDataPath, "UserPermissions.json");
            LoadPermissions();
        }

        private void LoadPermissions()
        {
            try
            {
                using (Stream s = fileSystem.File.OpenRead(dbFilePath))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    UserPermissions = serializer.Deserialize<List<UserPermission>>(reader);
                }
            }
            catch (FileNotFoundException)
            {
                UserPermissions = new List<UserPermission>();
            }
        }

        /// <summary>
        /// Checks if the mod is allowed for destribution
        /// </summary>
        /// <param name="mod">The mod to check</param>
        /// <param name="isPublic">Indicates if it's public or private destribution</param>
        /// <param name="isFTB">Indicates if it's destributed through FTB (Feed the beast)</param>
        /// <returns></returns>
        public PermissionPolicy CanModBeDestributed(Mcmod mod, bool isPublic, bool isFTB)
        {
            var permission = pg.FindPermissionPolicy(mod.Modid, isPublic);
            switch (permission)
            {
                case PermissionPolicy.Open:
                    return PermissionPolicy.Open;
                case PermissionPolicy.Notify:
                    return PermissionPolicy.Notify;
                case PermissionPolicy.Request:
                    return PermissionPolicy.Request;
                case PermissionPolicy.Unknown:
                    return PermissionPolicy.Unknown;
                case PermissionPolicy.FTB:
                    return isFTB ? PermissionPolicy.Open : PermissionPolicy.Request;
                case PermissionPolicy.Closed:
                    return PermissionPolicy.Closed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Dispose()
        {
            SavePermissions();
        }

        public void SavePermissions()
        {
            while (true)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(UserPermissions);
                    fileSystem.FileInfo.FromFileName(dbFilePath).Directory.Create();
                    fileSystem.File.WriteAllText(dbFilePath, json);
                    break;
                }
                catch (Exception e)
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}
