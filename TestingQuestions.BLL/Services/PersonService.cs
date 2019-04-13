using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.BLL.Interfaces;
using TestingQuestions.BLL.ViewModels;
using TestingQuestions.DAL.Entities;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.BLL.Services
{
    public class PersonService : IPersonService
    {
        public IRepositoryContext Database { get; set; }

        public PersonService(IRepositoryContext db)
        {
            Database = db;
        }
        public async Task<OperationResult> SavePerson(PersonViewModel personViewModel)
        {
            OperationResult result = new OperationResult();
            if (Database.PersonRepository.GetAll().Any(p=>p.Name.Equals(personViewModel.Name, StringComparison.Ordinal)))
            {
                Person person = Database.PersonRepository.Get(p => p.Name.Equals(personViewModel.Name, StringComparison.Ordinal)).FirstOrDefault();
                person.Name = personViewModel.Name;
                int recordsAffected = await Database.PersonRepository.UpdateAsync(person);
                if (recordsAffected > 0)
                {
                    result = new OperationResult()
                    {
                        Message = "Тестируемый успешно зарегистрирован",
                        Succeded = true
                    };
                }
                else
                {
                    result = new OperationResult()
                    {
                        Message = "Тестируемый незарегистрирован. Ошибка БД",
                        Succeded = false
                    };
                };
            }
        
            else
            {
                Person person = new Person() { Name = personViewModel.Name };
                int recordsAffected= await Database.PersonRepository.CreateAsync(person);
                if (recordsAffected>0)
                {
                    result = new OperationResult()
                    {
                        Message = "Тестируемый успешно зарегистрирован",
                        Succeded = true
                    };
                }
                else
                {
                    result = new OperationResult()
                    {
                        Message = "Тестируемый незарегистрирован. Ошибка БД",
                        Succeded = false
                    };
                }
                
            }

            return result;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
