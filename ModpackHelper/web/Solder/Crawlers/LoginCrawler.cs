using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    public class LoginCrawler : BaseCrawler<bool>
    {
        /// <summary>
        /// Checks if the login was successful. 
        /// </summary>
        /// <returns>Returns true if login successed</returns>
        public override bool Crawl()
        {
            // Find the dashboard <h1> node.
            // This node is only available is login was successful
            var dashboardHeaderNode = Document.DocumentNode.SelectSingleNode(@"//*[@id='page-wrapper']/div/div/div[1]/h1");

            // Login was successful if the dashboard node is available
            return dashboardHeaderNode != null;
        }
    }
}
