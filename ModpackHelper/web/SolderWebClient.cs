using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Web.Solder.Crawlers;
using ModpackHelper.Shared.Web.Solder.Responses;
using Newtonsoft.Json;
using RestSharp;
using Modpack = ModpackHelper.Shared.Utils.Config.Modpack;

namespace ModpackHelper.Shared.Web
{
    public static class IRestRequestExtension
    {
        public static void MakeAjaxRequestType(this IRestRequest restRequest)
        {
            // Get around stupid ajax checks in solder
            restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
        }
    }

    public class SolderWebClient : ISolderWebClient
    {
        private Dictionary<string, string> ModIdCache = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<string, string>> ModversionIdCache = new Dictionary<string, Dictionary<string, string>>();  


        public readonly Uri baseUrl;
        public readonly IRestClient client;
        private readonly CookieContainer cookieContainer;


        public SolderWebClient(string baseUrl, IRestClient c = null)
        {
            if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
            {
                baseUrl = "http://" + baseUrl;
            }
            client = c ?? new RestClient(baseUrl);
            cookieContainer = new CookieContainer();
            client.CookieContainer = cookieContainer;
            this.baseUrl = new Uri(baseUrl);
        }

        public bool Login(string email, string password)
        {
            var request = new RestRequest("login", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            var res = client.Execute(request);
            LoginCrawler crawler = new LoginCrawler {HTML = res.Content};
            return crawler.Crawl();
        }

        public string CreatePack(string modpackname, string slug)
        {
            // Only create the pack if it's not already there. 
            try
            {
                string id = GetModpackId(slug);
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception)
            {
                var request = new RestRequest("modpack/create", Method.POST);
                request.AddParameter("name", modpackname);
                request.AddParameter("slug", slug);
                request.AddParameter("hidden", false);
                var res = client.Execute(request);
                Debug.WriteLine(res);
                string id = res.ResponseUri.Segments.Last();
                int n;
                bool convert = int.TryParse(id, out n);
                if (convert)
                {
                    return id;
                }
                else
                {
                    Debug.WriteLine(res);
                }
            }
            return GetModpackId(slug);
        }

        public string AddMod(Mcmod mod)
        {
            var request = new RestRequest("mod/create", Method.POST);
            request.AddParameter("pretty_name", mod.Name);
            request.AddParameter("name", mod.GetSafeModId());
            request.AddParameter("author", string.Join(", ", mod.GetAuthors()));
            request.AddParameter("description", mod.Description);
            request.AddParameter("link", mod.Url);
            var res = client.Execute(request);
            Debug.WriteLine(res);
            return GetModId(mod);
        }

        public void AddModVersion(string modId, string md5, string version)
        {
            var request = new RestRequest("mod/add-version", Method.POST);
            request.AddParameter("mod-id", modId);
            request.AddParameter("add-version", version);
            request.AddParameter("add-md5", md5);
            request.MakeAjaxRequestType();
            var res = client.Execute(request);
            Debug.WriteLine(res);
        }

        public void RehashModVersion(string modversionId, string md5)
        {
            var request = new RestRequest("mod/rehash", Method.POST);
            request.AddParameter("version-id", modversionId);
            request.AddParameter("md5", md5);
            request.MakeAjaxRequestType();
            var res = client.Execute(request);
        }
        

        public bool IsModversionOnline(Mcmod mod)
        {
            var request = new RestRequest("api/mod/{modname}/{modversion}");
            request.AddParameter("modname", mod.GetSafeModId(), ParameterType.UrlSegment);
            request.AddParameter("modversion", mod.GetOnlineVersion(), ParameterType.UrlSegment);
            var res = client.Execute(request);
            ModVersion mv = JsonConvert.DeserializeObject<ModVersion>(res.Content);
            return !string.IsNullOrWhiteSpace(mv.Md5);
        }

        private Dictionary<string, Build> buildCache = new Dictionary<string, Build>(); 

        private Build GetBuild(string buildid)
        {
            if (buildCache.ContainsKey(buildid))
            {
                return buildCache[buildid];
            }

            var request = new RestRequest("modpack/build/{build}");
            request.AddParameter("build", buildid, ParameterType.UrlSegment);
            var res = client.Execute(request);
            var bc = new BuildCrawler() { HTML = res.Content };
            var build = bc.Crawl();

            buildCache.Add(buildid, build);
            return build;
        }

        public bool IsModInBuild(Mcmod mod, string buildid)
        {
            var build = GetBuild(buildid);
            var safeModId = mod.GetSafeModId();
            return build.Mods.Any(m => m.Name.Equals(safeModId));
        }

        public bool IsModversionActiveInBuild(Mcmod mod, string buildid)
        {
            var build = GetBuild(buildid);
            var safeModId = mod.GetSafeModId();
            var onlineVersion = mod.GetOnlineVersion();
            return build.Mods.Any(m => m.Name.Equals(safeModId) && m.Active.Equals(onlineVersion));
        }

        public string GetActiveModversionInBuildId(Mcmod mod, string buildid)
        {
            var request = new RestRequest("modpack/build/{build}");
            request.AddParameter("build", buildid, ParameterType.UrlSegment);
            var res = client.Execute(request);
            var bc = new BuildCrawler() { HTML = res.Content };
            var build = bc.Crawl();
            string version = build.Mods.FirstOrDefault(m => m.Name.Equals(mod.GetSafeModId()))?.Active;
            if (string.IsNullOrWhiteSpace(version)) return null;
            return GetModVersionId(mod.GetSafeModId(), version);
        }

        public void SetModversionInBuild(Mcmod mod, string buildid)
        {
            var modVersionId = GetModVersionId(mod);
            var request = new RestRequest("modpack/build/modify");
            request.MakeAjaxRequestType();
            request.AddParameter("action", "version");
            request.AddParameter("build-id", buildid);
            request.AddParameter("version", modVersionId);
            request.AddParameter("modversion-id", GetActiveModversionInBuildId(mod, buildid));
            var res = client.Execute(request);
            Debug.WriteLine(res);
        }

        public string GetModVersionId(Mcmod mod)
        {
            return GetModVersionId(mod.GetSafeModId(), mod.GetOnlineVersion());
        }

        public bool IsPackOnline(Modpack modpack)
        {
            return GetModpackId(modpack.GetSlug()) != null;
        }

        public bool IsBuildOnline(Modpack modpack)
        {
            return GetBuildId(modpack) != null;
        }
        
        private string GetModVersionId(string moid, string modversion)
        {
            if (ModversionIdCache.ContainsKey(moid))
            {
                if (ModversionIdCache[moid].ContainsKey(modversion))
                {
                    return ModversionIdCache[moid][modversion];
                }
            }

            var modid = GetModId(moid);
            var request = new RestRequest("mod/view/{modid}");
            request.AddParameter("modid", modid, ParameterType.UrlSegment);
            var res = client.Execute(request);
            var mvc = new ModVersionCrawler(res.Content);
            var modVersions = mvc.Crawl();
            string id = modVersions.FirstOrDefault(m => m.Version.Equals(modversion))?.Id;

            if (!string.IsNullOrWhiteSpace(id))
            {
                if (!ModversionIdCache.ContainsKey(moid))
                {
                    ModversionIdCache.Add(moid, new Dictionary<string, string>());
                }
                ModversionIdCache[moid].Add(modversion, id);
            }
            return id;

        }

        public string CreateBuild(Modpack modpack, string id)
        {
            var request = new RestRequest("modpack/add-build/{id}", Method.POST);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddParameter("version", modpack.Version);
            request.AddParameter("minecraft", modpack.MinecraftVersion);
            request.AddParameter("java-version", modpack.MinJava);
            request.AddParameter("memory-enabled", !string.IsNullOrWhiteSpace(modpack.MinMemory));
            request.AddParameter("memory", modpack.MinMemory);
            var res = client.Execute(request);
            Debug.WriteLine(res);

            // Return the build id because the response redirects
            return res.ResponseUri.Segments.Last();
        }

        public string CreateBuild(Modpack modpack)
        {
            string id = GetModpackId(modpack.GetSlug());
            return CreateBuild(modpack, id);
        }

        public string GetBuildId(Modpack modpack)
        {
            var request = new RestRequest("modpack/view/{id}");
            var modpackId = GetModpackId(modpack.GetSlug());
            request.AddParameter("id", modpackId, ParameterType.UrlSegment);
            var res = client.Execute(request);
            var crawler = new BuildListCrawler {HTML = res.Content};
            var builds = crawler.Crawl();
            var build = builds.SingleOrDefault(b => b.Version.Equals(modpack.Version));
            return build?.Id;
        }

        public string GetModpackId(string slug)
        {
            var request = new RestRequest("modpack/list");
            var res = client.Execute(request);
            ModpackListCrawler crawler = new ModpackListCrawler {HTML = res.Content};
            var modpacks = crawler.Crawl();
            var modpack = modpacks.SingleOrDefault(m => m.Name.Equals(slug));
            return modpack?.Id;
        }

        public string GetModId(Mcmod mod)
        {
            return GetModId(mod.GetSafeModId());
        }

        private string GetModId(string modid)
        {
            if (ModIdCache.ContainsKey(modid))
            {
                return ModIdCache[modid];
            }

            var request = new RestRequest("mod/list");
            var res = client.Execute(request);
            var crawler = new ModlistCrawler(res.Content);
            var mods = crawler.Crawl();
            var mo = mods.SingleOrDefault(m => m.Name.Equals(modid));

            string id = mo?.Id;
            if (!string.IsNullOrWhiteSpace(id))
            {
                ModIdCache.Add(modid, id);
            }
            return id;
        }

        public void AddModversionToBuild(Mcmod mod, string modpackbuildid)
        {
            var request = new RestRequest("modpack/modify/add", Method.POST);
            request.MakeAjaxRequestType();
            request.AddParameter("build", modpackbuildid);
            request.AddParameter("mod-name", mod.GetSolderModName());
            request.AddParameter("mod-version", mod.GetOnlineVersion());
            request.AddParameter("action", "add");
            var res = client.Execute<AddBuildToModpackResponse>(request);
            if(!res.Data.Status.Equals("success")) throw new Exception("Something went wrong when adding a mod to a build");
        }

        private class AddBuildToModpackResponse
        {
            public string Status { get; set; }
        }
    }
}
