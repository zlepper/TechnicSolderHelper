using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ModpackHelper.Shared.Web.Solder.Crawlers
{
    public abstract class BaseCrawler<T> : ICrawler<T>
    {
        protected const string namePattern = @"([^\r\n\(\)]+?) \(([^ \r\n\(\)]+)\)";

        /// <summary>
        /// The html to crawl
        /// </summary>
        private string html;

        /// <summary>
        /// The loaded html document
        /// </summary>
        protected HtmlDocument Document;

        /// <summary>
        /// The html to crawl
        /// </summary>
        public string HTML
        {
            get { return html; }
            set
            {
                html = value;
                Load();
            }
        }

        /// <summary>
        /// Crawls the html
        /// </summary>
        /// <returns></returns>
        public abstract T Crawl();

        private void Load()
        {
            // Load html into crawler
            Document = new HtmlDocument { OptionFixNestedTags = true };
            Document.LoadHtml(HTML);

            // Make sure there is a document to crawl 
            // There should always be.
            if (Document.DocumentNode == null) throw new NullReferenceException("document.DocumentNode was not set.. Somehow");
        }
    }
}
