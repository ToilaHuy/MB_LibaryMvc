
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class BorrowingHistoryRepository : Repository<BorrowingHistory>, IBorrowingHistoryRepository
    {
        public BorrowingHistoryRepository(MyDbContext context) : base(context)
        {
        }

        public override Task Add(BorrowingHistory entity)
        {
            entity.BorrowDate = DateTime.Now;
            return base.Add(entity);
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
                .SingleOrDefaultAsync(b => b.BorrowingHistoryId == id);
        }
        public override async Task<bool> Remove(int id)
        {
            var exist = await _context.BorrowingHistories
                                .Where(x => x.BorrowingHistoryId == id)
                                .SingleOrDefaultAsync();
            if (exist == null) return false;

            _context.BorrowingHistories.Remove(exist);
            return true;
        }
        public override async Task<bool> Update(BorrowingHistory entity)
        {
            var exist = await _context.BorrowingHistories
                .Where(x => x.BorrowingHistoryId == entity.BorrowingHistoryId)
                .SingleOrDefaultAsync();
            if (exist == null) return false;
            _context.BorrowingHistories.Update(entity);
            return true;
        }


        public async Task<IQueryable<BorrowingHistory>> GetByBorrowingHistory(bool? status, DateTime? date, string search)
        {
        
            var query = _context.BorrowingHistories
                .Include(b => b.Borrower)
                .Where(x => (!date.HasValue || x.BorrowDate.Date == date.Value.Date)
                            && (!status.HasValue || x.Status == status)
                            && (string.IsNullOrEmpty(search)
                                || x.Borrower.BorrowerName.Contains(search)
                                || x.HistoryDetails.Any(d => d.Item.Title.Contains(search))))
                .OrderByDescending(i => i.BorrowDate);
            return  query;
        }

    }
}
