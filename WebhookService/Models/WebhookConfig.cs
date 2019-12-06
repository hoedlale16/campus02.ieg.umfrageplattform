namespace SurveyAnalyticService.Models
{
    public class WebhookConfig
    {

        public string url { get; set; }
        public string contentType{ get; set; }

        public string secret { get; set;}
        public bool insecureSsl{ get; set; }

        public WebhookConfig(string url, string contentType, string secret, bool insecureSsl)
        {
            this.url = url;
            this.contentType = contentType;
            this.secret = secret;
            this.insecureSsl = insecureSsl;
        }
        
        
    }
}