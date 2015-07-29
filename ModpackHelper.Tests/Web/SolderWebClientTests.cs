using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Web;
using NUnit.Framework;

namespace ModpackHelper.Tests.Web
{
    [TestFixture]
    public class SolderWebClientTests
    {
        [Test]
        public void Login_Debug()
        {
            SolderWebClient solderWebClient = new SolderWebClient("http://solder.zlepper.dk");
            solderWebClient.Login("hansen13579@gmail.com", "Qut42fzv");
            string reply = solderWebClient.DownloadString("http://solder.zlepper.dk/login");

            Console.WriteLine(reply);
        }
    }
}
