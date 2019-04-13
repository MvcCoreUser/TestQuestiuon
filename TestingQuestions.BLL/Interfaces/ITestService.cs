using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.BLL.ViewModels;

namespace TestingQuestions.BLL.Interfaces
{
    public interface ITestService: IDisposable
    {
        Task<OperationResult> StartTest(int personId, DateTime startedAt);
        Task<OperationResult> AddQuestionAnswer(int testId, int questionId, int answerNum=0);
        Task<OperationResult> EndTest(int testId, int personId, DateTime finishedAt);
        IEnumerable<PersonQuestionAnswerView> GetPersonTestResult(int personId);
        PersonQuestionAnswerView GetPersonQuestion(int questionId);
        byte[] GetGeneralReport();
    }
}
