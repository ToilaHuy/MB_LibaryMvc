using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MyDbContext _context;
        private DbSet<T> entities;

        public Repository(MyDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }
        public virtual async Task Add(T entity)
        {
            await entities.AddAsync(entity);
        }
        public virtual async Task<IQueryable<T>> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate).AsQueryable();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {

            return await entities.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);

        }

        public virtual Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }

}
