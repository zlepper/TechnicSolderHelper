using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using RestSharp;
using RestSharp.Authenticators;

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

    public class SolderWebClient :  ISolderWebClient
    {
        private readonly Uri baseUrl;
        private readonly IRestClient client;
        private readonly CookieContainer cookieContainer;
        

        public SolderWebClient(string baseUrl, IRestClient c = null)
        {
            if (c == null)
            {
                client = new RestClient(baseUrl);
            }
            cookieContainer = new CookieContainer();
            client.CookieContainer = cookieContainer;
            this.baseUrl = new Uri(baseUrl);
        }

        public void Login(string email, string password)
        {
            client.Authenticator = new SimpleAuthenticator("email", email, "password", password);
            var request = new RestRequest("login", Method.POST);
            client.Execute(request);
        }

        public void CreatePack(string modpackname, string slug)
        {
            // Only create the pack if it's not already there. 
            try
            {
                string id = GetModpackId(slug);
            }
            catch (Exception)
            {
                var request = new RestRequest("modpack/create", Method.POST);
                request.AddParameter("name", modpackname);
                request.AddParameter("slug", slug);
                var res = client.Execute(request);
            }
            
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
            throw new NotImplementedException();
        }

        public string CreateBuild(Modpack modpack)
        {
            string id = GetModpackId(modpack.GetSlug());
            var request = new RestRequest("mods/add-build/{id}", Method.POST);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddParameter("version", modpack.Version);
            request.AddParameter("minecraft", modpack.MinecraftVersion);
            request.AddParameter("java-version", modpack.MinJava);
            request.AddParameter("memory-enabled", !string.IsNullOrWhiteSpace(modpack.MinMemory));
            request.AddParameter("memory", modpack.MinMemory);
            var res = client.Execute(request);
            Debug.WriteLine(res);

            // Return the build id because the response redirects
            return GetBuildId(modpack);
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
            var request = new RestRequest("api/{modpack}");
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
            var request = new RestRequest("modpack/build/modify/add", Method.POST);
            request.MakeAjaxRequestType();
            request.AddParameter("build", modpackbuildid);
            request.AddParameter("mod-name", mod.Name);
            request.AddParameter("mod-version", mod.Version);
            var res = client.Execute<AddBuildToModpackResponse>(request);
            if(!res.Data.Status.Equals("success")) throw new Exception("Something went wrong when adding a mod to a build");
        }

        private class AddBuildToModpackResponse
        {
            public string Status { get; set; }
        }
    }
}
