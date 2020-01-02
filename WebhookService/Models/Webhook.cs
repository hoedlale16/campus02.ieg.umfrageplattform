using SurveyAnalyticService.Models;
using WebhookService.Controllers;

namespace WebhookService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using WebhookService;


    public class Webhook
    {
        public string name { get; set; }

        public bool active { get; set; }

        public ArrayList events { get; set; }

        public WebhookConfig config { get; set; }


        public Webhook(string name, bool active, ArrayList events, WebhookConfig config)
        {
            this.name = name;
            this.active = active;
            this.events = events;
            this.config = config;
        }
    }
}