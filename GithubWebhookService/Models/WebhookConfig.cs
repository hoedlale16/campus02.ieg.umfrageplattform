using Newtonsoft.Json;

namespace GithubWebhookService.Controllers
{
    public class WebhookConfig
    {

        public string url { get; set; }
        
        [JsonProperty("content_type")]
        public string contentType{ get; set; }

        public string secret { get; set;}
        
        [JsonProperty("insecure_ssl")]
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