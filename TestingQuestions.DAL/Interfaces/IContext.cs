using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Interfaces
{
    public interface IRepositoryContext: IDisposable
    {
        IPersonRepository PersonRepository { get; }
        IQuestionRepository QuestionRepository { get;}
        ITestQuestionAnswerRepository TestQuestionAnswerRepository{ get;}
        ITestResultRepository TestResultRepository { get; }

        Task<int> SaveAsync();
    }
}
