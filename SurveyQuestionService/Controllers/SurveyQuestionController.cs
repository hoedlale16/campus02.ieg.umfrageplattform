using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SurveyQuestionService.Models;

namespace SurveyQuestionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class SurveyQuestionController : ControllerBase
    {

        

        private ILogger<SurveyQuestionController> _logger;
        public SurveyQuestionController(ILogger<SurveyQuestionController> logger)
        {   
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get()
        {
            List<SurveyQuestion> _questions = new List<SurveyQuestion>();

            _questions.Add(new SurveyQuestion(1, "Is this product cool?", true));
            _questions.Add(new SurveyQuestion(2, "Is this product ugly?", false));

            var json = JsonConvert.SerializeObject(_questions);
            return Ok(json);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] SurveyQuestion question)
        {
            _logger.LogError($"TODO add new question");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int questionID)
        {
            _logger.LogError($"TODO remove given question with ID <" + questionID + ">");
        }
    }
}