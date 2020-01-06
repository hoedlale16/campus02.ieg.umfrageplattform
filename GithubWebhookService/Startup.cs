using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubWebhookService.Controllers;
using GithubWebhookService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace GithubWebhookService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<WebhookController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            init();

        }

        public void init()
        {
            WebhookController webhookService = new Controllers.WebhookController();
            
            webhookService.Log("Starting Application...");

            ArrayList eventList = new ArrayList();
            eventList.Add("deployment");

            WebhookConfig config = new WebhookConfig("https://cd4dc1d6.ngrok.io/api/WebhookController/event", "json", "secret", false);
            Webhook webhook = new Webhook("web", true, eventList, config);

            webhookService.Log("Trying to register Webhook...");
            webhookService.RegisterWebhook("https://api.github.com/repos/hoedlale16/campus02.ieg.umfrageplattform/hooks", webhook);
            //webhookService.RegisterWebhook("https://api.github.com/repos/MrPink1992/web/hooks", webhook);
            webhookService.Log("Webhook registration finished");

        }
    }
}
