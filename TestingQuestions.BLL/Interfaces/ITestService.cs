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
        int? GetAnswerNum(int testId, int questionId);
        Task<OperationResult> EndTest(int testId, int personId, DateTime finishedAt);
        IEnumerable<PersonQuestionAnswerView> GetPersonTestResult(int personId, int testId);
        PersonQuestionAnswerView GetNextQuestion(int curQuestionId);
        PersonQuestionAnswerView GetPrevQuestion(int curQuestionId);
        PersonQuestionAnswerView GetQuestionByNum(int num);
        bool IsLastQuestion(int questionId);
        bool IsFirstQuestion(int questionId);
        byte[] GetGeneralReport();
        int GetQuestionCount();
    }
}
