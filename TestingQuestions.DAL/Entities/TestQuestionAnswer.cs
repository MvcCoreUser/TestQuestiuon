using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Entities
{
    public class TestQuestionAnswer
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int AnswerNum { get; set; }
        public int TestResultId { get; set; }
        public TestResult TestResult { get; set; }
    }
}
