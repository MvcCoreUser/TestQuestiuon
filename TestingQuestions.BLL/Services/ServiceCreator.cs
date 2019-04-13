using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.DAL.Repositories;

namespace TestingQuestions.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IPersonService CreatePersonService(string connectionName)
        => new PersonService(new RepositoryContext(connectionName));

        public ITestService CreateTestService(string connectionName)
        => new TestService(new RepositoryContext(connectionName));
    }
}
