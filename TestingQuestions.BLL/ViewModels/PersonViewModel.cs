using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.BLL.ViewModels
{
    public class PersonViewModel
    {
        [Display(Name="ФИО тестируемого")]
        public string Name { get; set; }
    }
}
