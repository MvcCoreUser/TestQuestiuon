using System;
using System.Collections.Generic;
using System.Drawing;
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
        public IRepositoryContext Database { get; set; }
        public TestService(IRepositoryContext db)
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
                testerCountER["A1"].Value = $"Кол-во тестируемых: {testerCount}";
                workSheet.Cells["A1:C1"].Merge = true;
            }
            ExcelRange tableHeader = workSheet.Cells["A2:C2"];
            tableHeader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableHeader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            tableHeader.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            tableHeader["A2"].Value = "Вопрос";
            tableHeader["B2"].Value = "Правильно";
            tableHeader["C2"].Value = "Неправильно";

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
            workSheet.Column(tableBody.End.Column - 1).AutoFit();
            workSheet.Column(tableBody.End.Column).AutoFit();

            ExcelBarChart chart = workSheet.Drawings.AddChart("barChart", eChartType.ColumnStacked) as ExcelBarChart;
            chart.SetSize(500, 300);
            chart.SetPosition(tableHeader.Start.Row, -2, tableHeader.End.Column, 10);
            chart.Title.Text = workSheet.Name;
            string serie = "B3:B7";
            string xSerie = "A3:A7";
            var serie1= chart.Series.Add(serie, xSerie);
            serie1.Header = "Правильно";
            serie1.Fill.Color = Color.FromArgb(79, 129, 189);
            serie = "C3:C7";
            xSerie = "A3:A7";
            var serie2= chart.Series.Add(serie, xSerie);
            serie2.Header = "Неправильно";
            serie2.Fill.Color = Color.FromArgb(192, 80, 77);

            tableHeader.Dispose();
            tableBody.Dispose();
            return excelPackage.GetAsByteArray();
        }

        public IEnumerable<PersonQuestionAnswerView> GetPersonTestResult(int personId, int testId)
        {
            List<PersonQuestionAnswerView> res = new List<PersonQuestionAnswerView>();
            TestResult testResult = Database.TestResultRepository.Get(t=>t.PersonId.Equals(personId) && t.Id.Equals(testId)).FirstOrDefault();
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
                FinishedAt= null
            };
            int recordsAffected= await Database.TestResultRepository.CreateAsync(testResult);
            if (recordsAffected>0)
            {
                int testId = Database.TestResultRepository.Get(t => t.StartedAt.Equals(startedAt) && t.PersonId.Equals(personId)).FirstOrDefault().Id;
                result = new OperationResult()
                {
                    Message = "Тест успешно стартовал",
                    Succeded = true,
                    Tag = testId
                };
                var questions = Database.QuestionRepository.GetAll().ToArray();
                foreach (var question in questions)
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

        public PersonQuestionAnswerView GetNextQuestion(int curQuestionId)
        {
            Question question = new Question();
            question = Database.QuestionRepository.Get(q => q.Id > curQuestionId).OrderBy(p => p.Id).FirstOrDefault();
            PersonQuestionAnswerView personQuestionAnswerView = PersonQuestionAnswerView.FromQuestion(question);

            return personQuestionAnswerView;
        }

        public PersonQuestionAnswerView GetPrevQuestion(int curQuestionId)
        {
            Question question = new Question();
            question = Database.QuestionRepository.Get(q => q.Id < curQuestionId).OrderBy(p=>p.Id).LastOrDefault();
            PersonQuestionAnswerView personQuestionAnswerView = PersonQuestionAnswerView.FromQuestion(question);
            return personQuestionAnswerView;
        }

        public bool IsLastQuestion(int questionId)
         => questionId.Equals(Database.QuestionRepository.GetAll().Max(q => q.Id));

        public bool IsFirstQuestion(int questionId)
        => questionId.Equals(Database.QuestionRepository.GetAll().Min(q => q.Id));

        public int? GetAnswerNum(int testId, int questionId)
        =>   Database.TestQuestionAnswerRepository.Get(qa => qa.TestResultId.Equals(testId) && qa.QuestionId.Equals(questionId)).FirstOrDefault()?.AnswerNum;

        public PersonQuestionAnswerView GetQuestionByNum(int num)
        => PersonQuestionAnswerView.FromQuestion(Database.QuestionRepository.GetAll().OrderBy(q=>q.Id).Skip(num - 1).Take(1).FirstOrDefault());

        public int GetQuestionCount()
        => Database.QuestionRepository.GetAll().Count();
    }
}
