using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyMISCService.Model
{
    public class ConsulServicesState
    {
        public ConsulServicesState()
        {
        }
        public ConsulServicesState(string json)
        {
            JObject jObject = JObject.Parse(json);
            Node = (string)jObject["Node"];
            Name = (string)jObject["Name"];
            CheckID = (string)jObject["CheckID"];
            Status = (string)jObject["Status"];
            Notes = (string)jObject["Notes"];
            Output = (string)jObject["Output"];
            ServiceID = (string)jObject["ServiceID"];
            ServiceName = (string)jObject["ServiceName"];
        }

        public string Node { get; set; }
        public string Name { get; set; }
        public string CheckID { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Output { get; set; }
        public string ServiceID { get; set; }
        public string ServiceName { get; set; }
    }
}
