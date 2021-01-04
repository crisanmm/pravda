using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NewsStorage.Data;
using NewsStorage.Entities;
using System.Linq;
using System.Net.Http;
using System.Dynamic;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsStorage.Controllers
{
    [Route("api/v1/cached_classifications")]
    [ApiController]
    public class CachedClassificationsController : ControllerBase
    {
        private readonly IClassifiedRepository repository;

        public CachedClassificationsController(IClassifiedRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, dynamic>> GetPrediction(CachedClassified classified)
        {

            var check = this.repository.UpdateCache(classified);
            Dictionary<string, dynamic> dictionaries = new Dictionary<string, dynamic>();

            if (check != null)
            {
                dictionaries["Found"] = true;
                dictionaries["Title"] = check.Title;
                dictionaries["Text"] = check.Text;
                dictionaries["Subject"] = check.Subject;
                dictionaries["Date"] = check.Date;
                dictionaries["IsClassifiedFake"] = check.isClassifiedFake;
            }
            else
            {
                dictionaries["Found"] = false;
            }

            return Ok(dictionaries);
        }

        [HttpPost("store")]
        public ActionResult StoreArticle(CachedClassified classified)
        {
            repository.Create(classified);

            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<CachedClassified>> GetAllClassified() => Ok(repository.GetAll().ToList());

        [HttpGet("{id}")]
        public ActionResult<CachedClassified> GetById(int id) => Ok(repository.GetById(id));

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            repository.Remove(id);
            return NoContent();
        }

        // [HttpGet("hotnews")]
        // public ActionResult<List<CachedClassified>> GetHotNews() => Ok(repository.GetAll()
        //                                                                     .Where(c => c.Today + c.Yesterday + c.Before_Yesterday >= 1 && (DateTime.Now - c.ResetTime).TotalHours <= 72)
        //                                                                     .OrderByDescending(c => c.Today + c.Yesterday + c.Before_Yesterday)
        //                                                                     .Take(10)
        //                                                                     .ToList());
    }
}
