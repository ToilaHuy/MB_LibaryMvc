
using WebApplication8.Data;
//using WebApplication8.Models;

namespace WebApplication8.Repositories
{
    public interface IBorrowingHistoryRepository : IRepository<BorrowingHistory>
    {

        public Task<IQueryable<BorrowingHistory>> GetByBorrowingHistory(bool? status, DateTime? date, string search);

    }
}
