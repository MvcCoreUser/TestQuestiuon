using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using TestingQuestions.BLL;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.BLL.ViewModels;

namespace TestingQuestions.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITestService testService=> HttpContext.GetOwinContext().Get<ITestService>();
        private IPersonService personService=> HttpContext.GetOwinContext().Get<IPersonService>();


        public HomeController()
        {
            
        }
        public ActionResult Index()
        {
            return View(nameof(this.RegisterPerson), new PersonViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> RegisterPerson(PersonViewModel person)
        {
            OperationResult result= await personService.SavePerson(person);
            if (result.Succeded)
            {
                Session["personId"] = result.Tag;
                return RedirectToAction(nameof(this.StartTest));
            }
            else
            {
                ModelState.AddModelError("person", result.Message);
                return View();
            }
            
        }

        public ActionResult StartTest()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> StartTestAsync()
        {
            DateTime? startedAt =  DateTime.Now;
            int personId = (int)Session[nameof(personId)];
            if (Session["testId"]!=null)
            {
                ModelState.AddModelError("test", "Тест уже запущен");
                return RedirectToAction(nameof(this.DisplayQuestionByNum), new { num = 1 });
            }
            OperationResult result = await testService.StartTest(personId, startedAt.Value);
            if (result.Succeded)
            {
                Session["testId"] = result.Tag;
                return RedirectToAction(nameof(this.DisplayQuestionByNum), new { num=1});
            }
            else
            {
                ModelState.AddModelError("test", result.Message);
                return View();
            }
            
        }

       

        [HttpPost]
        public async Task<string> SaveQuestionAsnwer(PersonQuestionAnswerView personQuestionAnswer)
        {
            if (personQuestionAnswer.QuestionId!= 0 && personQuestionAnswer.AnswerNum!=0)
            {
                int testId = (int)Session[nameof(testId)];
                OperationResult result = await testService.AddQuestionAnswer(testId, personQuestionAnswer.QuestionId, personQuestionAnswer.AnswerNum);
                //if (result.Succeded)
                //{
                //     return RedirectToAction(nameof(this.DisplayNextQuestion), new { curQuestionId = personQuestionAnswer.QuestionId });
                //}
                return result.Message;
            }
            return "error";
        }

        public async Task<ActionResult> DisplayTestResults()
        {
            int testId = (int) Session[nameof(testId)];
            int personId = (int)Session[nameof(personId)];
            OperationResult result= await testService.EndTest(testId, personId, DateTime.Now);
            IEnumerable<PersonQuestionAnswerView> personQuestionAnswers = testService.GetPersonTestResult(personId, testId);
            return View(personQuestionAnswers);
        }

        public ActionResult GetGeneralReport()
        {
            string fileName = "GeneralReport.xlsx";
            string contentType = MimeMapping.GetMimeMapping(fileName);
            byte[] fileContents = testService.GetGeneralReport();
            return File(fileContents, contentType, fileName);
        }

        public ActionResult DisplayQuestionByNum(int num)
        {
            int questionCount = testService.GetQuestionCount();
            bool isLast = num>questionCount;
            if (isLast)
            {
                return RedirectToAction(nameof(this.DisplayTestResults));
            }
            else
            {
                PersonQuestionAnswerView personQuestion = testService.GetQuestionByNum(num);
                personQuestion.IsLast = num.Equals(questionCount);
                personQuestion.IsFirst = num.Equals(1);
                ViewBag.qCount = questionCount;
                int testId = (int)Session[nameof(testId)];
                personQuestion.AnswerNum = testService.GetAnswerNum(testId, personQuestion.QuestionId).GetValueOrDefault();
                return View("DisplayQuestion", personQuestion);
            }
        }
    }
}