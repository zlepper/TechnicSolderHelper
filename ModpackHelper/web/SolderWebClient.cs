using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using ModpackHelper.Shared.Mods;
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
            var request = new RestRequest("modpack/create", Method.POST);
            request.AddParameter("name", modpackname);
            request.AddParameter("slug", slug);
            client.Execute(request);
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
            //request.AddParameter("add-md5", md5);
            request.MakeAjaxRequestType();
            var res = client.Execute(request);
            Debug.WriteLine(res);
        }

        public void RehashModVersion(string modversionId, string md5)
        {
            throw new NotImplementedException();
        }
    }
}
