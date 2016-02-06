using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ModpackHelper.Shared.Web.Solder.Responses;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    public class BuildCrawler : BaseCrawler<Build>
    {
        public override Build Crawl()
        {
            Build build = new Build();
            
            // Find java version
            string javaVersion = Document.DocumentNode.SelectSingleNode(
                    @"//*[@id='page-wrapper']/div/div/div[2]/div[2]/div[2]/label[1]/span").InnerText; ;
            build.Java = javaVersion.Equals("Not Required", StringComparison.OrdinalIgnoreCase) ? "" : javaVersion;

            // Find amount of memory
            string memory =
                Document.DocumentNode.SelectSingleNode(
                    @"//*[@id='page-wrapper']/div/div/div[2]/div[2]/div[2]/label[2]/span").InnerText;
            build.Memory = memory.Equals("Not Required", StringComparison.OrdinalIgnoreCase) ? "" : memory;

            build.Mods = new List<Mod>();

            // Find all the mods in the build
            var tableRows = Document.DocumentNode.SelectNodes("//table[@id='mod-list']/tbody/tr");

            if (tableRows == null)
            {
                return build;
            }

            foreach (HtmlNode row in tableRows)
            {
                var mod = new Mod();

                // Use regex to calculate modname and -slug
                string firstPart = row.SelectSingleNode(".//td[1]").InnerText;
                var r = Regex.Match(firstPart, namePattern, RegexOptions.IgnoreCase);
                mod.Name = r.Groups[2].Value;
                mod.PrettyName = r.Groups[1].Value;

                // Find the mod id
                var anchor = row.SelectSingleNode(".//a");
                var url = anchor.GetAttributeValue("href", "");
                mod.Id = url.Substring(url.LastIndexOf("/", StringComparison.OrdinalIgnoreCase)+1);

                // Find modversions
                var nodes = row.SelectNodes(".//select/option");
                
                mod.Versions = new List<string>();
                foreach (HtmlNode htmlNode in nodes)
                {
                    var attrs = htmlNode.Attributes;
                    if (attrs.Contains("selected"))
                    {
                        mod.Active = htmlNode.NextSibling.InnerText;
                    }
                    mod.Versions.Add(htmlNode.NextSibling.InnerText);
                }
                build.Mods.Add(mod);
            }

            return build;
        }
    }
}
