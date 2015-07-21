using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.web
{
    public class SolderWebClient : CookieAwareWebClient
    {
        public Uri baseUrl;
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
        /// See this: http://stackoverflow.com/questions/17183703/ for mrroe
        /// </summary>
        /// <param name="loginPageAddress">The adress to send the login data to</param>
        /// <param name="loginData">The login data, formattet like this: "email": "email", "password": "somethingelse"</param>
        public new void Login(string loginPageAddress, NameValueCollection loginData)
        {
            Uri loginUrl = new Uri(baseUrl, "login");
            base.Login(loginUrl.AbsoluteUri, loginData);
        }
    }
}
