using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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
        public void  Add(T entity)
        {   
            entities.Add(entity);
        }

        //public void AddRange(ICollection<T> entities)
        //{
        //    entities.AddRange(entities);
        //}

        public IEnumerable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
           return entities.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }

        public T GetById(int id)
        {
            return entities.Find(id);
        }

        public T GetById(Guid id)
        {
            return entities.Find(id);

        }

        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        //public void RemoveRange(T entities)
        //{
        //    var entity = entities;
        //    entity.RemoveRange(entity);
        //}

        public void Update(T entity)
        {
            entities.Update(entity);
        }
    }

}
