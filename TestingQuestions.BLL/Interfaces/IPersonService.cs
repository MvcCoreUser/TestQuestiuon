using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.BLL.ViewModels;

namespace TestingQuestions.BLL.Interfaces
{
    public interface IPersonService: IDisposable
    {
        Task<OperationResult> SavePerson(PersonViewModel personViewModel);
    }
}
