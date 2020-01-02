using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SurveyAnalyticService.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



//TODO change name to Hooker(s)
namespace WebhookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookService : ControllerBase
    {
        private string githubOAuthToken = "edac383b97cc2f6588924a053d3108070ccc228a";
        private string destinationUrl;
        private string endpointUrl;
        private readonly HttpClient client;


        public WebhookService(string destinationUrl, string endpointUrl)
        {
            this.destinationUrl = destinationUrl;
            this.endpointUrl = endpointUrl;
            this.client = new HttpClient();
        }

        [HttpPost]
        [Route("api/event")]
        public ActionResult Receive([FromBody] string body)
        {
            Console.WriteLine("Received request!");
            Console.WriteLine(body);
            return new OkResult();

        }

        private async void ProcessEvent()
        {
            
            
        }


        public Webhook CreateWebhook(string name, bool active, ArrayList events, WebhookConfig config)
        {
            return new Webhook(name, active, events, config);
        }
        

        public string RegisterWebhook(string destination, Webhook webhook)
        {
            string body = ConvertToJsonString(webhook);
            
            HttpContent content = new StringContent(body);
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.UserAgent.TryParseAdd("c#app");
            cli.DefaultRequestHeaders.Add("Authorization", "Bearer " + githubOAuthToken);
            HttpResponseMessage response = cli.PostAsync(destination, content).Result; 
            string message = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                // Get the response
                var responseString =  response.Content.ReadAsStringAsync();
                Log("Webhook successfully registered!");
                return responseString.Result;
            }
            
            if (response.StatusCode.Equals(HttpStatusCode.UnprocessableEntity))
            {
                String errorMsg = "";
                if (message.Contains("Hook already exists")) { errorMsg = "Webhook could not be created: Webhook already exists!"; }
                else errorMsg = "Webhook could not be created: " + response.Content.ReadAsStringAsync().Result;
                
                Log(errorMsg);
                
            } 
            
            return null;
            
        }

        
        private string ConvertToJsonString(Webhook webhook)
        {
            string result = JsonConvert.SerializeObject(webhook);
            return result;
        }
        
        
        private async void Log(string msg)
        {
            HttpClient cli = new HttpClient();
            await cli.PostAsJsonAsync("https://localhost:44332/api/SurveyMISC/log", new LogEntry
            {
                ServiceID = "WebhookServiceController",
                LogText = msg
            });
        }
    }
}