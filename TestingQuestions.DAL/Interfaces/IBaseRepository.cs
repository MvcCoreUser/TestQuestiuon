using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingQuestions.DAL.Interfaces
{
    public interface IBaseRepository<TEntity>: IDisposable where TEntity: class
    {
        Task<int> CreateAsync(TEntity item);
        Task<int> UpdateAsync(TEntity item);
        Task<int> RemoveAsync(TEntity item);

        TEntity FindById(int id);

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
    }
}
