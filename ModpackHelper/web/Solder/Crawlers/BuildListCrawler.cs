using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ModpackHelper.Shared.Web.Solder.Responses;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    class BuildListCrawler : BaseCrawler<List<Build>>
    {
        /// <summary>
        /// Crawls the html
        /// </summary>
        /// <returns>A list of all the builds for the modpack</returns>
        public override List<Build> Crawl()
        {
            // Find the table rows
            var tableRows = Document.DocumentNode.SelectNodes("//table[@id='dataTables']/tbody/tr");

            // Create the list of builds
            List<Build> builds = new List<Build>();

            // If there is not rows available, this means that there is no builds, at we might at well return an empty list
            if (tableRows == null)
            {
                return builds;
            }

            // Load all the build data
            builds.AddRange(tableRows.Select(row => new Build
            {
                Id = row.SelectSingleNode(".//td[1]").InnerText,
                Minecraft = row.SelectSingleNode(".//td[3]").InnerText,
                Version = row.SelectSingleNode(".//td[2]").InnerText
            }));


            return builds;
        }
    }
}
