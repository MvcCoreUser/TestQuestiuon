using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Entities;

namespace TestingQuestions.BLL.ViewModels
{
    public class PersonQuestionAnswerView
    {
        public int QuestionId { get; set; }
        [Display(Name = "Вопрос")]
        public string QuestionDescription { get; set; }
        [Display(Name = "Вариант 1")]
        public string Answer1 { get; set; }
        [Display(Name ="Вариант 2")]
        public string Answer2 { get; set; }
        [Display(Name ="Вариант 3")]
        public string Answer3 { get; set; }
        public int RightAnswerNum { get; set; }
        public int AnswerNum { get; set; } = 0;
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }

        public static PersonQuestionAnswerView FromQuestion(Question question)
        {
            PersonQuestionAnswerView personQuestionAnswerView = new PersonQuestionAnswerView();
            personQuestionAnswerView.QuestionId = question.Id;
            personQuestionAnswerView.QuestionDescription = question.Description;
            personQuestionAnswerView.Answer1 = question.Answer1;
            personQuestionAnswerView.Answer2 = question.Answer2;
            personQuestionAnswerView.Answer3 = question.Answer3;
            return personQuestionAnswerView;
        }
    }
}
