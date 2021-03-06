﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.DAL.Repositories
{
    public class RepositoryContext: IRepositoryContext
    {
        private AppDbContext context;
        private bool disposed=false;

        public RepositoryContext(string connectionName)
        {
            context = new AppDbContext(connectionName);
            PersonRepository = new PersonRepository(context);
            QuestionRepository = new QuestionRepository(context);
            TestQuestionAnswerRepository = new TestQuestionAnswerRepository(context);
            TestResultRepository = new TestResultRepository(context);
        }

        public IPersonRepository PersonRepository { get ;  }
        public IQuestionRepository QuestionRepository { get;  }
        public ITestQuestionAnswerRepository TestQuestionAnswerRepository { get;  }
        public ITestResultRepository TestResultRepository { get; }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    PersonRepository.Dispose();
                    QuestionRepository.Dispose();
                    TestQuestionAnswerRepository.Dispose();
                    TestResultRepository.Dispose();
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
