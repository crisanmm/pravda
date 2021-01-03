using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace webCrawler_1
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerAsync("Trump Junior TOTALLY Staged This Awkward Photo For The NYT, And Twitter Loves It").GetAwaiter();
        }

        private static async Task startCrawlerAsync(string title)
        {
            char[] delimiters = { ' ', ',', '.', ':', '\t' };
            string[] title_words = title.Split(delimiters);
            var url = "http://www.google.com/search?q=";
            foreach (string word in title_words)
            {
                url = url + "+" + word;
            }

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("id", "").Equals("main")).ToList();

            var pagesUrls = new List<HtmlNode>();
            for (int i = 14; i < 24; ++i)
            {
                pagesUrls.Add(divs[0].Descendants("a").ElementAt(i));
            }

            List<string> actualUrls = new List<string>();
            List<string> titles = new List<string>();
            foreach (var page in pagesUrls)
            {
                var page_link = page.GetAttributeValue("href", "");
                page_link = page_link.Remove(0, 7);
                var ind = page_link.IndexOf("&amp");
                page_link = page_link.Remove(ind, page_link.Length - ind);

                actualUrls.Add(page_link);

                titles.Add(page.Descendants("h3").ToList()[0].InnerText);
            }

            // Environment.Exit(0);
        }
    }
}
