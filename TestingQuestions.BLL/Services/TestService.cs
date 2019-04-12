using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.BLL.ViewModels;
using TestingQuestions.DAL.Entities;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.BLL.Services
{
    public class TestService : ITestService
    {
        public IContext Database { get; set; }
        public TestService(IContext db)
        {
            Database = db;
        }
        public async Task<OperationResult> AddQuestionAnswer(int testId, int questionId, int answerNum=0)
        {
            OperationResult result = new OperationResult();
            if (Database.TestQuestionAnswerRepository.GetAll().Any(qa=>qa.TestResultId.Equals(testId) && qa.QuestionId.Equals(questionId)))
            {
                TestQuestionAnswer testQuestionAnswer = Database.TestQuestionAnswerRepository.Get(qa => qa.TestResultId.Equals(testId) && qa.QuestionId.Equals(questionId)).FirstOrDefault();
                testQuestionAnswer.AnswerNum = answerNum;
                int recordsAffected = await Database.TestQuestionAnswerRepository.UpdateAsync(testQuestionAnswer);
                if (recordsAffected > 0)
                {
                    result = new OperationResult()
                    {
                        Message = "Ответ успешно изменен",
                        Succeded = true
                    };

                }
                else
                {
                    result = new OperationResult()
                    {
                        Message = "Произошла ошибка при изменении ответа",
                        Succeded = false
                    };
                }
                return result;
            }
            else
            {
                TestQuestionAnswer testQuestionAnswer = new TestQuestionAnswer();
                testQuestionAnswer.TestResultId = testId;
                testQuestionAnswer.QuestionId = questionId;
                testQuestionAnswer.AnswerNum = answerNum;
                int recordsAffected= await Database.TestQuestionAnswerRepository.CreateAsync(testQuestionAnswer);
                if (recordsAffected > 0)
                {
                    result = new OperationResult()
                    {
                        Message = "Ответ на вопрос успешно добавлен",
                        Succeded = true
                    };

                }
                else
                {
                    result = new OperationResult()
                    {
                        Message = "Произошла ошибка при добавлении ответа",
                        Succeded = false
                    };
                }
                return result;
            }
            
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<OperationResult> EndTest(int testId, int personId, DateTime finishedAt)
        {
            OperationResult result = new OperationResult();
            if (Database.TestResultRepository.GetAll().Any(t=>t.Id.Equals(testId)))
            {
                TestResult testResult = Database.TestResultRepository.Get(t => t.Id.Equals(testId) && t.PersonId.Equals(personId)).FirstOrDefault();
                testResult.FinishedAt = finishedAt;
                int recordsAffected= await Database.TestResultRepository.UpdateAsync(testResult);
                if (recordsAffected > 0)
                {
                    result = new OperationResult()
                    {
                        Message = "Тест успешно завершен",
                        Succeded = true
                    };

                }
                else
                {
                    result = new OperationResult()
                    {
                        Message = "Произошла ошибка при завершении теста",
                        Succeded = false
                    };
                }
                return result;
            }
            result = new OperationResult()
            {
                Message = "Тест не найден!",
                Succeded = false
            };
            return result;
        }

        public async Task<byte[]> GetGeneralReport()
        {
            ExcelPackage excelPackage = new ExcelPackage();
            var workSheet = excelPackage.Workbook.Worksheets.Add("Лист 1");

            int testerCount = Database.TestResultRepository.GetAll().Count();

            throw new NotImplementedException();
        }

        public IEnumerable<PersonQuestionAnswerView> GetPersonTestResult(int personId)
        {
            List<PersonQuestionAnswerView> res = new List<PersonQuestionAnswerView>();
            TestResult testResult = Database.TestResultRepository.FindById(personId);
            if (testResult!=null)
            {
                IEnumerable<TestQuestionAnswer> testQuestionAnswers = Database.TestQuestionAnswerRepository
                                                                     .GetWithInclude(tq => tq.TestResultId.Equals(testResult.Id), tq=>tq.Question)?.ToArray();
               
                foreach (var item in testQuestionAnswers)
                {
                    PersonQuestionAnswerView personQuestionAnswerView = new PersonQuestionAnswerView();
                    personQuestionAnswerView.QuestionDescription = item.Question.Description;
                    personQuestionAnswerView.Answer1 = item.Question.Answer1;
                    personQuestionAnswerView.Answer2 = item.Question.Answer2;
                    personQuestionAnswerView.Answer3 = item.Question.Answer3;
                    personQuestionAnswerView.RightAnswerNum = item.Question.RightAnswerNum;
                    personQuestionAnswerView.AnswerNum = item.AnswerNum;
                    res.Add(personQuestionAnswerView);
                }
                
            }
            
            return res;
        }

        public async Task<OperationResult> StartTest(int personId, DateTime startedAt)
        {
            OperationResult result = new OperationResult();
            TestResult testResult = new TestResult()
            {
                PersonId = personId,
                StartedAt = startedAt,
                FinishedAt = default(DateTime)
            };
            int recordsAffected= await Database.TestResultRepository.CreateAsync(testResult);
            if (recordsAffected>0)
            {
                result = new OperationResult()
                {
                    Message = "Тест успешно стартовал",
                    Succeded = true
                };
                
            }
            else
            {
                result = new OperationResult()
                {
                    Message = "Произошла ошибка при старте теста",
                    Succeded = false
                };
            }
            return result;
        }
    }
}
