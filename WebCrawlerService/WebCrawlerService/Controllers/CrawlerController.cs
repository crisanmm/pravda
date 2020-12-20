using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebCrawlerService.Entities;
using System.Net.Http.Json;

namespace WebCrawlerService.Controllers
{
    [Route("api/crawler")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        [HttpPost]
        public async Task<bool> crawlWeb(Article article)
        {
            char[] delimiters = { ' ', ',', '.', ':', '\t' };
            string[] title_words = article.Title.Split(delimiters);
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

            Dictionary<string, string> title_to_url = new Dictionary<string, string>();
            foreach (var page in pagesUrls)
            {
                var page_link = page.GetAttributeValue("href", "");
                page_link = page_link.Remove(0, 7);
                var ind = page_link.IndexOf("&amp");
                page_link = page_link.Remove(ind, page_link.Length - ind);

                title_to_url.Add(page.Descendants("h3").ToList()[0].InnerText, page_link);
            }

            List<Response> responses = new List<Response>();
            double totalSimilarity = 0;
            foreach (var t in title_to_url.Keys)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://webit-text-analytics.p.rapidapi.com/similarity/"),
                    Headers =
                    {
                        { "x-rapidapi-key", "5889e3dab7msh61cc65be6f2fee9p1745d0jsn2342f6e123f9" },
                        { "x-rapidapi-host", "webit-text-analytics.p.rapidapi.com" },
                    },
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "string1", article.Title
                        },
                        { "string2", t},
                    }),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadFromJsonAsync<Response>();
                    responses.Add(body);
                    totalSimilarity += body.Data["similarity"];
                    Console.WriteLine(body);
                }

            }

            var mean = totalSimilarity / responses.Count();

            if( mean >=  0.5 )
            {
                return true;
            } 
            else
            {
                return false;
            }

        }
    }
}
