using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SurveyAnalyticService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SurveyAnalyticService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class SurveyAnalyticController : ControllerBase
    {

        private static string reqiredUser = "AnalyticUser";
        private static string reqiredPassword = "TheSecretPassword";

        [HttpGet]
        public ActionResult Get()
        {
            var headers = this.Request.Headers;

            StringValues values = "Empty";
            headers.TryGetValue("Credentials", out values);

            if (String.IsNullOrEmpty(values)) { 
                //No Credentioals 
                return BadRequest("No Credentials given!");
            }

            var credentials = new SurveyCredentials(values);

            var username = credentials.User;
            var password = credentials.Password;

            if (username.Equals(reqiredUser) && password.Equals(reqiredPassword))
            {
                // Business logic
                return Ok();
            }
            return BadRequest();
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
                // --> da wird die Exception aber traurig sein 😟🐐
            }
        }
    }
}
