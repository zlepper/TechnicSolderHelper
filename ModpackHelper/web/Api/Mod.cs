using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ModpackHelper.Utils;

namespace ModpackHelper.Shared.Web.Api
{
    /// <summary>
    /// Information about a specific mod version
    /// </summary>
    public class Mod
    {
        /// <summary>
        /// Database key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The modid of the mod
        /// </summary>
        [Required]
        public string Modid { get; set; }

        /// <summary>
        /// The name of the mod
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The version of the mod
        /// </summary>
        [Required]
        public string Version { get; set; }

        /// <summary>
        /// The Minecraft version of the mod
        /// </summary>
        [Required]
        public string Mcversion { get; set; }

        /// <summary>
        /// A list of the authors of the mod
        /// </summary>
        [Required]
        public virtual ICollection<Author> Authors { get; set; }

        /// <summary>
        /// The user who uploaded this mod
        /// </summary>
        [JsonIgnore]
        public HelperUser HelperUser { get; set; }

        /// <summary>
        /// The id of the user who uploaded this mod
        /// </summary>
        [JsonIgnore]
        public int HelperUserId { get; set; }

        /// <summary>
        /// The md5 value of the jar of this mod
        /// </summary>
        [Required]
        public string JarMd5 { get; set; }

        [Required]
        public string Filename { get; set; }

        /// <summary>
        /// The info url of the mod
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Indicates the current status of the mod
        /// </summary>
        [JsonIgnore]
        public Status Status { get; set; }

        /// <summary>
        /// The user this mod was accepted by
        /// </summary>
        [JsonIgnore]
        public User AcceptedBy { get; set; }

        /// <summary>
        /// The id of the user who accepted this mod
        /// </summary>
        [JsonIgnore]
        public int? AcceptedById { get; set; }

        public bool Equals(Mod obj)
        {
            return obj.Mcversion.Equals(Mcversion)
                   && Lists.AreEqual(obj.Authors.ToList(), Authors.ToList())
                   && obj.Name.Equals(Name)
                   && obj.Version.Equals(Version)
                   && obj.Modid.Equals(Modid);
        }
    }

    /// <summary>
    /// An enum descripting how the mod is faring in the system
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The mod is currently awaiting confirmation to be accepted
        /// </summary>
        Awaiting,
        /// <summary>
        /// The mod data is valid, and can be destributed
        /// </summary>
        Accepted,
        /// <summary>
        /// The mod data was wrong, and should be ignored
        /// </summary>
        Denied
    }
}
