using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingQuestions.DAL.Interfaces;

namespace TestingQuestions.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: class
    {
        private AppDbContext context;
        private DbSet<TEntity> dbSet;
        public BaseRepository(AppDbContext dbContext)
        {
            this.context = dbContext;
            dbSet = context.Set<TEntity>();
        }
        public Task<int> CreateAsync(TEntity item)
        {
            dbSet.Add(item);
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public TEntity FindById(int id)
        => dbSet.Find(id);

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        => dbSet.Where(predicate).AsQueryable();

        public IQueryable<TEntity> GetAll()
        => dbSet.AsQueryable();

        public Task<int> RemoveAsync(TEntity item)
        {
            dbSet.Remove(item);
            return context.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }
    }
}
