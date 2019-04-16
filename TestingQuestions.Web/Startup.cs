using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.BLL.Services;

[assembly: OwinStartup(typeof(TestingQuestions.Web.Startup))]

namespace TestingQuestions.Web
{
    public class Startup
    {
        private readonly string connectionName = "DefaultConnection";
        private IServiceCreator serviceCreator = new ServiceCreator();
        private ITestService CreateTestService() => serviceCreator.CreateTestService(connectionName);
        private IPersonService CreatePersonService() => serviceCreator.CreatePersonService(connectionName);

        public void Configuration(IAppBuilder app)
        {
            // Дополнительные сведения о настройке приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888
            //ServiceFactory.ServiceInvoker = new ServiceInvoker(CreateTestService, CreatePersonService);
            app.CreatePerOwinContext<ITestService>(CreateTestService);
            app.CreatePerOwinContext<IPersonService>(CreatePersonService);
        }       
    }
}
