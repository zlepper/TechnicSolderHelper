using System;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.GUI.UserInteraction;
using ModpackHelper.IO;
using ModpackHelper.UserInteraction;
using Moq;
using NUnit.Framework;

namespace ModpackHelper.Tests.GUI
{
    [TestFixture]
    public class ModpackHelperTests
    {
        [Test]
        public void ModpackHelper_normalInitialization()
        {
            ModpackHelper.GUI.ModpackHelper mh = new ModpackHelper.GUI.ModpackHelper();
        }

        [Test]
        public void ModpackHelper_FromFirstInstall()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            ModpackHelper.GUI.ModpackHelper mh = new ModpackHelper.GUI.ModpackHelper(fileSystem, null, null);

            // Everything should have the default value
            Assert.AreEqual("", mh.InputDirectoryTextBox.Text);
            Assert.AreEqual(fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "ModpackHelper"), mh.OutputDirectoryTextBox.Text);
            Assert.IsTrue(mh.ClearOutpuDirectoryCheckBox.Checked);
        }

        [Test]
        public void ModpackHelper_inputDirectoryBrowseButtonClickedAndUserSelectedModsDirectory()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            Mock<IDirectoryFinder> directoryFinderMock = new Mock<IDirectoryFinder>();
            directoryFinderMock.Setup(d => d.GetDirectory(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(@"C:\minecraft\mods");
            ModpackHelper.GUI.ModpackHelper mh = new ModpackHelper.GUI.ModpackHelper(fileSystem, directoryFinderMock.Object, null);

            mh.browseForInputDirectoryButton_Click(null, null);
            
            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                Assert.AreEqual(@"C:\minecraft\mods", ch.Configs.InputDirectory);
            }

        }

        [Test]
        public void ModpackHelper_inputDirectoryBrowseButtonClickedAndUserDidNotSelectAModsDirectory()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            Mock<IDirectoryFinder> directoryFinderMock = new Mock<IDirectoryFinder>();
            Mock<IMessageShower> messageShowerMock = new Mock<IMessageShower>();
            directoryFinderMock.Setup(d => d.GetDirectory(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(@"C:\minecraft\NotAModsDirectory");
            messageShowerMock.Setup(m => m.ShowMessage(It.IsAny<string>()));
            ModpackHelper.GUI.ModpackHelper mh = new ModpackHelper.GUI.ModpackHelper(fileSystem, directoryFinderMock.Object, messageShowerMock.Object);

            mh.browseForInputDirectoryButton_Click(null, null);

            messageShowerMock.Verify(m => m.ShowMessage(It.IsNotNull<string>()), Times.Once);

            using (ConfigHandler ch = new ConfigHandler(fileSystem))
            {
                Assert.True(string.IsNullOrWhiteSpace(ch.Configs.InputDirectory));
            }

        }
    }
}
