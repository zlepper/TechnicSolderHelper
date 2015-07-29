using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace ModpackHelper.Shared.Web
{
    public class SolderWebClient : CookieAwareWebClient, ISolderWebClient
    {
        private readonly Uri baseUrl;
        public SolderWebClient(string baseUrl):base()
        {
            this.baseUrl = new Uri(baseUrl);
        }

        public SolderWebClient(string baseUrl, CookieContainer cc) : base(cc)
        {
            this.baseUrl = new Uri(baseUrl);
        }

        /// <summary>
        /// Logs into solder
        /// See this: http://stackoverflow.com/questions/17183703/ for more
        /// </summary>
        public void Login(string email, string password)
        {
            Uri loginUrl = new Uri(baseUrl, "login");
            NameValueCollection loginData = new NameValueCollection
            {
              { "email", email },
              { "password", password }
            };
            Login(loginUrl.AbsoluteUri, loginData);
        }

        public void CreatePack(string modpackname, string slug)
        {
            Uri createModpackUri = new Uri(baseUrl, "modpack/create");
            NameValueCollection modpackData = new NameValueCollection
            {
                {"name", modpackname },
                {"slug", slug }
            };
            byte[] responseBytes = UploadValues(createModpackUri, "POST", modpackData);
            string response = Encoding.UTF8.GetString(responseBytes);
            Debug.WriteLine(response);
        }

        public void AddMod(string modname, string modslug, string authors, string description, string modurl)
        {
            throw new NotImplementedException();
        }

        public void AddModVersion(string modId, string version)
        {
            throw new NotImplementedException();
        }

        public void RehashModVersion(string modversionId)
        {
            throw new NotImplementedException();
        }
    }
}
