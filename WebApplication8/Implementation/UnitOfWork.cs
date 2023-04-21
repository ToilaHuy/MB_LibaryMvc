using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;

        public UnitOfWork(MyDbContext context) 
        {
            _context = context;
            Item = new ItemRepository(_context);
            Borrower = new BorrowerRepository(_context);
            BorrowingHistory = new BorrowingHistoryRepository(_context);
            HistoryDetail = new HistoryDetailRepository(_context);

        }

        public IItemRepository Item { get; private set; }

        public IBorrowerRepository Borrower { get; private set; }

        public IBorrowingHistoryRepository BorrowingHistory { get; private set; }
        public IHistoryDetailRepository HistoryDetail { get; private set; }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

      

    }
}
