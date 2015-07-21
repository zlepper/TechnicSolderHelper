using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
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
        public void ConfigHandler_Save_FileNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ConfigHandler handler = new ConfigHandler(fileSystem);

            handler.Configs.InputDirectory = "Some value";
        }

        [Test]
        public void ConfigHandler_Get_FileNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ConfigHandler handler = new ConfigHandler(fileSystem);

            Assert.IsTrue(string.IsNullOrWhiteSpace(handler.Configs.InputDirectory));
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
