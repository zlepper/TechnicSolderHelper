using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ModpackHelper.Shared.Web.Solder.Responses;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    class ModpackListCrawler : BaseCrawler<List<Modpack>>
    {
        /// <summary>
        /// Gets all the modpacks in the system currently
        /// </summary>
        /// <returns></returns>
        public override List<Modpack> Crawl()
        {
            // Find the table that contains data about all the modpacks in the system
            var tableRows = Document.DocumentNode.SelectNodes("//table/tbody/tr");

            // Prepare the list of modpacks
            List<Modpack> modpacks = new List<Modpack>();

            // Just return if there isn't any table rows. This means that there is no modpacks available at the moment
            if (tableRows == null)
            {
                return modpacks;
            }

            // Read modpack data
            foreach (var row in tableRows)
            {
                Modpack modpack = new Modpack
                {
                    DisplayName = row.SelectSingleNode(".//td[1]").InnerText,
                    Name = row.SelectSingleNode(".//td[2]").InnerText,
                    Recommended = row.SelectSingleNode(".//td[3]").InnerText,
                    Latest = row.SelectSingleNode(".//td[4]").InnerText
                };

                // Find the modpack id
                string tid = row.SelectSingleNode(".//td[7]/a[1]").GetAttributeValue("href", "");
                tid = tid.Substring(tid.LastIndexOf("/", StringComparison.Ordinal) + 1);
                modpack.Id = tid;
                modpacks.Add(modpack);
            }

            // Return the modpacks to the caller
            return modpacks;

        }
    }
}
