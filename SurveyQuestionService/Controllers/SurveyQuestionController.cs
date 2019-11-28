using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        [HttpGet ]
        [Route("health")]
        public ActionResult GetHealthState()
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult Get()
        {
            /*Just for demonstration of Implementierung I – Discovery & Configuration
              required: Some sample Questions to show interaction between SurveyFormService 
              and SurveyQuestionService over SurveyMiscService. The SurveyMiscService is the 
              interface to CONSUL which manages the URLS of all services.
            */
            List<SurveyQuestion> _questions = getQuestionsFromPool();

            WriteLog("Found <" + _questions.Count + "> in question pool.");
            return Ok(_questions);
        }

        /**
         * Represents the reading of questions from a pool (e.g. CSV, DB, ...)
         * In our case a simple mock, just to demonstrate functionality of 
         * Implementierung I – Discovery & Configuration
         */
        private List<SurveyQuestion> getQuestionsFromPool()
        {
            List<SurveyQuestion> _questions = new List<SurveyQuestion>();
            _questions.Add(new SurveyQuestion(1, "Is this product cool?", true));
            _questions.Add(new SurveyQuestion(2, "Is this product ugly?", false));

            return _questions;
        }
        
        private async void WriteLog(string log)
        {
            //Call SurveyMiscService as central log service to write log
            try
            {
                HttpClient client = new HttpClient();
                await client.PostAsJsonAsync("https://localhost:44332/api/SurveyMISC/log", new LogEntry
                {
                    ServiceID = "SurveyFormController",
                    LogText = log
                });
            }
            catch (Exception e)
            {
                //Nobody cares about an execption while writing a log...
            }
        }
    }
}