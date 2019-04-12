using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        [StringLength(50)]
        public string Answer1 { get; set; }
        [StringLength(50)]
        public string Answer2 { get; set; }
        [StringLength(50)]
        public string Answer3 { get; set; }
        public int RightAnswerNum { get; set; }

        public ICollection<TestQuestionAnswer> TestQuestionAnswers{ get; set; }
        public Question()
        {
            TestQuestionAnswers = new List<TestQuestionAnswer>();
        }
    }
}
