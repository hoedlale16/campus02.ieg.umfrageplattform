using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        //Single Point of Failure. Wenn CONSUL nicht erreichbar ist alles aus.
        private static readonly string _surveyMiscServiceURL = "https://localhost:44332/api/SurveyMISC";

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
        public ActionResult Post(string surveyName, int numOfQuestions)
        {
            //Aufrufen des MISC Service um URLs für maximal verfügbare Fragenanzahl zu erhalten. 
            //Das MISCService liefert dazu die verfügbaren URLs des SurveyQuestionServices
            List<SurveyQuestion> availableQuestions = new List<SurveyQuestion>();
            string surveyQuestionServiceURL = GetFirstSurveyQuestionServiceURL();
            if (surveyQuestionServiceURL != null)
            {
                //Wir nehmen immer die erste funktionierende URL und fragen dort den 
                //verfügbaren Fragenpool ab. Die zurückgelieferte Summe an Fragen ist 
                //die maximale Fragenanzahl
                availableQuestions = GetAvailableQuestions(surveyQuestionServiceURL);
            }

            //Check ob Fragen im Pool vorhanden sind.
            if(availableQuestions == null || availableQuestions.Count <= 0)
            {
                WriteLog("Received no available questions from surveyQuestionService - Maybe down or no questions in pool?");
                return BadRequest();
            }

            //Fragen vorhanden: Wenn vorhandene Fragen < als Geforderte müssen diese reduziert werden
            if (availableQuestions.Count < numOfQuestions)
            {
                numOfQuestions = availableQuestions.Count;
                WriteLog($"Not enough questions available - Must reduce them to  <" + numOfQuestions + ">");
            }

            //Fragen für Umfragen erstellen:
            Dictionary<int, bool> surveyQuestions = new Dictionary<int, bool>();
            for (int i = 0; i < numOfQuestions; i ++)
            {
                SurveyQuestion q = availableQuestions.ElementAt(i);
                surveyQuestions.Add(q.QuestionID, true);
            }

            //Umfrage erstellen
            SurveyForm NewSurvey = new SurveyForm
            {
                SurveyName = surveyName,
                QuestionAnswers = surveyQuestions
            };

            WriteLog($"Creates a new survey <" + NewSurvey.SurveyName + ">with <" + NewSurvey.QuestionAnswers.Count + "> questions");


            //Sollte eigtl Created zurückliefern, kennt C# aber irgendwie nicht...
            return Ok(JsonConvert.SerializeObject(NewSurvey));
        }


        private List<SurveyQuestion> GetAvailableQuestions(String surveyQuestionServiceURL)
        {
            
            List<SurveyQuestion> availableQuestions = new List<SurveyQuestion>();
            HttpClient client = new HttpClient();
            try
            {
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(surveyQuestionServiceURL).Result;
                if (response.IsSuccessStatusCode)
                {
                    availableQuestions = response.Content.ReadAsAsync<List<SurveyQuestion>>().Result;
                }
            }catch (Exception e)
            {
                WriteLog("Error(" + e.Message + ") while retrieving questions from  <" + surveyQuestionServiceURL + ">");
            } 

            return availableQuestions;
        }

        /**
         * A3 - Einsatz eines KonfigurationsServices (MISC) um Adressen von Services zu Verwalten
         */
        private string GetFirstSurveyQuestionServiceURL()
        {
            try { 
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(_surveyMiscServiceURL + "/SurveyQuestionService").Result;
                if (response.IsSuccessStatusCode)
                {
                    List<string> acceptedSurveyQuestionServiceURLs = response.Content.ReadAsAsync<List<string>>().Result;


                    if (acceptedSurveyQuestionServiceURLs != null && 
                        acceptedSurveyQuestionServiceURLs.Count > 0)
                    {
                        return acceptedSurveyQuestionServiceURLs[0];
                    }
                }
                } catch (Exception e)
                {
                    WriteLog("Error(" + e.Message + ") while get surveyQuestionURL");
                }

            return null ;
        }

        private async void WriteLog(string log)
        {
            //Call SurveyMiscService as central log service to write log
            try
            {
                HttpClient client = new HttpClient();
                await client.PostAsJsonAsync(_surveyMiscServiceURL + "/log", new LogEntry {
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