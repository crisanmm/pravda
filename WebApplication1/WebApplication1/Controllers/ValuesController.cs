using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebApplication1ML.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpPost("getPrediction")]
        public ActionResult<ModelOutput> GetPrediction(ModelInput modelInput)
        {
            var predictionResult = ConsumeModel.Predict(modelInput);

            Dictionary<string, string> ret = new Dictionary<string, string>();

            ret.Add("prediction", predictionResult.Prediction);
            ret.Add("score", predictionResult.Score.ToString());
            // JObject json = JObject.Parse(ret);

            return predictionResult;
        }
    }
}
