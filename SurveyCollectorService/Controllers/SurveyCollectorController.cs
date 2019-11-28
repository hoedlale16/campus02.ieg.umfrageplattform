using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyCollectorService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SurveyCollectorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class SurveyCollectorController : ControllerBase
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _consulURL = "http://localhost:8500";

        [HttpGet]
        public ActionResult Get()
        {
            String _credentials = null;
            String result = null;

            //Call Consul stuff to retreive serviceurls
            _credentials = GetCredentials().Result;

            if (_credentials != null)
            {
                result = GetAnalytic(_credentials).Result;
                return Ok(result);
            }

            return BadRequest();
        }

        private async Task<string> GetCredentials()
        {
            String credentials = null;
            String encoded = null;
            try
            {
                _client.DefaultRequestHeaders.Accept.Clear();
                var filterPassing = "/v1/kv/User";
                //http://127.0.0.1:8500/v1/kv/User
                HttpResponseMessage response = await _client.GetAsync(_consulURL + filterPassing);
                if (response.IsSuccessStatusCode)
                {
                    List<ConsulCollectorDetails> states = await response.Content.ReadAsAsync<List<ConsulCollectorDetails>>();
                    foreach (var state in states)
                    {
                        if (state.Value != null)
                        {
                            byte[] data = System.Convert.FromBase64String(state.Value);
                            encoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
                            credentials = encoded;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                WriteLog("An error occurred connecting to Consul");
            }

            //Fallback return empty list
            return credentials;
        }

        private async Task<string> GetAnalytic(string credentials)
        {
            String result = null;
            try
            {
                //object json = Newtonsoft.Json.JsonConvert.DeserializeObject(credentials);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Add("Credentials", credentials);
                //var filterPassing = "/v1/kv/User";
                //http://127.0.0.1:8500/v1/kv/User
                HttpResponseMessage response = await _client.GetAsync("https://localhost:44329/api/SurveyAnalytic");
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }

            }
            catch (HttpRequestException ex)
            {
                result = "Mäh";
                WriteLog("An error occurred connecting to Consul");
            }

            //Fallback return empty list
            return result;
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
