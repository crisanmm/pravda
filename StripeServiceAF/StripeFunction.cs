using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stripe;

namespace SripeFunction
{
    public class PaymentIntentApiController : Controller
    {
        [FunctionName("create-payment-intent")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/create-payment-intent")] HttpRequest req)
        {
            // PaymentIntentCreateRequest request)
            // System.Console.WriteLine("something");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // System.Console.WriteLine(requestBody.ToString());
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(data.Items),
                Currency = "usd",
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        private int CalculateOrderAmount(Item[] items)
        {
            return 500;
        }

        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }
    }
}
