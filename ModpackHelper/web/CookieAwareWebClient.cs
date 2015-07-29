using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace ModpackHelper.Shared.Web
{
    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        { }

        /// <summary>
        /// Logs into a service
        /// See this: http://stackoverflow.com/questions/17183703/ for more
        /// </summary>
        /// <param name="loginPageAddress">The adress to send the login data to</param>
        /// <param name="loginData">The login data, formattet like this: "email": "something", "password": "somethingelse"</param>
        protected void Login(string loginPageAddress, NameValueCollection loginData)
        {
            /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = Encoding.ASCII.GetBytes(loginData.ToString());
            request.ContentLength = buffer.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            CookieContainer container = request.CookieContainer = new CookieContainer();

            WebResponse response = request.GetResponse();
            response.Close();
            CookieContainer = container;*/
            byte[] responbytes = UploadValues(loginPageAddress, "POST", loginData);
            string responseBody = Encoding.UTF8.GetString(responbytes);
            Debug.WriteLine(responseBody);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            return request;
        }
    }
}
