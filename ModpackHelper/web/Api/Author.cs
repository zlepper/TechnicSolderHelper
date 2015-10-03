using System.Collections.Generic;
using Newtonsoft.Json;

namespace ModpackHelper.Shared.Web.Api
{
    /// <summary>
    /// A mod author
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Database key
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// The authors name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of all the mods the author made
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Mod> Mods { get; set; } 
    }
}
