using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Runtime.Serialization;
using ModpackHelper.Shared.Mods;
using NUnit.Framework;

namespace ModpackHelper.Tests
{
    [TestFixture]
    class ModExtractorTests
    {
       /* [TestFixtureSetUp]
        public void Init()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"C:\mcmod.json", new MockFileData()}
            });
        }*/

        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"version\":\"4\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        [TestCase("[{'modid':'appliedenergistics2','name':'Applied Energistics 2','description':'A Mod about Matter, Energy and using them to conquer the world..','version':'rv2-beta-18','mcversion':'1.7.10','url':'http://ae2.ae-mod.info','updateUrl':'','authorList':['AlgorithmX2'],'credits':'AlgorithmX2','logoFile':'','screenshots':[]}]")]
        public void ModExtractor_GetMcmodDataFromFile_ProperJson(string json)
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"C:\mcmod.json", new MockFileData(json)}
            });

            ModExtractor modExtractor = new ModExtractor("1.7.10", fileSystem);

            Mcmod mod = modExtractor.GetMcmodDataFromFile(@"C:\mcmod.json");

            Assert.IsNotNull(mod);
        }

        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        [TestCase("{\"name\":\"macros\",\"mcversion\":\"1.7.10\",\"version\":\"4\",\"revision\":\"1012\",\"author\":\"Mumfrey\",\"description\":\"Desc\",\"classTransformerClasses\":\"net.eq2online.macros.transformers.CollectionPacketTransformer\"}")]
        [TestCase("[{'modid':'appliedenergistics2','name':'Applied Energistics 2','description':'A Mod about Matter, Energy and using them to conquer the world..','version':'rv2-beta-18','mcversion':'1.7.10','url':'http://ae2.ae-mod.info','updateUrl':'','authorList':['AlgorithmX2'],'credits':'AlgorithmX2','logoFile':'','screenshots':[]}]")]
        public void ModExtractor_GetMcmodDataFromJson_ProperJson(string json)
        {
            Mcmod mod = ModExtractor.GetMcmodDataFromJson(json);

            Assert.IsNotNull(mod);
        }


        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void ModExtractor_GetMcmodDataFromFile_ShouldThrowSerializationException()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"C:\mcmod.json", new MockFileData("{{{{{")}
            });

            ModExtractor modExtractor = new ModExtractor("1.7.10", fileSystem);

            modExtractor.GetMcmodDataFromFile(@"C:\mcmod.json");
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void ModExtractor_GetMcmodDataFromJson_ShouldThrowSerializationException()
        {
            ModExtractor.GetMcmodDataFromJson("{{{{");
        }
    }
}
