using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Web.Solder.Responses;
using Newtonsoft.Json;
using ModpackHelper.Shared.Utils.Config;
using RestSharp;
using RestSharp.Authenticators;
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
        private readonly Uri baseUrl;
        private readonly IRestClient client;
        private readonly CookieContainer cookieContainer;


        public SolderWebClient(string baseUrl, IRestClient c = null)
        {
            if (!baseUrl.StartsWith("http://") || !baseUrl.StartsWith("https://"))
            {
                baseUrl = "http://" + baseUrl;
            }
            client = c ?? new RestClient(baseUrl);
            cookieContainer = new CookieContainer();
            client.CookieContainer = cookieContainer;
            this.baseUrl = new Uri(baseUrl);
        }

        public void Login(string email, string password)
        {
            //client.Authenticator = new SimpleAuthenticator("email", email, "password", password);
            var request = new RestRequest("login", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            var res = client.Execute(request);
            Debug.WriteLine(res);
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
            return !string.IsNullOrWhiteSpace(mv.Id);
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
            var request = new RestRequest("api/modpack/{modpack}/{build}");
            request.AddParameter("modpack", modpack.GetSlug(), ParameterType.UrlSegment);
            request.AddParameter("build", modpack.Version, ParameterType.UrlSegment);
            var res = client.Execute<Mods.Solder.ModpackWithBuild>(request);
            return res.Data.id;
        }

        public string GetModpackId(string slug)
        {
            var request = new RestRequest("api/modpack/{modpack}");
            request.AddParameter("modpack", slug, ParameterType.UrlSegment);
            var res = client.Execute<Mods.Solder.ModpackWithoutBuild>(request);
            try
            {
                return res.Data.id;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public string GetModId(Mcmod mod)
        {
            var request = new RestRequest("api/mod/{mod}");
            request.AddParameter("mod", mod.GetSafeModId(), ParameterType.UrlSegment);
            var res = client.Execute<Mods.Solder.ModpackWithBuild.Mod>(request);
            try
            {
                return res.Data.id;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public void AddBuildToModpack(Mcmod mod, string modpackbuildid)
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
