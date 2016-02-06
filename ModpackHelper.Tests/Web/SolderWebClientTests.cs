using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Web;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace ModpackHelper.Tests.Web
{
    [TestFixture]
    public class SolderWebClientTests
    {
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

        [Test]
        public void IsModversionOnline_ReturnFalse_IfModVersionIsNotOnline()
        {
            var responseMock = new Mock<IRestResponse>();
            responseMock.Setup(r => r.Content)
                .Returns("{error: \"No mod requested/Mod does not exist/Mod version does not exist\"}");
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(responseMock.Object);
            var modMock = new Mock<Mcmod>();
            modMock.Setup(m => m.GetSafeModId()).Returns("testmod");
            modMock.Setup(m => m.GetOnlineVersion()).Returns("1.7.10-1.2.3");
            SolderWebClient sw = new SolderWebClient("http://solder.zlepper.dk", clientMock.Object);

            var result = sw.IsModversionOnline(modMock.Object);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void IsModversionOnline_ReturnsTrue_IfModversionIsOnline()
        {
            var responseMock = new Mock<IRestResponse>();
            responseMock.Setup(r => r.Content)
                .Returns("{\"md5\":\"a5eb911f1b42d956ec5bc98895b28fe9\",\"filesize\":null,\"url\":\"repo.zlepper.dk\\/mods\\/test\\/test-1.0.0.zip\"}");
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(responseMock.Object);
            var modMock = new Mock<Mcmod>();
            modMock.Setup(m => m.GetSafeModId()).Returns("test");
            modMock.Setup(m => m.GetOnlineVersion()).Returns("1.0.0");
            SolderWebClient sw = new SolderWebClient("http://solder.zlepper.dk", clientMock.Object);

            var result = sw.IsModversionOnline(modMock.Object);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void LoginReturnsTrueIfSuccessful()
        {
            ISolderWebClient wc = new SolderWebClient("http://solder.zlepper.dk");
            
            bool response = wc.Login("hansen13579@outlook.com", "password");

            Assert.That(response, Is.True);
        }
    
        [Test]
        public void LoginReturnsFalseIfFailed()
        {
            ISolderWebClient wc = new SolderWebClient("http://solder.zlepper.dk");

            bool response = wc.Login("hansen13579@outlook.com", "password2");

            Assert.That(response, Is.False);
        }

        [TestCase("zlepper.dk")]
        [TestCase("solder.zlepper.dk")]
        [TestCase("something.solder.zlepper.dk")]
        public void HttpIsPrependedIfUserDidntProvideIt(string url)
        {
            SolderWebClient wc = new SolderWebClient(url);

            Assert.That(wc.baseUrl.AbsoluteUri.StartsWith("http://"), Is.True);
        }
        
        [Test]
        public void HttpIsNotPrependedIfUserProvidedIt([Values("zlepper.dk/", "solder.zlepper.dk/")] string url, [Values("http://", "https://")] string start)
        {
            string s = start + url;
            
            SolderWebClient wc = new SolderWebClient(s);

            Assert.That(wc.baseUrl.AbsoluteUri, Is.EqualTo(s));
        }
    }
}
