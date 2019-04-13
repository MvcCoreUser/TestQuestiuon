using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.BLL.Interfaces
{
    public interface IServiceCreator
    {
        ITestService CreateTestService(string connectionName);
        IPersonService CreatePersonService(string connectionName);
    }
}
