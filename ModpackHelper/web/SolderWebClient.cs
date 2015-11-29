using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Web.Solder.Responses;
using Newtonsoft.Json;
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

    public class SolderWebClient : ISolderWebClient
    {
        private readonly Uri baseUrl;
        private readonly IRestClient client;
        private readonly CookieContainer cookieContainer;


        public SolderWebClient(string baseUrl, IRestClient c = null)
        {
            client = c ?? new RestClient(baseUrl);
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
            var request = new RestRequest("modpack/create", Method.POST);
            request.AddParameter("name", modpackname);
            request.AddParameter("slug", slug);
            var res = client.Execute(request);
            
        }

        public void AddMod(Mcmod mod)
        {
            var request = new RestRequest("mod/create", Method.POST);
            request.AddParameter("pretty_name", mod.Name);
            request.AddParameter("name", mod.GetSafeModId());
            request.AddParameter("author", string.Join(", ", mod.GetAuthors()));
            request.AddParameter("description", mod.Description);
            request.AddParameter("link", mod.Url);
            var res = client.Execute(request);
            Debug.WriteLine(res);
        }

        public void AddModVersion(string modId, string md5, string version)
        {
            var request = new RestRequest("mod/add-version", Method.POST);
            request.AddParameter("mod-id", modId);
            request.AddParameter("add-version", version);
            request.AddParameter("md5", md5);
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

        public string GetModId(Mcmod mod)
        {
            var request = new RestRequest("api/mod/{modname}");
            request.AddParameter("modname", mod.GetSafeModId(), ParameterType.UrlSegment);
            var res = client.Execute(request);
            Mod m = JsonConvert.DeserializeObject<Mod>(res.Content);
            return !string.IsNullOrWhiteSpace(m.Id) ? m.Id : null;
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

        public string GetModpackId(string modpackName)
        {
            var request = new RestRequest("api/");
        }
    }
}