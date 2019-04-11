using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Entities
{
    public class PersonQuestionAnswer
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int? AnswerNum { get; set; }
    }
}
