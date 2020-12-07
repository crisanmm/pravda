using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ClassificationService.Model;
using ClassificationService.Data;
using ClassificationService.Entities;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassificationService.Controllers
{
    [Route("api/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        private readonly IClassifiedRepository repository;

        public ClassificationsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, dynamic>> GetPrediction(ModelInput modelInput)
        {
            var predictionResult = ConsumeModel.Predict(modelInput);

            Dictionary<string, dynamic> ret = new Dictionary<string, dynamic>();
            bool isClassifiedFake = predictionResult.Prediction == "Fake" ? true : false;

            // TO DO
            // verificare ca nu exista deja in baza de date
            repository.Create(new Classified(
                modelInput.Title,
                modelInput.Text,
                modelInput.Subject,
                modelInput.Date,
                modelInput.Type,
                isClassifiedFake
            ));


            ret.Add("title", modelInput.Title);
            ret.Add("text", modelInput.Text);
            ret.Add("subject", modelInput.Subject);
            ret.Add("date", modelInput.Date);
            ret.Add("type", modelInput.Type);
            ret.Add("isClassifiedFake", isClassifiedFake);
            // ret.Add("score", predictionResult.Score.ToString());

            return ret;
        }

        [HttpGet]
        public ActionResult<List<Classified>> GetAllClassified() => repository.GetAll().ToList();

        [HttpGet("{id}")]
        public ActionResult<Classified> GetById(int id) => repository.GetById(id);
    }
}
