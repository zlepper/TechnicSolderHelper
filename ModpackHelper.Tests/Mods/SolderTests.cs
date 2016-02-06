using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModpackHelper.Shared.Mods;
using ModpackHelper.Shared.Utils.Config;
using ModpackHelper.Shared.Web.FTP;
using NUnit.Framework;

namespace ModpackHelper.Tests.Mods
{
    [TestFixture]
    public class SolderTests
    {
        private List<Mcmod> mods;
        private Modpack modpack;
        private IFileSystem fileSystem;
        [SetUp]
        public void Setup()
        {
            mods = new List<Mcmod>();
            for (int i = 0; i < 10; i++)
            {
                Mcmod mod = new Mcmod
                {
                    Authors = new List<string> {"Zlepper", Guid.NewGuid().ToString()},
                    Name = "Mod-" + i,
                    Modid = "mod-" + i,
                    Description = "Some description " + i,
                    Mcversion = "1.7.10",
                    Version = "1.5." + i,
                    IsSkipping = false
                };

                mod.OutputFile = string.Format("C:\\something\\{0}\\{0}-{1}-{2}.zip", mod.GetSafeModId(), mod.Mcversion, mod.Version);

                mods.Add(mod);
            }

            modpack = new Modpack
            {
                Name = "TEst apck",
                ForceSolder = false,
                MinJava = "1.7",
                MinMemory = "1024",
                MinecraftVersion = "1.7.10",
                Version = Guid.NewGuid().ToString()
            };

            var dict = mods.ToDictionary<Mcmod, string, MockFileData>(mod => mod.OutputFile, mod => Guid.NewGuid().ToString());

            fileSystem = new MockFileSystem(dict);
        }


        [Test]
        public void CanCreateBasicModpack()
        {
            var ftpLogin = new FTPLoginInfo();
            ftpLogin.Address = "ftp://46.101.238.40/repo";
            ftpLogin.Password = "password";
            ftpLogin.Username = "zlepper";

            var Ftp = new FTPUploader(ftpLogin, fileSystem);
            var dir = fileSystem.DirectoryInfo.FromDirectoryName("C:\\something");
            Ftp.UploadFolder(dir);


            var solder = new Solder(fileSystem);
            solder.Initialize("hansen13579@outlook.com", "password", mods, modpack);
            solder.Update("solder.zlepper.dk");
        }

        [Test]
        public void CanHandleExistingModsAndModpack()
        {
            for (int i = 0; i < mods.Count; i += 2)
            {
                var mod = mods[i];
                mod.Version = mod.Version + "." + i;
                mod.OutputFile = string.Format("C:\\something\\{0}\\{0}-{1}-{2}.zip", mod.GetSafeModId(), mod.Mcversion, mod.Version);
            }

            var dict = mods.ToDictionary<Mcmod, string, MockFileData>(mod => mod.OutputFile, mod => Guid.NewGuid().ToString());

            fileSystem = new MockFileSystem(dict);


            var ftpLogin = new FTPLoginInfo();
            ftpLogin.Address = "ftp://46.101.238.40/repo";
            ftpLogin.Password = "password";
            ftpLogin.Username = "zlepper";

            var Ftp = new FTPUploader(ftpLogin, fileSystem);
            var dir = fileSystem.DirectoryInfo.FromDirectoryName("C:\\something");
            Ftp.UploadFolder(dir);

            //modpack.Version = Guid.NewGuid().ToString();

            var solder = new Solder(fileSystem);
            solder.Initialize("hansen13579@outlook.com", "password", mods, modpack);
            solder.Update("solder.zlepper.dk");
        }
    }
}
