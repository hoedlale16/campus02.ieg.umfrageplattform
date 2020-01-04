using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SurveyAnalyticService.Models;
using WebhookService.Models;

namespace WebhookService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookService : ControllerBase
    {
        private string repository = "campus02.ieg.umfrageplattform";
        private string githubOAuthToken = "edac383b97cc2f6588924a053d3108070ccc228a";
        private string secret = "secret";
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
            string hmac = ""; // get X-Hub-Signature
            string calculatedHmac = CalculateHmac(body);

            if (!hmac.Equals(calculatedHmac))
            {
                Log("Invalid webhook received: Signature does not match!");
                return BadRequest();
            }


            ProcessEvent(body);


            return new OkResult();
        }


        private void ProcessEvent(string content)
        {
            var pushEvent = WebhookPushEvent.FromJson(content);
            if (pushEvent == null)
            {
                Log("Cannot parse Event: Empty Request Body");
                return;
            }

            Pusher pusher = pushEvent.Pusher;
            string name = pusher.Name;

            Log("Webhook received: New Push from " + name);

            string referecnce = pushEvent.Ref;
            GithubCommit commit = FetchCommit(referecnce);
            File[] changedFiles = commit.Files;
            
            foreach (File changedFile in changedFiles)
            {
                string filename = changedFile.Filename;
                if (filename.Equals("surveyQuestionService.json"))
                {
                    Log("Changes to Consul-Configuration have been detected!");
                }
            }
        }

        private GithubCommit FetchCommit(string _ref)
        {
            string destination = "https://api.github.com/repos/" + repository + "/commits/" + _ref;
            HttpClient cli = new HttpClient();
            cli.DefaultRequestHeaders.UserAgent.TryParseAdd("c#app");
            cli.DefaultRequestHeaders.Add("Authorization", "Bearer " + githubOAuthToken);
            HttpResponseMessage response = cli.GetAsync(destination).Result;

            if (response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;
                return GithubCommit.FromJson(body);
            }
            
            Log("Unable to parse github push event: Commit could not be fetched!");
            return null;
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
                var responseString = response.Content.ReadAsStringAsync();
                Log("Webhook successfully registered!");
                return responseString.Result;
            }

            if (response.StatusCode.Equals(HttpStatusCode.UnprocessableEntity))
            {
                String errorMsg = "";
                if (message.Contains("Hook already exists"))
                {
                    errorMsg = "Webhook could not be created: Webhook already exists!";
                }
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


        private string CalculateHmac(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string CalculateSignature(string payload, string secret)
        {
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using (var hmSha256 = new HMACSHA256(secretBytes))
            {
                var hash = hmSha256.ComputeHash(payloadBytes);

                return ToHexString(hash);
            }
        }


        private static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
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