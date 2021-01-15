using System.IO;
using Microsoft.AspNetCore.Mvc;
using ClassificationService.Model;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ClassificationService
{
    // public static class classif
    // {
    //     [FunctionName("classif")]
    //     public static async Task<IActionResult> Run(
    //         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    //         ILogger log)
    //     {
    //         log.LogInformation("C# HTTP trigger function processed a request.");

    //         string name = req.Query["name"];

    //         string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    //         dynamic data = JsonConvert.DeserializeObject(requestBody);
    //         name = name ?? data?.name;

    //         string responseMessage = string.IsNullOrEmpty(name)
    //             ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
    //             : $"Hello, {name}. This HTTP triggered function executed successfully.";

    //         return new OkObjectResult(responseMessage);
    //     }
    // }
    [Route("api/v1/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        [HttpPost]
        [FunctionName("classify")]
        public async Task<ActionResult<Dictionary<string, dynamic>>> GetPrediction([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/classifications")] HttpRequest req,
                            ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory);
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ModelInput modelInput = JsonConvert.DeserializeObject<ModelInput>(requestBody);

            var predictionResult = ConsumeModel.Predict(modelInput);

            bool isClassifiedFake = predictionResult.Prediction == "Fake" ? true : false;

            var ret = new Dictionary<string, dynamic>
            {
                {"isClassifiedFake", isClassifiedFake},
                {"score", (1 - predictionResult.Score[0]) }
            };
            return ret;
        }

    }
}
