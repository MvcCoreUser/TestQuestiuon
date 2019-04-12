using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Entities
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public ICollection<TestQuestionAnswer>TestQuestionAnswers { get; set; }
        public TestResult()
        {
            TestQuestionAnswers = new List<TestQuestionAnswer>();
        }
    }
}
