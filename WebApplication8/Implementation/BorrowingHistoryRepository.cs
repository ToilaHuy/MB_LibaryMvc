//using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class BorrowingHistoryRepository : Repository<BorrowingHistory>, IBorrowingHistoryRepository
    {
        //private readonly IMapper _mapper;
        public BorrowingHistoryRepository(MyDbContext context) : base(context)
        {  
        }
        //public BorrowingHistoryRepository(MyDbContext context, IMapper mapper) : base(context) {
        //    _mapper = mapper;
        //}
        //public async Task<IEnumerable<BorrowingHistoryModel>> GetBorrowingHistory(Guid? id)
        //{
        //    var getBorrowingHistory = await _context.BorrowingHistories.Include(b => b.Borrower).FirstOrDefaultAsync(m => m.BorrowingHistoryId == id);
        //    var convert = _mapper.Map<BorrowingHistory,BorrowingHistoryModel>(getBorrowingHistory);
        //    return (IEnumerable<BorrowingHistoryModel>)convert;
        //}
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
             return  getBorrowingHistory;
        }
    }
}
