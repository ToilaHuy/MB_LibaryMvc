using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class BorrowerRepository : Repository<Borrower>, IBorrowerRepository
    {
        public BorrowerRepository(MyDbContext context) : base(context) { }
        public override async Task<bool> Remove(Guid id)
        {
            var exist = await _context.Borrowers
                                .Where(x => x.BorrowerId == id)
                                .FirstOrDefaultAsync();
            if (exist == null) return false;

            _context.Borrowers.Remove(exist);
            return true;
        }
        public override async Task<bool> Update(Borrower entity)
        {
            var exist = await _context.Borrowers
                                .Where(x => x.BorrowerId == entity.BorrowerId)
                                .FirstOrDefaultAsync();
            if (exist == null) return false;
            exist.BorrowerName = entity.BorrowerName;
            exist.Address = entity.Address;
            exist.BorrowingHistories = entity.BorrowingHistories;
            return true;
        }
    }
}
