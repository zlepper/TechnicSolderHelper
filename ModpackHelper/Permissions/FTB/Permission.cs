// ReSharper disable InconsistentNaming

namespace ModpackHelper.Shared.Permissions.FTB
{
    public class Permission
    {
        public string modName { get; set; }
        public string modAuthors { get; set; }
        public string licenseLink { get; set; }
        public string modLink { get; set; }
        public string privateLicenceLink { get; set; }
        public string privateStringPolicy { get; set; }
        public PermissionPolicy privatePolicy { get; set; }
        public string publicStringPolicy { get; set; }
        public PermissionPolicy publicPolicy { get; set; }
        public string modids { get; set; }
        public string customData { get; set; }
        public string shortName { get; set; }
    }

    public enum PermissionPolicy
    {
        Open,
        Notify,
        Request,
        Unknown,
        FTB,
        Closed
    }
}
