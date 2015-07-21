using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using ModpackHelper.Permissions;
using ModpackHelper.Shared.Permissions;
using NUnit.Framework;

namespace ModpackHelper.Tests.Permissions
{
    [TestFixture]
    public class PermissionGetterTests
    {
        [Test]
        public void PermissionGetter_initialize_normally()
        {
            PermissionGetter getter = new PermissionGetter();

            Assert.NotNull(getter);
        }

        [Test]
        public void PermissionsGetter_LoadOnlinePermissions_ShouldLoadNormally()
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
               // {PermissionGetter.PermissionsFile, new MockFileData("")}
            });
            PermissionGetter getter = new PermissionGetter(fileSystem);

            Assert.True(fileSystem.FileExists(PermissionGetter.PermissionsFile));
        }
    }
}
