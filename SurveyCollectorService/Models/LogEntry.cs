using Newtonsoft.Json.Linq;
using System;

namespace SurveyCollectorService.Models
{
    public class LogEntry
    { 
        public LogEntry()
        {
        }
        public LogEntry(String json)
        {
            JObject jObject = JObject.Parse(json);
            ServiceID = (string)jObject["ServiceID"];
            LogText = (string)jObject["LogEntry"];
        }

        public String ServiceID { get; set; }
        public String LogText { get; set; }
    }
}
