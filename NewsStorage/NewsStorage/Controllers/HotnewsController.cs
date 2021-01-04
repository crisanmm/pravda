using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NewsStorage.Data;
using NewsStorage.Entities;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace NewsStorage.Controllers
{
    [Route("api/v1/hotnews")]
    [ApiController]
    public class HotnewsController : ControllerBase
    {

        private readonly IClassifiedRepository repository;

        private static readonly HttpClient http = new HttpClient();

        public HotnewsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("pravda")]
        public ActionResult<List<CachedClassified>> GetPravdaNews() => Ok(repository.GetAll()
                                                                            .Where(c => c.Today + c.Yesterday + c.Before_Yesterday >= 1 && (DateTime.Now - c.ResetTime).TotalHours <= 72)
                                                                            .OrderByDescending(c => c.Today + c.Yesterday + c.Before_Yesterday)
                                                                            .Take(10)
                                                                            .ToList());

        [HttpGet("google")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetGoogleNews()
        {
            string XmlString = await http.GetStringAsync("https://news.google.com/rss");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XmlString);

            // XML structure reference:
            // xml
            // rss
            //      channel
            //          generator
            //          title
            //          ...
            //          ...
            //          item
            //          item
            List<Dictionary<string, dynamic>> articles = new List<Dictionary<string, dynamic>>();
            XmlNodeList items = xmlDocument.SelectSingleNode("rss").SelectSingleNode("channel").SelectNodes("item");
            foreach (XmlNode item in items)
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
                dict.Add("Title", item.SelectSingleNode("title").InnerText);
                dict.Add("Link", item.SelectSingleNode("link").InnerText);
                dict.Add("Guid", item.SelectSingleNode("guid").InnerText);
                dict.Add("PubDate", DateTime.Parse(item.SelectSingleNode("pubDate").InnerText));
                dict.Add("Source", item.SelectSingleNode("source").InnerText);

                // process Description because it consists of HTML elements, extract the inner text
                HtmlDocument descriptionDocument = new HtmlDocument();
                string encodedHtml = item.SelectSingleNode("description").InnerXml;
                string decodedHtml = HtmlAgilityPack.HtmlEntity.DeEntitize(encodedHtml);
                descriptionDocument.LoadHtml(decodedHtml);
                dict.Add("Description", descriptionDocument.DocumentNode.QuerySelector("a").InnerText);

                articles.Add(dict);
            }

            return articles.ToList();
        }
    }
}
