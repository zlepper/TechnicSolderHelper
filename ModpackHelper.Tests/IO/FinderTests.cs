using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.Shared.IO;
using NUnit.Framework;

namespace ModpackHelper.Tests.IO
{
    [TestFixture]
    public class FinderTests
    {
        [TestCase("jar")]
        [TestCase("zip")]
        [TestCase("litemod")]
        [TestCase("disabled")]
        public void Finder_GetModFiles_find2Files(string fileEnding)
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"C:\Subdir\mod1." + fileEnding, new MockFileData("")},
                {@"C:\Subdir\mod2." + fileEnding, new MockFileData("")},
                {$@"C:\Subdir\mod2.{fileEnding}blarg", new MockFileData("")}
            });
            Finder finder = new Finder(fileSystem);

            List<FileInfoBase> files = finder.GetModFiles(@"C:\Subdir");

            Assert.AreEqual(2, files.Count);
        }

        [Test]
        public void Finder_GetModFiles_find8Files()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"C:\Subdir\mod1.zip", new MockFileData("")},
                {@"C:\Subdir\mod2.zip", new MockFileData("")},
                {@"C:\Subdir\mod2.blarg", new MockFileData("")},
                {@"C:\Subdir\mod1.jar", new MockFileData("")},
                {@"C:\Subdir\mod2.jar", new MockFileData("")},
                {@"C:\Subdir\mod1.litemod", new MockFileData("")},
                {@"C:\Subdir\mod2.litemod", new MockFileData("")},
                {@"C:\Subdir\mod1.disabled", new MockFileData("")},
                {@"C:\Subdir\mod2.disabled", new MockFileData("")}
            });
            Finder finder = new Finder(fileSystem);

            List<FileInfoBase> files = finder.GetModFiles(@"C:\Subdir");

            Assert.AreEqual(8, files.Count);
        }

        //[Test]
        //public void Finder_GetModFiles_ParameterIsNotADirectory()
        //{
        //    MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        //    {
        //        {@"C:\Subdir\mod1.zip", new MockFileData("")}
        //    });

        //    Finder finder = new Finder(fileSystem);

        //    Assert.That(() => finder.GetModFiles(@"C:\Subdir\mod1.zip"), Throws.TypeOf<DirectoryNotFoundException>());
        //}

        [Test]
        public void Finder_Initialise_Normally()
        {
            Finder finder = new Finder();

            Assert.NotNull(finder);
        }
    }
}
