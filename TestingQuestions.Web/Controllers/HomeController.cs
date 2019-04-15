using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingQuestions.BLL;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.BLL.ViewModels;

namespace TestingQuestions.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITestService testService;
        private IPersonService personService;

        public HomeController()
        {
            testService = ServiceFactory.TestService;
            personService = ServiceFactory.PersonService;
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
        public async Task<ActionResult> StartTest(DateTime? startedAt)
        {
            startedAt = startedAt ?? DateTime.Now;
            int personId = (int)Session[nameof(personId)];
            OperationResult result = await testService.StartTest(personId, startedAt.Value);
            if (result.Succeded)
            {
                Session["testId"] = result.Tag;
                return RedirectToAction(nameof(this.DisplayNextQuestion), new { curQuestionId=0});
            }
            else
            {
                ModelState.AddModelError("test", result.Message);
                return View();
            }
            
        }

        public ActionResult DisplayNextQuestion(int curQuestionId)
        {
            bool isLast = testService.IsLastQuestion(curQuestionId);
            if (isLast)
            {
                return RedirectToAction(nameof(this.DisplayTestResults));
            }
            else
            {
                PersonQuestionAnswerView personQuestion = testService.GetNextQuestion(curQuestionId);
                personQuestion.IsLast = isLast;
                personQuestion.IsFirst = testService.IsFirstQuestion(curQuestionId);
                return View("DisplayQuestion", personQuestion);
            }
            
        }

        public ActionResult DisplayPrevQuestion(int curQuestionId)
        {
            PersonQuestionAnswerView personQuestion = testService.GetPrevQuestion(curQuestionId);
            personQuestion.IsLast = testService.IsLastQuestion(curQuestionId);
            personQuestion.IsFirst = testService.IsFirstQuestion(curQuestionId);
            return View("DisplayQuestion", personQuestion);
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
            IEnumerable<PersonQuestionAnswerView> personQuestionAnswers = testService.GetPersonTestResult(personId);
            return View(personQuestionAnswers);
        }

        public ActionResult GetGeneralReport()
        {
            string fileName = "GeneralReport.xlsx";
            string contentType = MimeMapping.GetMimeMapping(fileName);
            byte[] fileContents = testService.GetGeneralReport();
            return File(fileContents, contentType, fileName);
        }
    }
}