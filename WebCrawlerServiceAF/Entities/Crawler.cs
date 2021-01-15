using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Web;

namespace WebCrawlerService.Entities
{
    public class Crawler
    {
        public static async Task<List<(string, string)>> GetTitleUrlPair(string query)
        {
            char[] delimiters = { ' ', ',', '.', ':', '\t' };
            string[] title_words = query.Split(delimiters);
            var url = "http://www.google.com/search?q=";
            foreach (string word in title_words)
            {
                url = url + "+" + word;
            }

            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var divs = htmlDocument.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("id", "").Equals("main")).ToList();

            var pagesUrls = new List<HtmlNode>();

            try
            {
                for (int i = 14; i < 24; ++i)
                {
                    pagesUrls.Add(divs[0].Descendants("a").ElementAt(i));
                }
            }
            catch { }

            List<(string, string)> titleToUrl = new List<(string, string)>();
            foreach (var page in pagesUrls)
            {
                var page_link = page.GetAttributeValue("href", "");
                page_link = page_link.Remove(0, 7);
                var ind = page_link.IndexOf("&amp");
                page_link = page_link.Remove(ind, page_link.Length - ind);

                try
                {
                    titleToUrl.Add((page.Descendants("h3").ToList()[0].InnerText, page_link));
                }
                catch { }
            }

            return titleToUrl;
        }

        public static async Task<List<(string, string)>> GetTextUrlPair(string query, string directory)
        {
            // var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // var rootDirectory = Path.GetFullPath(Path.Combine(binDirectory, "../../.."));

            // System.Console.WriteLine(binDirectory);
            // System.Console.WriteLine(rootDirectory);
            using FileStream openStream = File.OpenRead(Path.Combine(directory, "Data", "siteTags.json"));
            var hostToSelector = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(openStream);

            List<(string, string)> titleUrlPair = await GetTitleUrlPair(query);
            List<(string, string)> textUrlPair = new List<(string, string)>();
            foreach (var pair in titleUrlPair)
            {
                var url = pair.Item2;

                Uri myUri = new Uri(url);
                string host = myUri.Host;
                if (host.Contains("www."))
                {
                    host = host.Substring(host.IndexOf("www.") + 4);
                }
                var tag = "article";
                if (hostToSelector.ContainsKey(host))
                {
                    tag = hostToSelector[host];
                }

                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                string html = "";
                try
                {
                    html = await httpClient.GetStringAsync(HttpUtility.UrlDecode(url));
                }
                catch { }
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                try
                {
                    textUrlPair.Add((htmlDocument.DocumentNode.QuerySelector(tag).InnerText, url));
                }
                catch { }
            }

            return textUrlPair;
        }
    }
}
