
using GithubWebhookService.Controllers;

namespace GithubWebhookService.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using GithubWebhookService;


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