using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.IO;
using NUnit.Framework;

namespace ModpackHelper.Tests.IO
{
    [TestFixture]
    public class IOTest
    {
        [Test]
        public void IOHandler_ReadJson_ShouldReadSuccessful()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"C:\file.json", new MockFileData("[{'key': 'value'}]")}
            });
            IOHandler iohandler = new IOHandler(fileSystem);

            string results = iohandler.ReadJson(@"C:\file.json");

            Assert.AreEqual("[{'key': 'value'}]", results);
        }
        
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void IOHandler_ReadJson_ShouldThrowFileNotFoundException()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            IOHandler iohandler = new IOHandler(fileSystem);

            iohandler.ReadJson(@"C:\file.json");
        }

        [Test]
        public void IOHandler_initialize_Normally()
        {
            IOHandler ioHandler = new IOHandler();
            Assert.NotNull(ioHandler);
        }
        
    }
}
