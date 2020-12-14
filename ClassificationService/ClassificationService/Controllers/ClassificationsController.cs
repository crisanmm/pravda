using Microsoft.AspNetCore.Mvc;
using ClassificationService.Model;

namespace ClassificationService.Controllers
{
    [Route("api/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<bool> GetPrediction(ModelInput modelInput)
        {

            var predictionResult = ConsumeModel.Predict(modelInput);

            bool isClassifiedFake = predictionResult.Prediction == "Fake" ? true : false;

            return predictionResult.Prediction == "Fake" ? true : false;


        }



    }
}
