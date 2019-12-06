using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyMiscService.Models
{
    public class SurveyCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }

        public SurveyCredentials()
        {
        }

        public SurveyCredentials(String user, String password)
        {
            this.User = user;
            this.Password = password;
        }

        public SurveyCredentials(String json)
        {
            JObject jObject = JObject.Parse(json);
            User = (string)jObject["user"];
            Password = (string)jObject["password"];
        }
    }
}
