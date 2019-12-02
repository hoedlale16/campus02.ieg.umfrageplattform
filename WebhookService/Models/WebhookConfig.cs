namespace SurveyAnalyticService.Models
{
    public class WebhookConfig
    {

        private string url { get; set; }
        private string contentType{ get; set; }
        private bool insecureSsl{ get; set; }

        public WebhookConfig(string url, string contentType, bool insecureSsl)
        {
            this.url = url;
            this.contentType = contentType;
            this.insecureSsl = insecureSsl;
        }
        
        
    }
}