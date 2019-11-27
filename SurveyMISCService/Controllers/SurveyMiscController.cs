using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyMISCService.Model;

namespace SurveyMISCService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class SurveyMiscController : ControllerBase
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _consulURL = "http://localhost:8500";

        private ILogger<SurveyMiscController> _logger;
        public SurveyMiscController(ILogger<SurveyMiscController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("health")]
        public ActionResult GetHealthState()
        {
            return Ok();
        }

        [HttpGet("{serviceID}")]
        public IActionResult Get(string serviceID)
        {
            List<string> _availableServiceUrls = null;

            //Call Consul stuff to retreive serviceurls
            _availableServiceUrls = GetUrlsForService(serviceID).Result;

            if(_availableServiceUrls != null && _availableServiceUrls.Count > 0)
            {
                return Ok(_availableServiceUrls);
            }

            return BadRequest();
        }

        private async Task<List<string>> GetUrlsForService(String serviceID)
        {
            List<string> urls = new List<string>();
            try
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                var filterPassing = "/v1/health/state/passing?filter=ServiceID==";
                //http://127.0.0.1:8500/v1/health/state/passing?filter=ServiceID==SurveyQuestionService
                HttpResponseMessage response = await _client.GetAsync(_consulURL + filterPassing + serviceID);
                if (response.IsSuccessStatusCode)
                {

                    List<ConsulServicesState> states = await response.Content.ReadAsAsync<List<ConsulServicesState>>();
                    foreach (var state in states)
                    {
                        if (state.Name != null)
                        {
                            urls.Add(state.Name);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("An error occurred connecting to SimpleCreditCartServiceClient");
            }

            //Fallback return empty list
            return urls;
        }
    }
}