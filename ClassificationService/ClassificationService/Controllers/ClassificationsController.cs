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

            var classified = new Classified(
                modelInput.Title,
                modelInput.Text,
                modelInput.Subject,
                modelInput.Date,
                predictionResult.Prediction == "Fake" ? true : false
            );

            // TO DO
            // verificare ca nu exista deja in baza de date
            repository.Create(classified);


            ret.Add("title", classified.Title);
            ret.Add("text", classified.Text);
            ret.Add("subject", classified.Subject);
            ret.Add("date", classified.Date);
            ret.Add("isClassifiedFake", classified.isClassifiedFake);
            ret.Add("id", classified.id);
            // ret.Add("score", predictionResult.Score.ToString());

            return ret;
        }

        [HttpGet]
        public ActionResult<List<Classified>> GetAllClassified() => repository.GetAll().ToList();

        [HttpGet("{id}")]
        public ActionResult<Classified> GetById(int id) => repository.GetById(id);
    }
}
