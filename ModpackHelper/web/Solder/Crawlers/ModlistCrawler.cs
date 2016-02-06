using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ModpackHelper.Shared.Web.Solder.Responses;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    /// <summary>
    /// Crawls the solder mod list and returns data from there
    /// </summary>
    public class ModlistCrawler : BaseCrawler<List<Mod>>
    {
        private const string complexNamePattern = @"(?:<.+?>)(.+?)(?:<.+?>) ?\((.+?)\)(?:.+)";

        public ModlistCrawler(string html)
        {
            HTML = html;
        }

        public override List<Mod> Crawl()
        {
            var tableRows = Document.DocumentNode.SelectNodes("//table/tbody/tr");

            List<Mod> mods = new List<Mod>(tableRows.Count);

            foreach (HtmlNode row in tableRows)
            {
                Mod mod = new Mod();
                var n = row.SelectSingleNode(".//td[1]");
                mod.Id = n.InnerText;

                // Read name and slug
                string content = row.SelectSingleNode(".//td[2]").InnerHtml;
                // Remove newlines
                content = Regex.Replace(content, @"\r\n?|\n|\t", "");
                // Remove double spaces
                content = content.Replace("  ", " ");
                // Get matches
                var r = Regex.Match(content, complexNamePattern, RegexOptions.IgnoreCase);
                mod.Name = r.Groups[2].Value;
                mod.PrettyName = r.Groups[1].Value;
                
                mods.Add(mod);
            }

            return mods;
        }
    }
}
