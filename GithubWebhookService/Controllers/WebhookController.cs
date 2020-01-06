using GithubWebhookService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static Microsoft.AspNetCore.WebSockets.Internal.Constants;
using Microsoft.Extensions.Primitives;
using System.IO;

namespace GithubWebhookService.Controllers
{
    [Route("api/WebhookController")]
    [ApiController]
    public class WebhookController : ControllerBase
    {

        private string repository = "campus02.ieg.umfrageplattform";
        private string repoOwner = "hoedlale16";
        private string githubOAuthToken = "95b06ea64a4ff5739e5f10cd1490a51640289e9f";
        private string secret = "secret";
        

        [HttpPost]
        [Route("event")]
        public ActionResult Receive([FromBody] WebhookPushEvent _event)
        {
            StringValues signature;
            string stringContent = _event.ToJson();
            
            try
            {
                bool signatureExists = Request.Headers.TryGetValue("X-Hub-Signature", out signature);

                if (!signatureExists)
                {
                    throw new ArgumentNullException();
                }

            }
            catch (ArgumentNullException ex)
            {
                Log("Could not verify integrity of received webhook! " + ex);
                //Console.WriteLine("Missing Siugnature!");
                return NotFound();
            }

            string calculatedHmac = CalculateHmac(stringContent);
            
            signature = signature.ToString().Replace("{", "").Replace("}", "");

            if (!signature.ToString().Equals(calculatedHmac))
            {
                Log("Webhook could not be verified - returning 404");
                //Console.WriteLine("Webhook could not be verified - returning 404");
                return NotFound();
            }

            ProcessEvent(_event);

            return Ok();

        }



        private void ProcessEvent(WebhookPushEvent _event)
        {
            

            Pusher pusher = _event.Pusher;
            string name = pusher.Name;

            Log("Webhook received: New Push from " + name);
            string commitSha = _event.HeadCommit; 
            GithubCommit commit = FetchCommit(commitSha);
            if(commit == null) { return; }

            GithubWebhookService.Models.File[] changedFiles = commit.Files;

            foreach (GithubWebhookService.Models.File changedFile in changedFiles)
            {
                string filename = changedFile.Filename;
                if (filename.Equals("surveyQuestionService.json"))
                {

                    Log("Changes to Consul-Configuration have been detected!");
                    Console.WriteLine("Changes to Consul-Configuration have been detected!");
                }
            }
        }

        private GithubCommit FetchCommit(string _ref)
        {
            if(_ref == null)
            {
                Log("Unable to fetch commit: Head - Reference was null");
                Console.WriteLine("Unable to fetch commit: Head-Reference was null");
                return null;
            }

            string destination = "https://api.github.com/repos/" + repoOwner + "/" + repository + "/commits/" + _ref;
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
            //Console.WriteLine("Unable to parse github push event: Commit could not be fetched!");
            return null;
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
                //Console.WriteLine("Webhook successfully registered!");
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
                Console.WriteLine(errorMsg);
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

        public async void Log(string msg)
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
