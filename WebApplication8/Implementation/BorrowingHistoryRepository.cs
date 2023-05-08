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
        public override async Task<IEnumerable<BorrowingHistory>> GetAll()
        {
            return await _context.BorrowingHistories.Include(b => b.Borrower).ToListAsync();
        }
        public override async Task<BorrowingHistory> GetById(int id)
        {
            return await _context.BorrowingHistories
              .Include(b => b.Borrower)
              .Include(b => b.HistoryDetails)
              .ThenInclude(i => i.Item)
              .FirstOrDefaultAsync(b => b.BorrowingHistoryId == id);
        }
        public override async Task<bool> Remove(int id)
        {
            var exist = await _context.BorrowingHistories
                                .Where(x => x.BorrowingHistoryId == id)                                               
                                .FirstOrDefaultAsync();
            if (exist == null) return false;

            _context.BorrowingHistories.Remove(exist);
            return true;

        }
      


    }
}
