using System;
using System.Collections.Specialized;
using System.Net;

namespace ModpackHelper.Shared.web
{
    public class SolderWebClient : CookieAwareWebClient
    {
        public Uri BaseUrl;
        public SolderWebClient(string baseUrl):base()
        {
            this.BaseUrl = new Uri(baseUrl);
        }

        public SolderWebClient(string baseUrl, CookieContainer cc) : base(cc)
        {
            this.BaseUrl = new Uri(baseUrl);
        }

        /// <summary>
        /// Logs into solder
        /// See this: http://stackoverflow.com/questions/17183703/ for mrroe
        /// </summary>
        /// <param name="loginPageAddress">The adress to send the login data to</param>
        /// <param name="loginData">The login data, formattet like this: "email": "email", "password": "somethingelse"</param>
        public new void Login(string loginPageAddress, NameValueCollection loginData)
        {
            Uri loginUrl = new Uri(BaseUrl, "login");
            base.Login(loginUrl.AbsoluteUri, loginData);
        }
    }
}
