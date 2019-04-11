using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.DAL.Repositories
{
    public class Context: IContext
    {
        private AppDbContext context;
        private bool disposed=false;

        public Context(string connectionName)
        {
            context = new AppDbContext(connectionName);
            PersonRepository = new PersonRepository(context);
            QuestionRepository = new QuestionRepository(context);
            PersonQuestionAnswerRepository = new PersonQuestionAnswerRepository(context);
        }

        public IPersonRepository PersonRepository { get ;  }
        public IQuestionRepository QuestionRepository { get;  }
        public IPersonQuestionAnswerRepository PersonQuestionAnswerRepository { get;  }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    PersonRepository.Dispose();
                    QuestionRepository.Dispose();
                    PersonQuestionAnswerRepository.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
