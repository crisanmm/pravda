using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using NewsStorage.Data;
using NewsStorage.Entities;
using System.Linq;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace NewsStorageService
{
    public class CachedClassificationsController : ControllerBase
    {
        private readonly IClassifiedRepository repository;

        public CachedClassificationsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        // [HttpPost]
        [FunctionName("GetPrediction")]
        public async Task<ActionResult<Dictionary<string, dynamic>>> GetPrediction([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/cached_classifications/getprediction")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CachedClassified classified = JsonConvert.DeserializeObject<CachedClassified>(requestBody);

            var check = this.repository.UpdateCache(classified);
            Dictionary<string, dynamic> dictionaries = new Dictionary<string, dynamic>();

            if (check != null)
            {
                dictionaries["Found"] = true;
                dictionaries["Title"] = check.Title;
                dictionaries["Text"] = check.Text;
                dictionaries["Subject"] = check.Subject;
                dictionaries["Date"] = check.Date;
                dictionaries["IsClassifiedFake"] = check.isClassifiedFake;
            }
            else
            {
                dictionaries["Found"] = false;
            }

            return Ok(dictionaries);
        }

        // [HttpPost("store")]
        [FunctionName("store")]
        public async Task<ActionResult> StoreArticle([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/cached_classifications/store")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CachedClassified classified = JsonConvert.DeserializeObject<CachedClassified>(requestBody);

            System.Console.WriteLine(classified.Title);
            System.Console.WriteLine(classified.Text);
            System.Console.WriteLine(classified.Subject);
            System.Console.WriteLine(classified.Date);
            System.Console.WriteLine(classified.isClassifiedFake);
            System.Console.WriteLine(classified.id);
            System.Console.WriteLine(classified.Today);
            System.Console.WriteLine(classified.Yesterday);
            System.Console.WriteLine(classified.Before_Yesterday);
            System.Console.WriteLine(classified.ResetTime);

            repository.Create(classified);

            return NoContent();
        }

        // [HttpGet]
        [FunctionName("get-cached_classifications")]
        public ActionResult<List<CachedClassified>> GetAllClassified([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/cached_classifications")] HttpRequest req)
        {
            return Ok(repository.GetAll().ToList());
        }

        // [HttpGet("{id}")]
        [FunctionName("get-id")]
        public ActionResult<CachedClassified> GetById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/cached_classifications/{id:int}")] HttpRequest req,
                                                                 int id)
        {
            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            return Ok(repository.GetById(id));
        }


        // [HttpDelete("{id}")]
        [FunctionName("delete-id")]
        public ActionResult DeleteById([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v1/cached_classifications/{id:int}")] HttpRequest req,
            int id)
        {
            repository.Remove(id);
            return NoContent();
        }
    }

    // [Route("api/v1/hotnews")]
    // [ApiController]
    public class HotnewsController : ControllerBase
    {

        private readonly IClassifiedRepository repository;

        private static readonly HttpClient http = new HttpClient();

        public HotnewsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("pravda")]
        [FunctionName("get-pravdanews")]
        public ActionResult<List<CachedClassified>> GetPravdaNews([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/hotnews/pravda")] HttpRequest req)
        {
            return Ok(repository.GetAll()
                   .Where(c => c.Today + c.Yesterday + c.Before_Yesterday >= 1 && (DateTime.Now - c.ResetTime).TotalHours <= 72)
                   .OrderByDescending(c => c.Today + c.Yesterday + c.Before_Yesterday)
                   .Take(10)
                   .ToList());
        }

        [HttpGet("google")]
        [FunctionName("get-googlenews")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetGoogleNews([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/hotnews/google")] HttpRequest req)
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
