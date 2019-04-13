using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
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

        public byte[] GetGeneralReport()
        {
            ExcelPackage excelPackage = new ExcelPackage();
            var workSheet = excelPackage.Workbook.Worksheets.Add("Отчет по тестам");

            int testerCount = Database.TestResultRepository.GetAll().Count();
            using (ExcelRange testerCountER = workSheet.Cells["A1:C1"])
            {
                testerCountER["A1"].Value = testerCount;
                testerCountER.Merge = true;
            }
            ExcelRange tableHeader = workSheet.Cells["A2:C2"];
            tableHeader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableHeader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            tableHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            tableHeader["A2"].Value = "Вопрос";
            tableHeader["B2"].Value = "Правильно";
            tableHeader["C2"].Value = "Не правильно";

            ExcelRange tableBody = workSheet.Cells["A3:C7"];
            tableBody.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableBody.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            tableBody.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            if (testerCount!=0)
            {
                int num = 0;
                foreach (var question in Database.QuestionRepository.GetAllWithInclude(q => q.TestQuestionAnswers))
                {
                    num++;
                    tableBody[$"A{num + 2}"].Value = num;
                    int rightAnswerCount = question.TestQuestionAnswers.Count(qa => qa.AnswerNum.Equals(question.RightAnswerNum));
                    tableBody[$"B{num + 2}"].Value = rightAnswerCount;
                    int notRightAnswerCount = question.TestQuestionAnswers.Count(qa => !qa.AnswerNum.Equals(question.RightAnswerNum));
                    tableBody[$"C{num + 2}"].Value = notRightAnswerCount;
                }
            }
            ExcelBarChart chart = workSheet.Drawings.AddChart("barChart", eChartType.BarStacked) as ExcelBarChart;
            chart.SetSize(300, 300);
            chart.SetPosition(tableHeader.Start.Row, -2, tableHeader.End.Column+1, 10);
            chart.Title.Text = workSheet.Name;

            chart.Series.Add(ExcelRange.GetAddress(tableBody.Start.Row, tableBody.Start.Column + 1, tableBody.End.Row, tableBody.End.Column), 
                                                        ExcelRange.GetAddress(tableBody.Start.Row, tableBody.Start.Column, tableBody.End.Row, tableBody.Start.Column));

            tableHeader.Dispose();
            tableBody.Dispose();
            return excelPackage.GetAsByteArray();
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
                int testId = Database.TestResultRepository.Get(t => t.StartedAt.Equals(startedAt) && t.PersonId.Equals(personId)).FirstOrDefault().Id;
                foreach (var question in Database.QuestionRepository.GetAll().ToArray())
                {
                   await this.AddQuestionAnswer(testId, question.Id);
                }   
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
