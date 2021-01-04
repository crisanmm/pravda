using Microsoft.AspNetCore.Mvc;
using ClassificationService.Model;
using System.Collections.Generic;

namespace ClassificationService.Controllers
{
    [Route("api/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<Dictionary<string, dynamic>> GetPrediction(ModelInput modelInput)
        {
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
