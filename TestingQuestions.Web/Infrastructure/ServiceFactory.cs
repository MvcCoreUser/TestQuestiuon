using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestingQuestions.BLL.Interfaces;

namespace TestingQuestions.Web
{
    public class ServiceInvoker
    {
        public ITestService TestService { get; }
        public IPersonService PersonService { get; }
        public ServiceInvoker(Func<ITestService> userServiceCallback, Func<IPersonService> movieServiceCallback)
        {
            TestService = userServiceCallback.Invoke();
            PersonService = movieServiceCallback.Invoke();
        }
    }

    public class ServiceFactory
    {
        public static ServiceInvoker ServiceInvoker { get; set; }
        public static ITestService TestService
            => ServiceInvoker.TestService;
        public static IPersonService PersonService
            => ServiceInvoker.PersonService;
    }
}