using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SurveyAnalyticService.Models;

namespace WebhookService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            Controllers.WebhookService webhookService = new Controllers.WebhookService("http://www.github.com/", "test.at/2");
            ArrayList eventList = new ArrayList();
            eventList.Add("hallo");
            eventList.Add("eliška");
            
            WebhookConfig config = new WebhookConfig("test.at", "json", "secret",false);
            Webhook webhook = webhookService.CreateWebhook("CaptainHook", true, eventList, config);
            webhookService.RegisterWebhooks(webhook);
            
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}