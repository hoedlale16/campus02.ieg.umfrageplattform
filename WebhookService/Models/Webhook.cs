using System.Collections.Generic;

namespace SurveyAnalyticService.Models
{
    public class Webhook
    {
        private string name { get; set; }

        private bool active { get; set; }

        private List<string> events { get; set; }

        private WebhookConfig config { get; set; }

        public Webhook(string name, bool active, List<string> events, WebhookConfig config)
        {
            this.name = name;
            this.active = active;
            this.events = events;
            this.config = config;
        }
    }
}