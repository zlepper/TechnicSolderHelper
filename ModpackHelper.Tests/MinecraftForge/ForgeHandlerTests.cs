using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.MinecraftForge;
using ModpackHelper.Shared.MinecraftForge;
using NUnit.Framework;

namespace ModpackHelper.Tests.MinecraftForge
{
    class ForgeHandlerTests
    {
        private readonly string forgeJsonFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SolderHelper", "forgeversions.json");

        [TestCase("{}")]
        [TestCase("{{{{{{")]
        [TestCase("[]")]
        [TestCase("")]
        [TestCase("null")]
        public void ForgeHandler_load_noDataStored(string json)
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData(json)}
            });
            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<ForgeVersion> fv = forgeHandler.LoadForgeVersions();

            Assert.AreEqual(0, fv.Count);
        }

        [Test]
        public void ForgeHandler_load_DataStored()
        {

            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData("[{\"Build\":1,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]")}
            });

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<ForgeVersion> fv = forgeHandler.LoadForgeVersions();

            Assert.AreEqual(1, fv.Count);
        }

        [Test]
        public void ForgeHandler_save_emptyList()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<ForgeVersion> fv = new List<ForgeVersion>()
            {
                new ForgeVersion()
                {
                    Build = 1,
                    DownloadUrl = "Something",
                    MinecraftVersion = "1.7.10"
                }
            };

            string data = forgeHandler.SaveForgeVersions(fv);
            Assert.AreEqual("[{\"Build\":1,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]", data);
        }

        [Test]
        public void ForgeHandler_save_DataInList()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            string data = forgeHandler.SaveForgeVersions();

            Assert.AreEqual("[]", data);
        }

        [Test]
        public void ForgeHandler_Dispose_DataInList()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            using (ForgeHandler forgeHandler = new ForgeHandler(fileSystem))
            {
                string data = forgeHandler.SaveForgeVersions();

                Assert.AreEqual("[]", data);
            }
        }

        [Test]
        public void ForgeHandler_GetMinecraftVersions_shouldReturnOnly1DestinctVersion()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData("[{\"Build\":1,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":2,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":3,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":4,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]")}
            });

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<string> minecraftVersions = forgeHandler.GetMinecraftVersions();

            Assert.AreEqual(1, minecraftVersions.Count);
        }

        [TestCase("1.7.10", 3)]
        [TestCase("1.6.4", 1)]
        public void ForgeHandler_GetForgeBuilds_shouldReturnExactly3Builds(string mcVersion, int count)
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData("[{\"Build\":1,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":2,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.6.4\"},{\"Build\":3,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":4,\"DownloadUrl\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]")}
            });

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<int> forgeBuilds = forgeHandler.GetForgeBuilds(mcVersion);

            Assert.AreEqual(count, forgeBuilds.Count);
        }

        [Test]
        public void ForgeHandler_GetDownloadUrl_buildFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData("[{\"Build\":1,\"DownloadUrl\":\"Something1\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":2,\"DownloadUrl\":\"Something2\",\"MinecraftVersion\":\"1.6.4\"},{\"Build\":3,\"DownloadUrl\":\"Something3\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":4,\"DownloadUrl4\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]")}
            });

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            string downloadUrl = forgeHandler.GetDownloadUrl(1);

            Assert.AreEqual("Something1", downloadUrl);
        }

        [Test]
        public void ForgeHandler_GetDownloadUrl_buildNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {forgeJsonFilePath, new MockFileData("[{\"Build\":1,\"DownloadUrl\":\"Something1\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":2,\"DownloadUrl\":\"Something2\",\"MinecraftVersion\":\"1.6.4\"},{\"Build\":3,\"DownloadUrl\":\"Something3\",\"MinecraftVersion\":\"1.7.10\"},{\"Build\":4,\"DownloadUrl4\":\"Something\",\"MinecraftVersion\":\"1.7.10\"}]")}
            });

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            Assert.That(() => forgeHandler.GetDownloadUrl(5), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ForgeHandler_downloadForgeVersions_shouldSuccessWithoutErrors()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ForgeHandler forgeHandler = new ForgeHandler(fileSystem);

            List<ForgeVersion> fv = forgeHandler.DownloadForgeVersions();

            Assert.Greater(fv.Count, 1000);
            Assert.True(fileSystem.FileExists(forgeJsonFilePath));
        }

        [Test]
        public void ForgeHandler_initialize_Normally()
        {
            ForgeHandler forgeHandler = new ForgeHandler();

            Assert.NotNull(forgeHandler);
        }
    }
}
