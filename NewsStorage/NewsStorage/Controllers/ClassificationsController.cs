using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ClassificationService.Data;
using ClassificationService.Entities;
using System.Linq;
using System.Net.Http;
using System.Dynamic;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClassificationService.Controllers
{
    [Route("api/classifications")]
    [ApiController]
    public class ClassificationsController : ControllerBase
    {

        private readonly IClassifiedRepository repository;

        private static readonly HttpClient client = new HttpClient();

        public ClassificationsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, dynamic>> GetPrediction(Classified classified)
        {

            var check = this.repository.CheckExistence(classified);
            Dictionary<string, dynamic> dictionaries = new Dictionary<string, dynamic>();

            if (check != null)
            {                
                dictionaries["Found"] = true;
                dictionaries["Title"] =  check.Title;
                dictionaries["Text"] = check.Text;
                dictionaries["Subject"] = check.Subject;
                dictionaries["Date"] = check.Date;
            }
            else
            {
                dictionaries["Found"] = false;
            }

            return Ok(dictionaries);
        }

        [HttpPost("storeArticle")]
        public ActionResult StoreArticle(Classified classified)
        {
            repository.Create(classified);

            return NoContent();
        }


        [HttpGet]
        public ActionResult<List<Classified>> GetAllClassified() => Ok(repository.GetAll().ToList());

        [HttpGet("{id}")]
        public ActionResult<Classified> GetById(int id) => Ok(repository.GetById(id));

        [HttpGet("getHotNews")]
        public ActionResult<List<Classified>> GetHotNews() => Ok(repository.GetAll().Where(c => c.Today + c.Yesterday + c.Before_Yesterday > 1 && (DateTime.Now - c.ResetTime).TotalHours < 72).OrderByDescending(c=> c.Today + c.Yesterday + c.Before_Yesterday).Take(10).ToList());
    }
}
