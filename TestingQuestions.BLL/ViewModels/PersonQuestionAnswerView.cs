using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.BLL.ViewModels
{
    public class PersonQuestionAnswerView
    {
        public string QuestionDescription { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public int RightAnswerNum { get; set; }
        public int AnswerNum { get; set; }
    }
}
