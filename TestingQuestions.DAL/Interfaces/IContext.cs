using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Interfaces
{
    public interface IContext: IDisposable
    {
        IPersonRepository PersonRepository { get; }
        IQuestionRepository QuestionRepository { get;}
        IPersonQuestionAnswerRepository PersonQuestionAnswerRepository{ get;}

        Task<int> SaveAsync();
    }
}
