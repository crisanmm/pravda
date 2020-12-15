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
        public ActionResult<Dictionary<string, bool>> GetPrediction(ModelInput modelInput)
        {
            var predictionResult = ConsumeModel.Predict(modelInput);

            bool isClassifiedFake = predictionResult.Prediction == "Fake" ? true : false;

            var ret = new Dictionary<string, bool>
            {
                {"isClassifiedFake", isClassifiedFake}
            };
            return ret;
        }

    }
}
