using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Repositories
{
    public interface IBorrowingHistoryRepository:IRepository<BorrowingHistory>
    {
   
        public IEnumerable<BorrowingHistory> GetAllBorrowingHistory();
        public BorrowingHistory GetDefaultBorrowingHistory(Guid? id);
    }
}
