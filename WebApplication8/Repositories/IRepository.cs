using System.Linq.Expressions;
using WebApplication8.Models;

namespace WebApplication8.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindAll(Expression<Func<T, bool>>predicate);
        void Add(T entity);
        void Update(T entity);
        //void AddRange(ICollection<T> entities);
        void Remove(T entity);
        //void RemoveRange(IEnumerable<T> entities);
        //IEnumerable<T> FindAll(string Name);
    }
}
