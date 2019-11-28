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

        private ILogger<SurveyAnalyticController> _logger;
        public SurveyAnalyticController(ILogger<SurveyAnalyticController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get()
        {
            var headers = this.Request.Headers;

            StringValues values = "Empty";
            headers.TryGetValue("Credentials", out values);


            //var convValues = Newtonsoft.Json.JsonConvert.DeserializeObject(values);

            var credentiaaaaals = new SurveyCredentials(values);

           _logger.LogError(credentiaaaaals.User);
           _logger.LogError(credentiaaaaals.Password);
            return Ok();
        }
    }
}
