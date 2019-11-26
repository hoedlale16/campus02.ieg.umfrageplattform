using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyFormService.Models
{
    public class SurveyForm
    {
        public String SurveyName { get; set; }

        public Dictionary<int,bool> QuestionAnswers { get; set; }
    }
}
