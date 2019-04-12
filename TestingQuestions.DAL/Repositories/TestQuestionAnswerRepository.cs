using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Entities;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.DAL.Repositories
{
    public class TestQuestionAnswerRepository : BaseRepository<TestQuestionAnswer>, ITestQuestionAnswerRepository
    {
        public TestQuestionAnswerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
