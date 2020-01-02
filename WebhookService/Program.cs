using System;
using System.Collections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using WebhookService.Controllers;

namespace WebhookService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            Controllers.WebhookService webhookService = new Controllers.WebhookService("http://www.github.com/", "test.at/2");
            ArrayList eventList = new ArrayList();
            eventList.Add("deployment");
            
            WebhookConfig config = new WebhookConfig("http://ngrok.io", "json", "secret",false);
            Webhook webhook = webhookService.CreateWebhook("web", true, eventList, config);

            webhookService.RegisterWebhook("https://api.github.com/repos/MrPink1992/web/hooks", webhook);
            Console.WriteLine("Webhook verification finished");
            
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}