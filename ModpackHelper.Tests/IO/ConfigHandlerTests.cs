using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.IO;
using NUnit.Framework;

namespace ModpackHelper.Tests.IO
{
    class ConfigHandlerTests
    {
        private readonly string configJsonFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SolderHelper", "settings.json");
        [Test]
        public void ConfigHandler_Save_SetNonExistirngProperty()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {configJsonFilePath, new MockFileData("{}")}
            });
            ConfigHandler handler = new ConfigHandler(fileSystem);
            string inputValue = "TestValue";
            string result = handler.SetProperty("TestKey", inputValue).ToString();

            Assert.AreEqual(inputValue, result);
        }

        [Test]
        public void ConfigHandler_Save_SetExistringProperty()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {configJsonFilePath, new MockFileData("{\"TestKey\":\"TestValue\"}")}
            });
            ConfigHandler handler = new ConfigHandler(fileSystem);
            string inputValue = "NEw test value";

            string result = handler.SetProperty("TestKey", inputValue).ToString();

            Assert.AreEqual(result, inputValue);
        }

        [Test]
        public void ConfigHandler_Get_GetExistingProperty()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {configJsonFilePath, new MockFileData("{\"TestKey\":\"TestValue\"}")}
            });
            ConfigHandler handler = new ConfigHandler(fileSystem);

            string result = handler.GetProperty("TestKey").ToString();

            Assert.AreEqual("TestValue", result);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ConfigHandler_Get_GetNonExistringPropertyShouldThrowExceptionIndexOutOfRange()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {configJsonFilePath, new MockFileData("{\"TestKey\":\"TestValue\"}")}
            });
            ConfigHandler handler = new ConfigHandler(fileSystem);
            handler.GetProperty("Someotherkey");
        }

        [Test]
        public void ConfigHandler_Save_FileNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ConfigHandler handler = new ConfigHandler(fileSystem);

            handler.SetProperty("Somekey", "Some value");
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ConfigHandler_Get_FileNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ConfigHandler handler = new ConfigHandler(fileSystem);

            handler.GetProperty("SomeKey");
        }

        [Test]
        public void ConfigHandler_initalize_NormallyAndDispose()
        {
            using (ConfigHandler handler = new ConfigHandler())
            {
                Assert.NotNull(handler);
            }
        }
    }
}
