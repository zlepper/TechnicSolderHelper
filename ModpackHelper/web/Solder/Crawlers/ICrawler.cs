using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    public interface ICrawler<out T>
    {
        /// <summary>
        /// The html to crawl
        /// </summary>
        string HTML { get; set; }

        /// <summary>
        /// Crawls the html
        /// </summary>
        T Crawl();


    }
}
