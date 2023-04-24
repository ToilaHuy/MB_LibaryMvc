
using WebApplication8.Data;
using WebApplication8.Repositories;

namespace WebApplication8.Implementation
{
    public class HistoryDetailRepository : Repository<HistoryDetail>, IHistoryDetailRepository
    {
        public HistoryDetailRepository(MyDbContext context) : base(context) { }

      
    }
}
