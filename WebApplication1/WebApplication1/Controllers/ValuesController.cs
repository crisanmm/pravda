using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1ML.Model;
using WebApplication1.Data;
using WebApplication1.Entities;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/classifications")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IClassifiedRepository repository;

        public ValuesController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, string>> GetPrediction(ModelInput modelInput)
        {
            var predictionResult = ConsumeModel.Predict(modelInput);

            Dictionary<string, string> ret = new Dictionary<string, string>();

            // TO DO
            // verificare ca nu exista deja in baza de date
            repository.Create(new Classified(
                modelInput.Title, 
                modelInput.Text,
                modelInput.Subject, 
                modelInput.Date, 
                modelInput.Type, 
                predictionResult.Prediction == "Fake" ? true : false
            ));

            ret.Add("Text", modelInput.Text);
            ret.Add("isClassifiedFake", predictionResult.Prediction);
            // ret.Add("score", predictionResult.Score.ToString());

            return ret;
        }

        [HttpGet]
        public ActionResult<List<Classified>> GetAllClassified() => repository.GetAll().ToList();

        [HttpGet("{id}")]
        public ActionResult<Classified> GetById(int id) => repository.GetById(id);
    }
}
