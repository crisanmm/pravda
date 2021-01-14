using System.Collections.Generic;

namespace WebCrawlerService.Entities
{
    public class Response
    {
        public Dictionary<string, double> Data { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
    }
}
