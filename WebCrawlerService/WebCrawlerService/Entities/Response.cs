using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawlerService.Entities
{
    public class Response
    {
        public Dictionary<string, double> Data { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
    }
}
