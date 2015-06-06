using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace ModpackHelper.Tests
{
    [TestFixture]
    public class IOHandlerTests
    {
        [Test]
        public void IOHandler_ReadJson_ShouldReadSuccessful()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {

            });
        }
    }
}
