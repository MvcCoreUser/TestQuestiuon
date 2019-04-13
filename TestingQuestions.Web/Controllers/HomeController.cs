using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestingQuestions.BLL.Interfaces;

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
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}