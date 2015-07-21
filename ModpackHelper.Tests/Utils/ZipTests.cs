using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.Shared.IO;
using NUnit.Framework;

namespace ModpackHelper.Tests.Utils
{
    [TestFixture]
    public class ZipTests
    {
        [Test]
        public void ZipUtils_GetInfoFilesFromArchive_ShouldExtractOneInfoFile()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                //Hax for jar file with .info file
                {@"C:\file.jar", new MockFileData(new byte[]{80, 75, 3, 4, 20, 0, 8, 8, 8, 0, 0, 0, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 109, 99, 109, 111, 100, 46, 105, 110, 102, 111, 157, 145, 187, 78, 195, 48, 20, 134, 247, 60, 197, 145, 23, 132, 148, 58, 141, 132, 90, 218, 13, 129, 144, 152, 41, 98, 168, 58, 24, 251, 164, 49, 248, 38, 251, 132, 80, 33, 222, 29, 39, 169, 90, 186, 50, 250, 247, 247, 217, 231, 178, 45, 190, 11, 0, 102, 189, 210, 138, 173, 129, 221, 69, 111, 69, 189, 90, 45, 19, 60, 104, 139, 46, 105, 239, 88, 57, 32, 78, 88, 188, 32, 174, 254, 32, 194, 192, 171, 143, 70, 77, 168, 194, 36, 163, 14, 52, 184, 217, 216, 180, 58, 65, 254, 1, 132, 82, 9, 4, 56, 236, 207, 38, 96, 10, 40, 181, 48, 230, 0, 141, 143, 96, 181, 211, 110, 207, 225, 137, 32, 91, 33, 34, 209, 1, 108, 39, 91, 48, 250, 3, 129, 90, 4, 255, 137, 177, 31, 126, 43, 225, 189, 75, 4, 74, 55, 13, 70, 116, 196, 97, 61, 187, 158, 74, 176, 50, 67, 233, 88, 64, 205, 151, 188, 158, 79, 23, 23, 113, 205, 231, 188, 158, 114, 227, 247, 254, 81, 155, 177, 197, 41, 233, 162, 25, 14, 45, 81, 88, 87, 85, 223, 247, 60, 215, 134, 50, 138, 134, 114, 161, 157, 229, 14, 169, 34, 31, 180, 172, 234, 197, 114, 181, 184, 189, 153, 29, 197, 160, 4, 225, 203, 127, 117, 209, 81, 235, 99, 202, 242, 246, 60, 108, 86, 2, 123, 150, 173, 54, 70, 8, 182, 27, 57, 25, 81, 105, 26, 56, 118, 239, 21, 194, 219, 1, 78, 120, 9, 27, 252, 162, 46, 98, 26, 226, 147, 56, 122, 65, 12, 179, 58, 247, 153, 119, 133, 121, 23, 173, 31, 223, 218, 238, 142, 43, 12, 232, 20, 58, 169, 113, 74, 139, 159, 98, 87, 252, 2, 80, 75, 7, 8, 155, 217, 32, 226, 52, 1, 0, 0, 46, 2, 0, 0, 80, 75, 1, 2, 20, 0, 20, 0, 8, 8, 8, 0, 0, 0, 33, 0, 155, 217, 32, 226, 52, 1, 0, 0, 46, 2, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 109, 99, 109, 111, 100, 46, 105, 110, 102, 111, 80, 75, 5, 6, 0, 0, 0, 0, 1, 0, 1, 0, 56, 0, 0, 0, 108, 1, 0, 0, 0, 0})}
                ,{@"C:\output", new MockDirectoryData()}
            });

            ZipUtils zip = new ZipUtils(fileSystem);

            zip.GetInfoFilesFromArchive(@"C:\file.jar", @"C:\other");

            IOHandler handler = new IOHandler(fileSystem);
            string data = handler.ReadText(@"C:\other\mcmod.info");
            string expected =
                "[\n{\n  \"modid\": \"Aroma1997s Dimension\",\n  \"name\": \"Aroma1997's Dimensional World\",\n  \"description\": \"This mod adds a new Dimension especially for mining. It is pretty much like the overworld, just different. :-)\",\n  \"mcversion\": \"1.7.10\",\n  \"version\": \"1.1.0.1\",\n  \"logoFile\": \"\",\n  \"url\": \"http://www.minecraftforum.net/topic/1679684-\",\n  \"updateUrl\": \"http://www.minecraftforum.net/topic/1679684-\",\n  \"authors\": [\"Aroma1997\", \"Schillaa\"],\n  \"credits\": \"Code by Aroma1997, Textures by Schillaa\",\n  \"parent\": \"\",\n  \"screenshots\": [],\n  \"dependencies\": []\n}\n]\n";

            Assert.AreEqual(expected, data);
        }

        [Test]
        public void ZipUtils_GetInfoFilesFromArchive_OutputDirectoryNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                //Hax for jar file with .info file
                {@"C:\file.jar", new MockFileData(new byte[]{80, 75, 3, 4, 20, 0, 8, 8, 8, 0, 0, 0, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 109, 99, 109, 111, 100, 46, 105, 110, 102, 111, 157, 145, 187, 78, 195, 48, 20, 134, 247, 60, 197, 145, 23, 132, 148, 58, 141, 132, 90, 218, 13, 129, 144, 152, 41, 98, 168, 58, 24, 251, 164, 49, 248, 38, 251, 132, 80, 33, 222, 29, 39, 169, 90, 186, 50, 250, 247, 247, 217, 231, 178, 45, 190, 11, 0, 102, 189, 210, 138, 173, 129, 221, 69, 111, 69, 189, 90, 45, 19, 60, 104, 139, 46, 105, 239, 88, 57, 32, 78, 88, 188, 32, 174, 254, 32, 194, 192, 171, 143, 70, 77, 168, 194, 36, 163, 14, 52, 184, 217, 216, 180, 58, 65, 254, 1, 132, 82, 9, 4, 56, 236, 207, 38, 96, 10, 40, 181, 48, 230, 0, 141, 143, 96, 181, 211, 110, 207, 225, 137, 32, 91, 33, 34, 209, 1, 108, 39, 91, 48, 250, 3, 129, 90, 4, 255, 137, 177, 31, 126, 43, 225, 189, 75, 4, 74, 55, 13, 70, 116, 196, 97, 61, 187, 158, 74, 176, 50, 67, 233, 88, 64, 205, 151, 188, 158, 79, 23, 23, 113, 205, 231, 188, 158, 114, 227, 247, 254, 81, 155, 177, 197, 41, 233, 162, 25, 14, 45, 81, 88, 87, 85, 223, 247, 60, 215, 134, 50, 138, 134, 114, 161, 157, 229, 14, 169, 34, 31, 180, 172, 234, 197, 114, 181, 184, 189, 153, 29, 197, 160, 4, 225, 203, 127, 117, 209, 81, 235, 99, 202, 242, 246, 60, 108, 86, 2, 123, 150, 173, 54, 70, 8, 182, 27, 57, 25, 81, 105, 26, 56, 118, 239, 21, 194, 219, 1, 78, 120, 9, 27, 252, 162, 46, 98, 26, 226, 147, 56, 122, 65, 12, 179, 58, 247, 153, 119, 133, 121, 23, 173, 31, 223, 218, 238, 142, 43, 12, 232, 20, 58, 169, 113, 74, 139, 159, 98, 87, 252, 2, 80, 75, 7, 8, 155, 217, 32, 226, 52, 1, 0, 0, 46, 2, 0, 0, 80, 75, 1, 2, 20, 0, 20, 0, 8, 8, 8, 0, 0, 0, 33, 0, 155, 217, 32, 226, 52, 1, 0, 0, 46, 2, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 109, 99, 109, 111, 100, 46, 105, 110, 102, 111, 80, 75, 5, 6, 0, 0, 0, 0, 1, 0, 1, 0, 56, 0, 0, 0, 108, 1, 0, 0, 0, 0})}
            });

            ZipUtils zip = new ZipUtils(fileSystem);

            zip.GetInfoFilesFromArchive(@"C:\file.jar", @"C:\other");

            IOHandler handler = new IOHandler(fileSystem);
            string data = handler.ReadText(@"C:\other\mcmod.info");
            string expected =
                "[\n{\n  \"modid\": \"Aroma1997s Dimension\",\n  \"name\": \"Aroma1997's Dimensional World\",\n  \"description\": \"This mod adds a new Dimension especially for mining. It is pretty much like the overworld, just different. :-)\",\n  \"mcversion\": \"1.7.10\",\n  \"version\": \"1.1.0.1\",\n  \"logoFile\": \"\",\n  \"url\": \"http://www.minecraftforum.net/topic/1679684-\",\n  \"updateUrl\": \"http://www.minecraftforum.net/topic/1679684-\",\n  \"authors\": [\"Aroma1997\", \"Schillaa\"],\n  \"credits\": \"Code by Aroma1997, Textures by Schillaa\",\n  \"parent\": \"\",\n  \"screenshots\": [],\n  \"dependencies\": []\n}\n]\n";

            Assert.AreEqual(expected, data);
        }

        [Test]
        public void ZipUtils_ZipDirectory_ShouldZipDirectory()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                {@"C:\input", new MockDirectoryData()},
                {@"C:\input\textfile1.txt", new MockFileData("afnkgaængafjkælnjkjaafgnæjfanælgfaælkjfklanfæl")},
                {@"C:\input\textfile2.txt", new MockFileData("afnkgaængafjkælnjkjaafgnæjfanælgfaælkjfklanfæl")},
                {@"C:\input\textfile3.txt", new MockFileData("afnkgaængafjkælnjkjaafgnæjfanælgfaælkjfklanfæl")},
                {@"C:\input\textfile4.txt", new MockFileData("afnkgaængafjkælnjkjaafgnæjfanælgfaælkjfklanfæl")}
            });

            ZipUtils zip = new ZipUtils(fileSystem);

            zip.ZipDirectory(@"C:\input", @"C:\input.zip");

            Assert.IsTrue(fileSystem.FileExists(@"C:\input.zip"));
        }

        [Test]
        public void ZipUtils_Initialize_Normally()
        {
            ZipUtils zip = new ZipUtils();

            Assert.NotNull(zip);
        }

        [Test]
        [ExpectedException(typeof (FileNotFoundException))]
        public void ZipUtils_GetInfoFilesFromArchive_FileNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ZipUtils zip = new ZipUtils(fileSystem);

            zip.GetInfoFilesFromArchive(@"C:\somenotExistingDir", @"C:\directoryThatDoesntMatter");
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void ZipUtils_ZipDirectory_OutputFileIsNotAZipDirectory()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());

            ZipUtils zip = new ZipUtils(fileSystem);

            zip.ZipDirectory(@"C:\somefolder", @"C:\notAZipFile.txt");
        }
    }
}
