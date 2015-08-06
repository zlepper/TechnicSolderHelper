using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.CLI;
using ModpackHelper.Shared.UserInteraction;
using Moq;
using NUnit.Framework;

namespace ModpackHelper.Tests.CLI
{
    [TestFixture]
    public class HandlerArgsTests
    {
        [Test]
        public void HandlerArgs_Start_NoArgsSpecified()
        {
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>();

            Handler h = new Handler();
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.Usage), Times.Exactly(1));
        }

        [Test]
        public void HandlerArgs_Start_IFlagIsLastFlag()
        {
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>() {"-o", "-i"};

            Handler h = new Handler();
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.MissingInputDirectory));
        }

        [Test]
        public void HandlerArgs_Start_InputDirectorySpecifiedButOutputDirectoryNotSpecified()
        {
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>() { "-i" };

            Handler h = new Handler();
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.MissingOutputDirectory));
        }

        [Test]
        public void HandlerArgs_Start_InputDirectoryNotSpecified()
        {
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>() { "-i", "-o" };

            Handler h = new Handler();
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.MissingInputDirectory));
        }

        [Test]
        public void HandlerArgs_Start_InputDirectoryIsNotAModsDirectory()
        {
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>() { "-i", @"C:\NotAModsDirectory", "-o" };

            Handler h = new Handler();
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.NotAModsDirectory));
        }

        [Test]
        public void HandlerArgs_Start_InputDirectoryNotFound()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            Mock<IMessageShower> messageShower = new Mock<IMessageShower>();
            messageShower.Setup(m => m.ShowMessageAsync(It.IsAny<string>()));
            List<string> args = new List<string>() { "-i", @"C:\mods", "-o" };

            Handler h = new Handler(fileSystem);
            bool success = h.Start(args, messageShower.Object);

            Assert.False(success);
            messageShower.Verify(m => m.ShowMessageAsync(Messages.InputDirectoryNotFound));
        }


    }
}
