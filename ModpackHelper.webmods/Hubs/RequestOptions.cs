namespace ModpackHelper.webmods.Hubs
{
    /// <summary>
    /// A helper class to keep order of the request parameters
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// The status of the mods to request
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// A string in the mods data to request
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// The max number of mods to be returned
        /// </summary>
        public int Limit { get; set; }
    }
}