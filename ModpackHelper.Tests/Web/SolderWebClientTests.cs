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
                .Returns("{\"id\":\"1\",\"name\":\"test\",\"pretty_name\":\"test\",\"author\":\"\",\"description\":\"\",\"link\":\"\",\"donate\":\"\",\"versions\":[\"1.0.0\"]}");
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(responseMock.Object);
            var modMock = new Mock<Mcmod>();
            modMock.Setup(m => m.GetSafeModId()).Returns("testmod");
            modMock.Setup(m => m.GetOnlineVersion()).Returns("1.7.10-1.2.3");
            SolderWebClient sw = new SolderWebClient("http://solder.zlepper.dk", clientMock.Object);

            var result = sw.IsModversionOnline(modMock.Object);

            Assert.That(result, Is.EqualTo(true));
        }
    }
}
