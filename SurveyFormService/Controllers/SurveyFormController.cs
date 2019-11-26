using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SurveyFormService.Models;

namespace SurveyFormService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class SurveyFormController : ControllerBase
    {
        private ILogger<SurveyFormController> _logger;
        public SurveyFormController(ILogger<SurveyFormController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            
            List<SurveyForm> _surveys = new List<SurveyForm>();
            /*_surveys.Add(new SurveyForm(1001, "Is this product cool?", true));
            _surveys.Add(new SurveyForm(1002, "Is this product ugly?", false));
            */

            var json = JsonConvert.SerializeObject(_surveys);
            return Ok(json);
        }

        // POST api/values
        [HttpPost]
        public void Post(string SurveyName, int NumOfQuestions)
        {
            SurveyForm NewSurvey = new SurveyForm
            {
                SurveyName = SurveyName
            };

            //TODO Aufrufen des MISC Service um Fragen abzuholen. 


            int MaxNrOfQuestions = NumOfQuestions;
            _logger.LogError($"Creates a new survey <" + SurveyName+ ">with <" + MaxNrOfQuestions + "> questions");


        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] SurveyForm survey)
        {
            _logger.LogError($"TODO Creates a survey");
            //TODO Aufrufen des MISC Service um überpfüen ob die Fragen gibt 

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int surveyID)
        {
            _logger.LogError($"TODO remove survey");
        }
    }
}