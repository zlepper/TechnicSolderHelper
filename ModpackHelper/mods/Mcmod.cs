using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Permissions;
using ModpackHelper.Shared.Permissions.FTB;
using ModpackHelper.Shared.UserInteraction;
using ModpackHelper.Shared.Web.Api;
using ModpackHelper.Utils;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Mods
{
    /// <summary>
    /// Used to descripe a minecraft mod
    /// Also the pattern in normal mcmod.info files
    /// </summary>
    public class Mcmod
    {
        /// <summary>
        /// Used to indicate if this mod should be skipped when packing the mods
        /// </summary>
        public bool IsSkipping;

        /// <summary>
        /// True if the file has been added to solder
        /// </summary>
        public bool IsOnSolder { get; set; }

        public bool Equals(Mcmod other)
        {
            return string.Equals(Modid, other.Modid) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Version, other.Version) &&
                   string.Equals(Mcversion, other.Mcversion) &&
                   string.Equals(Url, other.Url) &&
                   string.Equals(Description, other.Description) &&
                   Lists.AreEqual(AuthorList, other.AuthorList) &&
                   Lists.AreEqual(Authors, other.Authors) &&
                   PublicPerms == other.PublicPerms &&
                   PrivatePerms == other.PrivatePerms &&
                   FromSuggestion == other.FromSuggestion &&
                   string.Equals(Path, other.Path);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Modid != null ? Modid.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Version != null ? Version.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Mcversion != null ? Mcversion.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Url != null ? Url.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AuthorList != null ? AuthorList.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Authors != null ? Authors.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) PublicPerms;
                hashCode = (hashCode*397) ^ (int) PrivatePerms;
                hashCode = (hashCode*397) ^ FromSuggestion.GetHashCode();
                hashCode = (hashCode*397) ^ (Path != null ? Path.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        /// The modid of the mod
        /// </summary>
        public string Modid { get; set; }

        /// <summary>
        /// The name of the mod
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version of the mod
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The Minecraft version of the mod
        /// </summary>
        public string Mcversion { get; set; }

        /// <summary>
        /// The info url of the mod
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// A short description of the mod
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list of the authors of the mod
        /// </summary>
        public List<string> AuthorList { get; set; }

        /// <summary>
        /// A list of the authors of the mod
        /// </summary>
        public List<string> Authors { get; set; }

        /// <summary>
        /// Indicates the permissions needed to distribute the mod
        /// in a public modpack
        /// </summary>
        public PermissionLevel PublicPerms { get; }

        /// <summary>
        /// Indicates the permissions needed to distribute the mod
        /// in a private modpack
        /// </summary>
        public PermissionLevel PrivatePerms { get; }

        /// <summary>
        /// Indicates if this info was fetched from my remote database
        /// and therefor should not be put back into the databasee
        /// TODO write a json api instead of the direct db interaction
        /// TODO Mostly done with the json api
        /// </summary>
        [JsonIgnore]
        public bool FromSuggestion { get; set; }

        /// <summary>
        /// Indicates if the information was entered by the user
        /// rather than read from a .info file or from the webapi
        /// </summary>
        [JsonIgnore]
        public bool FromUser { get; set; }

        /// <summary>
        /// The path of the mod
        /// </summary>
        private string Path { get; set; }

        public void SetPath(FileInfoBase f)
        {
            Path = f.FullName;
        }

        public FileInfoBase GetPath()
        {
            return new FileInfo(Path);
        }

        /// <summary>
        /// The md5 value of the jar of this mod
        /// </summary>
        public string JarMd5 { get; set; }

        public static Mcmod GetMcmod(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Mcmod>>(json)[0];
            }
            catch (JsonSerializationException)
            {
                return null;
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mcmod) obj);
        }

        /// <summary>
        /// Checks if the mod is on the list of mods which has custom support.
        /// </summary>
        /// <param name="modFileName">The mod file name.</param>
        /// <returns>Returns the number of the method to call, if no match is found, returns zero</returns>
        public static int IsSpecialHandledMod(string modFileName)
        {
            string[] skipMods =
                {"CarpentersBlocksCachedResources",
                    "CodeChickenLib",
                    "ejml-",
                    "commons-codec",
                    "commons-compress",
                    "Cleanup"
                };
            if (skipMods.Any(t => modFileName.ToLower().Contains(t.ToLower())))
            {
                return 0;
            }
            string[] modPatterns =
                {
                    @"liteloader"
                };
            for (int i = 0; i < modPatterns.Length; i++)
            {
                if (Regex.IsMatch(modFileName, modPatterns[i], RegexOptions.IgnoreCase))
                {
                    return i + 1;
                }
            }

            return int.MaxValue;
        }

        /// <summary>
        /// Verifies is the current mcmod is fully valid
        /// </summary>
        /// <returns>True is the mod is valid, otherwise false</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Version) ||
                string.IsNullOrWhiteSpace(Mcversion) || string.IsNullOrWhiteSpace(Modid) || Modid.ToLower().Contains("example") || Name.ToLower().Contains("example") || Version.ToLower().Contains("example"))
                return false;
            return !Name.Contains("${") && !Version.Contains("${") && !Mcversion.Contains("${") && !Modid.Contains("${") && !Version.ToLower().Contains("@version@") && AuthorList != null && AuthorList.Count > 0;
        }

        /// <summary>
        /// Gets a list of authors for the mod
        /// </summary>
        /// <returns></returns>
        public List<string> GetAuthors()
        {
            return GetAuthors(new FileSystem());
        }

        /// <summary>
        /// Gets a list of authors for the mod
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <returns></returns>
        public List<string> GetAuthors(IFileSystem fileSystem)
        {
            if(AuthorList != null && AuthorList.Count != 0) return AuthorList;
            if (Authors != null && Authors.Count != 0)
                return AuthorList = Authors;
            // Id the Modid isn't set, then we can't do anymore to attempt to grab info,
            // so we should just return
            if (string.IsNullOrWhiteSpace(Modid))
                return AuthorList = new List<string>();

            // The FTB permission list might have some info we can use
            PermissionGetter pGetter = new PermissionGetter(fileSystem);
            Permission temp = pGetter.GetPermissionFromModId(Modid);

            if (temp != null)
                // They did!
                return AuthorList = temp.modAuthors.Split(',').ToList();
            using (ModsDBContext db = new ModsDBContext(fileSystem))
            {
                return AuthorList = db.GetSuggestedModAuthors(this);
            }
        }

        /// <summary>
        /// Updates this mod with results from the webapi
        /// </summary>
        /// <param name="mod">The webapi mod</param>
        public void UpdateFromApi(Mod mod)
        {
            Name = mod.Name;
            Modid = mod.Modid;
            Mcversion = mod.Mcversion;
            Version = mod.Version;

            // Push the author into the mod information
            if (AuthorList == null)
            {
                AuthorList = new List<string>();
            }
            foreach (Author author in mod.Authors)
            {
                AuthorList.Add(author.Name);
            }
            AuthorList = AuthorList.Distinct().ToList();
            // Make sure this mod doens't get reuploaded to the webapi
            FromSuggestion = true;
        }

        
        public delegate void DoneWithApiEventHandler();

        public event DoneWithApiEventHandler DoneWithApi;

        /// <summary>
        /// Called when the returns and a result is available
        /// </summary>
        protected virtual void OnDoneWithApi()
        {
            DoneWithApi?.Invoke();
        }

        /// <summary>
        /// Queries the webapi for mod information
        /// </summary>
        /// <param name="messageShower">The messageshower to display messages on</param>
        /// <returns></returns>
        public async Task GetModInfoFromApi(IMessageShower messageShower = null)
        {
            // Create a client so we can query the webapi
            using (var client = new HttpClient())
            {
                // set some basic information
                client.BaseAddress = new Uri("http://localhost:58013/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Query the api
                HttpResponseMessage response = await client.GetAsync("api/Mods/" + JarMd5);
                switch (response.StatusCode)
                {
                    // A mod was found
                    case HttpStatusCode.OK:
                        Mod m = await response.Content.ReadAsAsync<Mod>();
                        UpdateFromApi(m);
                        break;
                    // No mod was found, or another error happened
                    default:
                        messageShower?.ShowMessageAsync("The all knowing internetz doesn't contain any info about this mod!");
                        return;
                }
                // Tell our clients that we are done
                OnDoneWithApi();
            }
        }

        /// <summary>
        /// Uploads the mods info to the webapi
        /// </summary>
        public void UploadToApi()
        {
            if (FromUser && !FromSuggestion)
            {
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.DoWork += delegate(object sender, DoWorkEventArgs args)
                    {
                        UploadModInfoToApi().Wait();
                    };
                    bw.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// Uploads the mod to the webapi
        /// </summary>
        /// <returns></returns>
        public async Task UploadModInfoToApi()
        {
            // Create a client so we can query the webapi
            using (var client = new HttpClient())
            {
                // Set some basic information
                client.BaseAddress = new Uri("http://localhost:58013/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Mod mod = Mod.CreateFromMcmod(this);

                // Query the api
                //HttpResponseMessage response = await client.GetAsync("api/Mods/" + JarMd5);
                var response = await client.PostAsJsonAsync("api/Mods", mod);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(Name + " was uploaded to the api.");
                }
            }
        }
    }
}
