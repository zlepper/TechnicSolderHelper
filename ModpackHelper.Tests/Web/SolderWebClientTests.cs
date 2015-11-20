using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Web;
using NUnit.Framework;
using RestSharp;

namespace ModpackHelper.Tests.Web
{
    [TestFixture]
    public class SolderWebClientTests
    {
        [Test]
        public void Login_Debug()
        {
            ISolderWebClient solderWebClient = new SolderWebClient("http://solder.zlepper.dk");
            
            solderWebClient.Login("hansen13579@outlook.com", "password");            
        }

        [Test]
        public void CanCreateModOnSolder()
        {
            ISolderWebClient wc = new SolderWebClient("http://solder.zlepper.dk");
            Mcmod mod = new Mcmod()
            {
                Name = Guid.NewGuid().ToString(),
                Version = "1.1.1",
                Modid = Guid.NewGuid().ToString(),
                Mcversion = "1.7.10",
                Authors = new List<string>() { "zlepper" },
                Description = "Total random test mod"
            };

            wc.Login("hansen13579@outlook.com", "password");
            wc.AddMod(mod);

            
        }
    }
}
