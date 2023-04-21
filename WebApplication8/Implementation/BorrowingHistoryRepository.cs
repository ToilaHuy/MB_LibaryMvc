//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class BorrowingHistoryRepository : Repository<BorrowingHistory>, IBorrowingHistoryRepository
    {
        public BorrowingHistoryRepository(MyDbContext context) : base(context)
        {  
        }    
        public  IEnumerable<BorrowingHistory> GetAllBorrowingHistory()
        {
            var getBorrowingHistory =  _context.BorrowingHistories.Include(b => b.Borrower).ToList();
            return getBorrowingHistory;
        }
        public  BorrowingHistory GetDefaultBorrowingHistory(Guid? id)
        {
            var getBorrowingHistory =  _context.BorrowingHistories
                .Include(b => b.Borrower)
                .Include(b => b.HistoryDetails)
                .ThenInclude(i => i.Item)
                .FirstOrDefault(b => b.BorrowingHistoryId == id);
             return getBorrowingHistory;
        }
        public void DeleteAllRecordHistory()
        {
            _context.BorrowingHistories.RemoveRange(_context.BorrowingHistories);
        }
    }
}
