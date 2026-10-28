using System.Linq.Expressions;

namespace WebApplication8.Repositories
{
    public interface IRepository<T> where T : class
    {

        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<IQueryable<T>> FindAll(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task<bool> Update(T entity);
        //void AddRange(ICollection<T> entities);
        Task<bool> Remove(int id);
        //void RemoveRange(IEnumerable<T> entities);
        //IEnumerable<T> FindAll(string Name);
    }
}
