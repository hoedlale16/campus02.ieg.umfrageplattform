using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SurveyAnalyticService.Models;
using WebhookService.Models;
using Newtonsoft.Json.Linq;

namespace WebhookService.Controllers
{
    public class WebhookService
    {
        private string destinationUrl;
        private string endpointUrl;
        private readonly HttpClient client;


        public WebhookService(string destinationUrl, string endpointUrl)
        {
            this.destinationUrl = destinationUrl;
            this.endpointUrl = endpointUrl;
            this.client = new HttpClient();
        }


        public Webhook CreateWebhook(string name, bool active, ArrayList events, WebhookConfig config)
        {
            return new Webhook(name, active, events, config);
        }

        public void RegisterWebhooks(Webhook webhook)
        {
            string jsonWebhook = ConvertToJsonString(webhook);
            string responseBody = SendRequest(jsonWebhook);

            if (responseBody == null)
            {
                //TODO log error                    
            }
            else
            {
            } //TODO log success
        }

        private string ConvertToJsonString(Webhook webhook)
        {
            string result = JsonConvert.SerializeObject(webhook);
            return result;
        }


        private string SendRequest(string content)
        {
            // build request body
            StringContent body = new StringContent(content, Encoding.UTF8, "application/json");

            // declare request method and type
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(this.destinationUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            // add body to request
            StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(body);

            // send request and get response
            HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();
            HttpStatusCode status = httpResponse.StatusCode;
            var responseStream = httpResponse.GetResponseStream();

            // read response body
            if (status == HttpStatusCode.OK && responseStream != null)
            {
                var streamReader = new StreamReader(responseStream);
                return streamReader.ReadToEnd();
            }

            return null;
        }
    }
}