using System.Collections.Generic;
using ModpackHelper.Shared.Mods;
using NUnit.Framework;

namespace ModpackHelper.Tests.Mods
{
    [TestFixture]
    public class ModTests
    {
        [Test]
        public void Litemod_GetLitemod_ShouldReturnObject()
        {
            string json =
                "{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}";


            Litemod litemod = Litemod.GetLitemod(json);

            Assert.IsNotNull(litemod);
        }

        [TestCase("{[[[]})")]
        [TestCase("[{'modid':'appliedenergistics2','name':'Applied Energistics 2','description':'A Mod about Matter, Energy and using them to conquer the world..','version':'rv2-beta-18','mcversion':'1.7.10','url':'http://ae2.ae-mod.info','updateUrl':'','authorList':['AlgorithmX2'],'credits':'AlgorithmX2','logoFile':'','screenshots':[]}]")]
        [TestCase("{\"modListVersion\": 2,\"modList\": [{\"modid\": \"bspkrsCore\",\"name\": \"bspkrsCore\",\"description\": \"Shared classes used in the mods maintained by bspkrs.\",\"version\": \"6.15\",\"mcversion\": \"1.7.10\",\"url\": \"http://www.minecraftforum.net/topic/1114612-/\",\"updateUrl\": \"\",\"authorList\": [ \"DaftPVF\", \"bspkrs\" ],\"credits\": \"Much of this code was originally part of DaftPVF's mods that I maintain.\",\"logoFile\": \"\",\"screenshots\": [],\"parent\": \"\",\"requiredMods\": [\"Forge@[10.13.2.1230,)\"],\"dependencies\": [\"Forge@[10.13.2.1230,)\"],\"dependants\": [],\"useDependencyInformation\": \"true\"}]}")]
        public void Litemod_GetLitemod_ShouldReturnNull(string json)
        {
            Litemod litemod = Litemod.GetLitemod(json);

            Assert.IsNull(litemod);
        }

        [Test]
        public void Litemod_ToMcmod_ShouldConvertSuccessfully()
        {
            // Example litemod
            string litemodJson = "{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"version\":\"4\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}";
            Litemod litemod = Litemod.GetLitemod(litemodJson);
            Mcmod mod = new Mcmod()
            {
                Name = "macros",
                Modid = "macros",
                Mcversion = "1.7.10",
                Version = "4-1012",
                Description = "Desc",
                Authors = new List<string>()
                {
                    "Mumfrey"
                }
            };

            Mcmod convertedMcmod = litemod.ToMcmod();
            bool eq = convertedMcmod.Equals(mod);
            
            Assert.IsTrue(eq);
        }

        [Test]
        public void Mcmod_GetMcmod_ShouldGetAnObjectFromJson()
        {
            // Example mcmod
            string mcmodJson =
                "[{'modid':'appliedenergistics2','name':'Applied Energistics 2','description':'A Mod about Matter, Energy and using them to conquer the world..','version':'rv2-beta-18','mcversion':'1.7.10','url':'http://ae2.ae-mod.info','updateUrl':'','authorList':['AlgorithmX2'],'credits':'AlgorithmX2','logoFile':'','screenshots':[]}]";

            Mcmod mod = Mcmod.GetMcmod(mcmodJson);

            Assert.IsNotNull(mod);
        }

        [TestCase("{{{{{")]
        [TestCase("{\"modListVersion\": 2,\"modList\": [{\"modid\": \"bspkrsCore\",\"name\": \"bspkrsCore\",\"description\": \"Shared classes used in the mods maintained by bspkrs.\",\"version\": \"6.15\",\"mcversion\": \"1.7.10\",\"url\": \"http://www.minecraftforum.net/topic/1114612-/\",\"updateUrl\": \"\",\"authorList\": [ \"DaftPVF\", \"bspkrs\" ],\"credits\": \"Much of this code was originally part of DaftPVF's mods that I maintain.\",\"logoFile\": \"\",\"screenshots\": [],\"parent\": \"\",\"requiredMods\": [\"Forge@[10.13.2.1230,)\"],\"dependencies\": [\"Forge@[10.13.2.1230,)\"],\"dependants\": [],\"useDependencyInformation\": \"true\"}]}")]
        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        public void Mcmod_GetMcmod_ShouldReturnNull(string json)
        {
            Mcmod mod = Mcmod.GetMcmod(json);

            Assert.IsNull(mod);
        }

        [Test]
        public void Mcmod2_GetMcmod2_ShouldReturnAnObject()
        {
            string json =
                "{\"modListVersion\": 2,\"modList\": [{\"modid\": \"bspkrsCore\",\"name\": \"bspkrsCore\",\"description\": \"Shared classes used in the mods maintained by bspkrs.\",\"version\": \"6.15\",\"mcversion\": \"1.7.10\",\"url\": \"http://www.minecraftforum.net/topic/1114612-/\",\"updateUrl\": \"\",\"authorList\": [ \"DaftPVF\", \"bspkrs\" ],\"credits\": \"Much of this code was originally part of DaftPVF's mods that I maintain.\",\"logoFile\": \"\",\"screenshots\": [],\"parent\": \"\",\"requiredMods\": [\"Forge@[10.13.2.1230,)\"],\"dependencies\": [\"Forge@[10.13.2.1230,)\"],\"dependants\": [],\"useDependencyInformation\": \"true\"}]}";

            Mcmod2 mod = Mcmod2.GetMcmod2(json);

            Assert.IsNotNull(mod);
        }

        [TestCase("{{{{{")]
        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        [TestCase("[{'modid':'appliedenergistics2','name':'Applied Energistics 2','description':'A Mod about Matter, Energy and using them to conquer the world..','version':'rv2-beta-18','mcversion':'1.7.10','url':'http://ae2.ae-mod.info','updateUrl':'','authorList':['AlgorithmX2'],'credits':'AlgorithmX2','logoFile':'','screenshots':[]}]")]
        public void Mcmod2_GetMcmod2_ShouldReturnNull(string json)
        {
            Mcmod2 mod = Mcmod2.GetMcmod2(json);

            Assert.IsNull(mod);
        }
    }
}
