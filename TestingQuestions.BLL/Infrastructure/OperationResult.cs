using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.BLL
{
    public class OperationResult
    {
        public bool Succeded { get; set; }
        public string Message { get; set; }
        public object Tag { get; set; }
        public Exception Exception { get; set; }

    }
}
