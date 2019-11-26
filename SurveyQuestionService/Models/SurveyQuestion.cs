using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyQuestionService.Models
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
    }
}
