using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ModpackHelper.Shared.Web.Solder.Responses;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    class ModVersionCrawler : BaseCrawler<List<ModVersion>>
    {
        public ModVersionCrawler(string html)
        {
            HTML = html;
        }

        public override List<ModVersion> Crawl()
        {
            var tableRows = Document.DocumentNode.SelectNodes("//table/tbody/tr[@class='version']");

            List<ModVersion> mvs = new List<ModVersion>();

            if (tableRows == null)
            {
                return mvs;
            }

            mvs.AddRange(tableRows.Select(tableRow => new ModVersion
            {
                Id = tableRow.GetAttributeValue("rel", null),
                Version = tableRow.SelectSingleNode("//td[class='version']").InnerText,
                Url = tableRow.SelectSingleNode("//td[class='url']").InnerText,
                Md5 = tableRow.SelectSingleNode("//td/input").GetAttributeValue("placeholder", null)
            }));

            return mvs;
        }
    }
}
