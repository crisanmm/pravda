
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
    [Route("api/v1")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        [HttpPost("title")]
        public async Task<Dictionary<string, dynamic>> Title(Article article)
        {

            List<(string, string)> titleToUrl = await Crawler.GetTitleUrlPair(article.Title);
            List<Response> responses = new List<Response>();
            double totalSimilarity = 0;
            foreach (var t in titleToUrl)
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
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
                        { "string1", article.Title },
                        { "string2", t.Item1 },
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

            Dictionary<string, dynamic> ret = new Dictionary<string, dynamic>();

            if (mean >= 0.5)
            {
                ret.Add("isSimilar", true);
            }
            else
            {
                ret.Add("isSimilar", false);
            }
            ret.Add("similarity", mean);

            return ret;
        }

        [HttpPost("text")]
        public async Task<Dictionary<string, dynamic>> Text(Article article)
        {
            List<(string, string)> textUrlPair = await Crawler.GetTextUrlPair(article.Title);

            List<Response> responses = new List<Response>();
            double totalSimilarity = 0;
            foreach (var t in textUrlPair)
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
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
                        { "string1", article.Text },
                        { "string2", t.Item1 },
                    }),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadFromJsonAsync<Response>();
                    if (body.Data["similarity"] <= 1)
                    {
                        responses.Add(body);
                        totalSimilarity += body.Data["similarity"];
                    }
                }
            }

            var mean = totalSimilarity / responses.Count();

            Dictionary<string, dynamic> ret = new Dictionary<string, dynamic>();

            if (mean >= 0.5)
            {
                ret.Add("isSimilar", true);
            }
            else
            {
                ret.Add("isSimilar", false);
            }
            ret.Add("similarity", mean);

            return ret;
        }


    }
}
