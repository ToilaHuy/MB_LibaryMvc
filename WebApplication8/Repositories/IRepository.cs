using System.Linq.Expressions;
using WebApplication8.Models;

namespace WebApplication8.Repositories
{
    public interface IRepository<T> where T : class
    {
     
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>>predicate);
        Task Add(T entity);
        Task<bool> Update(T entity);
        //void AddRange(ICollection<T> entities);
        Task<bool> Remove(Guid id);
        //void RemoveRange(IEnumerable<T> entities);
        //IEnumerable<T> FindAll(string Name);
    }
}
