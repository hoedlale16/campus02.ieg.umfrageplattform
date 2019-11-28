using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyCollectorService.Models
{
    public class ConsulCollectorDetails
    {
        public ConsulCollectorDetails() 
        {
        }

        public ConsulCollectorDetails(string json)
        {
            JObject jObject = JObject.Parse(json);
            LockIndex = (string)jObject["LockIndex"];
            Key = (string)jObject["Key"];
            Flags = (string)jObject["Flags"];
            Value = (string)jObject["Value"];
            CreateIndex = (string)jObject["CreateIndex"];
            ModifyIndex = (string)jObject["ModifyIndex"];
        }

        public string LockIndex { get; set; }
        public string Key { get; set; }
        public string Flags { get; set; }
        public string Value { get; set; }
        public string CreateIndex { get; set; }
        public string ModifyIndex { get; set; }
    }
}
