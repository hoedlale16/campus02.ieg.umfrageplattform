using Newtonsoft.Json.Linq;
using System;

namespace SurveyFormService.Models
{
    public class SurveyQuestion
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public Boolean Answer { get; set; }

        public SurveyQuestion()
        {

        }
        public SurveyQuestion(int questionID, String question, Boolean answer)
        {
            this.QuestionID = questionID;
            this.Question = question;
            this.Answer = answer;
        }

        public SurveyQuestion(String json)
        {
            JObject jObject = JObject.Parse(json);
            QuestionID = (int)jObject["QuestionID"];
            Question = (string)jObject["Question"];
            Answer = (bool)jObject["Answer"];
        }

    }
}
